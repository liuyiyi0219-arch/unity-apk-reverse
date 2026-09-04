// 1.1 作业 · 批改器：不答题——去 APK 的五层关键目录各捡一个碎片，按层序拼成 flag。
// 批改器打开 材料/starforge.apk，从五个已知路径读碎片、自己拼出期望 flag，和你提交的比。
package main

import (
	"archive/zip"
	"fmt"
	"io"
	"os"
	"strings"
)

// _gen 从这里抓节点 flag；也是碎片按层序拼出来的正解。
const FLAG = "FLAG{urev-1.1-elf-metadata-luac-assetbundle-dex}"

// 五层 → 该层碎片在 APK 里的路径（顺序就是拼 flag 的顺序）。
var layers = []struct{ layer, dir, path string }{
	{"native", "lib/<abi>/", "lib/arm64-v8a/_layer_native.txt"},
	{"C#/il2cpp", "assets/bin/Data/Managed/Metadata/", "assets/bin/Data/Managed/Metadata/_layer_csharp.txt"},
	{"脚本", "assets/Scripts/", "assets/Scripts/_layer_script.txt"},
	{"资源", "assets/bin/Data/", "assets/bin/Data/_layer_resources.txt"},
	{"Java", "包根（classes.dex 旁）", "_layer_java.txt"},
}

const apkPath = "材料/starforge.apk"

func main() {
	pieces, err := readPieces()
	if err != nil {
		fmt.Println("读不到", apkPath, "：", err)
		fmt.Println("请在 作业/ 目录下跑（材料/starforge.apk 要在旁边）。")
		os.Exit(2)
	}
	expected := "FLAG{urev-1.1-" + strings.Join(pieces, "-") + "}"

	if len(os.Args) < 2 {
		fmt.Println("== 1.1 作业：拼出 APK 五层碎片 ==")
		fmt.Println("解包 材料/starforge.apk，去每层关键目录各找一个 _layer_*.txt，")
		fmt.Println("按 native→C#→脚本→资源→Java 顺序把碎片拼进 flag，再交上来：")
		fmt.Println("  check.exe FLAG{urev-1.1-<native>-<C#>-<脚本>-<资源>-<Java>}")
		os.Exit(2)
	}
	got := strings.TrimSpace(os.Args[1])

	if got == expected {
		fmt.Println("五层碎片拼对了 ✅ —— 你已经能在 APK 里认出每一层的家。")
		fmt.Println(expected)
		return
	}

	// 给方向提示：拆出学生的碎片，和期望逐层比。
	fmt.Println("✘ 还不对。逐层看看：")
	gotBody := strings.TrimPrefix(strings.TrimSuffix(got, "}"), "FLAG{urev-1.1-")
	gotPieces := strings.Split(gotBody, "-")
	for i, L := range layers {
		want := pieces[i]
		var g string
		if i < len(gotPieces) {
			g = gotPieces[i]
		}
		if g == want {
			fmt.Printf("  ✔ 第%d层 %s\n", i+1, L.layer)
		} else {
			fmt.Printf("  ✘ 第%d层 %s —— 去 %s 里找 _layer_*.txt（你填的是 %q）\n", i+1, L.layer, L.dir, g)
		}
	}
	fmt.Println("顺序是 native→C#→脚本→资源→Java，别漏 FLAG{urev-1.1-…} 的壳。")
	os.Exit(1)
}

// 从 APK 里读五层碎片的 codeword（碎片文件内容形如「…碎片：elf」）。
func readPieces() ([]string, error) {
	r, err := zip.OpenReader(apkPath)
	if err != nil {
		return nil, err
	}
	defer r.Close()
	byName := map[string]*zip.File{}
	for _, f := range r.File {
		byName[f.Name] = f
	}
	var out []string
	for _, L := range layers {
		f := byName[L.path]
		if f == nil {
			return nil, fmt.Errorf("APK 里缺 %s", L.path)
		}
		rc, err := f.Open()
		if err != nil {
			return nil, err
		}
		b, _ := io.ReadAll(rc)
		rc.Close()
		out = append(out, codeword(string(b)))
	}
	return out, nil
}

// 取第一行「碎片：xxx」冒号后的词。
func codeword(s string) string {
	line := s
	if i := strings.IndexByte(s, '\n'); i >= 0 {
		line = s[:i]
	}
	if i := strings.Index(line, "："); i >= 0 {
		return strings.TrimSpace(line[i+len("："):])
	}
	return strings.TrimSpace(line)
}
