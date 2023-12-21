using HarmonyLib;
using TMPro;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(MiniCaravanUI))]
    internal class MiniCaravanUIPatches
    {
        [HarmonyPatch("UpdateValues")]
        [HarmonyPostfix]
        public static void UpdateValues_Postfix(MiniCaravanUI __instance)
        {
            int supplyDays = Game.game.caravan.GetSupplyDays(int.MaxValue);
            var suppliesText = Traverse.Create(__instance).Field("suppliesText").GetValue() as TextMeshProUGUI;
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