# Guide

## How make a mod

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

3. To modify a `.tfhres` file you put that file in the root folder of the mod so for example if you want to modify `resources_prod.tfhres` you create a `resources_prod.tfhres` in the root folder of your mod (it should look like this: `<name-of-mod>/resources_prod.tfhres`). The same process can be done with `.gfs` files but instead of a file you create a folder named after one of the `.gfs` files in `data01`. So if you want to mod `levels.gfs` you create a folder `levels.gfs` in your mod folder (it should look like this: `<name-of-mod>/levels.gfs/temp/...`
4. ???
5. Profit
6. Done
