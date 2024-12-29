# usage: release.ps1 <version>
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
Remove-Item -Force -Recurse -Path .\$outputFolder\
New-Item -ItemType Directory -Force -Path .\$outputFolder\

# CLI
$operatingsystem.GetEnumerator() | ForEach-Object{
    $rid = $_.Value;
    $name = $_.Key;
    dotnet publish .\CLI\CLI.csproj --runtime $rid --configuration Release --self-contained=true
    Compress-Archive -Path .\CLI\bin\Release\net8.0\$rid\publish\* -DestinationPath .\$outputFolder\VelvetBeautifier.CLI.$Version.$name.zip
}
# GUI
$operatingsystem.GetEnumerator() | ForEach-Object{
    $rid = $_.Value;
    $name = $_.Key;
    dotnet publish .\GUI\GUI.csproj --runtime $rid --configuration Release --self-contained=true
    Compress-Archive -Path .\GUI\bin\Release\net8.0\$rid\publish\* -DestinationPath .\$outputFolder\VelvetBeautifier.GUI.$Version.$name.zip
}