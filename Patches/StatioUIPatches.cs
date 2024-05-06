using HarmonyLib;
using TMPro;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(StatioUI))]
    internal class StatioUIPatches
    {


        [HarmonyPatch("UpdatePrerequisites")]
        [HarmonyPostfix]
        public static void UpdatePrerequisites_Postfix(StatioUI __instance, TextMeshProUGUI ___productionText, TextMeshProUGUI ___productionValue)
        {
            var productionValue = ___productionValue;
            var productionText = ___productionText;

            productionText.text = string.Format("Production of {0}".FromDictionary(), BaseUI.Outpost.ProducedGoods.GetName(false).ToLower());
            int minProduction = BaseUI.Outpost.GetMinProduction();
            int maxProduction = BaseUI.Outpost.GetMaxProduction();
            string arg = ((maxProduction > 0) ? $"- {maxProduction}" : "");
            productionValue.text = $"<sprite=\"icon_collector\" index=4> {minProduction} {arg} / " + "day".FromDictionary();


        }
    }
}