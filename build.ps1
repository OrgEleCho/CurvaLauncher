mkdir -Force build
mkdir -Force build/tmp
mkdir -Force build/Plugins

foreach ($proj in ls "src/CurvaLauncher.Plugins.*") {
    if ($proj.Name.Contains("Test")) {
        continue
    }

    dotnet build $proj -c Release -o build/tmp
    copy "build/tmp/$($proj.Name).dll" "build/Plugins/"
}


dotnet publish src/CurvaLauncher -c Release -o build -r win-x64 --self-contained false /p:PublishSingleFile=true

# clean up
rm -Recurse -Force build/tmp
rm build/*.pdb

# Compress-Archive @("build/CurvaLauncher.exe", "build/Plugins") "build/CurvaLauncher.zip"