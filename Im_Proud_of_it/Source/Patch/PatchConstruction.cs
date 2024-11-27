using HarmonyLib;
using RimWorld;
using Verse;

namespace ImP;

[HarmonyPatch(typeof(QualityUtility), nameof(QualityUtility.SendCraftNotification))]
public class PatchConstruction
{
    [HarmonyPostfix]
    public static void Postfix(Pawn worker, Thing thing)
    {
        if (thing.def.category != ThingCategory.Building) return;

        var thought = ImpCore.GetFixedThoughtDefOf(worker, thing);

        if (thought != null)
        {
            worker?.needs?.mood?.thoughts?.memories?.TryGainMemory(thought);
            // Log.Message($"thought:: days: {thought.durationDays}; mood: {thought.stages}");
        }
    }
}