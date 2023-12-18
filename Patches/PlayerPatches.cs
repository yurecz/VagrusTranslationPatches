using HarmonyLib;
using System;

namespace VagrusTranslationPatches.Patches
{
    // TODO Review this file and update to your own requirements, or remove it altogether if not required

    /// <summary>
    /// Sample Harmony Patch class. Suggestion is to use one file per patched class
    /// though you can include multiple patch classes in one file.
    /// Below is included as an example, and should be replaced by classes and methods
    /// for your mod.
    /// </summary>
    [HarmonyPatch(typeof(Game))]
    internal class PlayerPatches
    {
        /// <summary>
        /// Patches the Player Awake method with prefix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(Game.LoadSpecialFont))]
        [HarmonyPatch(new Type[] { typeof(bool) })]
        [HarmonyPrefix]
        public static bool Awake_Prefix(Game __instance)
        {
            TranslationPatchesPlugin.Log.LogInfo("In Player Awake method Prefix.");
            return true;
        }

        /// <summary>
        /// Patches the Player Awake method with postfix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(Game.LoadSpecialFont))]
        [HarmonyPatch(new Type[] { typeof(bool) })]
        [HarmonyPostfix]
        public static void Awake_Postfix(Game __instance)
        {
            TranslationPatchesPlugin.Log.LogInfo("In Player Awake method Postfix.");
        }
    }
}