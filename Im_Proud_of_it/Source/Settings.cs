using System;
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
                var sec = list.BeginSection(24 * 6);
                sec.Label("Frustration".Translate());

                sec.CheckboxLabeled($"{"Frustration_Enabled".Translate()}", ref isFrustrationEnabled);
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

        Widgets.BeginScrollView(outRectRight, ref scrollPositionRight, scrollRectRight, true);
        list.Begin(scrollRectRight);
        {
            list.Label("Example".Translate());
            list.Label("Example_Desc".Translate());

            string workAmountBuffer = exampleWorkAmount.ToString();
            list.TextFieldNumericLabeled("Work_Amount".Translate(), ref exampleWorkAmount, ref workAmountBuffer);


            // Preview
            var eg = ImpCore.GenerateExameple(exampleWorkAmount);
            string str = "";
            const int ALIGN = 10;

            // align for Chinese Character
            string AlignFixed(string str, int align)
            {
                int width = str.Sum(ch => (ch >= 0x4e00 && ch <= 0x9fbb) ? 2 : 1);
                return new string(' ', Math.Max(align - width, 0)) + str;
            }

            // title
            str += AlignFixed("Skill".Translate(), 5);
            for (int i = 0; i < 7; i++)
            {
                string quality = ((QualityCategory)i).GetLabelShort();
                // str+=quality+new string(' ',align-Encoding.GetEncoding("gb2312").GetBytes(quality).Length); 
                // str += $"{quality,align - 1}";
                str += AlignFixed(quality, ALIGN);
            }

            str += "\n";

            for (int i = 0; i < 21; i++)
            {
                str += $"{i,5}";
                for (int j = 0; j < 7; j++)
                {
                    var (mood, duration) = eg[i, j];
                    var tmp = $"{Math.Round(mood)}x{Math.Round(duration, 1)}" + (j == 6 ? '\n' : ',');
                    str += $"{tmp,ALIGN}";
                }
            }

            // Log.Message(str);
            list.Label(str);
        }
        list.End();
        Widgets.EndScrollView();
    }
}