using ItemChanger;
using ItemChanger.Tags;

namespace RandoPlus
{
    /// <summary>
    /// Interop tag implementation for CMI pool groups
    /// </summary>
    /// <remarks>
    /// This is not strictly needed, but it's much more convenient than defining an InteropTag each time with a message and
    /// adding the poolgroup as a dict property. Additional properties (if needed in the future) can be added in a similar way
    /// </remarks>
    public class SupplementalMetadataTag : Tag, IInteropTag
    {
        private const string CmiPoolGroupProperty = "PoolGroup";

        public string Message => "RandoSupplementalMetadata";

        public string PoolGroup { get; set; }

        public bool TryGetProperty<T>(string propertyName, out T value)
        {
            if (propertyName == CmiPoolGroupProperty && PoolGroup is T group)
            {
                value = group;
                return true;
            }

            value = default;
            return false;
        }
    }
}
