using HarmonyLib;
using TMPro;
using Vagrus.UI;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(StickyUIValueDisplay))]
    internal class StickyUIValueDisplayPatches
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void Update_Postfix(StickyUIValueDisplay __instance)
        {
            int supplyDays = Game.game.caravan.GetSupplyDays(int.MaxValue);
            var suppliesText = Traverse.Create(__instance).Field("supplies").GetValue() as TextMeshProUGUI;
            suppliesText.text = supplyDays.FormatNumberByNomen("day");
            if (supplyDays <= CaravanTweak.WarningSupplyDays)
            {
                suppliesText.text = string.Concat(new string[]
                    {   "<color=",
                        VisualTweak.Red,
                        ">",
                        suppliesText.text,
                        "</color>"
                    });
            }
        }
    }
}