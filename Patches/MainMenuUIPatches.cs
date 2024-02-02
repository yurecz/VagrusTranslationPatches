using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(MainMenuUI))]
    internal class MainMenuUIPatches
    {
        [HarmonyPatch("HighLight")]
        [HarmonyPostfix]
        public static void HighLight_Postfix(MainMenuUI __instance, ref string __result, string text)
        {
            __result = "<color=" + Game.highLightColor + ">" + text.FromDictionary() + "</color> ";
        }
    }
}