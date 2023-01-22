using System.Linq;
using System.Reflection;

namespace RandoPlus.GhostEssence
{
    [RandoConstantGenerators.GenerateJsonConsts("$[*].Name", "ghostdata.json")]
    public static partial class GhostNames
    {
        public static string[] ToArray()
        {
            return typeof(GhostNames).GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.IsLiteral)
            .Select(f => (string)f.GetRawConstantValue())
            .ToArray();
        }

        // Generated consts
    }
}
