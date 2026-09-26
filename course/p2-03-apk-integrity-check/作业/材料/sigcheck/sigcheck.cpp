// ============================================================================
// sigcheck —— 重打包校验（签名证书完整性）关键代码 · 讲解样例
//
// 这是一套自制的教学样例，演示主流移动端加固方案里"重打包校验"普遍的做法：native 自解
// APK + 手写 DER 走 PKCS#7 定位签名证书、只对 tbsCertificate 算指纹比对、不符即处置。
// 做成自包含、能在 PC 上编译运行，方便你把"重打包校验到底怎么做"读明白。
//
// ── 重打包校验怎么做 ──────────────────────────────────────────────────────
// 重打包校验盯的是签名证书，不是整包文件哈希。做法是取 APK 签名证书里的
// tbsCertificate（证书的"待签名"主体，公钥就在其中）算一个哈希指纹，与期望值比对——
// 任何人重打包都得用自己的私钥重签，证书随之更换，这个指纹就对不上。
//
// 指纹按两条来源各算一遍、交叉佐证：一条走系统 API（getPackageInfo(GET_SIGNATURES)
// 取 signatures[0] 算哈希）；另一条在 native 里自解 APK——直接读 zip 中央目录定位
// META-INF/*.RSA|.DSA|.EC 签名块，用一个手写 DER 状态机走 PKCS#7 SignedData 结构，
// 定位到 tbsCertificate 那段字节再算哈希。两条算的是同一个指纹，任一条被绕过时另一条仍在。
// 比对不符时走统一惩罚：拉起一个回桌面的 Intent 把用户弹出去，随即 System.exit 结束进程。
//
// native 自解这条路不经过 PackageManager：签名查询 API 是常见挂钩点，自己读 zip、自己走
// DER，就把指纹的产出握在 native 手里。校 tbsCertificate 而非整证书，是因为公钥就落在这段
// 里、重签名必然改动它；这类校验哈希用 MD5 这类快算法即可（要的是"发现证书被换掉"，不需要抗
// 碰撞），这里换成 SHA-256 更清楚。
// ============================================================================
//
// 读代码路线（建议按这个顺序看）：
//   1) asn1 / der_parse        —— 手写 DER TLV 解析（含 32 位长度域溢出的防护）
//   2) pkcs7_extract_certificate + asn1_locate_tbs —— 从签名块定位到 tbsCertificate
//   3) ApkZip                  —— 不经 PackageManager，自己读 zip 找 META-INF/*.RSA
//   4) verify_apk_signature    —— 双路取证书 → 指纹 → 比对
//   5) punish_kick_home_and_exit —— 不符时踢回桌面 + System.exit
//
#include <algorithm>
#include <array>
#include <cctype>
#include <cstdint>
#include <cstdio>
#include <cstring>
#include <functional>
#include <string>
#include <vector>

