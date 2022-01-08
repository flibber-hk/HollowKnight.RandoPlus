using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandoPlus.MrMushroom
{
    public static class MrMushroom
    {
        public static void Hook(bool rando, bool ic)
        {
            if (ic) ICInterop.DefineItemsAndLocations();
            if (rando) RequestMaker.Hook();
            if (rando) LogicAdder.Hook();
        }
    }
}
