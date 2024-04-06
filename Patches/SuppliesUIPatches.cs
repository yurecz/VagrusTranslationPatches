using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(SuppliesUI))]
    internal class SuppliesUIPatches
    {

        [HarmonyPatch("FormatLeftBottom")]
        [HarmonyPostfix]
        public static void FormatLeftBottom_Postfix(SuppliesUI __instance, HFChance hfChance, ref string __result)
        {
            string text = "";
            if (hfChance.type == HFType.Hunting)
            {
                string text2 = ((hfChance.maxValue == 0) ? VisualTweak.Red : VisualTweak.Black);
                string text3 = "<sprite=\"icon_collector\" index=41><color=" + text2 + ">" + hfChance.minValue + " - " + hfChance.maxValue + "</color>";
                text += text3;
            }
            else if (hfChance.type == HFType.OddJobs)
            {
                int property = Game.game.caravan.GetProperty(Prop.OddJobsBase);
                string text4 = property > 0 ? string.Format("{0}% bonus".FromDictionary(), String.Sign(Mathf.RoundToInt(property))) : "";
                text += text4;
            }

            __result = text;
        }

        [HarmonyPatch("FormatRightBottom")]
        [HarmonyPostfix]
        public static void FormatRightBottom_Postfix(SuppliesUI __instance, HFChance hfChance, ref string __result)
        {
            string text = "";
            if (hfChance.type == HFType.Foraging)
            {
                string text2 = ((hfChance.maxValue == 0) ? VisualTweak.Red : VisualTweak.Black);
                string text3 = "<sprite=\"icon_collector\" index=41><color=" + text2 + ">" + hfChance.minValue + " - " + hfChance.maxValue + "</color>";
                text += text3;
            }
            else if (hfChance.type == HFType.OddJobs)
            {
                text = string.Format("from {0} to {1}".FromDictionary(), Game.game.caravan.FormatMoney(hfChance.minValue, showZero: true, showLeadZero: false), Game.game.caravan.FormatMoney(hfChance.maxValue, showZero: true, showLeadZero: false));
            }

            __result = text;
        }
    }
}