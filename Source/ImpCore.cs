using System;
using RimWorld;
using Verse;

namespace ImP;

public class ImpCore
{
    // 517:  
    private static readonly float K1 = 1, K2 = 1, K3 = 1, A = 2;
    private static readonly float CM = 1, CT = 0;
    private static readonly float S = 31000; // Work of chain shotgun

    /// <summary>
    /// level of skill.
    /// 0 -> level 0
    /// 1~4 -> level 1
    /// 5~10 -> level 2
    /// 11~18 -> level 3
    /// 19~ -> level 4
    /// </summary>
    private static readonly int[] LEVELS = [0, 1, 5, 10, 19, 9999];

    public struct Options
    {
        public float QI_Offset;
        public float Mood_Offset;
        public float Time_Offset;
    }

    public static ThoughtDef GetFixedThoughtDefOf(float skill, QualityCategory quality, float work, Options options)
    {
        float QI = GetQualityIndex(skill, quality);
        // Log.Message($"QI:{QI}, work:{work}");
        QI += options.QI_Offset;
        if (QI <= 0) return null;
        ThoughtDef def =
            QI >= 3 ? ImpThoughtDefOf.Pride_High :
            QI >= 2 ? ImpThoughtDefOf.Pride_Medium :
            ImpThoughtDefOf.Pride_Low;

        var (mood, time) = CalcMoodAndTime(quality, QI, work);

        def.stages[0].baseMoodEffect = mood + options.Mood_Offset;
        def.durationDays = time + options.Time_Offset / 24;
        return def;
    }

    private static (float, float) CalcMoodAndTime(QualityCategory quality, float QI, float work)
    {
        float mood = (float)(K1 * Math.Pow(A, QI) + K2 * Math.Log(work) / Math.Log(S) + CM);
        float time = (float)(K3 * work / S * QI + CT);
        if (quality == QualityCategory.Masterwork)
        {
            mood += 1;
            time += 1;
        }

        if (quality == QualityCategory.Legendary)
        {
            mood += 3;
            time += 3;
        }

        return (mood, time);
    }

    /// <summary>
    /// Returns a quality index that reflects the relative value of skill and quality
    /// <example>
    /// GetQualityIndex(8,3) -> 1;
    /// GetQualityIndex(20,2) -> -2
    /// </example>
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="quality"></param>
    /// <returns>quality index</returns>
    public static float GetQualityIndex(float skill, QualityCategory quality)
    {
        return (int)quality - (LEVELS.FirstIndexOf((l => skill < l)) - 1);
    }
}