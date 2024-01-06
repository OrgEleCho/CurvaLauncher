<div align="center">

![Icon](/Assets/Icon128.png)

# CurvaLauncher

✨ *Simple, light, and fast desktop launcher* ✨

</div>

<br />

# Introduction

CurvaLauncher is a simple desktop launcher for Windows. 

- Run applications or programs
- Calculate math expressions
- Get a summary of data
- Translate texts
- ...

<br />

# Plugins

All of CurvaLauncher's main features are available in the form of plugins

## Build-in Plugins

- RunApplication: 
- RunProgram
- Calculator
- OpenUrl

<br />

## Create your own

1. Create a library, targets to `net8.0-windows`.
2. In the downloaded program directory, you can find `CurvaLauncher.Plugin.dll`
3. Add `CurvaLauncher.Plugin.dll`  as an assembly reference to your project.
4. Create a plugin type that implements the 'ISyncPlugin' or 'IAsyncPlugin' interface
5. Write the plugin logic

<br />

---

<br />

<div align="center">

![](/Assets/Preview.png)

</div>