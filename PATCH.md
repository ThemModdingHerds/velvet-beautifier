# Patch File (totally not inspired by Starbounds's patch file system)

A patch file is a JSON file with `.patch` as the extension which tells the Mod Loader which files to... well patch...?

## Structure of `*.patch` file

```json
{
    "type": "I", // I = Insert (Add), U = Update (Replace), D = Delete (only if needed!)
    "value_type": "S", // S = string, N = number, B = bool, 0 = null
    "path": "path/to/jsonfile", // path to json/xml file
    "property":"somejson/property/array/[0]/", // the property path you take
    "value": "some value to patch in" // the value to patch in
}
```

## How does the `"property"` property in a `*.patch` properly work?

Let's say we have this very creative JSON file:

```json
{
    "balls": [
        0,
        1,
        3
    ]
}
```

You see that the property `balls` is having a wrong order of numbers (you know, 3 doesn't come after 1). We can fix that by patching so we create this patch file:

```json
{
    "type": "U", // we're updating a value
    "value_type": "N", // the value we set is a number
    "property": "balls/[2]", // we want to patch the array "balls" at index 2 (arrays start at 0, not 1)
    "value": 2 // the fix
}
```

Now we can apply the patch and the `3` in that JSON object is now `2`
