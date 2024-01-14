using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using VagrusTranslationPatches.Patches;

namespace VagrusTranslationPatches.Utils
{
   
    public class FontUtils
    {
        public static void Update(TextMeshProUGUI textMesh, TMP_FontAsset originalFont, string callerName = "")
        {
            if(textMesh == null) return;
            // Get call stack
            StackTrace stackTrace = new StackTrace();
            var name = callerName + "=>" + textMesh.gameObject.GetFullName();
            // Get calling method name
            TranslationPatchesPlugin.Log.LogMessage(name + " Font: " + textMesh.font.name);
            TranslationPatchesPlugin.Log.LogMessage(textMesh.text);

            var fontName = textMesh.font.name;
            if (LoadSpecialFontPatches.replacementFonts.TryGetValue(fontName, out var replacementFont))
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

                    //new font
                    textMesh.font = replacementFont;

                    textMesh.faceColor = faceColor;
                    textMesh.fontStyle = fontStyle;
                    textMesh.fontSize = fontSize;
                    textMesh.fontSizeMin = fontSizeMin;
                    textMesh.fontSizeMax = fontSizeMax;
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
