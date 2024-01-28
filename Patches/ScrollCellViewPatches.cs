using EnhancedUI.EnhancedScroller;
using HarmonyLib;
using System;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(EnhancedScrollerCellView))]
    public class ScrollCellViewPatches
    {

        //[HarmonyPatch(nameof(EnhancedScrollerCellView.Awake))]
        //[HarmonyPostfix]
        //public static void Awake_Postfix(EnhancedScrollerCellView __instance)
        //{
        //    PluginClass1.Log.LogDebug("In Player Awake method Postfix.");
        //}
    }

    //[HarmonyPatch(typeof(EnhancedScrollerCellView))]
    //public static class MyExtensions
    //{
    //    public static void Awake(this EnhancedScrollerCellView cellView)
    //    {
    //        cellView.gameObject.AddIfNotExistComponent<UIFontUpdater>();
    //    }
    //}

}