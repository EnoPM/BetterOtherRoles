using System;
using System.Collections.Generic;
using System.Linq;
using BetterOtherRoles.Modules;
using HarmonyLib;
using Il2CppSystem.Text;

namespace BetterOtherRoles.Patches;

[HarmonyPatch(typeof(NormalPlayerTask))]
public static class NormalPlayerTaskPatches
{
    private static readonly List<TaskTypes> TaskTypesToPatch = new()
        { TaskTypes.FixWiring, TaskTypes.RebootWifi, TaskTypes.RecordTemperature, TaskTypes.ChartCourse };

    [HarmonyPatch(nameof(NormalPlayerTask.AppendTaskText))]
    [HarmonyPrefix]
    private static bool AppendTaskTextPrefix(NormalPlayerTask __instance, StringBuilder sb)
    {
        if (!TaskTypesToPatch.Contains(__instance.TaskType)) return true;
        var flag = __instance.ShouldYellowText();
        if (flag)
        {
            sb.Append(__instance.IsComplete ? "<color=#00DD00FF>" : "<color=#FFFF00FF>");
        }

        var room = GetUpdatedRoom(__instance);

        sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(room));
        sb.Append(": ");
        sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(__instance.TaskType));
        if (__instance is { ShowTaskTimer: true, TimerStarted: NormalPlayerTask.TimerState.Started })
        {
            sb.Append(" (");
            sb.Append(DestroyableSingleton<TranslationController>.Instance.GetString(StringNames.SecondsAbbv,
                (int)__instance.TaskTimer));
            sb.Append(")");
        }
        else if (__instance.ShowTaskStep)
        {
            sb.Append(" (");
            sb.Append(__instance.taskStep);
            sb.Append("/");
            sb.Append(__instance.MaxStep);
            sb.Append(")");
        }

        if (flag)
        {
            sb.Append("</color>");
        }

        sb.AppendLine();

        return false;
    }

    private static SystemTypes GetUpdatedRoom(NormalPlayerTask task)
    {
        return task.TaskType switch
        {
            TaskTypes.FixWiring => GetFixWiringRoom(task),
            TaskTypes.RecordTemperature => GetRecordTemperature(task),
            TaskTypes.RebootWifi => GetRebootWifi(task),
            TaskTypes.ChartCourse => GetChartCourse(task),
            _ => task.StartAt
        };
    }

    private static SystemTypes GetChartCourse(NormalPlayerTask task)
    {
        if (!BetterPolus.Enabled.getBool()) return task.StartAt;
        return SystemTypes.Comms;
    }

    private static SystemTypes GetRebootWifi(NormalPlayerTask task)
    {
        if (!BetterPolus.Enabled.getBool()) return task.StartAt;
        return SystemTypes.Dropship;
    }

    private static SystemTypes GetRecordTemperature(NormalPlayerTask task)
    {
        if (!BetterPolus.Enabled.getBool()) return task.StartAt;
        return SystemTypes.Outside;
    }

    private static SystemTypes GetFixWiringRoom(NormalPlayerTask task)
    {
        try
        {
            if (task.HasLocation)
            {
                var pos = task.Locations.ToArray().FirstOrDefault();
                if (TaskPositionsRandomizer.RelocatedWires.TryGetValue($"{pos.x}#{pos.y}", out var newRoom))
                {
                    return newRoom;
                }
            }
        }
        catch (Exception e) { }

        return task.StartAt;
    }
}