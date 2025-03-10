# Process

This is how Velvet Beautifier works

## When running executable

- delete any temp folders
- create data folder if not exists
- migrate from older version if needed
- load or create config file
- check version of config and tool
  - if new version has been installed the [setup](#when-setup) is executed
- verify client/server paths
- if first time lanching then [setup](#when-setup)
- handle command line process

## when setup

- get game file information from external repo
- init Mod database
- [backup game files](#when-creating-backups)

## when resetting

- delete backups
- delete mods
- delete configuration
- delete local vanilla levels

## when creating backups

- create backup of file
- if file has been tampered with, warn user

## when reverting

- revert game file to its orignal state

## when applying

Mods are applied in this order:

- [Reverge Packages](#general-gfstfhres) (`.gfs`)
- [Levels](#levels) (requires Reverge Package `levels.gfs`)
- [TFH Resources](#general-gfstfhres) (`.tfhres`)
- [Game News](#news) (`GameNews.ini` and `news_images` in `Scripts` folder)

### General (`.gfs`/`.tfhres`)

- check if there are any mods with required files
- [revert file](#when-reverting)
- open orignal file
- create copy of orignal file
- go through each mod's file
  - add the mod's file content to copy or replace
- overwrite game file with copy

### Levels

- check if `levels.gfs` is valid and do any mods with levels exist
- create level pack from `levels.gfs` [that was modified](#general-gfstfhres)
- go through each mod's levels and add them to the level pack
- override modded `levels.gfs` with new level pack

### News

- check if `GameNews.ini` exists
- create outdated news page if Velvet Beautifier is outdated
- create mods stats news page
- add news pages from orginal file
- override orignal file with modded one
- copy news images to `news_images` folder
