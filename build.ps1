try {
    # This assumes Visual Studio 2019 is installed in C:. You might have to change this depending on your system.
    $VS_PATH = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise"

    if ( -not (Test-Path "$VS_PATH")) {
        echo "Error: VS_PATH isn't set correctly! Update the variable in build.ps1 for your system."
        echo "... or implement it properly with vswhere and submit a PR. (Please)"
        exit 1
    }

    if ( -not (Test-Path "Thirdparty\ACT\Advanced Combat Tracker.exe" )) {
        echo 'Error: Please run tools\fetch_deps.py'
        exit 1
    }


    if ( -not (Test-Path "Thirdparty\FFXIV_ACT_Plugin\SDK\FFXIV_ACT_Plugin.Common.dll" )) {
        echo 'Error: Please run tools\fetch_deps.py'
        exit 1
    }

    $ENV:PATH = "$VS_PATH\MSBuild\Current\Bin;${ENV:PATH}";
    if (Test-Path "C:\Program Files\7-Zip\7z.exe") {
        $ENV:PATH = "C:\Program Files\7-Zip;${ENV:PATH}";
    }

    echo "==> Building..."

    msbuild -p:Configuration=Release -p:Platform=x64 "OverlayPlugin.sln" -t:Restore
    msbuild -p:Configuration=Release -p:Platform=x64 "OverlayPlugin.sln"
    if (-not $?) { exit 1 }

    echo "==> Building archive..."

    cd out\Release

    if (Test-Path OverlayPlugin) { rm -Recurse OverlayPlugin }
    mkdir OverlayPlugin\libs

    cp @("OverlayPlugin.dll", "OverlayPlugin.dll.config", "README.md", "LICENSE.txt") OverlayPlugin
    cp -Recurse libs\resources OverlayPlugin
    cp -Recurse libs\*.dll OverlayPlugin\libs

    # Translations
    cp -Recurse @("de-DE", "fr-FR", "ja-JP", "ko-KR", "zh-CN") OverlayPlugin
    cp -Recurse @("libs\de-DE", "libs\fr-FR", "libs\ja-JP", "libs\ko-KR", "libs\zh-CN") OverlayPlugin\libs


    $text = [System.IO.File]::ReadAllText("$PWD\..\..\SharedAssemblyInfo.cs");
    $regex = [regex]::New('\[assembly: AssemblyVersion\("([0-9]+\.[0-9]+\.[0-9]+)\.[0-9]+"\)');
    $m = $regex.Match($text);

    if (-not $m) {
        echo "Error: Version number not found in the AssemblyInfo.cs!"
        exit 1
    }

    $version = $m.Groups[1]
    $archive = "..\OverlayPlugin-$version.7z"

    if (Test-Path $archive) { rm $archive }
    cd OverlayPlugin
    & "C:\Program Files\7-Zip\7z.exe" a ..\$archive .
    cd ..

    $archive = "..\OverlayPlugin-$version.zip"

    if (Test-Path $archive) { rm $archive }
    & "C:\Program Files\7-Zip\7z.exe" a $archive OverlayPlugin
} catch {
    Write-Error $Error[0]
}
