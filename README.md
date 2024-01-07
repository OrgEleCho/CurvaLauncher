<div align="center">

![Icon](/Assets/Icon128.png)

# CurvaLauncher

✨ *Simple, light, and fast desktop launcher* ✨

</div>

<br />

## Introduction

CurvaLauncher is a simple desktop launcher for Windows. 

- Run applications or programs
- Calculate math expressions
- Get a summary of data
- Translate texts
- ...

<br />

## Installation

1. Download the [Latest release](https://github.com/OrgEleCho/CurvaLauncher/releases).
2. Unzip it, and you will can find `CurvaLauncher.exe` in the directory.
3. Run `CurvaLauncher.exe` and enjoy it!

> Notice: Ensure that the [.NET Desktop Runtime 8.0.0 (x64)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is installed on your computer.

<br />

## Usage

- Use `Alt + Space` to open the launcher
- Input something to get the result
- Use `Up` and `Down` to select the item
- Press `Enter` to invoke the selected item

> Tips: Some invoking result are copied to the clipboard.

<br />

## Plugins

Most of CurvaLauncher's features are available in the form of plugins

### Build-in Plugins

- RunApplication: Input the name of the application to run.
  (Applications in the start menu and desktop are supported)
- RunProgram: Input a command to run.
  (You can configure the include and exclude directories in the settings)
- Calculator: Input a math expression with prefix '=' to calculate.
  (Common math functions are supported, `PI` and `E` are also supported)
- OpenUrl: Input a url to open.
  (Use your default browser)
- Translator: Input `>trans` and a text to translate. 
  (A space character is required between `>trans` and the text, you can also configure the source and target language or switch translation engine in the settings)
- Hashing: Input `#` and a hashing method, then type a text or file path to get summary
  (For example, '#md5 123' or '#sha256 C:\Users\OrgEleCho\Desktop\test.txt'. Now support `md5`, `sha1`, `sha256`, `sha384`, `sha512`)

<br />

### Create your own

1. Create a library, targets to `net8.0-windows`.
2. In the downloaded program directory, you can find `CurvaLauncher.Plugin.dll`
3. Add `CurvaLauncher.Plugin.dll`  as an assembly reference to your project.
4. Create a plugin type that implements the 'ISyncPlugin' or 'IAsyncPlugin' interface
5. Write the plugin logic

> Tip: Synchronous and asynchronous plug-ins indicate whether your plug-in performs queries synchronously or asynchronously. You can choose one of them based on your plug-in logic. Query results are also divided into synchronous and asynchronous, just inherit the corresponding QueryResult.
> 
> For example, a translator plug-in returns immediately after triggering a keyword. It does not require any asynchronous operation, so the plug-in is synchronous. However, when the user presses Enter to perform a translation operation, this process requires a network request. In other words, this plug-in The result is asynchronous, so you should use `ISyncPlugin` and `AsyncQueryResult` to implement this plugin.

<br />

## Thanks

- [Securify.ShellLink](https://github.com/securifybv/ShellLink/): A .NET Class Library for processing ShellLink (LNK) files

<br />

---

<br />

<div align="center">

Preview

![](/Assets/Preview.png)

</div>