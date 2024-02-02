using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [HarmonyPatch(typeof(Game), "LoadSpecialFont", new Type[] { typeof(bool) })]
        [HarmonyPostfix]
        public static void LoadSpecialFont_Postfix(Game __instance, bool secondary = false)
        {
            var onLanguageChange = Traverse.Create(typeof(Game)).Field("onLanguageChange").GetValue() as UnityEvent;
            if (Game.GetLanguageCode(secondary) == "ru")
            {
                FontUtils.ReplacementRecords.Clear();

                Game.LoadedLanguagePacks.TryGetValue("ru", out var value);
                if (value == null)
                    return;

                FontUtils.LoadFonts();
                
            }
        }

        [HarmonyPatch(typeof(UIFontUpdater), "Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(UIFontUpdater __instance)
        {
            //TranslationPatchesPlugin.Log.LogInfo("Awake UIFontUpdater on " + __instance.gameObject.GetFullName());
        }

        [HarmonyPatch(typeof(UIFontUpdater), "Start")]
        [HarmonyPrefix]
        public static bool Start_Prefix(UIFontUpdater __instance)
        {
            //TranslationPatchesPlugin.Log.LogInfo($"Start UIFontUpdater on {__instance.gameObject.GetFullName()}");

            PatchObject.instance.UpdateFonts(__instance);
            return false;
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
                    TranslationPatchesPlugin.Log.LogInfo("UIFontUpdater on: " + __instance.gameObject.GetFullName() + " removing key/pair");
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

        [HarmonyPatch(typeof(UIFontUpdater), "FindAllTextAndChangeFont")]
        [HarmonyPrefix]
        public static bool FindAllTextAndChangeFont_Prefix(UIFontUpdater __instance)
        {
            if (__instance.name.Contains("TaskRow("))
            {
                TranslationPatchesPlugin.Log.LogInfo("UIFontUpdater Task Row");
            }
            var keyUIPairs = Traverse.Create(__instance).Field("keyPairs").GetValue() as List<UITranslator.KeyUIPair>;

            keyUIPairs.Clear();
            var textMeshes = __instance.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
            if (textMeshes == null || textMeshes.Count() == 0)
            {
                TranslationPatchesPlugin.Log.LogInfo("UIFontUpdater no TextMeshProUGUI components found: " + __instance.gameObject.GetFullName());
            }

            var textMeshes2 = __instance.transform.GetComponents<TextMeshProUGUI>();
            if (textMeshes2 != null && textMeshes.Count() > 0)
            {
                TranslationPatchesPlugin.Log.LogInfo("UIFontUpdater found directly assigned TextMeshProUGUI: " + __instance.gameObject.GetFullName());
            }
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
                } else {
                    TranslationPatchesPlugin.Log.LogInfo("UIFontUpdater skip: " + textMesh.GetComponentInParent<RectTransform>().gameObject.GetFullName());
                }
            }

            keyUIPairs = Traverse.Create(__instance).Field("keyPairs").GetValue() as List<UITranslator.KeyUIPair>;
            TranslationPatchesPlugin.Log.LogInfo($"UIFontUpdater on {__instance.gameObject.GetFullName()} found: {keyUIPairs.Count()} components");
            return false;
        }

        [HarmonyPatch(typeof(UITranslator), "Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(UITranslator __instance)
        {
            TranslationPatchesPlugin.Log.LogInfo("Registered UITranslator on " + __instance.gameObject.GetFullName());
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
                        //keyUIPair.textMesh.text = (keyUIPair.NoScaling ? value : Game.GetScaledLocalizedText(keyUIPair.textMesh, keyUIPair.KeyText, value));
                        keyUIPair.textMesh.text = keyUIPair.textMesh.text.FromDictionary();
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
        [HarmonyPrefix]
        public static void Start_Prefix(UIElementTranslator __instance)
        {
            if (__instance.textMesh != null)
                Translators.translators.Add(__instance.textMesh, __instance);
        }

        [HarmonyPatch(typeof(UIElementTranslator), "Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(UIElementTranslator __instance)
        {
            TranslationPatchesPlugin.Log.LogInfo("Registered UIElementTranslator on " + __instance.gameObject.GetFullName());
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
                //textMesh.text = (NoScaling ? textTemplate.GetText() : Game.GetScaledLocalizedText(textMesh, textTemplate.text, textTemplate.GetText()));
                textMesh.text = textTemplate.GetText();
            }
            else if (Game.Dictionary.TryGetValue(keyText.ToLower().Trim('\n', ' '), out value))
            {
                textMesh.richText = true;
                //textMesh.text = (NoScaling ? value : Game.GetScaledLocalizedText(textMesh, keyText, value));
                textMesh.text = textMesh.text.FromDictionary();
            }
            return false;
        }

        [HarmonyPatch(typeof(UIElementTranslator), "OnDestroy")]
        [HarmonyPrefix]
        public static void OnDestroy_Prefix(UIElementTranslator __instance)
        {
            if (__instance.textMesh != null)
                Translators.translators.Remove(__instance.textMesh);
        }
    }
}