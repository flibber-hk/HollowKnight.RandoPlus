using ItemChanger;
using RandomizerMod.IC;

namespace RandoPlus.AreaRestriction
{
    public class MultiLocationConfig
    {
        public static void Hook()
        {

        }

        public static void PreventMultiChests()
        {
            foreach (AbstractPlacement pmt in ItemChanger.Internal.Ref.Settings.GetPlacements())
            {
                if (pmt.HasTag<RandoPlacementTag>())
                {
                    pmt.AddTag<ItemChanger.Tags.UnsupportedContainerTag>().containerType = Container.Chest;
                }
            }
        }

    }
}
