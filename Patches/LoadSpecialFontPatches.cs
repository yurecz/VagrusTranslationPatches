using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch]
    internal class LoadSpecialFontPatches
    {

        static TMP_FontAsset specialFont = null;

        [HarmonyPatch(typeof(Game),"LoadSpecialFont", new Type[] { typeof(bool) })]
        [HarmonyPostfix]
        public static void LoadSpecialFont_Postfix(Game __instance,bool secondary = false)
        {
            var onLanguageChange = Traverse.Create(typeof(Game)).Field("onLanguageChange").GetValue() as UnityEvent; ;
            if (Game.GetLanguageCode(secondary) == "ru" && specialFont==null)
            {
                Game.LoadedLanguagePacks.TryGetValue("ru", out var value);
                long currentLanguagePackID = Game.GetCurrentLanguagePackID(secondary);
                if (value == null)
                {
                    return;
                }
                int i;
                for (i = 0; i < value.Count && value[i].ID != currentLanguagePackID; i++)
                {
                }
                i = ((i != value.Count) ? i : 0);
                string text = ((value[i].ID == -1) ? Path.Combine(value[i].InstallPath, "reviewed") : Path.Combine(value[i].InstallPath, "lang"));
                if (Directory.Exists(text))
                {
                    string path = Path.Combine(text, "font.ttf");
                    if (File.Exists(path))
                    {
                        TMP_FontAsset font = TMP_FontAsset.CreateFontAsset(new UnityEngine.Font(path));
                        specialFont = font;
                        specialFont = font;
                        onLanguageChange.Invoke();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Game),"TryGetSpecialFont")]
        [HarmonyPostfix]
        public static bool TryGetSpecialFont_Postfix(bool result, string languageCode, ref TMP_FontAsset fontasset)
        {
            if (languageCode == "ru" && specialFont != null)
            {
                fontasset = specialFont;
                return true;
            }
            else return result;
        }

        [HarmonyPatch(typeof(UIFontUpdater),"FindAllTextAndChangeFont")]
        [HarmonyPrefix]
        public static bool FindAllTextAndChangeFont_Prefix(UIFontUpdater __instance)
        {
            if (__instance.name.Contains("ChartUI"))
            {
                return false;
            }
            return true;
        }
    }
}