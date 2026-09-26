# 1.2 作业：把靶子游戏装到手机上

## 题干

这一课的作业不是答题——是**把靶子游戏真的装到你的设备上**。批改器直接探你的手机，装上了就过。

要准备好三项（都是必需项）：

- **adb 可用** —— `adb version` 能跑（见 0.0）。
- **有设备在线** —— `adb devices` 里有一台状态是 `device`。
- **目标游戏已安装** —— 设备上装了靶子游戏。两个靶子**装任一即可**，批改都收：
  - **CompleteDemo**（`com.course.demo`，本章正文用的完整靶）：
    ```bash
    adb install -r ../demo/build/CompleteDemo.apk
    ```
  - **Vampire**（`com.course.vampire`，进阶实战靶）：
    ```bash
    adb install -r ../demo/VampireSurvivors/build/VampireSurvivors.apk
    ```

  > 路径相对本章目录 `p1-02-env-and-target/`（正文第二节装的就是 `CompleteDemo.apk`）。

## 怎么交

在塔里点这个节点 → **✍ 做作业** → **▶ 运行环境检查**。三项全绿 → 自动点亮过关。
