# HollowKnight.RandoPlus

Extension mod for Randomizer 4 that adds alternative item randomization options. 
The following options are included; more may be added in later versions, and some may be removed to move to base Randomizer.

### Mr Mushroom rando
- Add seven Mr Mushroom Level items to the randomization pool; each increases the Mr Mushroom level by 1. Collecting all three dreamers also increases the Mr Mushroom level by 1. 
Achieving Mr Mushroom level 8 unlocks the ending cutscene.
- Add the seven Mr Mushroom locations to the randomization pool; location n requires Mr Mushroom level at least n (as well as Spore Shroom equipped).
- For the purposes of split item groups, Mr Mushroom locations are considered to be in the same group as Lore Tablets (even if tablets are not randomized).

### No Tear, Swim, Lantern
Options to remove the tear, swim and lantern items from the game.

Replaces the skills with items that respectively
* Protect against acid damage (and remove the blocker opposite Leg Eater)
* Protect against swim damage
* Enable dark room tolls and the No Eyes fight.

### Area Blitz
Squishes all randomized locations into 7 randomly selected map areas, which will always include Dirtmouth and the starting area, 
and will always exclude White Palace if Randomization in White Palace is disabled.
- The selected areas will be displayed in the inventory focus tracker.
- Randomized locations outside the selected areas will all receive Lumafly Escape items, which will not count for in-game completion percentage.
- Locations in the selected areas may receive more than one item. By default, a location with multiple items (unless given a special container 
such as a grub) will become a chest containing several shinies; the PreferMultiShiny option instead places a shiny that gives several items.
- This setting will respect split groups, with the exception that if a group has all of its locations excluded, then all of its items will be
placed at Sly. (If a group has at least one included location, then none of its items will be placed at Sly unless Sly is part of the group.)

It may still be necessary to pass through excluded areas; for instance, Mask_Shard-Bretta is considered part of Dirtmouth, so may be relevant
even if Fungal Wastes (the map area containing Bretta) is excluded.

### Custom Pools and Logic
Any external logic modifications can be made via the custom logic injector mod; logic modifications using terms defined by this mod
must be made with a priority greater than 50.

The keywords NOACID, NOSWIM and NOLANTERN can be used to represent having obtained the relevant items in No Tear, Swim and Lantern mode.

The Define Refs option means that all refs will be defined; for example, this means that rando will think that Mr Mushroom level ups are placed
vanilla at Mr Mushroom locations (rather than not knowing they exist), and any built-in logic changes will be made. This should have no effect on
seed generation if none of the other options are selected, but the option to disable has been provided as a failsafe (and disabling
this option will have no effect if any other option is selected). However, this option *must* be enabled in order to define any
custom pools and logic using any items and locations defined in this mod.