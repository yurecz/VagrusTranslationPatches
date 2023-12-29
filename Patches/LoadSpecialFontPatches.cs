using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Vagrus;
using VagrusTranslationPatches.Utils;
using static Vagrus.UITranslator;


namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch]
    internal class LoadSpecialFontPatches
    {

        public static TMP_FontAsset specialFont = null;
        public static TMP_FontAsset specialFont2 = null;

        [HarmonyPatch(typeof(Game), "LoadSpecialFont", new Type[] { typeof(bool) })]
        [HarmonyPostfix]
        public static void LoadSpecialFont_Postfix(Game __instance, bool secondary = false)
        {
            var onLanguageChange = Traverse.Create(typeof(Game)).Field("onLanguageChange").GetValue() as UnityEvent;
            if (Game.GetLanguageCode(secondary) == "ru" && specialFont == null)
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
                        string path2 = Path.Combine(text, "ofont.ru_Caslon Becker.ttf");
                        if (File.Exists(path2))
                        {
                            TMP_FontAsset font2 = TMP_FontAsset.CreateFontAsset(new UnityEngine.Font(path2));
                            specialFont2 = font2;
                        }
                        onLanguageChange.Invoke();
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Game), "TryGetSpecialFont")]
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

        [HarmonyPatch(typeof(UIFontUpdater), "Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(UIFontUpdater __instance)
        {
            TranslationPatchesPlugin.Log.LogInfo("Registered UIFontUpdater on " + __instance.gameObject.GetFullName());
        }

        [HarmonyPatch(typeof(UITranslator), "Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(UITranslator __instance)
        {
            TranslationPatchesPlugin.Log.LogInfo("Registered UITranslator on " + __instance.gameObject.GetFullName());
        }

        [HarmonyPatch(typeof(UIElementTranslator), "Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(UIElementTranslator __instance)
        {
            TranslationPatchesPlugin.Log.LogInfo("Registered UIElementTranslator on " + __instance.gameObject.GetFullName());
        }


        [HarmonyPatch(typeof(UIFontUpdater), "OnLanguageChange")]
        [HarmonyPrefix]
        public static bool OnLanguageChange_Prefix(UIFontUpdater __instance)
        {
            var keyUIPairs = Traverse.Create(__instance).Field("keyPairs").GetValue() as List<UITranslator.KeyUIPair>;
            Game.TryGetSpecialFont(Game.GetLanguageCode(), out var fontasset);
            for (int i = 0; i < keyUIPairs.Count; i++)
            {
                UITranslator.KeyUIPair keyUIPair = keyUIPairs[i];
                if (keyUIPair.textMesh == null || keyUIPair.textMesh.CompareTag("Translated"))
                {
                    keyUIPairs.RemoveAt(i);
                    i--;
                }
                else if (fontasset != null)
                {
                    FontUtils.Update(keyUIPair.textMesh, fontasset, "UIFontUpdater");
                }
                else
                {
                    keyUIPair.textMesh.font = keyUIPair.firstFont;
                }
            }

            return false;
        }

        [HarmonyPatch(typeof(UIFontUpdater), "FindAllTextAndChangeFont")]
        [HarmonyPrefix]
        public static bool FindAllTextAndChangeFont_Prefix(UIFontUpdater __instance)
        {
            if (__instance.name.Contains("ChartUI"))
            {
               //return false;
            }
            var keyUIPairs = Traverse.Create(__instance).Field("keyPairs").GetValue() as List<UITranslator.KeyUIPair>;

            keyUIPairs.Clear();
            var textMeshes = __instance.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var textMesh in textMeshes)
            {
                if (!textMesh.CompareTag("Translated"))
                {
                    keyUIPairs.Add(new UITranslator.KeyUIPair
                    {
                        textMesh = textMesh,
                        firstFont = textMesh.font
                    });
                    if (Game.TryGetSpecialFont(Game.GetLanguageCode(false), out TMP_FontAsset tmp_FontAsset))
                    {
                        FontUtils.Update(textMesh, tmp_FontAsset, "UIFontUpdater");
                        if (__instance.name.Contains("ChartUI"))
                        {
                            //textMesh.outlineColor = new Color32(236, 226, 198, 255);
                            textMesh.outlineColor = new Color32(240, 255, 255, 255);
                            textMesh.outlineWidth = 0.23f;
                            textMesh.materialForRendering.EnableKeyword(ShaderUtilities.Keyword_Outline);
                        }
                    }
                }
            }

            return false;
        }


        [HarmonyPatch(typeof(UITranslator), "TranslateUIElements")]
        [HarmonyPrefix]
        public static bool TranslateUIElements_Prefix(UITranslator __instance)
        {
            var keyUIPairs = Traverse.Create(__instance).Field("keyUIPairs").GetValue() as KeyUIPair[];
            Game.TryGetSpecialFont(Game.GetLanguageCode(), out var fontasset);
            KeyUIPair[] array = keyUIPairs;
            foreach (KeyUIPair keyUIPair in array)
            {
                if (!(keyUIPair.textMesh != null))
                {
                    continue;
                }
                if (keyUIPair.KeyText != "")
                {
                    if (Game.Dictionary.TryGetValue(keyUIPair.KeyText.ToLower().Trim('\n', ' '), out var value))
                    {
                        keyUIPair.textMesh.text = (keyUIPair.NoScaling ? value : Game.GetScaledLocalizedText(keyUIPair.textMesh, keyUIPair.KeyText, value));
                        keyUIPair.textMesh.richText = true;
                        if (fontasset != null)
                        {
                            FontUtils.Update(keyUIPair.textMesh, fontasset, "UITranslator");
                        }
                        else
                        {
                            keyUIPair.textMesh.font = keyUIPair.firstFont;
                        }
                    }
                    else
                    {
                        keyUIPair.textMesh.text = keyUIPair.KeyText;
                        keyUIPair.textMesh.font = keyUIPair.firstFont;
                    }
                }
                else
                {
                    keyUIPair.textMesh.font = keyUIPair.firstFont;
                }

            }
            return false;
        }

        [HarmonyPatch(typeof(UIElementTranslator), "TranslateUIElement")]
        [HarmonyPrefix]
        public static bool TranslateUIElement_Prefix(UIElementTranslator __instance)
        {
            var keyText = __instance.KeyText;
            var textMesh = __instance.textMesh;
            var firstFont = __instance.firstFont;
            var NoScaling = __instance.NoScaling;
            var IsTextTemplate = __instance.IsTextTemplate;
            Game.TryGetSpecialFont(Game.GetLanguageCode(), out var fontasset);
            string value;
            if (IsTextTemplate)
            {
                TextTemplate textTemplate = TextTemplate.FindByName(keyText);
                if (textTemplate == null)
                {
                    Debug.LogError("No text template exists with the name: " + keyText);
                }
                textMesh.text = (NoScaling ? textTemplate.GetText() : Game.GetScaledLocalizedText(textMesh, textTemplate.text, textTemplate.GetText()));
                textMesh.richText = true;
                if (fontasset != null)
                {
                    FontUtils.Update(textMesh, fontasset, "UIElementTranslator");
                }
                else
                {
                    textMesh.font = firstFont;
                }
            }
            else if (Game.Dictionary.TryGetValue(keyText.ToLower().Trim('\n', ' '), out value))
            {
                textMesh.text = (NoScaling ? value : Game.GetScaledLocalizedText(textMesh, keyText, value));
                textMesh.richText = true;
                if (fontasset != null)
                {
                    FontUtils.Update(textMesh, fontasset, "UIElementTranslator");
                }
                else
                {
                    textMesh.font = firstFont;
                }
            }
            else
            {
                textMesh.text = keyText;
                textMesh.font = firstFont;
            }
            return false;
        }
    }
}