using System;
using System.Collections.Generic;
using System.Linq;
using ItemChanger;
using ItemChanger.Items;
using ItemChanger.UIDefs;

namespace RandoPlus.RemoveUsefulItems.Items
{
    public static class ItemDefinition
    {
        private static void AddSkillMetadata(AbstractItem item)
        {
            SupplementalMetadataTagFactory.AddTagToItem(item, Consts.SkillsPoolGroup);
        }

        public static void DefineItems()
        {
            UIDef lanternUIdef = Finder.GetItem(ItemNames.Lumafly_Lantern).UIDef.Clone();
            AbstractItem noLantern = new CustomSkillItem()
            {
                name = Consts.NoLantern,
                boolName = nameof(NoLanternModule.gotNoLantern),
                moduleName = "RandoPlus.RemoveUsefulItems.Items.NoLanternModule, RandoPlus",
                UIDef = lanternUIdef
            };
            AddSkillMetadata(noLantern);
            Finder.DefineCustomItem(noLantern);

            BigUIDef tearUIdef = Finder.GetItem(ItemNames.Ismas_Tear).UIDef.Clone() as BigUIDef;
            tearUIdef.descOne = new BoxedString("Acid shall not be repelled.");
            tearUIdef.descTwo = new BoxedString("Don't swim in acidic waters without coming to any harm.");
            AbstractItem noTear = new CustomSkillItem()
            {
                name = Consts.NoTear,
                boolName = nameof(NoTearModule.gotNoTear),
                moduleName = "RandoPlus.RemoveUsefulItems.Items.NoTearModule, RandoPlus",
                UIDef = tearUIdef
            };
            AddSkillMetadata(noTear);
            Finder.DefineCustomItem(noTear);

            BigUIDef swimUIdef = Finder.GetItem(ItemNames.Swim).UIDef.Clone() as BigUIDef;
            swimUIdef.name = new BoxedString("Not Swim");
            swimUIdef.descOne = new BoxedString("The power of buoyancy is not yours.");
            swimUIdef.shopDesc = new BoxedString("I may look like it, but I'm actually not a fully certified swimming instructor. One easy payment, I toss you in the deep end, and, yeah, you won't figure it out from there.");
            AbstractItem noSwim = new CustomSkillItem()
            {
                name = Consts.NoSwim,
                boolName = nameof(NoSwimModule.gotNoSwim),
                moduleName = "RandoPlus.RemoveUsefulItems.Items.NoSwimModule, RandoPlus",
                UIDef = swimUIdef
            };
            AddSkillMetadata(noSwim);
            Finder.DefineCustomItem(noSwim);
        }
    }
}