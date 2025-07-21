using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ImP;

[HarmonyPatch(typeof(GenRecipe), "PostProcessProduct")]
public class PatchMakeRecipeProducts
{
    [HarmonyPostfix]
    static Thing Postfix(
        Thing __result,
        RecipeDef recipeDef,
        Pawn worker
)
    {
        var thought = ImpCore.GetFixedThoughtDefOf(worker, recipeDef, __result);

        if (thought != null)
        {
            worker?.needs?.mood?.thoughts?.memories?.TryGainMemory(thought);
            // Log.Message($"thought:: days: {thought.durationDays}; mood: {thought.stages}");
        }

        return __result;
    }
}