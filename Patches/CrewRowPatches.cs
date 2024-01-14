using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using TMPro;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    public class CrewRowPatches
    {
        [HarmonyPatch(typeof(CrewRow))]
        internal class CalendarUIPatch
        {
            [HarmonyPatch("UpdateValues", new System.Type[] { typeof(string), typeof(string), typeof(int), typeof(int), typeof(int), typeof(int), typeof(float), typeof(float) })]
            [HarmonyPostfix]
            public static void UpdateValues_Postfix(CrewRow __instance, string title)
            {
                if (__instance.title.text == "Total")
                {
                    __instance.title.text = "Total".FromDictionary();
                }
            }
        }
    }
}
