using System;
using HarmonyLib;
using Verse;
using UnityEngine;

namespace ImP;

public class Mod : Verse.Mod
{
    public ImpModSettings settings;

    public Mod(ModContentPack content) : base(content)
    {
        this.settings = GetSettings<ImpModSettings>();
        var harmony = new Harmony("zrex.ImP");
        harmony.PatchAll();
        

    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var listingStandard = new Listing_Standard();
        listingStandard.Begin(inRect);

        {

            listingStandard.Label("Settings_Desc".Translate());
            settings.QI_Offset =
                (int)Math.Round(listingStandard.SliderLabeled($"{"QI_Offset".Translate()}: {settings.QI_Offset}",
                    settings.QI_Offset, -6, 6));
            string moodBuffer = settings.Mood_Offset.ToString(), timeBuffer = settings.Time_Offset.ToString();
            listingStandard.TextFieldNumericLabeled($"{"Mood_Offset".Translate()}: ", ref settings.Mood_Offset,
                ref moodBuffer);
            listingStandard.TextFieldNumericLabeled($"{"Time_Offset".Translate()}: ", ref settings.Time_Offset,
                ref timeBuffer);
        }

        listingStandard.End();

        base.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory()=>"ImProudOfIt".Translate();
    
}