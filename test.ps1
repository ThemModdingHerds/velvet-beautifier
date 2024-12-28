$executable = ".\CLI\bin\Debug\net8.0\VelvetBeautifier.CLI.exe"
$appdata = "$ENV:LOCALAPPDATA\velvetbeautifier"
$mods = "$appdata\mods"
# reset
& $executable "--reset" "--force"
& $executable
# create mods
& $executable "--create" "balls"
# install mods
& $executable "--install" "https://gamebanana.com/mods/485706"
# list mods
& $executable "--list"
# apply
& $executable "--apply"
# revert
& $executable "--revert"