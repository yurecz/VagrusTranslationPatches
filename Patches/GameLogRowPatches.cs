using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(GameLogRow))]
    internal class GameLogRowPatches
    {
        //[HarmonyPatch(nameof(GameLogRow.SetData))]
        //[HarmonyPostfix]
        //public static void SetData_Postfix(GameLogRow __instance)
        //{
        //    FontUtils.Update(__instance.logText, null, "GameLogRow=>SetData");
        //    FontUtils.Update(__instance.dateText, null, "GameLogRow=>SetData");
        //}
    }
}