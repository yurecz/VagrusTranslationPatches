using HarmonyLib;
using TMPro;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required

    /// <summary>
    /// Sample Harmony Patch class. Suggestion is to use one file per patched class
    /// though you can include multiple patch classes in one file.
    /// Below is included as an example, and should be replaced by classes and methods
    /// for your mod.
    /// </summary>
    [HarmonyPatch(typeof(CampUI))]
    internal class CampUIPatches
    {
        [HarmonyPatch("UpdateUpkeep")]
        [HarmonyPostfix]
        public static void Refresh_Postfix(CampUI __instance)
        {
           var unpaidDaysText = Game.game.camp.GetUnpaidDays().FormatNumberByNomen("day");
           var unpaidDays = Traverse.Create(Game.game.caravan.campUI).Field("unpaidDays").GetValue() as TextMeshProUGUI;
           unpaidDays.text = unpaidDaysText;
        }
    }
}