using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(Line))]
    internal class LinePatches
    {
        [HarmonyPatch("LoadStaticResources")]
        [HarmonyPostfix]
        public static void LoadStaticResources_Postfix(Line __instance, GameObject ___lineCanvasPrefab, GameObject ___lineLiteCanvasPrefab)
        {

        }
    }
}