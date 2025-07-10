# Command Line Usage

![Icon](./assets/icon_medium.png)

the executable is called `VelvetBeautifier.CLI.exe` on Windows and `VelvetBeautifier.CLI` on Unix (that is Mac and any Linux distro)

## Some neat features

- you can drag & drop a file or folder to install it as a mod
- you can open a GUI-like interface with [`--gui`](#--gui)

## Arguments

some commands require these arguments

### `--input`

should be a path to a file/folder for input

```sh
--input <path-to-sth>
```

### `--output`

should be a path to a file/folder for output

```sh
--output <path-to-sth>
```

## Commands

### `--install <sth>`

install a mod. `<sth>` can be:

- a folder with a valid structure
- a zip file
- a gamebanana link
- a gamebanana id

examples:

```sh
VelvetBeautifier.CLI.exe --install ./example # example is a folder and has a valid mod structure
VelvetBeautifier.CLI.exe --install example.zip # will extract example.zip and install it as a folder
VelvetBeautifier.CLI.exe --install https://gamebanana.com/mods/485712 # will isntall the gamebanana from that url
VelvetBeautifier.CLI.exe --install 485712 # will install the gamebanana mod with id 485712
```

### `--remove <identifier`

removes the mod with id `identifier`

example:

```sh
VelvetBeautifier.CLI.exe --remove randomuncoolmod
```

### `--update <identifier>`

update the mod if it has any, it acts as installing the mod again

example:

```sh
VelvetBeautifier.CLI.exe --update randomcoolmodthatisold
```

### `--apply`

applies all enabled mods, after this step you can play the game

example:

```sh
VelvetBeautifier.CLI.exe --apply
```

### `--revert`

removes all modifications to the game (makes it back to normal, unmodded state)

example:

```sh
VelvetBeautifier.CLI.exe --revert
```

### `--create <identifier>`

creates a empty mod template for you in the `mods` folder

example:

```sh
VelvetBeautifier.CLI.exe --create mycoolmod # creates a valid mod called "mycoolmod" in mods (mods/mycoolmod)
```

### `--create-gfs --input <folder> --output <file>`

creates a `.gfs` file from `folder` and saves it as `file`, it also adds the extension if it can't find it

example:

```sh
VelvetBeautifier.CLI.exe --create-gfs --input afolder --output afolder.gfs
```

### `--create-tfhres <file>`

creates a empty valid `.tfhres` file and save it as `file`, it also adds the extension if it can't find it, useful for modding `.tfhres` files.

example:

```sh
VelvetBeautifier.CLI.exe --create-tfhres resouces_prod.tfhres # creates this .tfhres file so we can modify it
```

### `--extract --input <file> --output <folder>`

extracts the `.gfs`/`.tfhres` file named `file` to the folder `folder`

example:

```sh
VelvetBeautifier.CLI.exe --extract --input levels.gfs --output levels # extracts a .gfs file
VelvetBeautifier.CLI.exe --extract --input resources_prod.tfhres --output resources_prod # extracts a .tfhres file
```

### `--enable <identifier>` and `--disable <identifier>`

enables/disables a mod with the id `identifier`

example:

```sh
VelvetBeautifier.CLI.exe --enable mycoolmod # enables the mod called "mycoolmod"
VelvetBeautifier.CLI.exe --disable randomuncoolmod # disables the mod called "randomuncoolmod"
```

### `--reset`

resets the program to it's default state

example:

```sh
VelvetBeautifier.CLI.exe --reset
```

### `--list [<mode>]`

lists mods based on the `mode`. It defaults to `local`

- `local`: shows mods installed on your system including if they are enabled/disabled
- `gamebanana`: shows mods available on GameBanana
- `online`: alias for `gamebanana` (and future mod site providers?)

example:

```sh
VelvetBeautifier.CLI.exe --list # lists local mods
VelvetBeautifier.CLI.exe --list local # same as above
VelvetBeautifier.ClI.exe --list gamebanana # list mods that are on GameBanana
VelvetBeautifier.CLI.exe --list online # same as above (for future plans?)
```

### `--register-scheme` (currently only Windows)

registers the executable as a 1-Click Installer (for GameBanana or urls)

example:

```sh
VelvetBeautifier.CLI.exe --register-scheme
```

### `--config`

Configure various properties in the config file:

- `--client-path <path>`
- `--server-path <path>`

example:

```sh
VelvetBeautifier.CLI.exe --config --client-path path/to/client
```

### `--gui`

Opens up a GUI-like interface inside the terminal. [Follow this guide on how to use it](./GUI.md)

example:

```sh
VelvetBeautifier.CLI.exe --gui
```
