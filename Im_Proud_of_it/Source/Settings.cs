using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace ImP;

public class Settings : ModSettings
{
    public static bool isFrustrationEnabled = true;

    public static float prideMoodFactor = 1;

    public static float prideDurationFactor = 1;

    public static int prideRQOffset = 0;


    public static float frustrationMoodFactor = 0.5f;

    public static float frustrationDurationFactor = 0.5f;

    public static int frustrationRQOffset = 0;

    public static int mounumentMood = 8;
    public static int mounumentDuration = 10;

    private static int exampleWorkAmount = 31000;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref mounumentMood, nameof(mounumentMood), 8);
        Scribe_Values.Look(ref mounumentDuration, nameof(mounumentDuration), 10);
        Scribe_Values.Look(ref prideDurationFactor, nameof(prideDurationFactor), 1);
        Scribe_Values.Look(ref prideMoodFactor, nameof(prideMoodFactor), 1);
        // Scribe_Values.Look(ref prideDurationOffset,nameof(prideDurationOffset),0);
        // Scribe_Values.Look(ref prideMoodOffset,nameof(prideMoodOffset),0);
        Scribe_Values.Look(ref prideRQOffset, nameof(prideRQOffset), 0);
        Scribe_Values.Look(ref frustrationMoodFactor, nameof(frustrationMoodFactor), 1);
        Scribe_Values.Look(ref frustrationDurationFactor, nameof(frustrationDurationFactor), 1);
        // Scribe_Values.Look(ref frustrationDurationOffset,nameof(frustrationDurationOffset),0);
        // Scribe_Values.Look(ref frustrationMoodOffset,nameof(frustrationMoodOffset),0);
        Scribe_Values.Look(ref frustrationRQOffset, nameof(frustrationRQOffset), 0);
        Scribe_Values.Look(ref exampleWorkAmount, nameof(exampleWorkAmount), 31000);
        base.ExposeData();
    }

    private static Vector2 scrollPositionLeft = Vector2.zero;
    private static Vector2 scrollPositionRight = Vector2.zero;

    public static void DoSettingsWindowContents(Rect inRect)
    {
        var list = new Listing_Standard();

        var outRectLeft = new Rect(inRect.x, inRect.y, inRect.width / 2, inRect.height);
        var outRectRight = new Rect(inRect.x + inRect.width / 2, inRect.y, inRect.width / 2, inRect.height);
        var scrollRectLeft = new Rect(0, 0, inRect.width / 2 - 16f, inRect.height);
        var scrollRectRight = new Rect(0, 0, inRect.width / 2 + 20f, inRect.height);
        Widgets.BeginScrollView(outRectLeft, ref scrollPositionLeft, scrollRectLeft, true);

        list.Begin(scrollRectLeft);
        list.Label("Settings_Desc".Translate());

        list.Gap(5);
        {
            {
                var sec = list.BeginSection(24 * 3);
                sec.Label("Monument".Translate());

                string moodBuffer = mounumentMood.ToString(),
                    durationBuffer = mounumentDuration.ToString();
                sec.TextFieldNumericLabeled($"{"Mood".Translate()}: ",
                    ref mounumentMood,
                    ref moodBuffer);
                sec.TextFieldNumericLabeled($"{"Duration".Translate()}: ",
                    ref mounumentDuration,
                    ref durationBuffer);
                sec.EndSection(sec);
            }

            list.Gap(5);
            {
                var sec = list.BeginSection(24 * 5);
                sec.Label("Pride".Translate());
                // TODO: sec.CheckboxLabeled($"{"Pride".Translate()}", ref isPrideEnabled);
                
                prideRQOffset =
                    (int)Math.Round(sec.SliderLabeled(
                        $"{"RQ_Offset".Translate()}: {prideRQOffset}",
                        prideRQOffset, 0, 6));

                prideMoodFactor =
                    (float)Math.Round(sec.SliderLabeled(
                        $"{"Mood_Factor".Translate()}: {prideMoodFactor}",
                        prideMoodFactor, 0, 3), 1);

                prideDurationFactor =
                    (float)Math.Round(sec.SliderLabeled(
                        $"{"Duration_Factor".Translate()}: {prideDurationFactor}",
                        prideDurationFactor, 0, 3), 1);

                list.EndSection(sec);
            }
            list.Gap(5);
            {
                var sec = list.BeginSection(24 * 5);
                sec.CheckboxLabeled($"{"Frustration".Translate()}", ref isFrustrationEnabled);
                frustrationRQOffset =
                    (int)Math.Round(sec.SliderLabeled(
                        $"{"RQ_Offset".Translate()}: {frustrationRQOffset}",
                        frustrationRQOffset, 0, 4));

                frustrationMoodFactor =
                    (float)Math.Round(sec.SliderLabeled(
                        $"{"Mood_Factor".Translate()}: {frustrationMoodFactor}",
                        frustrationMoodFactor, 0, 3), 1);

                frustrationDurationFactor =
                    (float)Math.Round(sec.SliderLabeled(
                        $"{"Duration_Factor".Translate()}: {frustrationDurationFactor}",
                        frustrationDurationFactor, 0, 3), 1);
                list.EndSection(sec);
            }
        }

        list.End();
        Widgets.EndScrollView();

        {
            Widgets.BeginScrollView(outRectRight, ref scrollPositionRight, scrollRectRight, true);
            list.Begin(scrollRectRight);
            DrawEgTable(list, exampleWorkAmount);
            list.End();
            Widgets.EndScrollView();
        }
    }

    // Hardcoded map for (skillLevel, qualityIndex) where probability is 0.00%
    // Quality indices: 0=Awful, 1=Poor, 2=Normal, 3=Good, 4=Excellent, 5=Masterwork, 6=Legendary
    private static readonly HashSet<(int skillLevel, int qualityIndex)> ZeroChanceMap =
    [
        (0, 4), // Excellent
        (0, 5), // Masterwork
        (0, 6), // Legendary (if exists, your table only goes to Masterwork)

        // Skill 1
        (1, 5), // Masterwork
        (1, 6), // Legendary

        // Skill 2
        (2, 5), // Masterwork
        (2, 6), // Legendary

        // Skill 3
        (3, 5), // Masterwork
        (3, 6), // Legendary

        // Skill 4
        (4, 6), // Legendary (not in table explicitly, but implied if masterwork is 0.01% it's impossible)

        // Skill 5
        (5, 6), // Legendary

        // From Skill 12 onwards, 'Awful' is 0.00%
        (12, 0), // Awful
        (13, 0), // Awful
        (14, 0), // Awful
        (15, 0), // Awful
        (16, 0), // Awful
        (17, 0), // Awful
        (18, 0), // Awful
        (19, 0), // Awful
        (20, 0), // Awful

        // Any other 0.00% explicitly from your table
        // For Skill 1 to Skill 11, Masterwork is not 0.00%
        // Check "Legendary" column for all skills if you have one.
        // Based on the provided snippet, Masterwork is never 0% from skill 6 onwards.
        // I'll add all Legendary occurrences as 0% for qualities up to 6.
        // If your game defines 7 quality levels (Awful to Legendary), and your table only shows 6,
        // then the 7th (Legendary) is effectively 0% for all those rows.
        // Assuming your QualityCategory enum has 7 levels, with index 6 being Legendary.

        // Adding Legendary for all skills as it's not shown in table but implied 0.00%
        (0, 6), (1, 6), (2, 6), (3, 6), (4, 6), (5, 6), (6, 6), (7, 6), (8, 6), (9, 6),
        (10, 6), (11, 6), (12, 6), (13, 6), (14, 6), (15, 6), (16, 6), (17, 6), (18, 6), (19, 6), (20, 6)
    ];


    public static void DrawEgTable(Listing_Standard list, int exampleWorkAmount)
    {
        list.Label("Example".Translate());
        list.Label("Example_Desc".Translate());

        string workAmountBuffer = exampleWorkAmount.ToString();
        list.TextFieldNumericLabeled("Work_Amount".Translate(), ref exampleWorkAmount, ref workAmountBuffer);

        var eg = ImpCore.GenerateExameple(exampleWorkAmount);

        int numSkillLevels = 21; // 0-20
        int numQualities = 7;    // QualityCategory enum count (Awful to Legendary)

        // Calculate maximum column widths for proper alignment
        float[] columnWidths = new float[1 + numQualities];

        // Calculate header row widths
        string skillHeader = "Skill".Translate();
        columnWidths[0] = Math.Max(columnWidths[0], Text.CalcSize(skillHeader).x);

        for (int j = 0; j < numQualities; j++)
        {
            string qualityLabel = ((QualityCategory)j).GetLabelShort();
            columnWidths[1 + j] = Math.Max(columnWidths[1 + j], Text.CalcSize(qualityLabel).x);
        }

        // Calculate data row widths
        for (int i = 0; i < numSkillLevels; i++)
        {
            // Skill Level column
            string skillLevelString = i.ToString();
            columnWidths[0] = Math.Max(columnWidths[0], Text.CalcSize(skillLevelString).x);

            // Data columns for each quality
            for (int j = 0; j < numQualities; j++)
            {
                string cellContent;
                if (ZeroChanceMap.Contains((i, j))) // Check against the hardcoded map
                {
                    cellContent = "-";
                }
                else
                {
                    var (mood, duration, score) = eg[i, j];
                    cellContent = $"{Math.Round(mood, 0)}x{Math.Round(duration, 1)}";
                }
                columnWidths[1 + j] = Math.Max(columnWidths[1 + j], Text.CalcSize(cellContent).x);
            }
        }

        float columnPadding = 8f;
        for (int k = 0; k < columnWidths.Length; k++)
        {
            columnWidths[k] += columnPadding;
        }

        StringBuilder strBuilder = new StringBuilder();

        // Helper for right-aligning text to a target pixel width
        string AlignRightToWidth(string text, float targetWidth)
        {
            float currentWidth = Text.CalcSize(text).x;
            if (currentWidth >= targetWidth) return text;
            float spaceWidth = Text.CalcSize(" ").x;
            int spacesNeeded = Mathf.CeilToInt((targetWidth - currentWidth) / spaceWidth);
            return new string(' ', spacesNeeded) + text;
        }

        // Helper for left-aligning text to a target pixel width
        string AlignLeftToWidth(string text, float targetWidth)
        {
            float currentWidth = Text.CalcSize(text).x;
            if (currentWidth >= targetWidth) return text;
            float spaceWidth = Text.CalcSize(" ").x;
            int spacesNeeded = Mathf.CeilToInt((targetWidth - currentWidth) / spaceWidth);
            return text + new string(' ', spacesNeeded);
        }

        // Title Row
        strBuilder.Append(AlignLeftToWidth(skillHeader, columnWidths[0]));
        for (int j = 0; j < numQualities; j++)
        {
            string qualityLabel = ((QualityCategory)j).GetLabelShort();
            strBuilder.Append(AlignRightToWidth(qualityLabel, columnWidths[1 + j]));
        }
        strBuilder.AppendLine();

        // Data Rows
        for (int i = 0; i < numSkillLevels; i++)
        {
            string skillLevelString = i.ToString();
            strBuilder.Append(AlignRightToWidth(skillLevelString, columnWidths[0]));

            for (int j = 0; j < numQualities; j++)
            {
                string cellContent;
                string finalText;

                if (ZeroChanceMap.Contains((i, j))) // Check against the hardcoded map
                {
                    cellContent = "-";
                    finalText = cellContent; // No color for "-"
                }
                else
                {
                    var (mood, duration, score) = eg[i, j];
                    cellContent = $"{Math.Round(mood, 0)}x{Math.Round(duration, 1)}";
                    
                    // Apply color only if mood and duration are both non-zero
                    if (mood != 0f && duration != 0f)
                    {
                        finalText = ImpRQColors.ColorText(cellContent, score);
                    }
                    else // If either mood or duration is 0, display without color
                    {
                        finalText = cellContent;
                    }
                }
                
                strBuilder.Append(AlignRightToWidth(finalText, columnWidths[1 + j]));
            }
            strBuilder.AppendLine();
        }

        list.Label(strBuilder.ToString());
    }
}

