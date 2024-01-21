using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches
{
   
    public class FontUtils
    {

        public static List<FontReplacerRecord> ReplacementRecords = new List<FontReplacerRecord>();

        public static bool FindFont(out TMP_FontAsset replacementFont, string fontName = null, string target = null)
        {
            var replacementRecordsCount = new Dictionary<FontReplacerRecord, int>();

            ProcessMatchingRecords(fontName, r => r.FontName == fontName, replacementRecordsCount);
            ProcessMatchingRecords(target, r => Regex.IsMatch(target, r.TargetRegEx), replacementRecordsCount);

            if (replacementRecordsCount.Count > 0)
            {
                var sortedList = replacementRecordsCount.ToList();
                sortedList.Sort((a, b) => a.Value.CompareTo(b.Value));

                replacementFont = sortedList[0].Key.FontAsset;
                return true;
            }
            else
            {
                replacementFont = null;
                return false;
            }
        }

        private static void ProcessMatchingRecords(string criteria, Func<FontReplacerRecord, bool> predicate, Dictionary<FontReplacerRecord, int> replacementRecordsCount)
        {
            if (!string.IsNullOrEmpty(criteria))
            {
                foreach (var replacementRecord in ReplacementRecords.Where(predicate))
                {
                    replacementRecordsCount.TryGetValue(replacementRecord, out var record);
                    replacementRecordsCount[replacementRecord] = record + 1;
                }
            }
        }

        public static void Update(TextMeshProUGUI textMesh, TMP_FontAsset originalFont, string callerName = "")
        {
            if(textMesh == null) return;
            // Get call stack
            //StackTrace stackTrace = new StackTrace();
            var fullName = textMesh.gameObject.GetFullName();
            var name = callerName + "=>" + textMesh.gameObject.GetFullName();
            // Get calling method name
            TranslationPatchesPlugin.Log.LogMessage(name + " Font: " + textMesh.font.name);
            TranslationPatchesPlugin.Log.LogMessage(textMesh.text);

            var fontName = textMesh.font.name;
            if (FindFont(out var replacementFont, fontName,fullName))
            {
                if (replacementFont != textMesh.font)
                {
                    string[] shaderKeywords = textMesh.materialForRendering.shaderKeywords;
                    Color32 outlineColor = textMesh.outlineColor;
                    float outlineWidth = textMesh.outlineWidth;
                    FontStyles fontStyle = textMesh.fontStyle;
                    float fontSize = textMesh.fontSize;
                    float fontSizeMin = textMesh.fontSizeMin;
                    float fontSizeMax = textMesh.fontSizeMax;
                    Color32 faceColor = textMesh.faceColor;
                    Shader shader = textMesh.material.shader;
                    string text3 = textMesh.text;
                    var alignment = textMesh.alignment;
                    //new font
                    textMesh.font = replacementFont;

                    textMesh.faceColor = faceColor;
                    textMesh.fontStyle = fontStyle;
                    textMesh.fontSize = fontSize;
                    textMesh.fontSizeMin = fontSizeMin;
                    textMesh.fontSizeMax = fontSizeMax;
                    textMesh.alignment = alignment;
                    foreach (string keyword in shaderKeywords)
                    {
                        textMesh.materialForRendering.EnableKeyword(keyword);
                        if (keyword == ShaderUtilities.Keyword_Outline)
                        {
                            TranslationPatchesPlugin.Log.LogMessage("OUTLINE_ON");
                        }
                    }
                    textMesh.outlineColor = outlineColor;
                    textMesh.outlineWidth = outlineWidth;

                    textMesh.fontSharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, outlineColor);
                    textMesh.fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, outlineWidth);

                    textMesh.UpdateFontAsset();
                    textMesh.ForceMeshUpdate(true, true);
                    textMesh.UpdateMeshPadding();

                    TranslationPatchesPlugin.Log.LogMessage("Font replaced");
                }
            } else if(originalFont!=null && originalFont != textMesh.font)
            {
                textMesh.font = originalFont;
                TranslationPatchesPlugin.Log.LogMessage("Font restored");
            }
        }
    }

}
