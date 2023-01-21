using Modding;
using RandomizerCore;
using RandomizerCore.Exceptions;
using RandomizerCore.Randomization;
using RandomizerMod.RC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RandoPlus.Advanced
{
    public static class AdvancedRequests
    {
        private static readonly ILogger _logger = new SimpleLogger("RandoPlus.AdvancedRequests");

        public static void Hook(bool rando, bool ic)
        {
            if (rando) HookRequestBuilder();
        }

        public static void HookRequestBuilder()
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
                        _logger.Log($"Constraint violated for group {groupLabel}: {item.Name} @ {loc.Name}");
                        LogConstraintViolationInfo(dgps, item, loc);
                        throw new OutOfLocationsException();
                    };
                }
            }
        }

        private static void LogConstraintViolationInfo(DefaultGroupPlacementStrategy dgps, IRandoItem item, IRandoLocation loc)
        {
            List<Func<IRandoItem, IRandoLocation, bool>> constraintList;
            try
            {
                constraintList = ReflectionHelper.GetField<DefaultGroupPlacementStrategy, List<Func<IRandoItem, IRandoLocation, bool>>>(dgps, "_constraints");
            }
            catch (MemberAccessException)
            {
                constraintList = null;
            }

            if (constraintList is null)
            {
                _logger.LogDebug("Cannot find constraint list.");
                return;
            }

            bool foundError = false;
            foreach (Func<IRandoItem, IRandoLocation, bool> constraint in constraintList)
            {
                if (!constraint(item, loc))
                {
                    MethodInfo method = constraint.Method;
                    _logger.LogDebug($"Constraint {method.Name} on {method.DeclaringType.Name} in {method.DeclaringType.Assembly.FullName} violated");
                    foundError = true;
                }
            }
            if (!foundError)
            {
                _logger.LogDebug("No constraint consistently violated");
                foreach (Func<IRandoItem, IRandoLocation, bool> constraint in constraintList)
                {
                    MethodInfo method = constraint.Method;
                    _logger.LogDebug($" - constraint {method.Name} on {method.DeclaringType.Name} in {method.DeclaringType.Assembly.FullName}");
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

                _logger.LogDebug($"Dispersion: stage {label}");
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
                        _logger.LogError("Failed to reassign MainItemStage");
                    }
                }
            }
        }

    }
}
