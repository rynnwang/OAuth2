## BuildNuGetPackage.ps1

$assemblyName = "Beyova.OAuth2.Client.dll"
$currentPath =  [System.IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Path)
$specPath = "Beyova.OAuth2.Client.nuspec"

    $assemblyPath = [System.IO.Path]::Combine($currentPath, $assemblyName)
    $assembly = [System.Reflection.Assembly]::LoadFrom($assemblyPath)
    $version = ([System.Diagnostics.FileVersionInfo]::GetVersionInfo($assembly.Location)).ProductVersion

if(![string]::IsNullOrWhiteSpace($specPath) -and ![string]::IsNullOrWhiteSpace($version)){

    $cmdText = [string]::Format("cd /d {0}\n{1} pack `"{0}\{2}`" -Version `"{3}`"",
        #{0}
        $currentPath,
        #{1}
        "nuget.exe",
        #{2}
        "Beyova.OAuth2.Client.nuspec",
        #{3}
        $version)
    
    $msCmd = [System.IO.Path]::Combine($currentPath, "packCmd.bat")
    
    [System.IO.File]::WriteAllText($msCmd, $cmdText, [System.Text.Encoding]::ASCII)
}
