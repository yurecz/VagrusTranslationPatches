using HarmonyLib;
using System;
using System.Reflection;
using TMPro;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(POIAreaMarker))]
    internal class POIAreaMarkerPatches
    {
        [HarmonyPatch(nameof(POIAreaMarker.Setup))]
        [HarmonyPatch(new Type[] { typeof(TaskInstance),typeof(JournalMarker) })]
        [HarmonyPostfix]
        public static void Setup_Postfix(POIAreaMarker __instance, TaskInstance taskInstance, JournalMarker marker)
        {
            var detailsText = Traverse.Create(__instance).Property("DetailsText");
            detailsText.SetValue("");

        }
    }
}