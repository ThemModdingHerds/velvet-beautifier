# VelvetBeautifier

![icon-path]

Them's Fighting' Herds Mod Loader/Tool

- [GameBanana link][gamebanana-link]

## Features

- extract `.tfhres` files
- add/patch files to `.tfhres` files

## Possible Features

- custom GeoBox maps
  - PixelLobbies
  - Story Mode
- custom Stages

## Planned features

- `.gfs` extraction
- GameBanana support?
- modify existing files (patching)

## How does this load mods?

1. checks for folders in `mods` directory
2. check if these folders have a `mod.json` file

## How do I make mods?

[here][guide-path]

## Mod Structure

### Folder Structure

- `mod.json` - contains metadata about the mod
- `<folders-with-tfhres-extension>` - TFH resources to add

### `mod.json` Structure

```json
{
  "id": "com.example.mod",
  "name": "Example Mod",
  "author": "Example Author"
}
```

## Frequently asked questions

Q: yo, you can add custom stages?  
A: not yet, it is possible but I don't have the knowledge of how they work  

Q: when is the next update?  
A: when I can work, I can't work 24/7  

Q: can I modify base assets?  
A: no and for security reasons I won't add this feature

## License

This tool is licensed under the [GPL-3.0 license][license-path]

[license-path]: ./LICENSE
[icon-path]: ./assets/icon_small.ico
[guide-path]: ./GUIDE.md
[gamebanana-link]: https://gamebanana.com/tools/15674
