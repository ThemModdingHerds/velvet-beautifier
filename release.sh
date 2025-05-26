#!/bin/bash

# usage: ./release.sh <version>

set -e

if [ -z "$1" ]; then
    echo "Usage: $0 <version>"
    exit 1
fi

VERSION="$1"

declare -A OPERATING_SYSTEM=(
    ["Windows"]="win-x64"
    ["Linux"]="linux-x64"
    ["MacOS"]="osx-x64"
)

OUTPUT_FOLDER="release"

# Remove output folder if it exists
if [ -d "$OUTPUT_FOLDER" ]; then
    rm -rf "$OUTPUT_FOLDER"
fi

# Create output folder
mkdir -p "$OUTPUT_FOLDER"

for OS in "${!OPERATING_SYSTEM[@]}"; do
    RID="${OPERATING_SYSTEM[$OS]}"

    # Publish the .NET project
    dotnet publish ./CLI/CLI.csproj --runtime "$RID" --configuration Release --self-contained true

    # Create a ZIP archive
    ZIP_NAME="$OUTPUT_FOLDER/VelvetBeautifier.CLI.$VERSION.$OS.zip"
    PUBLISH_DIR="./CLI/bin/Release/net8.0/$RID/publish"
    7z a -r "$ZIP_NAME" "$PUBLISH_DIR"/*
done
