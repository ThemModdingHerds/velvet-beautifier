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
        "depends": { // this is similar to package.json from NodeJS. See here https://docs.npmjs.com/cli/v6/configuring-npm/package-json#dependencies
            "ntf.someother.mod": "1.0.0"
        },
        "url": "https://somelink.to/the/mod" // link to your mod download (GameBanana, direct zip download)
    }
    ```

3. To modify a `.tfhres` file you create a directory with the name of the `.tfhres` file (including the `.tfhres` part!). An Example would be `resources_prod.tfhres` as a directory
4. Now add [patches][patch-path]/directories in that folder
5. ???
6. Profit
7. Done

[patch-path]: ./PATCH.md
