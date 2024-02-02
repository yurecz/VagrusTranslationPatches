using EnhancedUI.EnhancedScroller;
using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(EnhancedScrollController))]
    internal class EnhancedScrollControllerPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(EnhancedScrollController __instance, ScrollCellView[] ___CellViewPrefabs)
        {

            foreach (var prefab in ___CellViewPrefabs)
            {
                prefab.gameObject.UpdatePrefabFonts();                   
            }
        }
    }
}