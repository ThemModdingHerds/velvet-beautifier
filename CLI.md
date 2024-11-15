# Command Line Usage

the executable is called `VelvetBeautifier.CLI.exe`

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

### `--id`

the id of any mod installed in `mods`

```sh
--id <identifier>
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

### `--create --id <identifier>`

creates a empty mod template for you in the `mods` folder

example:

```sh
VelvetBeautifier.CLI.exe --create --id mycoolmod # creates a valid mod called "mycoolmod" in mods (mods/mycoolmod)
```

### `--create-gfs --input <folder> --output <file>`

creates a `.gfs` file from `folder` and saves it as `file`, it also adds the extension if it can't find it

example:

```sh
VelvetBeautifier.CLI.exe --create-gfs --input afolder --output afolder.gfs
```

### `--create-tfhres --output <file>`

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

### `--enable --id <identifier>` and `--disable --id <identifier>`

enables/disables a mod with the id `identifier`

example:

```sh
VelvetBeautifier.CLI.exe --enable mycoolmod # enables the mod called "mycoolmod"
VelvetBeautifier.CLI.exe --disable randomuncoolmod # disables the mod called "randomuncoolmod"
```
