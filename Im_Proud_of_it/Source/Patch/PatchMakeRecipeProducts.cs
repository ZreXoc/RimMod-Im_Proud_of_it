using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ImP;

[HarmonyPatch(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts))]
public class PatchMakeRecipeProducts
{

    [HarmonyPostfix]
    static IEnumerable<Thing> Postfix(IEnumerable<Thing> __result,Pawn worker, RecipeDef recipeDef)
    {

        foreach (var thing in __result)
        {

            var thought = ImpCore.GetFixedThoughtDefOf(worker, recipeDef, thing);

            if (thought != null)
            {
                worker?.needs?.mood?.thoughts?.memories?.TryGainMemory(thought);
                // Log.Message($"thought:: days: {thought.durationDays}; mood: {thought.stages}");
            }

            yield return thing;
        }
    }
}