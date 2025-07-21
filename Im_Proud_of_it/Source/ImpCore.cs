using System;
using RimWorld;
using Verse;

namespace ImP;

public class ImpCore
{
    private static readonly float K1 = 1, K2 = 1, K3 = 1, A = 2;
    private static readonly float CM = 1, CT = 0;
    private static readonly float S = 31000; // Work of chain shotgun

    /// <summary>
    ///     level of skill.
    ///     0 -> level 0
    ///     1~4 -> level 1
    ///     5~10 -> level 2
    ///     11~18 -> level 3
    ///     19~ -> level 4
    /// </summary>
    private static readonly int[] LEVELS = [0, 1, 5, 10, 19, 9999];


    public static ThoughtDef GetThoughtDefOfByRQ(float RQ)
    {
        return RQ switch
        {
            >= 3 => ImpThoughtDefOf.ImP_Pride_High,
            >= 2 => ImpThoughtDefOf.ImP_Pride_Medium,
            >= 1 => ImpThoughtDefOf.ImP_Pride_Low,
            <= -3 => ImpThoughtDefOf.ImP_Frustration_High,
            <= -2 => ImpThoughtDefOf.ImP_Frustration_Medium,
            <= -1 => ImpThoughtDefOf.ImP_Frustration_Low,
            _ => null
        };
    }

    // Should be Construction
    public static ThoughtDef GetFixedThoughtDefOf(Pawn worker, Thing thing)
    {
        QualityCategory quality;

        if (worker?.skills == null || !thing.TryGetQuality(out quality)) return null;
        // Log.Message(quality);

        // if (thing.HasThingCategory(ThingCategoryDefOf.Buildings))
        if (thing.def.category != ThingCategory.Building)
        {
            Log.ErrorOnce($"{thing} is not in the category of Buildings", "Imp_not_a_building".GetHashCode());
            return null;
        }

        quality -= (QualityCategory)GetRoleEffectQualityOffset(worker);
        var level = worker.skills.GetSkill(SkillDefOf.Construction).Level;
        var workAmount = thing.GetStatValue(StatDefOf.WorkToBuild);
        var thought = GetFixedThoughtDefOf(level, quality, workAmount);
        return thought;
    }

    // Craft and sculpture
    public static ThoughtDef GetFixedThoughtDefOf(Pawn worker, RecipeDef recipeDef, Thing thing)
    {
        QualityCategory quality;

        if (worker?.skills == null || !thing.TryGetQuality(out quality)) return null;

        if (recipeDef.workSkill == SkillDefOf.Crafting) quality -= (QualityCategory)GetRoleEffectQualityOffset(worker);

        var level = worker.skills.GetSkill(recipeDef.workSkill).Level;
        var thought = GetFixedThoughtDefOf(level, quality,
            recipeDef.WorkAmountTotal(thing));

        return thought;
    }

    private static int GetRoleEffectQualityOffset(Pawn worker)
    {
        if (worker.Ideo == null) return 0;
        var role = worker.Ideo.GetRole(worker);
        var roleEffect =
            role?.def.roleEffects?.FirstOrDefault(eff => eff is RoleEffect_ProductionQualityOffset);
        return ((RoleEffect_ProductionQualityOffset)roleEffect)?.offset ?? 0;
    }

    public static ThoughtDef GetFixedThoughtDefOf(float skill, QualityCategory quality, float work)
    {
        if (!GetParams(skill, quality, out var RQ, out var moodFactor, out var durationFactor)) return null;

        var def = GetThoughtDefOfByRQ(RQ);

        var (mood, duration) = CalcMoodAndDuration(quality, RQ, work, moodFactor, durationFactor);

        def.stages[0].baseMoodEffect = mood;
        def.durationDays = duration;

        return def;
    }

    private static bool GetParams(float skill, QualityCategory quality, out float RQ, out float moodFactor,
        out float durationFactor, bool isFrustrainEnabled = true, float RQOffset = 0)
    {
        RQ = GetQualityIndex(skill, quality) + RQOffset;

        // Log.Message($"RQ:{RQ}, work:{work}");

        moodFactor = 0;
        durationFactor = 0;

        if (RQ > 0)
        {
            RQ -= ImpSettings.prideRQOffset;
            if (RQ <= 0) return false;
            moodFactor = ImpSettings.prideMoodFactor;
            durationFactor = ImpSettings.prideDurationFactor;
        }
        else if (RQ < 0)
        {
            if (!isFrustrainEnabled) return false;
            RQ += ImpSettings.frustrationRQOffset;
            if (RQ >= 0) return false;
            moodFactor = ImpSettings.frustrationMoodFactor;
            durationFactor = ImpSettings.frustrationDurationFactor;
        }
        else
        {
            return false;
        }

        return true;
    }

    public static (float, float) CalcMoodAndDuration(QualityCategory quality, float RQ, float workAmount,
        float moodFactor = 1,
        float durationFactor = 1)
    {
        if (RQ < 0) moodFactor *= -1;
        RQ = Math.Abs(RQ);
        var mood = (float)(K1 * Math.Pow(A, RQ) + K2 * Math.Log(workAmount) / Math.Log(S) + CM);
        var duration = K3 * Math.Sqrt(workAmount / S) * Math.Log(RQ + 1) + CT;

        mood *= moodFactor;
        duration *= durationFactor;

        return (mood, (float)duration);
    }

    /// <summary>
    ///     Returns relative quality that reflects the relative value of skill and quality
    ///     <example>
    ///         GetQualityIndex(8,3) -> 1;
    ///         GetQualityIndex(20,2) -> -2
    ///     </example>
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="quality"></param>
    /// <returns>relative quality</returns>
    public static float GetQualityIndex(float skill, QualityCategory quality)
    {
        return (int)quality - (LEVELS.FirstIndexOf(l => skill < l) - 1);
    }

    public static (float, float, float)[,] GenerateExameple(int workAmount)
    {
        var eg = new (float, float, float)[21, 7];

        for (var i = 0; i < 21; i++)
        for (var j = 0; j < 7; j++)
        {
            GetParams(i, (QualityCategory)j, out var RQ, out var moodFactor, out var durationFactor,
                ImpSettings.isFrustrationEnabled);
            var (mood, duration) = CalcMoodAndDuration((QualityCategory)j, RQ, workAmount, moodFactor, durationFactor);
            eg[i, j] = (mood, duration, RQ);
        }

        return eg;
    }
}