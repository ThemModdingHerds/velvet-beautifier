# VelvetBeautifier

![icon-path]

Them's Fighting' Herds Mod Loader/Tool

- [GameBanana link][gamebanana-link]

## Features

- extract/create `.tfhres`/`.gfs` files
- easy way of modifying `.tfhres`/`.gfs` files

### Possible Features

- custom Stages
- custom Lobbies
- custom Color palettes

### Planned features

- GUI program
- modding `.tfhres` a little bit easier

## Usage

### How do I use this?

[Configurations (config.json)](CONFIG.md)  
[The Command Line (VelvetBeautifier.CLI.exe)](./CLI.md)  
The Graphical Userinterface does not exist yet

### How do I make mods?

[here][guide-path]

### How does this work?

[here][process-path]

## Frequently asked questions

Q: yo, you can add custom stages?  
A: yes but you'll have to modify the `worlds.ini` and other mods might cause a conflict but I will work on making some kind of sub manager for custom stages

Q: when is the next update?  
A: when I can work, I can't work 24/7  

Q: can I modify base assets?  
A: yes but I hope you know what you're doing

Q: can I get banned for using this?  
A: maybe if you're unlucky or stupid like me but of course this is against their TOS so yeah...

Q: I modified a lobby but I am bugging out on servers  
A: you've only changed the lobby on your side, the server should also have the custom lobby installed

Q: I added a mod but nothing happenend after applying  
A: Did you enable the mod?

## Building

You need the .NET 8.0 SDK and Visual Studio, open the solution and build. That's it

## License

This tool is licensed under the [GPL-3.0 license][license-path]

[license-path]: ./LICENSE
[icon-path]: ./assets/icon_small.ico
[guide-path]: ./GUIDE.md
[gamebanana-link]: https://gamebanana.com/tools/15674
[process-path]: PROCESS.md
