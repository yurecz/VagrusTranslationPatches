using EnhancedUI.EnhancedScroller;
using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(EnhancedScrollController))]
    public static class EnhancedScrollControllerPatches
    {
        //[HarmonyPatch("Start")]
        //[HarmonyPostfix]
        //public static void Start_Postfix(EnhancedScrollController __instance, ScrollCellView[] ___CellViewPrefabs)
        //{

        //    foreach (var prefab in ___CellViewPrefabs)
        //    {
        //        prefab.gameObject.UpdatePrefabFonts();                   
        //    }
        //}
        public static ScrollCellView[] cellViewPrefabs(this EnhancedScrollController instance)
        {
            Type myType = typeof(EnhancedScrollController);
            FieldInfo privateFieldInfo = myType.GetField("CellViewPrefabs", BindingFlags.NonPublic | BindingFlags.Instance);

            return (ScrollCellView[])privateFieldInfo.GetValue(instance);
        }
    }
}