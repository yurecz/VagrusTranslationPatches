using HarmonyLib;
using System.Diagnostics.Eventing.Reader;
using TMPro;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.PriceHistory
{
    [HarmonyPatch(typeof(PriceHistoryColumn))]
    internal class PriceHistoryColumnPatch
    {

        [HarmonyPatch("SetBuyPrice")]
        [HarmonyPostfix]
        public static void SetBuyPrice_Postfix(PriceHistoryColumn __instance, GameObject ___buyHolder, int columnidx, TextMeshProUGUI ___buyValue, Goods goods)
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
        public static void SetSellPrice_Postfix(PriceHistoryColumn __instance, GameObject ___sellHolder, int columnidx, TextMeshProUGUI ___sellValue, Goods goods)
        {
            if (PriceHistoryBoxPatch.displayPriceToggle == 2)
            {
                if (columnidx == 0)
                {
                    var marketInfo = __instance.GetSettlement().FindLastMarket(goods.GetID());
                    ___sellValue.text = $"{"Available".FromDictionary()}: <size=50><sprite=\"icon_collector\" index=26></size>{marketInfo.qty}";
                    ___sellHolder.SetActive(true);
                    ___sellHolder.FindDeep("Title").SetActive(false);
                    ___sellHolder.FindDeep("PriceBorder").SetActive(false);
                }
                else
                {
                    ___sellHolder.SetActive(true);
                    ___sellHolder.FindDeep("PriceBorder").SetActive(true);
                }
            }
            else if (PriceHistoryBoxPatch.displayPriceToggle == 3)
            {
                if (columnidx == 0)
                {
                    ___sellHolder.SetActive(true);
                    ___sellHolder.FindDeep("Title").SetActive(true);
                    ___sellHolder.FindDeep("PriceBorder").SetActive(true);
                }
                else
                {
                    var marketInfo = __instance.GetSettlement().FindLastMarket(goods.GetID());
                    ___sellValue.text = $"{"News".FromDictionary()}: <size=50><sprite=\"icon_collector\" index=26></size>{marketInfo.qty}";
                    ___sellHolder.SetActive(true);
                    ___sellHolder.FindDeep("PriceBorder").SetActive(false);
                }
            }
            else
            {
                ___sellHolder.SetActive(true);
                if (columnidx == 0)
                {
                    ___sellHolder.FindDeep("Title").SetActive(true);
                }
                ___sellHolder.FindDeep("PriceBorder").SetActive(true);

            }

        }

    }
}