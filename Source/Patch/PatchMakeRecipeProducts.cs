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

            var settings = LoadedModManager.GetMod<Mod>().GetSettings<ImpModSettings>();

            var options = new ImpCore.Options
            {
                QI_Offset = settings.QI_Offset,
                Mood_Offset = settings.Mood_Offset,
                Time_Offset = settings.Time_Offset,
            };


            // Log.Message(options.QI_Offset);
            // Log.Message(options.Mood_Offset);
            // Log.Message(options.Time_Offset);

            QualityCategory qc;

            if (worker.skills !=null && thing.TryGetQuality(out qc))
            {
                var level = worker.skills.GetSkill(recipeDef.workSkill).Level;
                var thought = ImpCore.GetFixedThoughtDefOf(level, qc,
                    recipeDef.WorkAmountTotal(thing), options);

                if (thought != null)
                {
                    worker?.needs?.mood?.thoughts?.memories?.TryGainMemory(thought);
                    // Log.Message($"thought:: days: {thought.durationDays}; mood: {thought.stages}");
                }
            }

            yield return thing;
        }
    }
}