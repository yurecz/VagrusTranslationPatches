using HarmonyLib;
using System.Diagnostics.Eventing.Reader;
using TMPro;
using UnityEngine;

namespace VagrusTranslationPatches.PriceHistory
{
    [HarmonyPatch(typeof(PriceHistoryColumn))]
    internal class PriceHistoryColumnPatch
    {

        [HarmonyPatch("SetBuyPrice")]
        [HarmonyPostfix]
        public static void SetBuyPrice_Postfix(PriceHistoryColumn __instance, GameObject ___buyHolder, int columnidx, TextMeshProUGUI ___buyValue)
        {
            if (PriceHistoryBoxPatch.displayPriceToggle == 2)
            {
                ___buyHolder.SetActive(columnidx == 0);
            }
            else if (PriceHistoryBoxPatch.displayPriceToggle == 3)
            {
                ___buyHolder.SetActive(columnidx != 0);
            }
            else ___buyHolder.SetActive(true);
        }


        [HarmonyPatch("SetSellPrice")]
        [HarmonyPostfix]
        public static void SetSellPrice_Postfix(PriceHistoryColumn __instance, GameObject ___sellHolder, int columnidx, TextMeshProUGUI ___sellValue)
        {
            if (PriceHistoryBoxPatch.displayPriceToggle == 2)
            {
                ___sellHolder.SetActive(columnidx != 0);
            }
            else if (PriceHistoryBoxPatch.displayPriceToggle == 3)
            {
                ___sellHolder.SetActive(columnidx == 0);
            }
            else ___sellHolder.SetActive(true);

        }

    }
}