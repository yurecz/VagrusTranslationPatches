using HarmonyLib;
using System;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(MansioUI))]
    internal class MansioUIPatches
    {
        [HarmonyPatch("GetStatTooltip")]
        [HarmonyPostfix]
        public static void GetStatTooltip_Postfix(MansioUI __instance, Prop prop, string tooltipstat, string v1, string v2, string change, ref string __result)
        {
            PropInstance propInstance = BaseUI.game.caravan.FindProperty(prop);
            if (propInstance == null)
            {
                __result = "?";
            }
            __result = string.Concat(string.Concat(string.Concat("" + propInstance.GetTooltip(), "\n", tooltipstat.FromDictionary(true)), "\n"+"value:".FromDictionary(true), " " + v1), "\n"+"change:".FromDictionary(), " " + change);
        }
    }
}