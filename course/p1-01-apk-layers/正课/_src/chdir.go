package main

import (
	"os"
	"path/filepath"
)

// 演示 exe 装在 <章>/正课/ 下，demo 里的相对路径（../demo、../tools、跨章路径）按"exe 在章节根"设计。
// 启动时 chdir 回章节根，让这些相对路径按原样解析（正课/_src 已是 _ 前缀目录、不打包，本文件无需 _ 前缀）。
func init() {
	if exe, err := os.Executable(); err == nil {
		_ = os.Chdir(filepath.Join(filepath.Dir(exe), ".."))
	}
}
