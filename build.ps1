Write-Output "CurvaLauncher AutoBuild Script"

{
    New-Item -ItemType Directory -Force "build" 
    New-Item -ItemType Directory -Force "build/tmp" 
    New-Item -ItemType Directory -Force "build/Plugins" 
} > $null

Write-Output "Building app"

dotnet publish src/CurvaLauncher -c Release -o build -r win-x64 --self-contained false /p:PublishSingleFile=true

Write-Output "Building plugins"

foreach ($proj in Get-ChildItem "src/CurvaLauncher.Plugins.*") {
    if ($proj.Name.Contains("Test")) {
        continue
    }

    dotnet build $proj -c Release -o build/tmp
    Copy-Item "build/tmp/$($proj.Name).dll" "build/Plugins/"
}

# clean up
Remove-Item -Recurse -Force build/tmp
Remove-Item build/*.pdb

# Compress-Archive @("build/CurvaLauncher.exe", "build/Plugins") "build/CurvaLauncher.zip"