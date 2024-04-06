using HarmonyLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Vagrus;
using Vagrus.UI;
using VagrusTranslationPatches;
using VagrusTranslationPatches.Patches;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches
{
    public class FontUtils
    {
        public static List<ReplacementFontMapping> ReplacementRecords = new List<ReplacementFontMapping>();

        public static bool FindFont(string fontName, string target, out ReplacementFont replacementFont)
        {
            var replacementRecords = FindRelevantReplacementRecords(fontName, target);

            if (replacementRecords.Count() > 0)
            {
                var record = replacementRecords.Last().Key;
                replacementFont = record.ReplacementFont;
                return true;
            }
            else
            {
                replacementFont = default;
                return false;
            }
        }

        public static IOrderedEnumerable<KeyValuePair<ReplacementFontMapping, int>> FindRelevantReplacementRecords(string fontName, string target)
        {
            var replacementRecords = new Dictionary<ReplacementFontMapping, int>();
            ProcessMatchingRecords(
                r => !string.IsNullOrEmpty(r.FontAssetName)
                     && r.FontAssetName == fontName
                     && string.IsNullOrEmpty(r.TargetRegEx)
                     , replacementRecords
                     , 1
            );
            ProcessMatchingRecords(
                r => !string.IsNullOrEmpty(r.FontAssetName)
                     && r.FontAssetName == fontName
                     && !string.IsNullOrEmpty(r.TargetRegEx) && Regex.IsMatch(target, r.TargetRegEx)
                     , replacementRecords
                     , 2
            );
            return replacementRecords.OrderBy(kv => kv.Value);
        }

        private static void ProcessMatchingRecords(Func<ReplacementFontMapping, bool> predicate, Dictionary<ReplacementFontMapping, int> replacementRecordsCount, int score = 1)
        {
            foreach (var replacementRecord in ReplacementRecords.Where(predicate))
            {
                //replacementRecordsCount.TryGetValue(replacementRecord, out var record);
                replacementRecordsCount[replacementRecord] = score;
            }
        }

        public static void LoadFonts()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            string iniFontFile = Path.Combine(currentDirectory, "BepInEx\\plugins\\VagrusTranslationPatches", "fonts.ini");

            if (File.Exists(iniFontFile))
            {

                JObject fontSettings = JObject.Parse(File.ReadAllText(iniFontFile));
                var tooltipHeaderSize = (int?)fontSettings["TooltipHeaderSize"];
                if (tooltipHeaderSize != null && tooltipHeaderSize > 100)
                {
                    VisualTweak.TooltipHeaderSize = (int)tooltipHeaderSize;
                }
                foreach (var replacementFontFile in (JArray)fontSettings["FontMappings"])
                {
                    string fontPath = (string)replacementFontFile["FontPath"];
                    string fontAssetName = (string)replacementFontFile["FontAsset"];
                    string comment = (string)replacementFontFile["Comment"];
                    if (!Path.IsPathRooted(fontPath))
                    {
                        fontPath = Path.Combine(currentDirectory, "BepInEx\\plugins\\VagrusTranslationPatches", "Fonts", fontPath);
                    }

                    if (File.Exists(fontPath))
                    {
                        Font font = new Font(fontPath);
                        TMP_FontAsset fontAsset = TMP_FontAsset.CreateFontAsset(font);
                        fontAsset.name = fontAssetName;

                        var replacementRecords = new ReplacementFontMapping(fontAsset, fontAssetName, comment, (string)replacementFontFile["FontPath"]);

                        string targetRegEx = (string)replacementFontFile["TargetRegEx"];
                        if (!string.IsNullOrEmpty(targetRegEx)) replacementRecords.TargetRegEx = targetRegEx;

                        replacementRecords.ReplacementFont.FontSize = (float?)replacementFontFile["FontSize"];
                        replacementRecords.ReplacementFont.LineSpacing = (float?)replacementFontFile["LineSpacing"];
                        replacementRecords.ReplacementFont.WordSpacing = (float?)replacementFontFile["WordSpacing"];
                        replacementRecords.ReplacementFont.ParagraphSpacing = (float?)replacementFontFile["ParagraphSpacing"];
                        replacementRecords.ReplacementFont.CharacterSpacing = (float?)replacementFontFile["CharacterSpacing"];
                        replacementRecords.ReplacementFont.EnableWordWrapping = (bool?)replacementFontFile["EnableWordWrapping"];

                        if (Enum.TryParse<HorizontalAlignmentOptions>(replacementFontFile["HorizontalAlignment"]?.ToString() ?? "", out HorizontalAlignmentOptions resultH))
                        {
                            replacementRecords.ReplacementFont.HorizontalAlignment = resultH;
                        }
                        else
                        {
                            replacementRecords.ReplacementFont.HorizontalAlignment = null;
                        }

                        if (Enum.TryParse<VerticalAlignmentOptions>(replacementFontFile["VerticalAlignment"]?.ToString() ?? "", out VerticalAlignmentOptions resultV))
                        {
                            replacementRecords.ReplacementFont.VerticalAlignment = resultV;
                        }
                        else
                        {
                            replacementRecords.ReplacementFont.VerticalAlignment = null;
                        }

                        if (Enum.TryParse<TextOverflowModes>(replacementFontFile["OverflowMode"]?.ToString() ?? "", out TextOverflowModes resultO))
                        {
                            replacementRecords.ReplacementFont.OverflowMode = resultO;
                        }
                        else
                        {
                            replacementRecords.ReplacementFont.OverflowMode = null;
                        }

                        var outline = replacementFontFile["Outline"];
                        if (outline != null)
                        {
                            var color = ((string)outline["Color"]).HexToColor32();
                            var outlineWidth = (float)outline["Width"];
                            replacementRecords.ReplacementFont.Outline.Color = color;
                            replacementRecords.ReplacementFont.Outline.Width = outlineWidth;
                        }

                        var underlay = replacementFontFile["Underlay"];
                        if (underlay != null)
                        {
                            var color = ((string)underlay["Color"]).HexToColor32();
                            var offsetX = (float)underlay["OffsetX"];
                            var offsetY = (float)underlay["OffsetY"];
                            var dilate = (float)underlay["Dilate"];
                            var softness = (float)underlay["Softness"];

                            replacementRecords.ReplacementFont.Underlay = new Underlay(color, offsetX, offsetY, dilate, softness);
                        }
                        FontUtils.ReplacementRecords.Add(replacementRecords);
                    }
                    else
                    {
                        TranslationPatchesPlugin.Log.LogError("Font file missing:" + fontPath);
                    }

                }
                var onLanguageChange = Traverse.Create(typeof(Game)).Field("onLanguageChange").GetValue() as UnityEvent;
                onLanguageChange.Invoke();
            }
        }

        public static void Update(TextMeshProUGUI textMesh, TMP_FontAsset originalFont, string callerName = "")
        {
            if (textMesh == null) return;

            var fullName = textMesh.gameObject.GetFullName();
            var name = callerName + "=>" + fullName;

            TranslationPatchesPlugin.Log.LogMessage($" Full path: {callerName}");
            TranslationPatchesPlugin.Log.LogMessage($" Original font: {textMesh.font.name}");
            TranslationPatchesPlugin.Log.LogMessage(" " + textMesh.text);

            var fontName = textMesh.font.name;
            if (FindFont(fontName, callerName, out var replacementFont))
            {
                TranslationPatchesPlugin.Log.LogMessage($" Replacement font: {replacementFont.FontName }");
                if (replacementFont.FontAsset != textMesh.font
                    || replacementFont.FontSize != textMesh.fontSize
                    || replacementFont.HorizontalAlignment != textMesh.horizontalAlignment
                    || replacementFont.VerticalAlignment != textMesh.verticalAlignment
                    || replacementFont.WordSpacing != textMesh.wordSpacing
                    || replacementFont.LineSpacing != textMesh.lineSpacing
                    || replacementFont.CharacterSpacing != textMesh.characterSpacing
                    || replacementFont.ParagraphSpacing != textMesh.paragraphSpacing
                    || replacementFont.EnableWordWrapping != textMesh.enableWordWrapping
                    || replacementFont.OverflowMode != textMesh.overflowMode
                    )
                {
                    var originalParameters = PreserveParameters(textMesh);

                    textMesh.font = replacementFont.FontAsset;

                    RestoreParameters(textMesh, originalParameters);
                    
                    if (replacementFont.FontSize != null)
                    {
                        textMesh.fontSize = replacementFont.FontSize.Value;
                    }

                    if (replacementFont.HorizontalAlignment != null)
                    {
                        textMesh.horizontalAlignment = replacementFont.HorizontalAlignment.Value;
                    }

                    if (replacementFont.VerticalAlignment != null)
                    {
                        textMesh.verticalAlignment = replacementFont.VerticalAlignment.Value;
                    }

                    if (replacementFont.CharacterSpacing != null)
                    {
                        textMesh.characterSpacing = replacementFont.CharacterSpacing.Value;
                    }

                    if (replacementFont.WordSpacing != null)
                    {
                        textMesh.wordSpacing = replacementFont.WordSpacing.Value;
                    }

                    if (replacementFont.LineSpacing != null)
                    {
                        textMesh.lineSpacing = replacementFont.LineSpacing.Value;
                    }

                    if (replacementFont.ParagraphSpacing != null)
                    {
                        textMesh.paragraphSpacing = replacementFont.ParagraphSpacing.Value;
                    }

                    textMesh.UpdateFontAsset();

                    if (replacementFont.Outline.Width != default)
                    {
                        textMesh.materialForRendering.EnableKeyword(ShaderUtilities.Keyword_Outline);
                        textMesh.outlineColor = replacementFont.Outline.Color;
                        textMesh.outlineWidth = replacementFont.Outline.Width;
                        //textMesh.fontSharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, replacementFont.Outline.Color);
                        //textMesh.fontSharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, replacementFont.Outline.Width);
                    }
                    else
                    {
                        textMesh.materialForRendering.DisableKeyword(ShaderUtilities.Keyword_Outline);
                    }

                    if (replacementFont.Underlay.OffsetX != default || replacementFont.Underlay.OffsetY != default)
                    {
                        textMesh.materialForRendering.EnableKeyword(ShaderUtilities.Keyword_Underlay);

                        textMesh.materialForRendering.SetColor(ShaderUtilities.ID_UnderlayColor, replacementFont.Underlay.Color);
                        textMesh.materialForRendering.SetFloat(ShaderUtilities.ID_UnderlayOffsetX, replacementFont.Underlay.OffsetX);
                        textMesh.materialForRendering.SetFloat(ShaderUtilities.ID_UnderlayOffsetY, replacementFont.Underlay.OffsetY);
                        textMesh.materialForRendering.SetFloat(ShaderUtilities.ID_UnderlayDilate, replacementFont.Underlay.Dilate);
                        textMesh.materialForRendering.SetFloat(ShaderUtilities.ID_UnderlaySoftness, replacementFont.Underlay.Softness);
                    }
                    else
                    {
                        textMesh.materialForRendering.DisableKeyword(ShaderUtilities.Keyword_Underlay);
                    }

                    if (replacementFont.EnableWordWrapping != null)
                    {
                        textMesh.enableWordWrapping = replacementFont.EnableWordWrapping.Value;
                    }

                    if (replacementFont.OverflowMode != null)
                    {
                        textMesh.overflowMode = replacementFont.OverflowMode.Value;
                    }

                    //textMesh.ForceMeshUpdate(true, true);
                    //textMesh.UpdateMeshPadding();

                    TranslationPatchesPlugin.Log.LogMessage(" Font replaced");
                }
            }
            else if (originalFont != null && originalFont != textMesh.font)
            {
                textMesh.font = originalFont;
                TranslationPatchesPlugin.Log.LogMessage(" Font restored");
            } else
            {
                TranslationPatchesPlugin.Log.LogMessage(" Font not found");
            }
        }

        private static TextMeshProParameters PreserveParameters(TextMeshProUGUI textMesh)
        {
            return new TextMeshProParameters
            {
                ShaderKeywords = textMesh.materialForRendering.shaderKeywords,
                OutlineColor = textMesh.outlineColor,
                OutlineWidth = textMesh.outlineWidth,
                FontStyle = textMesh.fontStyle,
                FontSize = textMesh.fontSize,
                FontSizeMin = textMesh.fontSizeMin,
                FontSizeMax = textMesh.fontSizeMax,
                FaceColor = textMesh.faceColor,
                Alignment = textMesh.alignment,
                CharacterSpacing = textMesh.characterSpacing,
                LineSpacing = textMesh.lineSpacing,
                ParagraphSpacing = textMesh.paragraphSpacing,
                RichText = textMesh.richText,
                EnableAutoSizing = textMesh.enableAutoSizing,
                FontSharedMaterial = textMesh.fontSharedMaterial,
                Margin = textMesh.margin,
                Text = textMesh.text,
                EnableWordWrapping = textMesh.enableWordWrapping,
                OverflowMode = textMesh.overflowMode,
                IsOrthographic = textMesh.isOrthographic,
                VertexGradient = textMesh.colorGradient,
                EnableCulling = textMesh.enableCulling,
                RaycastTarget = textMesh.raycastTarget
            };
        }

        private static void RestoreParameters(TextMeshProUGUI textMesh, TextMeshProParameters parameters)
        {
            textMesh.faceColor = parameters.FaceColor;
            textMesh.fontStyle = parameters.FontStyle;
            textMesh.fontSize = parameters.FontSize;
            textMesh.fontSizeMin = parameters.FontSizeMin;
            textMesh.fontSizeMax = parameters.FontSizeMax;
            textMesh.alignment = parameters.Alignment;
            textMesh.characterSpacing = parameters.CharacterSpacing;
            textMesh.lineSpacing = parameters.LineSpacing;
            textMesh.paragraphSpacing = parameters.ParagraphSpacing;
            textMesh.richText = parameters.RichText;
            textMesh.enableAutoSizing = parameters.EnableAutoSizing;
            textMesh.fontSharedMaterial = parameters.FontSharedMaterial;
            textMesh.margin = parameters.Margin;
            textMesh.text = parameters.Text;
            textMesh.enableWordWrapping = parameters.EnableWordWrapping;
            textMesh.overflowMode = parameters.OverflowMode;
            textMesh.isOrthographic = parameters.IsOrthographic;
            textMesh.colorGradient = parameters.VertexGradient;
            textMesh.enableCulling = parameters.EnableCulling;
            textMesh.raycastTarget = parameters.RaycastTarget;

            foreach (string keyword in parameters.ShaderKeywords)
            {
                textMesh.materialForRendering.EnableKeyword(keyword);
                if (keyword == ShaderUtilities.Keyword_Outline)
                {
                    TranslationPatchesPlugin.Log.LogMessage("OUTLINE_ON");
                }
            }

            textMesh.outlineColor = parameters.OutlineColor;
            textMesh.outlineWidth = parameters.OutlineWidth;
        }

        private class TextMeshProParameters
        {
            // Parameters to preserve and restore
            public string[] ShaderKeywords { get; set; }
            public Color32 OutlineColor { get; set; }
            public float OutlineWidth { get; set; }
            public FontStyles FontStyle { get; set; }
            public float FontSize { get; set; }
            public float FontSizeMin { get; set; }
            public float FontSizeMax { get; set; }
            public Color32 FaceColor { get; set; }
            public TextAlignmentOptions Alignment { get; set; }
            public float CharacterSpacing { get; set; }
            public float LineSpacing { get; set; }
            public float ParagraphSpacing { get; set; }
            public bool RichText { get; set; }
            public bool EnableAutoSizing { get; set; }
            public Material FontSharedMaterial { get; set; }
            public Vector4 Margin { get; set; }
            public string Text { get; set; }
            public bool EnableWordWrapping { get; set; }
            public TextOverflowModes OverflowMode { get; set; }
            public bool IsOrthographic { get; set; }
            public VertexGradient VertexGradient { get; set; }
            public bool EnableCulling { get; set; }
            public bool RaycastTarget { get; set; }
        }
    }
}

