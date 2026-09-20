// hook_sig.js —— 绕过 CompleteDemo 的签名校验（2.4）
//
// 原理：游戏的 SignatureCheck 通过 PackageManager.getPackageInfo(pkg, GET_SIGNATURES)
// 取签名、算 SHA1、比对硬编码的原始证书。重打包重签后签名变了 → 被判篡改 → 弹窗退出。
// 这个 hook 让 getPackageInfo 永远返回【原始签名】，于是校验永远通过（ApkSignatureKiller 思路）。
//
// 跑（frida 环境就绪时）：
//   frida -U -f com.course.demo -l hook_sig.js
//   （本机设备 frida 不稳时，可改用 frida-gadget 注入，见课文）

// 原始签名证书 DER（从官方包 META-INF/*.RSA 抠出，744 字节）
var ORIG_CERT_HEX = "308202e4308201cc020101300d06092a864886f70d01010b050030373116301406035504030c0d416e64726f69642044656275673110300e060355040a0c07416e64726f6964310b30090603550406130255533020170d3234313132393033353135385a180f32303534313132323033353135385a30373116301406035504030c0d416e64726f69642044656275673110300e060355040a0c07416e64726f6964310b300906035504061302555330820122300d06092a864886f70d01010105000382010f003082010a02820101009c758e56d950513f1a1e49a783fd241d9711d15a2ee432e785987c2a50916ef18924bbe2db36078614b936df890875e1c7115055a92c7828ad7a1d239e46eb4966cdff5be360befc4f74dc8d51eb295ac9a053d2dbcd92ed59f7a338d0bb653b38a546db82fa4141176bb92e00742b53a3a12c1043d60e11cf053002fe64b2b33dc5a305440d93fb12a538f4e4071af4774bb5b96741c3216089d97eb08d3929161af7909a164d733fd3c9769717cd3844d7cc14eee759f583f77d62548af780c69cb98dfbb5eb90e871b60642dd3f3de0f14a02de8c12b7a271076cba5533c1a8b6c1536ecb9833069e5b5b2dd70d27bbcd9f07b17c8ee3ca4cc09a5528ce210203010001300d06092a864886f70d01010b0500038201010093d1c853c5bda419b2ba41a9061780e209764fc1128b9fcd5699546d14effed3b0999db8ee28080b86bd8d405e39ab658ebfa55b8f7d70a1ecd22cf83a5f876c7dcd7d061faedb9376c98f86f7bc5ae0ff3de70fbbe73033ef1e733728ee2abdfc9e78ebf1ca404ef2cf0a6cea16d597597d5e76e1bb90b1ffcafbaadc91976b3f09d7362e527839078bd8662dc289e912ad8ba17729e8fcf1ae6f4ab8dee853b676d339f086377da6e8e44378783853fdba8d339f674482ef12c08680966e4e73393e0fd78500bf57c682842d93b6730b2a7deb5c4a4aba342b73acecf92aced1ea0cccd885caf6f0351b7e53bbb7e6fde31984d2e01780db134bb3c60ff694";

Java.perform(function () {
    var PM = Java.use('android.app.ApplicationPackageManager');
    var Signature = Java.use('android.content.pm.Signature');
    var origSig = Signature.$new(ORIG_CERT_HEX);

    PM.getPackageInfo.overload('java.lang.String', 'int').implementation = function (pkg, flags) {
        var info = this.getPackageInfo(pkg, flags);
        if ((flags & 0x40) !== 0) {                 // GET_SIGNATURES
            try { info.signatures.value = [origSig]; } catch (e) {}
        }
        return info;
    };
    console.log('[hook_sig] getPackageInfo hooked -> 永远返回原始签名，校验必过');
});
