# usage: release.ps1 <version>
[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $Version = "0.0.0"
)
$operatingsystem = @{
    Windows = "win-x64"
    Linux = "linux-x64"
    MacOS = "osx-x64"
}
# create output folder if it doesn't exist
$outputFolder = "release"
Remove-Item -Force -Path .\$outputFolder\
New-Item -ItemType Directory -Force -Path .\$outputFolder\

# CLI
$operatingsystem.GetEnumerator() | ForEach-Object{
    $rid = $_.Value;
    $name = $_.Key;
    dotnet build .\CLI\CLI.csproj --runtime $rid --configuration Release
    Compress-Archive -Path .\CLI\bin\Release\net8.0\$rid\* -DestinationPath .\$outputFolder\VelvetBeautifier.CLI.$Version.$name.zip
}