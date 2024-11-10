# The `config.json` file

when you run the mod loader for the first time, a `config.json` is generated in the same folder as the executable and probably looks something like this:

```json
{"client_path":"<some-path>","server_path":"","version":"1.0.0.0"}
```

or

```json
{"client_path":"","server_path":"","version":"1.0.0.0"}
```

## So what can I do with this?

you can configure how the mod loader finds stuff:

- `client_path`: the path to the game client folder (can detect Steam/Epic Games installs)
- `server_path`: the path to the server folder, this is empty when created but can be added for server-side mods
- `version`: is the version of the mod loader, do not touch this one

technically in the `client_path` you could put the folder to the install of SkullGirls and mod this game instead but SkullGirls works different from Them's Fightin' Herds
