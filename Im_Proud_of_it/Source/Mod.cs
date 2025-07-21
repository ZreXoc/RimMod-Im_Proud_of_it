using HarmonyLib;
using Verse;
using UnityEngine;

namespace ImP;

// ReSharper disable once UnusedType.Global
public class ImpMod : Mod
{

    public ImpMod(ModContentPack content) : base(content)
    {
        var harmony = new Harmony("zrex.ImP");
        harmony.PatchAll();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Settings.DoSettingsWindowContents(inRect);
    }

    public override string SettingsCategory() => "ImProudOfIt".Translate();
}

// ReSharper disable once UnusedType.Global
public class ImpGameComponent : GameComponent
{
    public ImpGameComponent()
    {
    }
    
    public ImpGameComponent(Game game)
    {
    }
    public override void LoadedGame()
    {
        ImpThoughts.Monument.Register();
    }
}