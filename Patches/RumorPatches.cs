using HarmonyLib;
using TMPro;
using Vagrus.Data;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch()]
    internal class RumorPatches
    {
        [HarmonyPatch(typeof(Rumor), "GetTitle")]
        [HarmonyPostfix]
        public static string GetTitle_Postfix(string result, Rumor __instance)
        {
            if (Game.GetLocalization(DataType.Rumor, __instance.ID, out AirtableLocalizedData airtableLocalizedData) && airtableLocalizedData.Title.Length > 0)
            {
                return airtableLocalizedData.Title;
            }
            return result;
        }
    }
}