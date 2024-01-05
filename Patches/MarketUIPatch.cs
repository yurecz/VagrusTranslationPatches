using HarmonyLib;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(MarketUI))]
    internal class CalenMarketUIPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(MarketUI __instance)
        {
            __instance.btnCargoText.text = "Goods";
        }
    }
}