# Velvet Beautifier

![Icon](./assets/icon.png)

[![Build (Debug)](https://github.com/ThemModdingHerds/velvet-beautifier/actions/workflows/build.yml/badge.svg)](https://github.com/ThemModdingHerds/velvet-beautifier/actions/workflows/build.yml)
![GitHub License](https://img.shields.io/github/license/ThemModdingHerds/velvet-beautifier)

Mod Loader/Tool for Z-Engine games

- [GameBanana link (Them's Fightin' Herds)][gamebanana-link]

## Features

- extract/create `.tfhres`/`.gfs` files
- easy way of modifying `.tfhres`/`.gfs` files
- install mods via:
  - files and links (`.zip`/`.7z`,/`.rar`/`.gfs`)
  - folders
  - GameBanana links/ids
  - URI Scheme
    - for GameBanana: `velvetbeautifier:https://gamebanana.com/mmdl/<id>,Mod,<mod-id>` [Example](velvetbeautifier:https://gamebanana.com/mmdl/1108068,Mod,485712)
    - for anything else: `velvetbeautifier:<url/file/folder>`

### Possible Features

- custom Stages (levels)
- custom Pixel Lobbies

### Planned features

- modding `.tfhres` a little bit easier

## Usage

### How do I use this?

[Configurations (config.json)](CONFIG.md)

![CLI Icon](./assets/inverted_icon_medium.png)
[The Command Line (VelvetBeautifier.CLI)](./CLI.md)  
![GUI Icon](./assets/icon_medium.png)
[The Graphical User Interface (VelvetBeautifier.GUI)](GUI.md)

### How do I make mods?

[here][guide-path]

### How does this work?

[here][process-path]

## Frequently asked questions

Q: _yo, you can add custom stages?_  
A: yes you can check [this](GUIDE.md#level-packs) guide out

Q: _when is the next update?_  
A: when I can work, I am a busy man just like you

Q: _can I modify base assets?_  
A: yes but I hope you know what you're doing

Q: _can I get banned for using this?_  
A: maybe if you're unlucky or stupid like me but of course this is against their TOS so yeah...

Q: _I modified a Pixel lobby but I am bugging out on servers_  
A: you've only changed the lobby on your side, the server should also have the custom lobby installed

Q: _I installed a mod but nothing happenend after applying_  
A: Did you enable the mod?

Q: _What's the `backup` folder and why is it taking so much space?_  
A: these are backups of games files, required for modding the game, if there's one missing, then you'll have to redownload the game (it means you modified it already without this tool)

## 3rd-Party Libraries

- [Gameloop.Vdf][gameloop-vdf-library-link] (MIT License) for reading Steam related files
- [SharpCompress][sharpcompress-library-link] (MIT License) for extracting compressed files
- [ThemModdingHerds.GFS][gfs-library-link] (MIT License) for creating/modifying `.gfs` files
- [ThemModdingHerds.TFHRES][tfhres-library-link] (MIT License) for creating/modifying `.tfhres` files

## Building

You need the .NET 8.0 SDK, open the solution and build. That's it

## Disclaimer

To make this clear, some terms have a strict definition:  
`We` is defined as:

- this software (Velvet Beautifier)
- me (N1ghtTheF0x)
- any contributer on this repository

_We_ have **no relation** to Mane6/Modus/Them's Fightin' Herds, all assets used here are from the game and fall under their copyright.  
_We_ are **not responsible** for you getting banned from online play if you use any mods that might change online gameplay/features.  
_We_ do **not support** piracy/cheating/hacking or any kind of harm to the online community

## License

This tool is licensed under the [GPL-3.0 license][license-path]

[license-path]: ./LICENSE
[guide-path]: ./GUIDE.md
[gamebanana-link]: https://gamebanana.com/tools/15674
[process-path]: PROCESS.md
[gameloop-vdf-library-link]: https://github.com/shravan2x/Gameloop.Vdf
[sharpcompress-library-link]: https://github.com/adamhathcock/sharpcompress
[tfhres-library-link]: https://github.com/ThemModdingHerds/tfhres
[gfs-library-link]: https://github.com/ThemModdingHerds/gfs
