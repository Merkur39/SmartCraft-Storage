using System.Collections.Generic;
using SmartCraftStorage.Shared;

namespace SmartCraftStorage.Stations
{
    // The windmill, spinning wheel, blast furnace and eitr refinery are all built as a
    // Smelter, so the per-station switches have to tell them apart by name. The name
    // comes from the prefab, like the kiln lookup in KilnDetection.
    internal static class SmelterStations
    {
        private static readonly HashSet<string> Seen = new HashSet<string>();

        public static bool IsAutomationEnabled(Smelter smelter)
        {
            string name = smelter.m_name;
            LogOnce(name);

            switch (name)
            {
                case "$piece_windmill":
                    return StationConfig.WindmillAutomation.Value;
                case "$piece_spinningwheel":
                    return StationConfig.SpinningWheelAutomation.Value;
                case "$piece_blastfurnace":
                    return StationConfig.BlastFurnaceAutomation.Value;
                case "$piece_eitrrefinery":
                    return StationConfig.EitrRefineryAutomation.Value;
                default:
                    return true;
            }
        }

        // Lets a report show which names the game actually uses for each station.
        private static void LogOnce(string name)
        {
            if (name != null && Seen.Add(name))
            {
                DebugLog.Log("[SmartCraftStorage] Smelter-type station seen: m_name=" + name);
            }
        }
    }
}
