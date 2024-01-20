using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(GameLogRow))]
    internal class GameLogRowPatches
    {

        /// <summary>
        /// Patches the Player Awake method with postfix code.
        /// </summary>
        /// <param name="__instance"></param>
        [HarmonyPatch(nameof(GameLogRow.SetData))]
        [HarmonyPostfix]
        public static void SetData_Postfix(GameLogRow __instance)
        {
            FontUtils.Update(__instance.logText, null, "GameLogRow=>SetData");
            FontUtils.Update(__instance.dateText, null, "GameLogRow=>SetData");
        }
    }
}