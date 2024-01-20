using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(ScoutUI))]
    internal class ScoutUIPatches
    {

        [HarmonyPatch("GetFilterTooltip")]
        [HarmonyPostfix]
        public static void GetFilterTooltip_Postfix(ScoutUI __instance, string __result, ScoutResultFilter type, bool interactable = true)
        {
                if (!interactable)
                {
                __result = "No results".FromDictionary();
                return;
                }
                string typeName = type.ToString().ToLower();
                __result = type switch
                {
                    ScoutResultFilter.Fail => "Failed results".FromDictionary(),
                    ScoutResultFilter.All => "All results".FromDictionary(),
                    ScoutResultFilter.EventTask => "Event/Task results".FromDictionary(),
                    ScoutResultFilter.Valuable => "Top results".FromDictionary(),
                    _ => ("Top " + typeName + " results").FromDictionary(),
                };
            }
        }
}