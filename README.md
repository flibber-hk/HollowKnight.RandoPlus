# HollowKnight.RandoPlus

Extension mod for Randomizer 4 that adds alternative item randomization options. 
The following options are included; more may be added in later versions, and some may be removed to move to base Randomizer.

### Mr Mushroom rando
- Add seven Mr Mushroom Level items to the randomization pool; each increases the Mr Mushroom level by 1. Collecting all three dreamers also increases the Mr Mushroom level by 1. 
Achieving Mr Mushroom level 8 unlocks the ending cutscene.
- Add the seven Mr Mushroom locations to the randomization pool; location n requires Mr Mushroom level at least n (as well as Spore Shroom equipped).
- Talking to Mr Mushroom without Spore Shroom equipped will give a preview of the item(s) there.
If the Lore Tablet Previews setting is disabled in rando, then this preview will not be shown.
- For the purposes of split item groups, Mr Mushroom locations are considered by default to be in the same group as Lore Tablets - specifically the Kingdom's 
Edge Lore Tablet - (even if tablets are not randomized).

### No Tear, Swim, Lantern
Options to remove the tear, swim and lantern items from the game.

Replaces the skills with items that respectively
* Protect against acid damage (and remove the blocker opposite Leg Eater)
* Protect against swim damage
* Enable dark room tolls and the No Eyes fight.

Finding any of the relevant items will put all relevant skips into logic. For example, finding the No Acid item will put all acid skips into logic, even 
if that skip setting was disabled.

### Area Blitz
Squishes all randomized locations into 7 randomly selected map areas, which will always include Dirtmouth and the starting area, 
and will always exclude White Palace if Randomization in White Palace is disabled.
- The selected areas will be displayed in the inventory focus tracker.
- Randomized locations outside the selected areas will all receive Lumafly Escape items, which will not count for in-game completion percentage.
- Locations in the selected areas may receive more than one item. By default, a location with multiple items (unless given a special container 
such as a grub) will become a chest containing several shinies; the PreferMultiShiny option instead places a shiny that gives several items.
- This setting will respect split groups, with the exception that if a group has all of its locations excluded, then all of its items will be
placed at Sly. (If a group has at least one included location, then none of its items will be placed at Sly unless Sly is part of the group.)
- With the Full Flexible Count setting, normal locations can accept more than one item but no locations will be removed. This is useful to avoid
overinflating shops' importance, if other connections add a lot of items.
This setting is automatically enabled with Area Blitz (as there are many more items than locations). It can be enabled separately
from Area Blitz, however.

It may still be necessary to pass through excluded areas; for instance, Mask_Shard-Bretta is considered part of Dirtmouth, so may be relevant
even if Fungal Wastes (the map area containing Bretta) is excluded.

### Nail Upgrades
Adds the four nail upgrades to the randomization pool.
- Nailsmith has four items as usual. By default each can be previewed if the previous location has been obtained. If the Nailmaster Previews setting
is disabled in rando, then previews will not be shown for the Nailsmith's items.
- Nail upgrades must be claimed from the inventory by pressing attack while the nail icon is selected. This is to avoid being
locked out of skips, but the process of claiming a nail upgrade is not reversible. (Consult the Randomizer readme for information about
how many nail upgrades should be claimed depending on skip settings). This behaviour can be disabled (so nail upgrades are given immediately) before
randomization by selecting the Give Nail Upgrades On Pickup option.
- The nailsmith questline (kill / Sheo) is tied to the location, not the items.
- For the purposes of split item groups, Nail Upgrades are considered by default to be in the same group as Skills - specifically Vengeful Spirit.

### Advanced settings
The following settings change the randomization in fairly technical ways, and are probably best ignored by most players.

- Disperse Groups moves groups that were otherwise randomized simultaneously to different stages. 
In concrete terms, it can sometimes reduce the number of attempts needed to generate placements in cases where it would otherwise take many attempts.
- Enforce All Constraints causes constraints added to groups to become mandatory. For example, the deranged constraint
usually allows vanilla placements if the alternative is another randomization attempt. This setting will increase the
number of attempts taken. With some combinations of settings this may make seed generation impossible (for example,
if grubs and mimics are randomized together and deranged is enabled).

### Custom Pools and Logic
Any external logic modifications can be made via the custom logic injector mod; logic modifications using terms defined by this mod
must be made with a priority greater than 50.

The keywords NOACID, NOSWIM and NOLANTERN can be used to represent having obtained the relevant items in No Tear, Swim and Lantern mode.

The Define Refs option means that all refs will be defined; for example, this means that rando will think that Mr Mushroom level ups are placed
vanilla at Mr Mushroom locations (rather than not knowing they exist), and any built-in logic changes will be made. This should have no effect on
seed generation if none of the other options are selected, but the option to disable has been provided as a failsafe (and disabling
this option will have no effect if any other option is selected). However, this option *must* be enabled in order to define any
custom pools and logic using any items and locations defined in this mod.
