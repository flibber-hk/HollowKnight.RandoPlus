using System.Linq;
using System.Reflection;

namespace RandoPlus.GhostEssence
{
    // TODO - rename ghostlogic.json back to logic.json when RCG is fixed
    [RandoConstantGenerators.GenerateJsonConsts("$[*].name", "ghostlogic.json")]
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
