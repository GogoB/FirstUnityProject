# PetarProekt

A Unity car collection game. Collect 10 capsules to finish the level, then use Restart to reset the game.

## Open and play

1. Extract the ZIP if you received this project as an archive.
2. In Unity Hub, choose Add project from disk and select the PetarProekt folder containing Assets, Packages, and ProjectSettings.
3. Open with Unity 6000.6.4f1. Allow Unity to restore packages and import assets on first launch.
4. Open Assets/Scene.unity, press Play, then click Start in the Game view.
5. Click inside the Game view so it receives keyboard input.

## Controls

| Input | Action |
| --- | --- |
| W / A / S / D | Move |
| Q / E | Rotate |
| H | Toggle the car's two texture skins |
| Start | Begin the level |
| Restart | Restore the level and return to Start |

## Recent changes

- Added a 26 by 22 unit platform covering all existing collectibles.
- Keep the player on the platform, including after growth and rotation.
- Fixed completion panel layout: "Level done" is above a separate Restart button.
- Removed the duplicate Movement component from the unused empty object.
- Registered Assets/Scene.unity as the build scene so it can be reloaded.
- Guard optional pickup audio so missing audio does not prevent completion.
- H switches the car body's and wheels' texture skins instead of tinting the cube.
- Texture overrides do not modify the shared material asset.

There are 13 collectible capsules in the scene; the existing completion target remains 10. The player grows when collecting a capsule.

## Verification

Unity compiled the scripts successfully. Automated Play mode checks covered Start, completion after 10 pickups, Restart, restored collectibles, platform boundaries, and texture cycling. Texture switching was also verified visually with the H key in the running project.

If editing scene files outside Unity while the editor is open, stop Play mode and reopen Assets/Scene.unity before testing so updated Inspector references are loaded.

## Package contents

This is the editable Unity project, not a standalone executable. Assets and their .meta files, Packages, and ProjectSettings are included. Unity regenerates Library, Temp, Logs, UserSettings, and IDE project files; they are excluded from the ZIP and Git.

Third-party car assets are retained from the supplied project under Assets/Low Poly Cars (Free). Their original authors retain their rights; this README does not grant additional licensing rights.
