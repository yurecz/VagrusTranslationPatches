using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(LoadingScreen))]
    internal class LoadingScreenPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(LoadingScreen __instance)
        {
            var loadingScreen = __instance.gameObject;
            if (loadingScreen != null && !loadingScreen.HasComponent<UIFontUpdater>())
            {
                loadingScreen.AddComponent<UIFontUpdater>();
            }
        }
    }
}