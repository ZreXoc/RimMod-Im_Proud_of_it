using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ImP;

[HarmonyPatch(typeof(GenRecipe), nameof(GenRecipe.MakeRecipeProducts))]
public class PatchMakeRecipeProducts
{
    struct PassState
    {
        public Pawn worker;
        public RecipeDef recipeDef;
    }

    [HarmonyPrefix]
    static void Prefix(ref Pawn worker, ref RecipeDef recipeDef, out PassState __state)
    {
        __state = new PassState
        {
            worker = worker,
            recipeDef = recipeDef
        };
    }

    [HarmonyPostfix]
    static IEnumerable<Thing> Postfix(IEnumerable<Thing> __result, PassState __state)
    {
        var worker = __state.worker;
        var recipeDef = __state.recipeDef;
        // Log.Message(worker);
        // Log.Message(recipeDef.label);

        foreach (var thing in __result)
        {
            // Log.Message($"wa: {recipeDef.WorkAmountTotal(thing)}");

            // Log.Message(options.RQ_Offset);
            // Log.Message(options.Mood_Offset);
            // Log.Message(options.Time_Offset);

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