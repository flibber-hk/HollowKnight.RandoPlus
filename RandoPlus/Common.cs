using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using Modding;
using Newtonsoft.Json;
using RandomizerCore.Exceptions;
using RandomizerCore.Randomization;
using RandomizerMod.Logging;
using RandomizerMod.RC;

namespace RandoPlus
{
    public static class Common
    {
        public static void Hook(bool rando, bool ic)
        {
            if (rando) HookSettingsLog();
            if (rando) HookMinorChanges();
        }

        private static void HookMinorChanges()
        {
            RequestBuilder.OnUpdate.Subscribe(100_000f, DisperseGroups);
            RequestBuilder.OnUpdate.Subscribe(500f, EnforceAllConstraints);
        }

        private static void EnforceAllConstraints(RequestBuilder rb)
        {
            if (!RandoPlus.GS.EnforceAllConstraints) return;

            foreach (GroupBuilder gb in rb.Stages.SelectMany(stage => stage.Groups))
            {
                string groupLabel = gb.label;

                if (gb.strategy is DefaultGroupPlacementStrategy dgps)
                {
                    dgps.OnConstraintViolated += (item, loc) =>
                    {
                        RandoPlus.instance.Log($"Constraint violated for group {groupLabel}: {item.Name} @ {loc.Name}");
                        throw new OutOfLocationsException();
                    };
                }
            }
        }

        private static void DisperseGroups(RequestBuilder rb)
        {
            if (!RandoPlus.GS.DisperseGroups) return;

            List<GroupBuilder> groups = rb.Stages.SelectMany(sb => sb.Groups).ToList();

            List<StageBuilder> stages = ReflectionHelper.GetField<RequestBuilder, List<StageBuilder>>(rb, "_stages");
            Dictionary<string, StageBuilder> stageLookup = stages.ToDictionary(stage => stage.label, stage => stage);
            stages.Clear();

            Dictionary<string, int> seenStageLabels = new();

            foreach (GroupBuilder group in groups)
            {
                string label = group.stageLabel;
                if (seenStageLabels.TryGetValue(label, out int count))
                {
                    seenStageLabels[label] += 1;
                    label += $"_{count}";
                }
                else
                {
                    seenStageLabels[label] = 1;
                }

                StageBuilder sb = rb.AddStage(label);
                sb.Add(group);

                if (label == RBConsts.MainItemGroup)
                {
                    try
                    {
                        ReflectionHelper.SetField(rb, "MainItemStage", sb);
                    }
                    catch (Exception)
                    {
                        RandoPlus.instance.LogError("Failed to reassign MainItemStage");
                    }
                }
            }
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
