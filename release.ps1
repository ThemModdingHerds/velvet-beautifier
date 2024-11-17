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
$guiframework = @{
    Windows = "Wpf"
    Linux = "Gtk"
    MacOS = "Mac"
}
$guinet = @{
    Windows = "net8.0-windows"
    Linux = "net8.0"
    MacOS = "net8.0"
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
# GUI
$operatingsystem.GetEnumerator() | ForEach-Object{
    $rid = $_.Value;
    $name = $_.Key;
    $gui = $guiframework[$name]
    $net = $guinet[$name]
    dotnet build .\GUI\GUI.$gui\GUI.$gui.csproj --runtime $rid --configuration Release
    Compress-Archive -Path .\GUI\GUI.$gui\\bin\Release\$net\$rid\* -DestinationPath .\$outputFolder\VelvetBeautifier.GUI.$Version.$name.zip
}