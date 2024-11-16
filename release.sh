#!/bin/bash

# usage: release.sh <version>
VERSION="${1:-0.0.0}"

# Define operating systems and their runtimes
declare -A OPERATINGSYSTEM=(
    [Windows]="win-x64"
    [Linux]="linux-x64"
    [MacOS]="osx-x64"
)

# Create output folder if it doesn't exist
OUTPUT_FOLDER="release"
rm -rf "./$OUTPUT_FOLDER"
mkdir -p "./$OUTPUT_FOLDER"

# CLI
for NAME in "${!OPERATINGSYSTEM[@]}"; do
    RID="${OPERATINGSYSTEM[$NAME]}"
    dotnet build ./CLI/CLI.csproj --runtime "$RID" --configuration Release
    zip -r "./$OUTPUT_FOLDER/VelvetBeautifier.CLI.$VERSION.$NAME.zip" "./CLI/bin/Release/net8.0/$RID/"
done
