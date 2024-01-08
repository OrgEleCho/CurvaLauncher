<div align="center">

![图标](/Assets/Icon128.png)

# CurvaLauncher

✨ *简单、轻量、快速的桌面启动器* ✨

[![图标](https://img.shields.io/github/license/OrgEleCho/CurvaLauncher
)](LICENSE.txt) [![版本](https://img.shields.io/github/v/release/OrgEleCho/CurvaLauncher?include_prereleases
)](https://github.com/OrgEleCho/CurvaLauncher/releases) / [![EN-US](https://img.shields.io/badge/EN-US-blue)](README.md) [![ZH-Hans](https://img.shields.io/badge/中文-简体-red)](README.zh.md)


</div>

<br />

## 介绍

CurvaLauncher 是一个简单的 Windows 桌面启动器。

- 运行应用程序或程序
- 计算数学表达式
- 获取数据摘要
- 翻译文本
- ...

<br />

## 安装

1. 下载 [最新版本](https://github.com/OrgEleCho/CurvaLauncher/releases)。
2. 解压缩，在目录中你可以找到 `CurvaLauncher.exe`。
3. 运行 `CurvaLauncher.exe`，享受使用！

> 注意：请确保您的计算机上已安装 [.NET Desktop Runtime 8.0.0 (x64)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)。

<br />

## 使用

- 使用 `Alt + Space` 打开启动器
- 输入内容以获取结果
- 使用 `Up` 和 `Down` 选择项目
- 按 `Enter` 调用所选项目

> 提示：某些调用结果是以复制到剪贴板的形式提供的。

<br />

## 插件

CurvaLauncher 的大多功能都以插件形式提供

### 内建插件

- RunApplication：输入要运行的应用程序名称。
  (支持开始菜单和桌面中的应用程序)
- RunProgram：输入要运行的命令。
  (您可以在设置中配置包含和排除的目录)
- Calculator：输入以 '=' 为前缀的数学表达式进行计算。
  (支持常见数学函数，还支持 `PI` 和 `E` 常量)
- OpenUrl：输入要打开的 URL。
  (使用您的默认浏览器)
- Translator：输入 `>trans` 和要翻译的文本。
  (在 `>trans` 和文本之间需要一个空格字符，您还可以在设置中配置源语言和目标语言，或切换翻译引擎)
- Hashing：输入 `#` 和一些哈希方法，然后输入文本或文件路径以获取摘要
  (例如，`#md5 123` 或 `#sha256 C:\Users\OrgEleCho\Desktop\test.txt`。现在支持 MD5、SHA1、SHA256、SHA384、SHA512)

<br />

### 创建自己的插件

1. 克隆本仓库代码.
2. 创建一个新项目, 目标框架为 `net8.0-windows`.
3. 将 `CurvaLauncher.Plugin` 添加到项目引用中.
4. 创建一个插件类, 实现 'ISyncPlugin' 或 'IAsyncPlugin' 接口.
5. 实现接口成员, 并编写主逻辑.

> 提示: 同步和异步插件表示你的插件是以同步还是异步方式进行查询, 你可以根据你的插件逻辑选择其中之一。查询结果也分同步和异步, 继承对应的 QueryResult 即可.
> 
> 举例, 一个翻译插件, 触发关键词后立即返回, 它不需要任何异步操作, 所以该插件是同步的, 但用户按下 Enter 进行翻译操作, 这个过程需要进行网络请求, 也就是说, 这个插件的结果是异步的, 所以你应该使用 `ISyncPlugin` 和 `AsyncQueryResult` 来实现这个插件.

<br />

## 感谢

- [Securify.ShellLink](https://github.com/securifybv/ShellLink/): 用于处理 ShellLink (LNK) 文件的 .NET 类库

<br />

---

<br />

<div align="center">

预览

![](/Assets/Preview2.png)


![](/Assets/Preview3.png)

</div>
