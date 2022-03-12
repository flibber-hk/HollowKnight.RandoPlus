using ItemChanger;
using ItemChanger.Tags;

namespace RandoPlus
{
    /// <summary>
    /// Class containing methods for adding supplemental metadata tags to taggable objects.
    /// </summary>
    internal static class SupplementalMetadataTagFactory
    {
        private const string CmiPoolGroupProperty = "PoolGroup";
        private const string CmiModSourceProperty = "ModSource";
        private const string CmiVanillaItemProperty = "VanillaItem";
        private const string TagMessage = "RandoSupplementalMetadata";

        public static InteropTag AddTagToObject(TaggableObject obj)
        {
            InteropTag tag = obj.AddTag<InteropTag>();
            tag.Message = TagMessage;
            tag.Properties[CmiModSourceProperty] = RandoPlus.instance.GetName();
            return tag;
        }

        public static InteropTag AddTagToItem(AbstractItem item, string poolGroup)
        {
            InteropTag tag = AddTagToObject(item);
            tag.Properties[CmiPoolGroupProperty] = poolGroup;
            return tag;
        }

        public static InteropTag AddTagToLocation(AbstractLocation loc, string poolGroup, string vanillaItem)
        {
            InteropTag tag = AddTagToObject(loc);
            tag.Properties[CmiPoolGroupProperty] = poolGroup;
            if (!string.IsNullOrEmpty(vanillaItem))
            {
                tag.Properties[CmiVanillaItemProperty] = vanillaItem;
            }
            return tag;
        }
    }
}
