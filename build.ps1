param (
    [switch]$ci = $false
)

try {
    # This assumes Visual Studio 2022 is installed in C:. You might have to change this depending on your system.
    # $DEFAULT_VS_PATH = "C:\Program Files\Microsoft Visual Studio\2022\Enterprise"
    # $VS_PATH = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise"
    $VS_PATH = .\vswhere.exe -prerelease -latest -property installationPath

    if ( -not (Test-Path "$VS_PATH")) {
        echo "Error: VS_PATH isn't set correctly! Update the variable in build.ps1 for your system."
        echo "... or implement it properly with vswhere and submit a PR. (Please)"
        exit 1
    }

    $ENV:PATH = "$VS_PATH\MSBuild\Current\Bin;${ENV:PATH}";
    if (Test-Path "C:\Program Files\7-Zip\7z.exe") {
        $ENV:PATH = "C:\Program Files\7-Zip;${ENV:PATH}";
    }

    if ($ENV:CI -eq "true" -and $ENV:GITHUB_RUN_ID -ne $null) {
        echo "==> Running as a GitHub Action, pre-running clientstructs scripts"

        .\tools\strip-clientstructs.ps1
    }

    if ($ci) {
        echo "==> Continuous integration flag set. Building Debug..."
        dotnet publish -c debug
        
        if (-not $?) { exit 1 }
    }

    echo "==> Building..."

    dotnet publish -c release
    
    if (-not $?) { exit 1 }

    echo "==> Building archive..."

    cd out\Release

    if (Test-Path OverlayPlugin) { rm -Recurse OverlayPlugin }
    mkdir OverlayPlugin\libs

    cp @("OverlayPlugin.dll", "OverlayPlugin.dll.config", "README.md", "LICENSE.txt") OverlayPlugin
    cp -Recurse libs\resources OverlayPlugin
    cp -Recurse libs\*.dll OverlayPlugin\libs
    del OverlayPlugin\libs\CefSharp.*

    # Translations
    cp -Recurse @("de-DE", "fr-FR", "ja-JP", "ko-KR", "zh-CN") OverlayPlugin
    cp -Recurse @("libs\de-DE", "libs\fr-FR", "libs\ja-JP", "libs\ko-KR", "libs\zh-CN") OverlayPlugin\libs


    [xml]$csprojcontents = Get-Content -Path "$PWD\..\..\Directory.Build.props";
    $version = $csprojcontents.Project.PropertyGroup.AssemblyVersion;
    $version = ($version | Out-String).Trim()
    $archive = "..\OverlayPlugin-$version.7z"

    if (Test-Path $archive) { rm $archive }
    cd OverlayPlugin
    & 7z a ..\$archive .
    cd ..

    $archive = "..\OverlayPlugin-$version.zip"

    if (Test-Path $archive) { rm $archive }
    7z a $archive OverlayPlugin

    cd ..\..
} catch {
    Write-Error $Error[0]
}
