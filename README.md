# I'm Proud of it! (ImP)

## Overview

Im_Proud_of_it (ImP) is a mod for RimWorld that adds a layer of emotional feedback to the crafting experience. Pawns will gain a sense of pride after crafting a high-quality item or building a monument! Conversely, crafting a shoddy product will lead to frustration.

The mood bonus is influenced by item quality, the pawn's crafting skill, and the amount of work. As a baseline, crafting a "Good" quality Chain Shotgun with a Crafting skill of 8 grants a +4 mood bonus for one day. These values can be viewed and adjusted in the mod's settings.

## Features

*   **Emotional Crafting Feedback:** Pawns feel `Pride` for high-quality crafts and monument completions, and optionally `Frustration` for low-quality outcomes.
*   **Dynamic Mood & Duration Calculation:** Mood buffs and durations are based on item quality, pawn skill, and work amount.
*   **Detailed In-Game Settings:** Adjustable parameters for `Relative Quality offset`, `Mood offset`, `Duration offset (hours)`, `Mood factor`, `Duration factor`, and `Monument` impact.
*   **Frustration Toggle:** Option to `Enable Frustration` for a more challenging experience.
*   **Example Table:** Provides a clear table showing `Mood buff × Duration (days)` for different crafting `Skill` levels and item qualities. (Note: It's impossible to produce very high or very low quality items at certain skill levels.)

## Installation

### Steam Workshop (Recommended)

1.  Subscribe to the `[Im_Proud_of_it (ImP)]` mod on Steam.
2.  Launch RimWorld and enable `Im_Proud_of_it (ImP)` in the Mods menu.

### Manual Installation

1.  Download the latest version of the mod package from reputable sources.
2.  Extract the downloaded archive. The mod folder should typically contain subfolders like `About` and `Defs`.
3.  Place the extracted mod folder into your RimWorld Mods folder:
    *   Windows: `%userprofile%\AppData\LocalLow\Ludeon Studios\RimWorld\Mods\`
    *   Mac: `~/Library/Application Support/RimWorld/Mods/`
    *   Linux: `~/.config/unity3d/Ludeon Studios/RimWorld/Mods/`
4.  Launch RimWorld and enable `Im_Proud_of_it (ImP)` in the Mods menu.

## Usage

Once enabled, you can configure Im_Proud_of_it (ImP)'s settings through the standard RimWorld Mod Settings menu. Access it via `Options -> Mod Settings`.

The `Settings_Desc` explains the core logic: "The mood bonus and duration are determined by Relative Quality. Relative Quality is the difference between the item's quality rank and the pawn's crafting skill rank. This allows pawns with lower skills to also feel pride when crafting good items. The quality ranks are specified as follows:
0: Awful / Skill Level 0
1: Poor / Skill Level 1-4
2: Normal / Skill Level 5-9
3: Good / Skill Level 10-18
4: Excellent / Skill Level 19+
5: Masterwork
6: Legendary
For example: If a pawn with a crafting skill of 8 crafts a Chain Shotgun (Good), their Relative Quality is 3 - 2 = 1."

The settings panel will allow you to adjust:
*   `RQ_Offset` (Relative Quality offset)
*   `Mood_Offset`
*   `Duration_Offset` (hours)
*   `Mood_Factor`
*   `Duration_Factor`
*   `Monument` (Mood bonus for monument completion)
*   `Frustration_Enabled` (Toggle for frustration effect)
*   `Work_Amount` (ticks, for example table preview)

The `Example` section in settings will display "The gains (Mood buff × Duration (days)) for different crafting skill levels and item qualities are shown below."

## Compatibility

May have compatibility issues with some races and Mechanical mods, but works well most of times. If there are any problems, please report them to me!

## Contributing

Contributions are welcome! Feel free to report issues, suggest features, or submit pull requests.