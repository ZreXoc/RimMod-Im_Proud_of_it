using System;
using System.Linq;
using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace ImP;

//TODO
[HarmonyPatch(typeof(MonumentMarker), nameof(MonumentMarker.Tick))]
public class PatchMounumentMakerTick
{
    [HarmonyPrefix]
    public static void Prefix(MonumentMarker __instance)
    {
      if (!__instance.IsHashIntervalTick(177))
        return;
      
      // Log.Message("monument complete");
      // When Complete
      if (!__instance.complete & __instance.AllDone)
      {
        var def = ImpThoughtDefOf.ImP_Pride_Monument;
        def.stages[0].baseMoodEffect = Settings.mounumentMood;
        def.durationDays = Settings.mounumentDuration;
          foreach (var pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
          {
              pawn.needs?.mood?.thoughts?.memories.TryGainMemory(ImpThoughtDefOf.ImP_Pride_Monument);
          }
      }
    }
}