static class ImpRQColors
{
    // Define the hexadecimal color codes for various score ranges.
    public const string PrideHighColor = "#00FF00"; // Bright Green for high pride (>= 3.0)
    public const string PrideMediumColor = "#ADFF2F"; // GreenYellow for medium pride (>= 2.0)
    public const string PrideLowColor = "#90EE90"; // LightGreen for low pride (>= 1.0)

    public const string NormalColor = "#CCCCCC"; // Light Gray for neutral scores (between -1.0 and 1.0)

    public const string FrustrationLowColor = "#FFB6C1"; // LightPink for low frustration (<= -1.0)
    public const string FrustrationMediumColor = "#FF6347"; // Tomato for medium frustration (<= -2.0)
    public const string FrustrationHighColor = "#FF0000"; // Red for high frustration (<= -3.0)

    /// <summary>
    /// Gets the hexadecimal color string corresponding to a given float score.
    /// </summary>
    public static string GetColorHex(float score)
    {
        return score switch
        {
            >= 3f => PrideHighColor,
            >= 2f => PrideMediumColor,
            >= 1f => PrideLowColor,
            <= -3f => FrustrationHighColor,
            <= -2f => FrustrationMediumColor,
            <= -1f => FrustrationLowColor,
            _ => NormalColor // Default case if no other conditions are met (e.g., -1.0 < score < 1.0)
        };
    }

    /// <summary>
    /// Wraps a given text string with a color tag based on the provided float score.
    /// </summary>
    public static string ColorText(string text, float score)
    {
        var hexColor = GetColorHex(score);
        return $"<color={hexColor}>{text}</color>";
    }
}