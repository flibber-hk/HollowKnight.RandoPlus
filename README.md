# HollowKnight.RandoPlus

Extension mod for Randomizer 4 that adds alternative item randomization options. 
The following options are included; more may be added in later versions, and some may be removed to move to base Randomizer.

### Mr Mushroom rando
- Add seven Mr Mushroom Level items to the randomization pool; each increases the Mr Mushroom level by 1. Collecting all three dreamers also increases the Mr Mushroom level by 1. Achieving Mr Mushroom level 8 unlocks the ending cutscene.
- Add the seven Mr Mushroom locations to the randomization pool; location n requires Mr Mushroom level at least n (as well as Spore Shroom equipped).
- For the purposes of split item groups, Mr Mushroom locations are considered to be in the same group as Lore Tablets (even if tablets are not randomized).

### No Tear, Swim, Lantern
Options to remove the tear, swim and lantern skills from the game. (Require acidskips, acidskips and darkrooms enabled respectively.)

Replaces the skills with items that 
* Protect against acid damage (and remove the blocker opposite Leg Eater)
* Protect against swim damage
* Enable dark room tolls and the No Eyes fight respectively.

Json files in the Logic/ subdirectory of the installed mod directory can be used to override logic (in a similar way to base rando); 
the keywords NOACID, NOSWIM and NOLANTERN can be used to represent having obtained the relevant items in No Tear, Swim and Lantern mode.
