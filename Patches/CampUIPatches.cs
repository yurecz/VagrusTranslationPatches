using HarmonyLib;
using TMPro;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(CampUI))]
    internal class CampUIPatches
    {
        [HarmonyPatch("UpdateUpkeep")]
        [HarmonyPostfix]
        public static void UpdateUpkeep_Postfix(CampUI __instance)
        {
           var unpaidDaysText = Game.game.camp.GetUnpaidDays().FormatNumberByNomen("day");
           var unpaidDays = Traverse.Create(Game.game.caravan.campUI).Field("unpaidDays").GetValue() as TextMeshProUGUI;
           unpaidDays.text = unpaidDaysText;
        }
    }
}