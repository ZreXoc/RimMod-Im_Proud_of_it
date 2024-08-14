using System;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace ImP;

public class Settings : ModSettings
{
    public static bool isFrustrationEnabled = true;

    public static float prideMoodFactor = 1;

    public static float prideTimeFactor = 1;

    // public int prideTimeOffset = 0;
    // public int prideMoodOffset = 0;
    public static int prideRQOffset = 0;


    public static float frustrationMoodFactor = 0.5f;

    public static float frustrationTimeFactor = 0.5f;

    // public int frustrationTimeOffset = 0;
    // public int frustrationMoodOffset = 0;
    public static int frustrationRQOffset = 0;

    public static int mounumentMood = 8;
    public static int mounumentTime = 10;

    public override void ExposeData()
    {
        Scribe_Values.Look(ref mounumentMood, nameof(mounumentMood), 8);
        Scribe_Values.Look(ref mounumentTime, nameof(mounumentTime), 10);
        Scribe_Values.Look(ref prideTimeFactor, nameof(prideTimeFactor), 0);
        Scribe_Values.Look(ref prideMoodFactor, nameof(prideMoodFactor), 0);
        // Scribe_Values.Look(ref prideTimeOffset,nameof(prideTimeOffset),0);
        // Scribe_Values.Look(ref prideMoodOffset,nameof(prideMoodOffset),0);
        Scribe_Values.Look(ref prideRQOffset, nameof(prideRQOffset), 0);
        Scribe_Values.Look(ref frustrationMoodFactor, nameof(frustrationMoodFactor), 0);
        Scribe_Values.Look(ref frustrationTimeFactor, nameof(frustrationTimeFactor), 0);
        // Scribe_Values.Look(ref frustrationTimeOffset,nameof(frustrationTimeOffset),0);
        // Scribe_Values.Look(ref frustrationMoodOffset,nameof(frustrationMoodOffset),0);
        Scribe_Values.Look(ref frustrationRQOffset, nameof(frustrationRQOffset), 0);
        base.ExposeData();
    }

    public static Vector2 scrollPositionLeft = Vector2.zero;
    public static Vector2 scrollPositionRight = Vector2.zero;

    public static void DoSettingsWindowContents(Rect inRect)
    {
        var list = new Listing_Standard();

        var outRectLeft = new Rect(inRect.x, inRect.y, inRect.width / 2, inRect.height);
        var outRectRight = new Rect(inRect.x + inRect.width / 2, inRect.y, inRect.width / 2, inRect.height);
        var scrollRectLeft = new Rect(0, 0, inRect.width / 2 - 16f, inRect.height * 2f + 50);
        var scrollRectRight = new Rect(0, 0, inRect.width / 2 + 20f, inRect.height);
        Widgets.BeginScrollView(outRectLeft, ref scrollPositionLeft, scrollRectLeft, true);

        list.Begin(scrollRectLeft);
        {
            list.Label("Settings_Desc".Translate());

            list.CheckboxLabeled($"{"Frustration_Enabled".Translate()}", ref isFrustrationEnabled);
                // string moodBuffer = prideMoodOffset.ToString(),
                         //     timeBuffer = prideTimeOffset.ToString();
                         // listingStandard.TextFieldNumericLabeled($"{"Mood_Offset".Translate()}: ",
                         //     ref prideMoodOffset,
                         //     ref moodBuffer);
                         // listingStandard.TextFieldNumericLabeled($"{"Time_Offset".Translate()}: ",
                         //     ref prideTimeOffset,
                         //     ref timeBuffer);   
                         //TODO
                /*
                mounumentMood =
                    (int)Math.Round(list.SliderLabeled(
                        $"{"Monument_Mood".Translate()}: {mounumentMood}",
                        prideRQOffset, 0, 30));
            
                mounumentTime =
                    (int)Math.Round(list.SliderLabeled(
                        $"{"Monument_Mood".Translate()}: {mounumentMood}",
                        prideRQOffset, 0, 30));
            */
            list.GapLine();

            {
                list.Label("Pride".Translate());
                prideRQOffset =
                    (int)Math.Round(list.SliderLabeled(
                        $"{"RQ_Offset".Translate()}: {prideRQOffset}",
                        prideRQOffset, 0, 3));

                prideMoodFactor =
                    (float)Math.Round(list.SliderLabeled(
                        $"{"Mood_Factor".Translate()}: {prideMoodFactor}",
                        prideMoodFactor, 0, 3), 1);

                prideTimeFactor =
                    (float)Math.Round(list.SliderLabeled(
                        $"{"Time_Factor".Translate()}: {prideTimeFactor}",
                        prideTimeFactor, 0, 3), 1);

                // string moodBuffer = prideMoodOffset.ToString(),
                //     timeBuffer = prideTimeOffset.ToString();
                // listingStandard.TextFieldNumericLabeled($"{"Mood_Offset".Translate()}: ",
                //     ref prideMoodOffset,
                //     ref moodBuffer);
                // listingStandard.TextFieldNumericLabeled($"{"Time_Offset".Translate()}: ",
                //     ref prideTimeOffset,
                //     ref timeBuffer);
            }
            list.GapLine();
            {
                list.Label("Frustration".Translate());
                frustrationRQOffset =
                    (int)Math.Round(list.SliderLabeled(
                        $"{"RQ_Offset".Translate()}: {frustrationRQOffset}",
                        frustrationRQOffset, 0, 3));

                frustrationMoodFactor =
                    (float)Math.Round(list.SliderLabeled(
                        $"{"Mood_Factor".Translate()}: {frustrationMoodFactor}",
                        frustrationMoodFactor, 0, 3), 1);

                frustrationTimeFactor =
                    (float)Math.Round(list.SliderLabeled(
                        $"{"Time_Factor".Translate()}: {frustrationTimeFactor}",
                        frustrationTimeFactor, 0, 3), 1);


                /*
                string moodBuffer = frustrationMoodOffset.ToString(),
                    timeBuffer = frustrationTimeOffset.ToString();
                listingStandard.TextFieldNumericLabeled($"{"Mood_Offset".Translate()}: ",
                    ref frustrationMoodOffset,
                    ref moodBuffer);
                listingStandard.TextFieldNumericLabeled($"{"Time_Offset".Translate()}: ",
                    ref frustrationTimeOffset,
                    ref timeBuffer);
            */
            }
        }

        list.End();
        Widgets.EndScrollView();

        Widgets.BeginScrollView(outRectRight, ref scrollPositionRight, scrollRectRight, true);
        list.Begin(scrollRectRight);
        {
            list.Label("Example".Translate());
            list.Label("Example_Desc".Translate());


            var eg = ImpCore.GenerateExameple(31000);
            string str = "";
            const int ALIGN = 10;

            // fix for Chinese Character
            string AlignFixed(string str, int align)
            {
                int width = str.Length;
                if (str[0] >= 0x4e00 && str[0] <= 0x9fbb) width *= 2;
                return new string(' ', Math.Max( align - width,0)) + str;
            }

            // title
            str += AlignFixed("Skill".Translate(),5);
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
                    var (mood, time) = eg[i, j];
                    var tmp = $"{Math.Round(mood)}x{Math.Round(time, 1)}" + (j == 6 ? '\n' : ',');
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