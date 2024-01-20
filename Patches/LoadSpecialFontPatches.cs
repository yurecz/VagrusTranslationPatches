using BepInEx;
using HarmonyLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Vagrus;
using Vagrus.IO;
using VagrusTranslationPatches.Utils;
using static Vagrus.UITranslator;


namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch]
    internal class LoadSpecialFontPatches
    {

        public static Dictionary<string, TMP_FontAsset> replacementFonts = new Dictionary<string, TMP_FontAsset>();

        [HarmonyPatch(typeof(Game), "LoadSpecialFont", new Type[] { typeof(bool) })]
        [HarmonyPostfix]
        public static void LoadSpecialFont_Postfix(Game __instance, bool secondary = false)
        {
            var onLanguageChange = Traverse.Create(typeof(Game)).Field("onLanguageChange").GetValue() as UnityEvent;
            if (Game.GetLanguageCode(secondary) == "ru")
            {
                replacementFonts.Clear();

                //replacementFontFiles.Add("Romanesco-Regular SDF", "ofont.ru_Caslon Becker.ttf");
                //replacementFontFiles.Add("CrimsonText-Italic SDF", "ofont.ru_Academy.ttf");
                //replacementFontFiles.Add("CrimsonText-BoldItalic SDF", "ofont.ru_Palatino-Bold-Italic.ttf");
                //replacementFontFiles.Add("CrimsonText-Regular SDF", "ofont.ru_Academy Condensed.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBold SDF", "ofont.ru_BrushType-SemiBold.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBoldItalic SDF", "ofont.ru_BrushType-SemiBold-Italic.ttf");

                //replacementFontFiles.Add("Romanesco-Regular SDF", "ofont.ru_Caslon Becker.ttf");
                //replacementFontFiles.Add("CrimsonText-Italic SDF", "Spectral\\Spectral-Italic.ttf");
                //replacementFontFiles.Add("CrimsonText-BoldItalic SDF", "EB Garamond\\EBGaramond-BoldItalic.ttf");
                //replacementFontFiles.Add("CrimsonText-Regular SDF", "Spectral\\Spectral-Regular.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBold SDF", "EB Garamond\\EBGaramond-SemiBold.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBoldItalic SDF", "EB Garamond\\EBGaramond-SemiBoldItalic.ttf");

                //replacementFontFiles.Add("Romanesco-Regular SDF", "ofont.ru_Caslon Becker.ttf");
                //replacementFontFiles.Add("CrimsonText-Italic SDF", "Sofia Sans Extra Condensed\\SofiaSansExtraCondensed-Italic.ttf");
                //replacementFontFiles.Add("CrimsonText-BoldItalic SDF", "Sofia Sans Extra Condensed\\SofiaSansExtraCondensed-BoldItalic.ttf");
                //replacementFontFiles.Add("CrimsonText-Regular SDF", "Sofia Sans Extra Condensed\\SofiaSansExtraCondensed-Regular.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBold SDF", "Sofia Sans Extra Condensed\\SofiaSansExtraCondensed-SemiBold.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBoldItalic SDF", "Sofia Sans Extra Condensed\\SofiaSansExtraCondensed-SemiBoldItalic.ttf");

                //replacementFontFiles.Add("Romanesco-Regular SDF", "ofont.ru_Caslon Becker.ttf");
                //replacementFontFiles.Add("CrimsonText-Italic SDF", "Sofia Sans Condensed\\SofiaSansCondensed-Italic.ttf");
                //replacementFontFiles.Add("CrimsonText-BoldItalic SDF", "Sofia Sans Condensed\\SofiaSansCondensed-BoldItalic.ttf");
                //replacementFontFiles.Add("CrimsonText-Regular SDF", "Sofia Sans Condensed\\SofiaSansCondensed-Regular.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBold SDF", "Sofia Sans Condensed\\SofiaSansCondensed-SemiBold.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBoldItalic SDF", "Sofia Sans Condensed\\SofiaSansCondensed-SemiBoldItalic.ttf");

                //replacementFontFiles.Add("Romanesco-Regular SDF", "ofont.ru_Caslon Becker.ttf");
                //replacementFontFiles.Add("CrimsonText-Italic SDF", "Sofia Sans Semi Condensed\\SofiaSansSemiCondensed-Italic.ttf");
                //replacementFontFiles.Add("CrimsonText-BoldItalic SDF", "Sofia Sans Semi Condensed\\SofiaSansSemiCondensed-BoldItalic.ttf");
                //replacementFontFiles.Add("CrimsonText-Regular SDF", "Sofia Sans Semi Condensed\\SofiaSansSemiCondensed-Regular.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBold SDF", "Sofia Sans Semi Condensed\\SofiaSansSemiCondensed-SemiBold.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBoldItalic SDF", "Sofia Sans Semi Condensed\\SofiaSansSemiCondensed-SemiBoldItalic.ttf");

                //replacementFontFiles.Add("Romanesco-Regular SDF", "ofont.ru_Caslon Becker.ttf");
                //replacementFontFiles.Add("CrimsonText-Italic SDF", "ofont.ru_Academy.ttf");
                //replacementFontFiles.Add("CrimsonText-BoldItalic SDF", "Sofia Sans Condensed\\SofiaSansCondensed-BoldItalic.ttf");
                //replacementFontFiles.Add("CrimsonText-Regular SDF", "ofont.ru_Academy Condensed.ttf");
                //replacementFontFiles.Add("CrimsonText-Menu-Regular SDF", "ofont.ru_Franklin Gothic Medium Cond.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBold SDF", "Sofia Sans Condensed\\SofiaSansCondensed-SemiBold.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBoldItalic SDF", "Sofia Sans Condensed\\SofiaSansCondensed-SemiBoldItalic.ttf");
                //replacementFontFiles.Add("CrimsonText - Overlay - SemiBold SDF", "Sofia Sans Condensed\\SofiaSansCondensed-SemiBold.ttf");

                //replacementFontFiles.Add("Romanesco-Regular SDF", "ofont.ru_Caslon Becker.ttf");
                //replacementFontFiles.Add("CrimsonText-Italic SDF", "ofont.ru_Academy.ttf");
                //replacementFontFiles.Add("CrimsonText-BoldItalic SDF", "Sofia Sans Condensed\\SofiaSansCondensed-BoldItalic.ttf");
                //replacementFontFiles.Add("CrimsonText-Regular SDF", "CMU Sans Serif\\cmunssdc.ttf");
                //replacementFontFiles.Add("CrimsonText-Menu-Regular SDF", "ofont.ru_Franklin Gothic Medium Cond.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBold SDF", "Sofia Sans Condensed\\SofiaSansCondensed-SemiBold.ttf");
                //replacementFontFiles.Add("CrimsonText-SemiBoldItalic SDF", "Sofia Sans Condensed\\SofiaSansCondensed-SemiBoldItalic.ttf");
                //replacementFontFiles.Add("CrimsonText - Overlay - SemiBold SDF", "Sofia Sans Condensed\\SofiaSansCondensed-SemiBold.ttf");



                Game.LoadedLanguagePacks.TryGetValue("ru", out var value);
                long currentLanguagePackID = Game.GetCurrentLanguagePackID(secondary);
                if (value == null)
                {
                    return;
                }

                var currentDirectory = Directory.GetCurrentDirectory();
                string iniFontFile = Path.Combine(currentDirectory, "BepInEx\\plugins\\VagrusTranslationPatches","fonts.ini");

                if (File.Exists(iniFontFile))
                {

                    JObject fontSettings = JObject.Parse(File.ReadAllText(iniFontFile));
                    foreach (var replacementFontFile in (JArray)fontSettings["FontMappings"])
                    {
                        string fontPath = (string)replacementFontFile["FontPath"];
                        string fontAssetName = (string)replacementFontFile["FontAsset"];
                        if (!Path.IsPathRooted(fontPath))
                        {
                            fontPath = Path.Combine(currentDirectory, "BepInEx\\plugins\\VagrusTranslationPatches", "Fonts", fontPath);
                        }

                        if (File.Exists(fontPath))
                        {
                            Font font = new Font(fontPath);
                            TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(font);
                            fontAsset.name = fontAssetName;
                            replacementFonts.Add(fontAssetName, fontAsset);
                        }
                        else
                        {
                            TranslationPatchesPlugin.Log.LogError("Font file missing:" + fontPath);
                        }
                     
                    }
                    onLanguageChange.Invoke();
                }
            }
        }

        //[HarmonyPatch(typeof(Game), "TryGetSpecialFont")]
        //[HarmonyPostfix]
        //public static bool TryGetSpecialFont_Postfix(bool result, string languageCode, ref TMP_FontAsset fontasset)
        //{
        //    if (languageCode == "ru" && specialFont != null)
        //    {
        //        fontasset = specialFont;
        //        return true;
        //    }
        //    else return result;
        //}

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


        [HarmonyPatch(typeof(UIFontUpdater), "OnLanguageChange")]
        [HarmonyPrefix]
        public static bool OnLanguageChange_Prefix(UIFontUpdater __instance)
        {
            var keyUIPairs = Traverse.Create(__instance).Field("keyPairs").GetValue() as List<UITranslator.KeyUIPair>;
            for (int i = 0; i < keyUIPairs.Count; i++)
            {
                UITranslator.KeyUIPair keyUIPair = keyUIPairs[i];
                if (keyUIPair.textMesh == null || keyUIPair.textMesh.CompareTag("Translated"))
                {
                    keyUIPairs.RemoveAt(i);
                    i--;
                }
                else
                {
                    FontUtils.Update(keyUIPair.textMesh, keyUIPair.firstFont, "UIFontUpdater");
                }
            }

            return false;
        }

                   //if (__instance.name.Contains("ChartUI"))
                   //     {
                   //         //textMesh.outlineColor = new Color32(236, 226, 198, 255);
                   //         textMesh.outlineColor = new Color32(240, 255, 255, 255);
                   //         textMesh.outlineWidth = 0.23f;
                   //         textMesh.materialForRendering.EnableKeyword(ShaderUtilities.Keyword_Outline);
                   //     }

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
                    FontUtils.Update(textMesh, null, "UIFontUpdater");
                }
            }

            return false;
        }


        [HarmonyPatch(typeof(UITranslator), "TranslateUIElements")]
        [HarmonyPrefix]
        public static bool TranslateUIElements_Prefix(UITranslator __instance)
        {
            var keyUIPairs = Traverse.Create(__instance).Field("keyUIPairs").GetValue() as KeyUIPair[];
            foreach (KeyUIPair keyUIPair in keyUIPairs)
            {
                if (keyUIPair.textMesh == null)
                {
                    continue;
                }

                FontUtils.Update(keyUIPair.textMesh, keyUIPair.firstFont, "UITranslator");

                if (keyUIPair.KeyText != "")
                {
                    if (Game.Dictionary.TryGetValue(keyUIPair.KeyText.ToLower().Trim('\n', ' '), out var value))
                    {
                        keyUIPair.textMesh.richText = true;
                        keyUIPair.textMesh.text = (keyUIPair.NoScaling ? value : Game.GetScaledLocalizedText(keyUIPair.textMesh, keyUIPair.KeyText, value));
                    }
                    else
                    {
                        keyUIPair.textMesh.text = keyUIPair.KeyText;
                    }
                }

            }
            return false;
        }
        [HarmonyPatch(typeof(UIElementTranslator), "Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(UIElementTranslator __instance)
        {
            TranslationPatchesPlugin.Log.LogInfo("Registered UIElementTranslator on " + __instance.gameObject.GetFullName());
        }

        [HarmonyPatch(typeof(UIElementTranslator), "Start")]
        [HarmonyPrefix]
        public static void Start_Prefix(UIElementTranslator __instance)
        {
            if (__instance.textMesh!=null)
                Translators.translators.Add(__instance.textMesh, __instance);
        }

        [HarmonyPatch(typeof(UIElementTranslator), "OnDestroy")]
        [HarmonyPrefix]
        public static void OnDestroy_Prefix(UIElementTranslator __instance)
        {
            if (__instance.textMesh != null)
                Translators.translators.Remove(__instance.textMesh);
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
            string value;
            FontUtils.Update(textMesh, firstFont, "UIElementTranslator");
            if (IsTextTemplate)
            {
                TextTemplate textTemplate = TextTemplate.FindByName(keyText);
                if (textTemplate == null)
                {
                    Debug.LogError("No text template exists with the name: " + keyText);
                }
                textMesh.richText = true;
                textMesh.text = (NoScaling ? textTemplate.GetText() : Game.GetScaledLocalizedText(textMesh, textTemplate.text, textTemplate.GetText()));
            }
            else if (Game.Dictionary.TryGetValue(keyText.ToLower().Trim('\n', ' '), out value))
            {
                textMesh.richText = true;
                textMesh.text = (NoScaling ? value : Game.GetScaledLocalizedText(textMesh, keyText, value));
            }
            return false;
        }
    }
}