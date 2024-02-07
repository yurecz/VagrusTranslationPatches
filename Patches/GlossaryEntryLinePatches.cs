using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(GlossaryEntryLine))]
    internal class GlossaryEntryLinePatches
    {

        //[HarmonyPatch("Start")]
        //[HarmonyPostfix]
        //public static void Start_Postfix(GlossaryEntryLine __instance)
        //{
        //    var gameObject = __instance.gameObject;
        //    if (gameObject != null && !gameObject.HasComponent<UIFontUpdater>())
        //    {
        //        gameObject.AddComponent<UIFontUpdater>();
        //    }
        //}
    }
}