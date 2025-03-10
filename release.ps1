# usage: release.ps1 -Version=<version>
Param
    (
        [parameter(Position=0,Mandatory=$true)]
        [String]
        $Version
    )
$operatingsystem = @{
    Windows = "win-x64"
    Linux = "linux-x64"
    MacOS = "osx-x64"
}
# create output folder if it doesn't exist
$outputFolder = "release"
if(Test-Path -Path $outputFolder)
{
    # remove folder if it exists already for new release
    Remove-Item -Force -Recurse -Path .\$outputFolder\
}
New-Item -ItemType Directory -Force -Path .\$outputFolder\

$operatingsystem.GetEnumerator() | ForEach-Object{
    $rid = $_.Value;
    $name = $_.Key;
    # CLI
    dotnet publish .\CLI\CLI.csproj --runtime $rid --configuration Release --self-contained=true
    Compress-Archive -Path .\CLI\bin\Release\net8.0\$rid\publish\* -DestinationPath .\$outputFolder\VelvetBeautifier.CLI.$Version.$name.zip
}