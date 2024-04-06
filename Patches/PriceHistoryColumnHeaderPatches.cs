using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(PriceHistoryColumnHeader))]
    internal class PriceHistoryColumnHeaderPatches
    {
        [HarmonyPatch("FormatPriceDate")]
        [HarmonyPostfix]
        public static void FormatPriceDate_Postfix(PriceHistoryColumnHeader __instance, int ___daysOld, ref string __result)
        {
                string text = "";
                var daysOld = ___daysOld;
                if (Game.game.calendar.GetDayCount() == daysOld)
                {
                __result = text + "<color=" + VisualTweak.Gold + ">"+"Prices\nUnknown".FromDictionary()+"</color>";
                return;
                }
                text = text + "<color=" + VisualTweak.Gold + ">"+"Price Date:".FromDictionary()+"</color>\n";
                __result =text + "<i>" + daysOld.ToDaysAgoText(true) + "</i>";
            }
        }
}