namespace sigcheck {

// ============================================================== SHA-256 =====
// 自包含 SHA-256（FIPS 180-4），不依赖任何 crypto 库，PC 和 Android 都能一样编。
namespace {
inline uint32_t rotr(uint32_t x, uint32_t n) { return (x >> n) | (x << (32 - n)); }
const uint32_t kK[64] = {
    0x428a2f98,0x71374491,0xb5c0fbcf,0xe9b5dba5,0x3956c25b,0x59f111f1,0x923f82a4,0xab1c5ed5,
    0xd807aa98,0x12835b01,0x243185be,0x550c7dc3,0x72be5d74,0x80deb1fe,0x9bdc06a7,0xc19bf174,
    0xe49b69c1,0xefbe4786,0x0fc19dc6,0x240ca1cc,0x2de92c6f,0x4a7484aa,0x5cb0a9dc,0x76f988da,
    0x983e5152,0xa831c66d,0xb00327c8,0xbf597fc7,0xc6e00bf3,0xd5a79147,0x06ca6351,0x14292967,
    0x27b70a85,0x2e1b2138,0x4d2c6dfc,0x53380d13,0x650a7354,0x766a0abb,0x81c2c92e,0x92722c85,
    0xa2bfe8a1,0xa81a664b,0xc24b8b70,0xc76c51a3,0xd192e819,0xd6990624,0xf40e3585,0x106aa070,
    0x19a4c116,0x1e376c08,0x2748774c,0x34b0bcb5,0x391c0cb3,0x4ed8aa4a,0x5b9cca4f,0x682e6ff3,
    0x748f82ee,0x78a5636f,0x84c87814,0x8cc70208,0x90befffa,0xa4506ceb,0xbef9a3f7,0xc67178f2};
}  // namespace

std::string sha256_hex(const uint8_t* data, std::size_t len) {
  uint32_t h[8] = {0x6a09e667,0xbb67ae85,0x3c6ef372,0xa54ff53a,0x510e527f,0x9b05688c,0x1f83d9ab,0x5be0cd19};
  std::vector<uint8_t> msg;
  if (data && len) msg.assign(data, data + len);
  const uint64_t bit_len = static_cast<uint64_t>(msg.size()) * 8;
  msg.push_back(0x80);
  while (msg.size() % 64 != 56) msg.push_back(0x00);
  for (int i = 7; i >= 0; --i) msg.push_back(static_cast<uint8_t>((bit_len >> (i * 8)) & 0xff));
  for (std::size_t off = 0; off < msg.size(); off += 64) {
    uint32_t w[64];
    for (int i = 0; i < 16; ++i)
      w[i] = (uint32_t(msg[off+i*4])<<24)|(uint32_t(msg[off+i*4+1])<<16)|(uint32_t(msg[off+i*4+2])<<8)|uint32_t(msg[off+i*4+3]);
    for (int i = 16; i < 64; ++i) {
      uint32_t s0 = rotr(w[i-15],7)^rotr(w[i-15],18)^(w[i-15]>>3);
      uint32_t s1 = rotr(w[i-2],17)^rotr(w[i-2],19)^(w[i-2]>>10);
      w[i] = w[i-16]+s0+w[i-7]+s1;
    }
    uint32_t a=h[0],b=h[1],c=h[2],d=h[3],e=h[4],f=h[5],g=h[6],hh=h[7];
    for (int i = 0; i < 64; ++i) {
      uint32_t S1=rotr(e,6)^rotr(e,11)^rotr(e,25), ch=(e&f)^(~e&g), t1=hh+S1+ch+kK[i]+w[i];
      uint32_t S0=rotr(a,2)^rotr(a,13)^rotr(a,22), maj=(a&b)^(a&c)^(b&c), t2=S0+maj;
      hh=g;g=f;f=e;e=d+t1;d=c;c=b;b=a;a=t1+t2;
    }
    h[0]+=a;h[1]+=b;h[2]+=c;h[3]+=d;h[4]+=e;h[5]+=f;h[6]+=g;h[7]+=hh;
  }
  static const char* kHex = "0123456789abcdef";
  std::string s; s.reserve(64);
  for (int i = 0; i < 8; ++i)
    for (int j = 3; j >= 0; --j) { uint8_t by=(h[i]>>(j*8))&0xff; s.push_back(kHex[by>>4]); s.push_back(kHex[by&0xf]); }
  return s;
}

// ================================================================ ASN.1 =====
struct DerTlv {
  bool ok = false;
  uint8_t tag = 0;
  std::size_t header_len = 0, length = 0, content_off = 0;
  std::size_t end() const { return content_off + length; }
};

// 溢出安全的边界判据：窗口 [off, off+len) 是否落在 [0,size) 内。
// ★ 关键防护（REV-FIX-1）：armeabi-v7a 上 size_t 是 32 位，DER 的 4 字节长度域可达
//   0xFFFFFFFF，朴素的 `off + len > size` 会 WRAP（回绕成一个小值）而误判"在界内"，
//   于是越界读。这里先证 off <= size，再用减法比较——任何字长都安全。
template <typename UInt>
inline bool tlv_len_in_bounds(UInt off, UInt len, UInt size) {
  return off <= size && len <= size - off;
}

// 解析绝对偏移 pos 处的一个 TLV；任何畸形（截断/不定长/长度越界）都 ok=false，绝不越界读。
DerTlv der_parse(const uint8_t* buf, std::size_t size, std::size_t pos) {
  DerTlv t;
  if (!buf || pos >= size) return t;
  std::size_t i = pos;
  t.tag = buf[i++];
  if ((t.tag & 0x1f) == 0x1f) return t;           // 高标签号不用，拒绝
  if (i >= size) return t;
  uint8_t lb = buf[i++];
  std::size_t length = 0;
  if (lb < 0x80) { length = lb; }
  else {
    int n = lb & 0x7f;
    if (n == 0 || n > 4) return t;                // 不定长 / 超长，拒绝
    if (!tlv_len_in_bounds<std::size_t>(i, (std::size_t)n, size)) return t;
    for (int k = 0; k < n; ++k) length = (length << 8) | buf[i++];
  }
  t.header_len = i - pos; t.content_off = i;
  if (!tlv_len_in_bounds<std::size_t>(t.content_off, length, size)) return t;
  t.length = length; t.ok = true;
  return t;
}

namespace {
inline bool is_constructed(uint8_t tag) { return (tag & 0x20) != 0; }

// 这个 TLV 看着像不像一张完整的 X.509 Certificate？
//   Certificate ::= SEQUENCE { tbsCertificate SEQUENCE, sigAlg SEQUENCE, signature BIT STRING }
bool looks_like_certificate(const uint8_t* buf, std::size_t size, std::size_t pos) {
  DerTlv cert = der_parse(buf, size, pos);
  if (!cert.ok || cert.tag != 0x30) return false;
  DerTlv tbs = der_parse(buf, size, cert.content_off);
  if (!tbs.ok || tbs.tag != 0x30) return false;
  DerTlv alg = der_parse(buf, size, tbs.end());
  if (!alg.ok || alg.tag != 0x30) return false;
  DerTlv sig = der_parse(buf, size, alg.end());
  if (!sig.ok || sig.tag != 0x03) return false;   // BIT STRING
  return true;
}

constexpr int kMaxDerDepth = 64;  // 递归深度上限，防恶意深嵌套把栈爆掉（DoS）

bool scan_for_certificate(const uint8_t* buf, std::size_t size, std::size_t start,
                          std::size_t end, std::vector<uint8_t>* out, int depth) {
  if (depth > kMaxDerDepth) return false;
  std::size_t pos = start;
  while (pos < end) {
    DerTlv t = der_parse(buf, size, pos);
    if (!t.ok) return false;
    if (looks_like_certificate(buf, size, pos)) { out->assign(buf + pos, buf + t.end()); return true; }
    if (is_constructed(t.tag) && t.length > 0)
      if (scan_for_certificate(buf, size, t.content_off, t.end(), out, depth + 1)) return true;
    if (t.end() <= pos) return false;             // 无前进 -> 退出
    pos = t.end();
  }
  return false;
}
}  // namespace

// 从签名块（PKCS#7 SignedData，或裸 X.509 Certificate）里取出完整的 X.509 Certificate DER。
std::vector<uint8_t> pkcs7_extract_certificate(const uint8_t* der, std::size_t size) {
  std::vector<uint8_t> out;
  if (!der || size == 0) return out;
  DerTlv outer = der_parse(der, size, 0);
  if (!outer.ok || outer.tag != 0x30) return out;
  if (looks_like_certificate(der, size, 0)) { out.assign(der, der + outer.end()); return out; }
  scan_for_certificate(der, size, outer.content_off, outer.end(), &out, 0);
  return out;
}

// 在 X.509 Certificate DER 里定位 tbsCertificate（第一个子 SEQUENCE）。
// ★ 校验只对 tbsCertificate 这段算指纹——公钥就落在这里、重签名必然改它。
bool asn1_locate_tbs(const uint8_t* cert_der, std::size_t size,
                     std::size_t* tbs_off, std::size_t* tbs_len) {
  if (!looks_like_certificate(cert_der, size, 0)) return false;
  DerTlv cert = der_parse(cert_der, size, 0);
  DerTlv tbs = der_parse(cert_der, size, cert.content_off);
  if (!tbs.ok) return false;
  if (tbs_off) *tbs_off = cert.content_off;
  if (tbs_len) *tbs_len = tbs.end() - cert.content_off;
  return true;
}

// ============================================================== ApkZip =======
// 极简 zip 读取：不经 PackageManager，自己读 EOCD -> 中央目录 -> 定位 META-INF/*.RSA。
namespace {
inline uint16_t rd16(const uint8_t* p) { return (uint16_t)(p[0] | (p[1] << 8)); }
inline uint32_t rd32(const uint8_t* p) {
  return (uint32_t)p[0] | ((uint32_t)p[1]<<8) | ((uint32_t)p[2]<<16) | ((uint32_t)p[3]<<24);
}
constexpr uint32_t kEocdSig = 0x06054b50, kCenSig = 0x02014b50, kLocSig = 0x04034b50;
bool ends_with_ci(const std::string& s, const char* suf) {
  std::size_t n = std::strlen(suf);
  if (s.size() < n) return false;
  for (std::size_t i = 0; i < n; ++i)
    if (std::toupper((unsigned char)s[s.size()-n+i]) != std::toupper((unsigned char)suf[i])) return false;
  return true;
}
}  // namespace

class ApkZip {
 public:
  bool parse(const uint8_t* data, std::size_t size) {
    ok_ = false; entries_.clear(); data_ = data; size_ = size;
    if (!data || size < 22) return false;
    std::size_t eocd = std::string::npos, start = size - 22;
    for (std::size_t i = start + 1; i-- > 0;) {
      if (rd32(data + i) == kEocdSig) { eocd = i; break; }
      if (start - i > 0xffff + 22) break;
    }
    if (eocd == std::string::npos || eocd + 22 > size) return false;
    uint16_t total = rd16(data + eocd + 10);
    uint32_t cd_off = rd32(data + eocd + 16), cd_size = rd32(data + eocd + 12);
    if (cd_off > size || cd_size > size - cd_off) return false;  // 溢出安全
    std::size_t p = cd_off;
    for (uint16_t n = 0; n < total; ++n) {
      if (p > size || size - p < 46 || rd32(data + p) != kCenSig) return false;
      Entry e;
      e.method = rd16(data + p + 10);
      e.comp_size = rd32(data + p + 20);
      e.local_off = rd32(data + p + 42);
      uint16_t name_len = rd16(data+p+28), extra_len = rd16(data+p+30), comment_len = rd16(data+p+32);
      if (name_len > size - (p + 46)) return false;
      e.name.assign((const char*)(data + p + 46), name_len);
      entries_.push_back(std::move(e));
      p += 46 + name_len + extra_len + comment_len;
    }
    ok_ = true; return true;
  }
  std::string find_signature_entry() const {
    for (const auto& e : entries_) {
      if (e.name.rfind("META-INF/", 0) != 0) continue;
      if (ends_with_ci(e.name, ".RSA") || ends_with_ci(e.name, ".DSA") || ends_with_ci(e.name, ".EC"))
        return e.name;
    }
    return {};
  }
  bool extract_stored(const std::string& name, std::vector<uint8_t>* out) const {
    if (!ok_ || !data_) return false;
    for (const auto& e : entries_) {
      if (e.name != name) continue;
      std::size_t lh = e.local_off;
      if (lh > size_ || size_ - lh < 30 || rd32(data_ + lh) != kLocSig) return false;
      uint16_t name_len = rd16(data_+lh+26), extra_len = rd16(data_+lh+28);
      std::size_t data_off = lh + 30 + name_len + extra_len;
      if (data_off > size_ || e.comp_size > size_ - data_off) return false;
      out->assign(data_ + data_off, data_ + data_off + e.comp_size);  // 签名块一般是 stored
      return true;
    }
    return false;
  }
 private:
  struct Entry { std::string name; uint16_t method=0; uint32_t comp_size=0, local_off=0; };
  const uint8_t* data_=nullptr; std::size_t size_=0;
  std::vector<Entry> entries_; bool ok_=false;
};

// =========================================================== 惩罚 ============
// 指纹不符（被重打包 / 重签名）时的统一处置：拉起回桌面的 Intent 把用户弹出去，
// 随即结束进程。真机上是 JNI 调 Android：
//   Intent i(ACTION_MAIN); i.addCategory(CATEGORY_HOME); i.setFlags(NEW_TASK);
//   startActivity(i);  android.os.Process.killProcess(myPid) / System.exit(exit_code);
// 这里在 PC 样例里只打印一行，方便你看到"触发了"。
[[noreturn]] void punish_kick_home_and_exit(int exit_code) {
  std::printf("[sigcheck] TAMPER -> kick to launcher home + System.exit(%d)\n", exit_code);
  std::exit(exit_code);
}

// =========================================================== verify ==========
// 路①：native 自解 APK 取证书 DER（读文件 -> zip -> META-INF/*.RSA -> PKCS#7 -> Certificate）。
std::vector<uint8_t> read_certificate_native(const std::string& apk_path) {
  std::vector<uint8_t> cert;
  FILE* fp = std::fopen(apk_path.c_str(), "rb");
  if (!fp) return cert;
  std::fseek(fp, 0, SEEK_END); long n = std::ftell(fp); std::fseek(fp, 0, SEEK_SET);
  std::vector<uint8_t> bytes(n > 0 ? n : 0);
  if (n > 0) { size_t got = std::fread(bytes.data(), 1, n, fp); bytes.resize(got); }
  std::fclose(fp);
  ApkZip zip;
  if (!zip.parse(bytes.data(), bytes.size())) return cert;
  std::string sig = zip.find_signature_entry();
  if (sig.empty()) return cert;
  std::vector<uint8_t> pkcs7;
  if (!zip.extract_stored(sig, &pkcs7)) return cert;
  return pkcs7_extract_certificate(pkcs7.data(), pkcs7.size());
}

// 路②：走系统 API getPackageInfo(GET_SIGNATURES) 取 signatures[0].toByteArray()。
// 真机上是 JNI 反射调用；PC 样例里用一个可注入的回调代替（默认不可用）。
using SigProbe = std::function<bool(std::vector<uint8_t>* der_out)>;

enum class Verdict { Match, Mismatch, Unavailable };

// 两路都要：先 native 自解（不经 PackageManager，抗挂钩），拿不到再退回系统 API。
// 取到证书 -> 定位 tbsCertificate -> 只对这段算 SHA-256 指纹 -> 和期望值比。
// 取不到证书（畸形/读不到）绝不惩罚（不误伤）。
Verdict verify_apk_signature(const std::string& apk_path, const std::string& expected_fp_hex,
                             SigProbe probe = nullptr, std::string* observed_out = nullptr) {
  std::vector<uint8_t> cert = read_certificate_native(apk_path);
  if (cert.empty() && probe) { std::vector<uint8_t> der; if (probe(&der)) cert = pkcs7_extract_certificate(der.data(), der.size()); }
  if (cert.empty()) return Verdict::Unavailable;          // 不误伤

  std::size_t tbs_off = 0, tbs_len = 0;
  if (!asn1_locate_tbs(cert.data(), cert.size(), &tbs_off, &tbs_len)) return Verdict::Unavailable;
  std::string fp = sha256_hex(cert.data() + tbs_off, tbs_len);  // ★ 只哈希 tbsCertificate
  if (observed_out) *observed_out = fp;
  if (expected_fp_hex.empty()) return Verdict::Unavailable;

  std::string a = fp, b = expected_fp_hex;
  std::transform(a.begin(), a.end(), a.begin(), [](unsigned char c){ return std::tolower(c); });
  std::transform(b.begin(), b.end(), b.begin(), [](unsigned char c){ return std::tolower(c); });
  return a == b ? Verdict::Match : Verdict::Mismatch;
}

}  // namespace sigcheck

