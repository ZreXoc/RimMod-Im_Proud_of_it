using System;
using HarmonyLib;
using Verse;
using UnityEngine;
using Listing_Standard = Verse.Listing_Standard;

namespace ImP;

public class Mod : Verse.Mod
{
    public Settings settings;

    public Mod(ModContentPack content) : base(content)
    {
        this.settings = GetSettings<Settings>();
        var harmony = new Harmony("zrex.ImP");
        harmony.PatchAll();
    }


    public override void DoSettingsWindowContents(Rect inRect)
    {
        Settings.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory() => "ImProudOfIt".Translate();
}