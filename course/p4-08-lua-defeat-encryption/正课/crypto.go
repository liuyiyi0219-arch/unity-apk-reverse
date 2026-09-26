package main

// 真实游戏 Lua 加密里最常见的几种算法（AES-256-CBC / XXTEA / XOR），Go 实现。
// 参数照搬 lua-extract cache 里的真实 recipe：
//   - AES: com.lighthouse.freeplay.ameuno —— key=md5(secret) 的 hex 当 32 字节，iv=md5(ab_name)[8:24]
//   - XOR: com.global.szsslg —— 单字节 XOR，key 在 C# BuildLuaXorKey
//   - XXTEA: 通用 tolua setXXTEAKeyAndSign 形态（key + sign 前缀）

import (
	"crypto/aes"
	"crypto/cipher"
	"crypto/md5"
	"encoding/binary"
	"encoding/hex"
	"fmt"
)

// —— AES-256-CBC-PKCS7，key/iv 从硬编码串派生（ameuno 方案）——
func aesKeyIV(secret, abName string) (key, iv []byte) {
	kh := md5.Sum([]byte(secret))
	key = []byte(hex.EncodeToString(kh[:])) // 32 位小写 hex 的 ASCII → 正好 32 字节 = AES-256 key
	nh := md5.Sum([]byte(abName))
	iv = []byte(hex.EncodeToString(nh[:]))[8:24] // md5(name) hex 的 [8:24] → 16 字节 iv
	return
}
func pkcs7pad(b []byte, bs int) []byte {
	n := bs - len(b)%bs
	for i := 0; i < n; i++ {
		b = append(b, byte(n))
	}
	return b
}
func pkcs7unpad(b []byte) []byte {
	if len(b) == 0 {
		return b
	}
	n := int(b[len(b)-1])
	if n <= 0 || n > len(b) {
		return b
	}
	return b[:len(b)-n]
}
func aesEncrypt(plain []byte, secret, abName string) ([]byte, error) {
	key, iv := aesKeyIV(secret, abName)
	blk, err := aes.NewCipher(key)
	if err != nil {
		return nil, err
	}
	p := pkcs7pad(append([]byte{}, plain...), aes.BlockSize)
	out := make([]byte, len(p))
	cipher.NewCBCEncrypter(blk, iv).CryptBlocks(out, p)
	return out, nil
}
func aesDecrypt(ct []byte, secret, abName string) ([]byte, error) {
	key, iv := aesKeyIV(secret, abName)
	blk, err := aes.NewCipher(key)
	if err != nil {
		return nil, err
	}
	if len(ct)%aes.BlockSize != 0 {
		return nil, fmt.Errorf("密文长度不是块大小整数倍")
	}
	out := make([]byte, len(ct))
	cipher.NewCBCDecrypter(blk, iv).CryptBlocks(out, ct)
	return pkcs7unpad(out), nil
}

// —— XXTEA（Wheeler & Needham），Corona/tolua 常用；key 16 字节，sign 作前缀魔数 ——
const xxteaDelta = 0x9E3779B9

func xxteaKey128(key []byte) [4]uint32 {
	var k [4]uint32
	kb := make([]byte, 16)
	copy(kb, key)
	for i := 0; i < 4; i++ {
		k[i] = binary.LittleEndian.Uint32(kb[i*4:])
	}
	return k
}
// 严格照 Wheeler & Needham 的参考实现 btea（MX 用 e、key 索引 (p&3)^e、v[p]±=MX）
func xxteaEncryptWords(v []uint32, k [4]uint32) {
	n := len(v)
	if n < 2 {
		return
	}
	var y, z, sum uint32
	var e uint32
	var p int
	mx := func() uint32 {
		return ((z>>5 ^ y<<2) + (y>>3 ^ z<<4)) ^ ((sum ^ y) + (k[(uint32(p)&3)^e] ^ z))
	}
	rounds := 6 + 52/n
	z = v[n-1]
	for ; rounds > 0; rounds-- {
		sum += xxteaDelta
		e = (sum >> 2) & 3
		for p = 0; p < n-1; p++ {
			y = v[p+1]
			v[p] += mx()
			z = v[p]
		}
		p = n - 1
		y = v[0]
		v[n-1] += mx()
		z = v[n-1]
	}
}
func xxteaDecryptWords(v []uint32, k [4]uint32) {
	n := len(v)
	if n < 2 {
		return
	}
	var y, z, sum uint32
	var e uint32
	var p int
	mx := func() uint32 {
		return ((z>>5 ^ y<<2) + (y>>3 ^ z<<4)) ^ ((sum ^ y) + (k[(uint32(p)&3)^e] ^ z))
	}
	rounds := 6 + 52/n
	sum = uint32(rounds) * xxteaDelta
	y = v[0]
	for ; rounds > 0; rounds-- {
		e = (sum >> 2) & 3
		for p = n - 1; p > 0; p-- {
			z = v[p-1]
			v[p] -= mx()
			y = v[p]
		}
		p = 0
		z = v[n-1]
		v[0] -= mx()
		y = v[0]
		sum -= xxteaDelta
	}
}
func toWords(b []byte) []uint32 {
	for len(b)%4 != 0 {
		b = append(b, 0)
	}
	w := make([]uint32, len(b)/4)
	for i := range w {
		w[i] = binary.LittleEndian.Uint32(b[i*4:])
	}
	return w
}
func fromWords(w []uint32) []byte {
	b := make([]byte, len(w)*4)
	for i, x := range w {
		binary.LittleEndian.PutUint32(b[i*4:], x)
	}
	return b
}

// sign 作前缀（tolua: 加密文件 = sign + xxtea(原长度4字节 + 明文)）
func xxteaEncrypt(plain, key, sign []byte) []byte {
	body := make([]byte, 4)
	binary.LittleEndian.PutUint32(body, uint32(len(plain)))
	body = append(body, plain...)
	w := toWords(body)
	xxteaEncryptWords(w, xxteaKey128(key))
	return append(append([]byte{}, sign...), fromWords(w)...)
}
func xxteaDecrypt(data, key, sign []byte) ([]byte, error) {
	if len(data) < len(sign) || string(data[:len(sign)]) != string(sign) {
		return nil, fmt.Errorf("sign 前缀不符（不是这套 XXTEA 或 key/sign 错）")
	}
	w := toWords(data[len(sign):])
	xxteaDecryptWords(w, xxteaKey128(key))
	body := fromWords(w)
	if len(body) < 4 {
		return nil, fmt.Errorf("解出内容过短")
	}
	n := binary.LittleEndian.Uint32(body[:4])
	if int(n) > len(body)-4 {
		return nil, fmt.Errorf("长度字段荒谬 %d（key 错）", n)
	}
	return body[4 : 4+n], nil
}

// —— 单字节 XOR ——
func xorByte(b []byte, k byte) []byte {
	out := make([]byte, len(b))
	for i := range b {
		out[i] = b[i] ^ k
	}
	return out
}
