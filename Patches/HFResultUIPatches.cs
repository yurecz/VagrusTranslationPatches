using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(HFResultUI))]
    internal class HFResultUIPatches
    {

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(HFResultUI __instance, GameObject ___prefab)
        {
            ___prefab.AddIfNotExistComponent<UIFontUpdater>();
            HFResultUI.goodsPrefab.AddIfNotExistComponent<UIFontUpdater>();
        }
    }
}