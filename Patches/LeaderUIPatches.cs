using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(LeaderUI))]
    internal class LeaderUIPatches
    {
        //[HarmonyPatch("LoadResources")]
        //[HarmonyPostfix]
        //public static void LoadResources_Postfix(LeaderUI __instance)
        //{
        //    var statusRow = Resources.Load("UI/Prefab/StatusRow") as GameObject;
        //    statusRow.UpdatePrefabFonts();

        //    var leaderPerkDoubleRow = Resources.Load("UI/Prefab/LeaderPerkDoubleRow") as GameObject;
        //    leaderPerkDoubleRow.UpdatePrefabFonts();

        //    var leaderPerkSingleRow = Resources.Load("UI/Prefab/LeaderPerkSingleRow") as GameObject;
        //    leaderPerkSingleRow.UpdatePrefabFonts();

        //    var factionThumb = Resources.Load("Leader/Prefab/FactionThumb") as GameObject;
        //    factionThumb.UpdatePrefabFonts();
        //}
    }
}