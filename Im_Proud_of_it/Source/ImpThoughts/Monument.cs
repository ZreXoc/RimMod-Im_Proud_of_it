using RimWorld;
using Verse;

namespace ImP.ImpThoughts;

public class Monument
{
    public static void Register()
    {
        Find.SignalManager.RegisterReceiver(new MonumentCompletedSignalReceiver());
    }
}

class MonumentCompletedSignalReceiver : ISignalReceiver
{
    public void Notify_SignalReceived(Signal signal)
    {
        if (!signal.tag.Contains("MonumentCompleted") || !signal.args.TryGetArg(0, out var named) || named.label != "SUBJECT") return;

        if (named.arg is not MonumentMarker monumentMarker) return;
        
        Log.Message($"Monitor Completed, size: {monumentMarker.Size}");
        // TODO: based on monument size
        // _monumentMarker.Size;
        var def = ImpThoughtDefOf.ImP_Pride_Monument;
        def.stages[0].baseMoodEffect = Settings.mounumentMood;
        def.durationDays = Settings.mounumentDuration;
        foreach (var pawn in PawnsFinder.HomeMaps_FreeColonistsSpawned)
        {
            pawn.needs?.mood?.thoughts?.memories.TryGainMemory(ImpThoughtDefOf.ImP_Pride_Monument);
        }
    }
}