using HarmonyLib;
using TMPro;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(BaseUI))]
    internal class BaseUIExPatches
    {
        [HarmonyPatch("SetUITitle")]
        [HarmonyPostfix]
        public static void SetUITitle_Postfix(BaseUI __instance, string title, TextMeshProUGUI ___Title)
        {
            if (title != null && ___Title != null)
            {
                ___Title.text = title.FromDictionary();
            }
        }

    }
}