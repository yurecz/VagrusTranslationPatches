using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(Tooltip))]
    internal class TooltipPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(Tooltip __instance, GameObject ___prefab)
        {
            ___prefab.AddIfNotExistComponent<UIFontUpdater>();
        }
    }
}