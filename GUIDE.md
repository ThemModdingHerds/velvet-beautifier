# Guide

## How make a mod

1. Create a folder in the `mods` folder (name it whatever you want)
2. Create a `mod.json` file and fill it with the required information:
    - `id`: Unique Id of your mod (kinda useless right now)
    - `name`: Name of mod
    - `author`: The creator of the mod
    - `desc`: The Description of the mod  

    At the end the `mod.json` should look like this:

    ```json
    {
        "id": "ntf.test.mod",
        "name": "Test Mod",
        "author": "Night The Fox",
        "desc": "This is a Test Mod"
    }
    ```

3. To modify a `.tfhres` file you create a directory with the name of the `.tfhres` file (including the `.tfhres` part!). An Example would be `resources_prod.tfhres` as a directory
4. Now add files/directories in that folder
5. ???
6. Profit
7. Done
