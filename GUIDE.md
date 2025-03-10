# Guide

## How create a mod

### The `GUI` way

1. Launch `VelvetBeautifier.GUI`, it should look something like this:
    ![Empty GUI](./assets/screenshots/gui-empty.png)
2. Click on `Tools -> Create new mod` or just press `Ctrl + N` on your keyboard, you should see this:
    ![Create Mod GUI](./assets/screenshots/gui-create-mod.png)
3. Fill out the information, click on `Create` and confirm the message box by clicking on `Ok`
4. Your mod should be then on the modlist view
5. You can right click on your mod and then `Open Folder` to open the location of the mod

### The `CLI` way

1. Open up a Terminal where `VelvetBeautifier.CLI` is located
2. Type this command in:

    ```shell
    VelvetBeautifier.CLI --create <mod-id> # replace mod-id with your unique mod identifier
    ```

3. A new folder in `mods` should be called `<mod-id>`

### The manual way

1. Create a folder in the `mods` folder (name it whatever you want)
2. Create a `mod.json` file and fill it with the required information:

    ```json
    {
        "id": "ntf.test.mod", // best id is always com.<your-name>.<mod-name>
        "name": "Test Mod", // Humanreadable name
        "author": "Night The Fox", // Your name
        "desc": "This is a Test Mod", // Some details about the mod (what it adds/changes etc)
        "version": "1.0.0", // it should be always <major>.<minor>.<patch>
        "url": "https://somelink.to/the/mod" // link to your mod download (GameBanana, direct zip download)
    }
    ```

## How to mod stuff

When `<root>` is mentioned, it means your mod folder (`mods/<your-mod-folder>/`)

### Reverge Package files (`*.gfs`)

You can modify the following Reverge Packages:

- `ai-libs.gfs` - contains lua code for AI, wouldn't touch it for now
- `characters-*.gfs` - contains character data, wouldn't touch it for now
- `dev.gfs` - contains some various stuff, don't have a reason to modify this
- `levels.gfs` - contains all the stage data during a fight and some story mode sections, I wouldn't modify this because it's better to create a [Level Pack](#level-packs) instead (that is you want to create new levels)
- `nidra-baihe-texas-art-pt.gfs` - I don't know why this is a seperate thing but the same thing applies to `characters-*.gfs`
- `sprites.gfs` - contains sprite data of some effect used during a fight
- `trials.gfs` - contains scripted cutscenes used in Training and Story Mode (like with the snake boss)
- `ui*.gfs` - contains OtterUI scenes, currently there are no tools to modify them

To modify one of these files you create a folder with the same name as the file (for example to mod `dev.gfs` you create a folder named `<root>/dev.gfs/`). To change something inside the gfs file just copy the path of the file inside the gfs into there and make your changes (for example to modify `dev.gfs/temp/config/foit_files.ini` you create `<root>/def.gfs/temp/config/foit_files.ini`)

### TFH Resource files `.tfhres`

You can modify the following TFH Resource files:

- `arizonaopening_prod.tfhres` - contains entries to Chapter 1 Part 1 opening with Texas and Arizona
- `canyon_prod.tfhres` - contains entries to Chapter 1 Part 1 Canyon section
- `highlands_prod.tfhres` - contains entries to Chapter 1 Part 3/4 "At the mountains of Sadness" and "Temple of Gloom" sections
- `inttests_prod.tfhres`, `testroomd_prod.tfhres` - contains testing maps for various unit testing or just testing, not used by the game (but has some cool stuff to look at)
- `lobbymaps_prod.tfhres` - contains entries to Pixel Lobbies (map data)
- `prologue_prod.tfhres` - contains entries to Chapter 1 Part 0 Prologue
- `reine_prod.tfhres` - contains entries to Chapter 1 Part 2 Reine
- `resources_prod.tfhres` - contains a lot of different kind of entries, here you can find the relations to Pixel Lobby sections (which one to load, etc.)

TFH Resource files are just SQL databases so you can use any SQL client to make changes. For people that don't want to mess with SQL (which is understandable) they can use [DB Browser for SQLite](https://sqlitebrowser.org/). Just like with Reverge Package files to change something it has to be the same primary key (that is `hiberlite_id`)

### Level Packs

To not cause issues with `levels.gfs` mods, I've created something called **Level Packs**, a level pack contains entries to your stages and their data, when mods are applied, all level packs get merged into one and written to `levels.gfs`. A valid level pack rests inside `<root>/levels/`, more information can be found [here](https://github.com/ThemModdingHerds/levels/blob/main/Levels/README.md#level-packs)

### Any other file (ONLY USE THIS IF NEEDED)

**WARNING: THIS SECTION TALKS ABOUT MODIFYING OTHER FILES, USING IT
WRONG MIGHT LEAD TO VERY BAD THINGS!!!**

To modify files that are not listed above, you can create something called a patch. A patch can:

- replace a string (word) in a text file
- replace a whole file
- replace parts of a file

Patches are located under `<root>/patches` as `*.json` files and can be nested inside that folder (`<root>/patches/a/folder/patch.json` is a valid patch). One JSON file contains a array of patches

#### General Patch Structure

A patch is a `*.json` file with the base structure:

```json
{
    "$type": "<type-of-patch>", // can be either "text" or "binary"
    "target": "<game-relative-filepath>"
}
```

`target` is a relative path to a game file you want to modify (for example to modify `<game-folder>/Scripts/names/baihe.palette.names` you set `target` to `Scripts/names/baihe.palette.names`)

#### Text File Patch

To replace a value in a text file you create the following patch format:

```json
{
    "$type": "text",
    "target": "<game-relative-filepath>",
    "match": "<regex>",
    "value": "<value-to-use>"
}
```

- `$type` must be `text`
- `match` is a regular expression, if you don't know what it is [then you'll have to do your own research](https://regexr.com/) (keep in mind that you remove the `/` from the start and beginning which means `/([A-Z])\w+/g` should be `([A-Z])\w+`)
- `value` is the value you want to replace with

keep in mind that you have to respect the file format/structure of the text file

#### Binary File Patch

To replace files that can't be read by a text editor you'll have to use this patch format:

```json
{
    "$type": "binary",
    "target": "<game-relative-filepath>",
    "filepath": "<relative-filepath-of-patches-folder>",
    "offset": "<offset-in-bytes>" // optional
}
```

- `$type` must be `binary`
- `filepath` is a file relative to `<root>/patches` so for example to use `<root>/patches/oh/coolfile.bin` you set `filepath` to `oh/coolfile.bin`, this can be anything (image, text file, binary file, audio)
- when `offset` is not specified, `target` will be replaced by `filepath` (literally just copy and replace)
- when `offset` is specified it will open `target`, go to `offset` in bytes and write the bytes from `filepath` there, replacing bytes from `offset` to `offset` + size of `filepath`

Binary patching  should only be done **IF** required, it also requires advanced knowledge of how binary files works if you use `offset`

## How to upload mod

To make your mod ready to be uploaded, compress the content of your mod folder to a zip/tar.gz, which means it requires the following structure:

```txt
mymod.zip/mod.json
mymod.zip/*.gfs/**/*
mymod.zip/*.tfhres
mymod.zip/levels/**/*
mymod.zip/patches/**/*.json
```

You can also have multiple mods in one zip file by putting them in subfolders

Keep in mine that if you upload the mod on GameBanana, it will always download the latest release. Also don't put compressed files in compressed files, that's just stupid and defeating the point of compressed files.
