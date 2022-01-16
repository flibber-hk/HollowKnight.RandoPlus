using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using RandomizerMod.Logging;

namespace RandoPlus
{
    public static class Common
    {
        public static void Hook(bool rando, bool ic)
        {
            if (rando) HookSettingsLog();
        }

        public static void HookSettingsLog()
        {
            SettingsLog.AfterLogSettings += AddSettingsToLog;
        }

        private static void AddSettingsToLog(LogArguments args, TextWriter tw)
        {
            tw.WriteLine("Logging RandoPlus settings:");
            using JsonTextWriter jtw = new(tw) { CloseOutput = false, };
            RandomizerMod.RandomizerData.JsonUtil._js.Serialize(jtw, RandoPlus.GS);
            tw.WriteLine();
        }
    }
}
