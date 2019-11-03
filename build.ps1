try {
    if ( -not (Test-Path "Thirdparty\ACT\Advanced Combat Tracker.exe" )) {
        echo 'Error: Please copy "Advanced Combat Tracker.exe" into "Thirdparty\ACT" directory.'
        exit 1
    }


    if ( -not (Test-Path "Thirdparty\FFXIV_ACT_Plugin\SDK\FFXIV_ACT_Plugin.Common.dll" )) {
        echo 'Error: Please unpack the FFXIV ACT SDK into the "Thirdparty\FFXIV_ACT_Plugin\SDK" directory.'
        exit 1
    }

    # This assumes Visual Studio 2019 is installed in C:. You might have to change this depending on your system.
    $ENV:PATH = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin;C:\Program Files\7-Zip;${ENV:PATH}";

    echo "==> Building..."

    msbuild -p:Configuration=Release -p:Platform=x64 "OverlayPlugin.sln"
    if (-not $?) { exit 1 }

    echo "==> Building archive..."

    cd out\Release

    rm -Recurse resources
    mv libs\resources .

    $text = [System.IO.File]::ReadAllText("$PWD\..\..\OverlayPlugin\Properties\AssemblyInfo.cs");
    $regex = [regex]::New('\[assembly: AssemblyVersion\("([0-9]+\.[0-9]+\.[0-9]+)\.[0-9]+"\)');
    $m = $regex.Match($text);

    if (-not $m) {
        echo "Error: Version number not found in the AssemblyInfo.cs!"
        exit 1
    }

    $version = $m.Groups[1]
    $archive = "..\OverlayPlugin-$version.7z"

    if (Test-Path $archive) { rm $archive }
    7z a $archive "-x!*.xml" "-x!*.pdb" OverlayPlugin.dll OverlayPlugin.dll.config resources zh-CN README.md LICENSE.txt libs\ja-JP libs\ko-KR libs\zh-CN libs\*.dll

    $archive = "..\OverlayPlugin-$version.zip"

    if (Test-Path $archive) { rm $archive }
    7z a $archive "-x!*.xml" "-x!*.pdb" OverlayPlugin.dll OverlayPlugin.dll.config resources zh-CN README.md LICENSE.txt libs\ja-JP libs\ko-KR libs\zh-CN libs\*.dll
} catch {
    Write-Error $Error[0]
}
