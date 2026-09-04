//go:build windows

package main

import "syscall"

// Windows 下把控制台输出改成 UTF-8，双击 exe 也能正确显示中文。
func enableUTF8Console() {
	k := syscall.NewLazyDLL("kernel32.dll")
	k.NewProc("SetConsoleOutputCP").Call(65001)
	k.NewProc("SetConsoleCP").Call(65001)
}
