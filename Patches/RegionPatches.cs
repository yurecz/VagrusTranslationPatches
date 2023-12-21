using HarmonyLib;
using TMPro;
using Vagrus.Data;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch()]
    internal class RegionPatches
    {
        [HarmonyPatch(typeof(Region),"GetTitle")]
        [HarmonyPostfix]
        public static string GetTitle_Postfix(string result, Region __instance)
        {
            AirtableLocalizedData airtableLocalizedData;
            if (Game.GetLocalization(DataType.Region, __instance.uid, out airtableLocalizedData) && (airtableLocalizedData.AlternateTitle.Length > 0 || airtableLocalizedData.Title.Length > 0))
            {
                if (airtableLocalizedData.AlternateTitle.Length <= 0)
                {
                    return airtableLocalizedData.Title;
                }
                return airtableLocalizedData.AlternateTitle;
            }
            return result;
        }
    }
}