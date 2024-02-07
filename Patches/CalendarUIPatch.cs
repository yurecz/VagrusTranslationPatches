using HarmonyLib;
using TMPro;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CalendarUI))]
    internal class CalendarUIPatch
    {
        //[HarmonyPatch("LoadResources")]
        //[HarmonyPostfix]
        //public static void LoadResources_Postfix(BookUI __instance, GameObject ___prefab)
        //{
        //    ___prefab.AddIfNotExistComponent<UIFontUpdater>();
        //}
        
        [HarmonyPatch("Refresh")]
        [HarmonyPostfix]
        public static void Refresh_Postfix(CalendarUI __instance)
        {
            var game = Game.game;
            string year = Game.FromDictionary("Year").FirstLetterUpperCase() + " " + game.calendar.FormatDate("y");
            string month = Game.FromDictionary("Month").FirstLetterUpperCase() + " " + game.calendar.FormatDate("m");
            string day = Game.FromDictionary("Day").FirstLetterUpperCase() + " " + game.calendar.FormatDate("d");
            string week = Game.FromDictionary("Week").FirstLetterUpperCase() + " " + game.calendar.FormatDate("w");
            string dayName = game.calendar.FormatDate("l");
            dayName = Game.FromDictionary(dayName);
            var curveText = Traverse.Create(__instance).Field("curveText").GetValue() as TextMeshProUGUI;
            curveText.text = week + " " + dayName + "       " + year + " " + month + " " + day;
            CurveInfo curveInfo = new CurveInfo();
            curveInfo.radius = 415f;
            curveInfo.startAngle = 224f;
            curveInfo.spacing = 0f;
            curveInfo.flip = true;
            RichText.MakeCurveText(curveText, curveInfo);
        }
    }
}