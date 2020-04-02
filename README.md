# tyte - A simple tile editor built in Unity3D.

## Goal

The goal of this project is to provide a simple-to-use level editor intended to work with some javascript games I've been working on.  While there are lots of very popular level editors out there that provide integrations, I wanted more of hands on approach to work with the data formats that I am already planning to use in my javascript game.  I also wanted a solution that can be used by my family w/out lots of other bells-and-whistles (and confusion) that some of the more developed editors provide.

## Details

So, I'm working with a javascript game that is using a top-down grid of tiles.  I'm keeping track of all these tiles in a `sprites.json` file that includes data about each sprite (like path, collision info, and other game-specific data) and also includes a unique ID (just a number).  I'm representing my world through a set of zones or levels that lays out tiles on a fixed grid.  I also want the grid to be capable of having multiple layers (e.g.: a background layer might be stuff like grass, road, water) while a foreground layer might have stuff like buildings, trees, etc.  Therefore I can use transparency in the tiles and stack images to create a more realistic world without exploding the number of different tile types I need.  Each zone is represented by a separate zone file that references the tiles by their ID.  Sound good?

Here's what my overall project structure looks like:

```
projectdir
├── index.html
├── ...
└── src
    ├── img
    │   ├── 32grass.png
    │   ├── 32road.png
    │   ├── sprites.json
    ├── js
    │   ├── ...
    └── zones
        ├── start.json
        └── test.json
```

The `src/img` folder contains the `sprites.json` file containing all of the sprite records.  The path also contains all the individual sprite files (I'm not using a sprite map at this point, as I find this easier to manage and update). The `src/zones` folder contains all of the zone files.  The `sprites.json` records need to be separately maintained to keep track of all the sprites you wish to use for your project.

So, given this project structure, I wanted the tile editor to be able to read and update the project folder as I go about editing and creating new levels.  Also want to be able to add new sprites and have them automatically be picked up by the level editor.

## Usage

This repo is a Unity3D project.  Current build settings are for a Windows build, but builds to OSX/Linux should work as well (I haven't tested them however).  WebGL builds will not work at this time, due to the restrictions of how webgl interacts with the client system.  To use the project, you'll need to have Unity installed and either run it from the editor, or build a standalone app and use that.

When first launched, you will be asked for your project settings.  This includes the full path to your `projectdir` as outlined above.  If you're using the same folder structure as above, the defaults for other variables should work.  Otherwise, fill in the details as appropriate.

A brief intro to each of the editor sections is as follows.

### Top TyTe Bar

Besides just showing the editor title there is a gear button to bring up the project settings, where you can configure project folders and tile size.  These are stored as user preferences for the application and will be remembered across application sessions.

### Grid Area

The grid area is the left area of the screen showing the current level.  This is where you can add and remove tile selections for the selected level and layer.

### Level Management

The level management allows you to create new levels, save and load existing levels.  There is a reload button in the upper left corner that will reload the current level.  **Beware: this will cause any local changes to be lost.  There is no prompt or safeguard to protect against lost data**.

### Tool Selection

The current tools available include:

* Brush: Single tile brush.  Set selected grid tile to selected sprite.
* Fill: Fill tool.  Basic fill tool that takes the current selected grid tile, sets to the new selected sprite then finds adjacent tiles of the same type and sets those as well.
* Erase: Erase single tile.  Erase tile selection for the current selected grid tile.
* Eyedropper: Set the selected sprite to the tile setting of the selected grid tile.

The current selected tool is highlighed in blue.

### Tile Selection

The tile selection panel shows all of the tiles associated with your project.  Click on a sprite to select it for use with other tools.  The currently selected tile is highlighted in blue.

### Layer Management

Layer management shows the layers associated with the current level.  Current layers are listed in reverse order (the lower layer at the top of the list), as this represents the draw order.  For each layer you can raise or lower the layer or delete.  As with other major data functions, there is currently no confirm dialog for deleting a layer, and there is no undo.  On the left side of each layer are two buttons.  The left toggle button allows you to select the layer as the layer to edit.  Only the selected layer is modified when editing on the grid to the left.  The eyeglass button toggles the visibility of the layer.

### Example Project

I added an example project to this repository, available in the `ExampleProject` folder.  You can set your project settings base folder to this directory on your system and try it out.

Top-down tileset is by `Matiaan` from [OpenGameArt.Org](https://opengameart.org/content/top-down-grass-beach-and-water-tileset).
Dragon tile is used by permission and is by [Austin Blackwood](https://twitter.com/AABlackwood).

## Known Issues and Potential Enhancements

* Performance - currently implementation is using a naive approach for handling the tile grid map.  Lots of game objects are being used instead of a single canvas.  This can cause performance issues w/ large levels.
* Save warnings - currently if you don't save your work and you perform an operation that destructive to current state (e.g.: loading a new level), there is no prompt or warning that your work isn't saved.
* Undo - would be great to have an undo operation while editing.  For now, it's up to the user to keep track of what tool you're on.  A fill operation may not be what your after...
* More tools - operations like marquee/copy/paste would be good.
* Key bindings - would be great to have key bindings for the tools and to switch layers.
* Cursor control - While editing the map, have a cursor that can be moved via keyboard and used to operate the selected tool.
* Sprite linking - Support more complex sprite data that would allow a given sprite to be linked to sprites that can be connected.  For example, a cliff edge has two corners, etc.
* Animated sprites - Add support for animated sprites.
* Zoom control
* WebGL build support would be great