using HarmonyLib;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(BaseUI))]
    internal class BaseUIPatches
    {

        [HarmonyPatch(nameof(BaseUI.GetTooltipOrder))]
        [HarmonyPostfix]
        public static void GetTooltipOrder_Postfix(BaseUI __instance, ref int __result)
        {
            __result = 32766;
            }
        }
}