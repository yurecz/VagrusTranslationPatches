using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(MarketUI))]
    internal class CalenMarketUIPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(MarketUI __instance, GameObject ___noTradeOffers)
        {
            __instance.btnCargoText.text = "Goods";

            ___noTradeOffers.AddIfNotExistComponent<UIElementTranslator>();
        }
    }
}