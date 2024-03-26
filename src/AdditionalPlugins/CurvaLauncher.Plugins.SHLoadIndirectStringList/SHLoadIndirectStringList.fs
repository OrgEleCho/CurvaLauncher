namespace CurvaLauncher.Plugins.SHLoadIndirectStringList
open CurvaLauncher.Plugins
open CurvaLauncher
open System.Runtime.InteropServices
open System 
open Microsoft.FSharp.NativeInterop
open System.Reflection
open System.IO
open System.Linq
open Microsoft.Win32

#nowarn "9" // NativePtr

// 资源项
type ResourceItem =
    {
        PackageName: string // Registry SubKey
        IdentityName: Option<string> // Package/Identity/@Name
        ResourceNames: {| Name:string; FullName:string |} array // 
    }

// 查询结果
type public StringQueryResult(title, desc, weight) = 
    interface IQueryResult with
        member this.Title: string = title
        member this.Description: string = desc
        member this.Weight: float32 = weight
        member this.Icon: Windows.Media.ImageSource = null

// 插件
type public SHLoadIndirectStringPlugin(context: CurvaLauncherContext) = 
    inherit SyncPlugin(context)

    static do
        AppDomain.CurrentDomain.add_AssemblyResolve(fun _ args ->
            let asmName = new AssemblyName(args.Name)
            let dllName = "FSharp.Core.dll"
            if asmName.Name = "FSharp.Core" then
                [|
                    (Path.Combine(AppContext.BaseDirectory, dllName))
                    (Path.Combine(AppContext.BaseDirectory, "Libraries", dllName));
                    (Path.Combine(Directory.GetCurrentDirectory(), dllName))
                |].Where(File.Exists).First() |> Assembly.LoadFrom
            else
                null
        )

    [<DllImport("shlwapi.dll", EntryPoint = "SHLoadIndirectString", CharSet = CharSet.Unicode, ExactSpelling = true)>]
    static extern uint SHLoadIndirectString(string pszSource, char& pszOutBuf, int cchOutBuf, nativeint ppvReserved)

    static let GetIndirectString (str: string) : ValueOption<string> =
        let trim = str.AsSpan().Trim()
        if trim.StartsWith("@{") && trim.EndsWith("}") 
        then
            let ptr = NativePtr.stackalloc 1024;
            let buf = NativePtr.toByRef ptr;
            if SHLoadIndirectString(new string(trim), &buf, 1024, 0) = 0u
            then ValueSome(new string(ptr))
            else ValueNone
        else
            ValueNone

    static let GetUwpResourceStrings() = 
        use packagesKey = Registry.ClassesRoot.OpenSubKey("Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\Repository\Packages")
        let names = packagesKey.GetSubKeyNames()
        seq {
            for packageName in names do 
                let manifestFile = Path.Combine(string (packagesKey.OpenSubKey(packageName).GetValue("PackageRootFolder")), "AppxManifest.xml")
                let def() = { PackageName = packageName; ResourceNames = Array.empty; IdentityName = None }
                if File.Exists(manifestFile) then
                    try
                        let xml = Xml.XmlDocument()
                        xml.Load(manifestFile)
                        let ns = Xml.XmlNamespaceManager(xml.NameTable)
                        ns.AddNamespace("ns", "http://schemas.microsoft.com/appx/manifest/foundation/windows10")
                        ns.AddNamespace("uap", "http://schemas.microsoft.com/appx/manifest/uap/windows10")

                        let properties = xml.SelectSingleNode("/ns:Package/ns:Properties", ns)
                        let identity = xml.SelectSingleNode("/ns:Package/ns:Identity", ns)
                        let identityName = identity.Attributes["Name"].Value
                        let start = "ms-resource:"
                    
                        let r = seq {
                            let resources = 
                                seq {
                                    properties.SelectSingleNode("ns:DisplayName/text()", ns)
                                    properties.SelectSingleNode("ns:Description/text()", ns)
                                    properties.SelectSingleNode("ns:PublisherDisplayName/text()", ns)
                                }
                                |> Seq.map (fun node -> ValueOption.ofObj node)
                                |> Seq.map (fun nodeOpt -> nodeOpt |> ValueOption.bind (fun v -> (ValueOption.ofObj v.Value)))
                                |> Seq.where _.IsSome
                                |> Seq.distinct

                            for node in resources do
                                match node with
                                | ValueSome res when res.StartsWith(start) -> 
                                    let fullName = 
                                        if node.Value.StartsWith("ms-resource://") then node.Value
                                        else sprintf "@{%s?ms-resource://%s/Resources/%O}" packageName identityName (res.AsMemory start.Length)
                                    {| Name = node.Value; FullName = fullName |}
                                | _ -> ()
                        }
                        yield { PackageName = packageName; ResourceNames = r |> Seq.toArray; IdentityName = Some(identityName) }
                    with
                    | _ -> yield def()
                else
                    yield def()
        } |> Seq.toArray

    let mutable cache : ResourceItem[] = [||]

    let GetCompletion (str: string) = 
        seq {
            for res in cache do
                let mutable flag = false
                for item in res.ResourceNames do
                    if item.FullName.StartsWith(str) then
                        flag <- true
                        yield item.FullName
            
                if flag = false then
                    if res.PackageName.AsSpan().StartsWith(str.AsSpan(2))
                    then yield res.PackageName |> sprintf "@{%s"
                    else ()
        }

    override this.get_Name() = "IndirectString查询"
    override this.get_Description() = "查询SHLoadIndirectString的资源字符串"
    override this.get_Icon() = null

    override this.Initialize() =
        base.Initialize()
        let resources = GetUwpResourceStrings()
        cache <- resources
        #if DEBUG
        for res in resources do
            let reses = res.ResourceNames |> Seq.map _.FullName
            sprintf "%s   ->   %s" res.PackageName (String.Join(" | ", reses))
                |> System.Diagnostics.Debug.WriteLine
        #endif

    override this.Finish() =
        base.Finish()

    override this.Query(query: string) : IQueryResult seq = 
        let trim = query.AsSpan().Trim()
        if trim.StartsWith("@{")
        then
            seq {
                match GetIndirectString(query) with
                    | ValueSome str -> yield StringQueryResult(query, str.Trim(), 1.0f)
                    | _ -> yield! GetCompletion(query) |> Seq.map (fun v -> StringQueryResult(v, "", 0.9f) :> IQueryResult)
            }
        else []
