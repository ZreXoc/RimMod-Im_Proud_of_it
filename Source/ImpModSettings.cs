using Verse;

namespace ImP;

public class ImpModSettings:ModSettings
{
    public int QI_Offset = 0;
    public int Mood_Offset = 0;
    public int Time_Offset = 0;
    
    public override void ExposeData()
    {
        Scribe_Values.Look(ref QI_Offset, nameof(QI_Offset),0);
        Scribe_Values.Look(ref Mood_Offset, nameof(Mood_Offset),0);
        Scribe_Values.Look(ref Time_Offset, nameof(Time_Offset),0);
        base.ExposeData();
    }
}