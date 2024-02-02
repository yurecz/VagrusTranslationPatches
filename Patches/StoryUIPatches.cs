using HarmonyLib;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(StoryUI))]
    internal class StoryUIPatches
    {

        [HarmonyPatch("LoadResources")]
        [HarmonyPostfix]
        public static void LoadResources_Postfix(StoryUI __instance, GameObject ___storyElemPrefab)
        {
            ___storyElemPrefab.UpdatePrefabFonts();
            __instance.SetUITitle("Stories".FromDictionary());
        }
    }
}