// =========================================================== demo ============
// 演示：verify 一个 APK 的签名指纹，和期望值比；不符就触发惩罚。
//   用法： sigcheck <apk路径> <期望的tbsCertificate-SHA256指纹(hex)>
// 期望指纹怎么来的：正版包首次跑时把 observed 记下来当作 expected 硬编码进去即可。
int main(int argc, char** argv) {
  if (argc < 3) {
    std::printf("用法: %s <apk> <expected-tbsCert-sha256-hex>\n", argv[0]);
    std::printf("(先随便填一个期望值跑一遍，把打印出来的 observed 当 expected 再跑，即可看到 Match)\n");
    return 2;
  }
  std::string observed;
  auto v = sigcheck::verify_apk_signature(argv[1], argv[2], nullptr, &observed);
  std::printf("[sigcheck] observed tbsCert fingerprint = %s\n", observed.c_str());
  switch (v) {
    case sigcheck::Verdict::Match:       std::printf("[sigcheck] MATCH: 签名没被换过。\n"); return 0;
    case sigcheck::Verdict::Unavailable: std::printf("[sigcheck] UNAVAILABLE: 证书读不到，不惩罚(不误伤)。\n"); return 0;
    case sigcheck::Verdict::Mismatch:    sigcheck::punish_kick_home_and_exit(42);
  }
  return 0;
}
