using HarmonyLib;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Vagrus;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Patches;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.PrefabUpdater
{
    [HarmonyPatch(typeof(Map))]
    internal class MapPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        public static bool Awake_Prefix(Map __instance)
        {
            PrefabsList.UpdatePrefabs();

            return true;
        }
    }
}