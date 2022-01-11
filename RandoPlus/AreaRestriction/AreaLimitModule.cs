using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemChanger;
using ItemChanger.Modules;

namespace RandoPlus.AreaRestriction
{
    public class AreaLimitModule : Module
    {
        public List<string> PlacedAreas = new();

        public override void Initialize()
        {
            if (ItemChangerMod.Modules.Get<InventoryTracker>() is InventoryTracker t)
            {
                t.OnGenerateFocusDesc += ShowAreasInInventory;
            }
        }

        public override void Unload()
        {
            if (ItemChangerMod.Modules.Get<InventoryTracker>() is InventoryTracker t)
            {
                t.OnGenerateFocusDesc -= ShowAreasInInventory;
            }
        }
        private void ShowAreasInInventory(StringBuilder sb)
        {
            sb.AppendLine();
            sb.AppendLine("You feel drawn towards " + string.Join(", ", PlacedAreas) + ".");
        }
    }
}
