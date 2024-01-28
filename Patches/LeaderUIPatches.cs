using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(LeaderUI))]
    internal class LeaderUIPatches
    {
        [HarmonyPatch("LoadResources")]
        [HarmonyPostfix]
        public static void LoadResources_Postfix(LeaderUI __instance)
        {
            var statusRow = Resources.Load("UI/Prefab/StatusRow") as GameObject;
            statusRow.AddIfNotExistComponent<UIFontUpdater>();

            var leaderPerkDoubleRow = Resources.Load("UI/Prefab/LeaderPerkDoubleRow") as GameObject;
            leaderPerkDoubleRow.AddIfNotExistComponent<UIFontUpdater>();

            var leaderPerkSingleRow = Resources.Load("UI/Prefab/LeaderPerkSingleRow") as GameObject;
            leaderPerkSingleRow.AddIfNotExistComponent<UIFontUpdater>();

            var factionThumb = Resources.Load("Leader/Prefab/FactionThumb") as GameObject;
            factionThumb.AddIfNotExistComponent<UIFontUpdater>();
        }
    }
}