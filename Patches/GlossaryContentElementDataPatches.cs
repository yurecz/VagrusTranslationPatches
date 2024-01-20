using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(GlossaryContentElementData))]
    internal class GlossaryContentElementDataPatches
    {

        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(GlossaryContentElementData __instance)
        {
            var gameObject = __instance.gameObject;
            if (gameObject != null && !gameObject.HasComponent<UIFontUpdater>())
            {
                gameObject.AddComponent<UIFontUpdater>();
            }
        }
    }
}