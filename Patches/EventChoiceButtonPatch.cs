using HarmonyLib;
using TMPro;
using Vagrus.UI;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(EventChoiceButton))]
    internal class EventChoiceButtonPatch
    {
        [HarmonyPatch("CreateEndButton")]
        [HarmonyPostfix]
        public static void Refresh_Postfix(EventChoiceButton __result)
        {
            var choiceText = Traverse.Create(__result).Field("choiceText").GetValue() as TextMeshProUGUI;
            choiceText.text = "<size=46><b><smallcaps>" + "End".FromDictionary() + "</smallcaps></b></size>";
        }
    }
}