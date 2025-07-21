using HarmonyLib;
using RimWorld;
using Verse;

namespace ImP;

[HarmonyPatch(typeof(QualityUtility), nameof(QualityUtility.SendCraftNotification))]
public class PatchConstruction
{
    [HarmonyPostfix]
    public static void Postfix(Thing thing, Pawn worker)
    {
        // Patch Construction(building) only
        if (thing.def.category != ThingCategory.Building) return;

        var thought = ImpCore.GetFixedThoughtDefOf(worker, thing);

        if (thought != null)
        {
            worker?.needs?.mood?.thoughts?.memories?.TryGainMemory(thought);
            // Log.Message($"thought:: days: {thought.durationDays}; mood: {thought.stages}");
        }
    }
}
[HarmonyPatch(typeof(Frame), nameof(Frame.FailConstruction))]
public class PatchFailConstruction
{
    [HarmonyPostfix]
    public static void Postfix(Frame __instance, Pawn worker)
    {
        if (__instance.def.category != ThingCategory.Building) return;
        if (worker.skills == null) return;
        
        var level = worker.skills.GetSkill(SkillDefOf.Construction).Level;
        var workAmount = __instance.WorkToBuild;
        // TODO: Fail should be more frustrating than Awful.
        var thought = ImpCore.GetFixedThoughtDefOf(level, QualityCategory.Awful, workAmount );

        if (thought != null)
        {
            worker?.needs?.mood?.thoughts?.memories?.TryGainMemory(thought);
            // Log.Message($"thought:: days: {thought.durationDays}; mood: {thought.stages}");
        }
    }
}