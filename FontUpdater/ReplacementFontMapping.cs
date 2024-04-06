using TMPro;
using UnityEngine;

namespace VagrusTranslationPatches
{
    public struct ReplacementFontMapping
    {
        public string FontAssetName;
        public string TargetRegEx;
        public string Comment;
        public ReplacementFont ReplacementFont;

        public ReplacementFontMapping(TMP_FontAsset fontAsset, string fontAssetName, string comment, string replacementFontName) : this()
        {
            ReplacementFont.FontAsset = fontAsset;
            FontAssetName = fontAssetName;
            Comment = comment;
            ReplacementFont.FontName = replacementFontName;
        }
    }

    public struct Outline
    {
        public Color32 Color;
        public float Width;

        public override bool Equals(object obj)
        {
            if (obj is Outline)
            {
                Outline otherOutline = (Outline)obj;
                return Color.Equals(otherOutline.Color) && Width.Equals(otherOutline.Width);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Color.GetHashCode() ^ Width.GetHashCode();
        }
    }

    public struct Underlay
    {
        public Color32 Color;
        public float OffsetX;
        public float OffsetY;
        public float Dilate;
        public float Softness;

        public Underlay(Color32 color, float offsetX, float offsetY, float dilate, float softness)
        {
            Color = color;
            OffsetX = offsetX;
            OffsetY = offsetY;
            Dilate = dilate;
            Softness = softness;
        }

        public override bool Equals(object obj)
        {
            if (obj is Underlay)
            {
                Underlay otherUnderlay = (Underlay)obj;
                return Color.Equals(otherUnderlay.Color) &&
                       OffsetX.Equals(otherUnderlay.OffsetX) &&
                       OffsetY.Equals(otherUnderlay.OffsetY) &&
                       Dilate.Equals(otherUnderlay.Dilate) &&
                       Softness.Equals(otherUnderlay.Softness);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Color.GetHashCode() ^ OffsetX.GetHashCode() ^ OffsetY.GetHashCode() ^ Dilate.GetHashCode() ^ Softness.GetHashCode();
        }
    }

    public struct ReplacementFont
    {
        public string FontName;
        public TMP_FontAsset FontAsset;
        public float? FontSize;
        internal HorizontalAlignmentOptions? HorizontalAlignment;
        internal VerticalAlignmentOptions? VerticalAlignment;
        internal float? CharacterSpacing;
        internal float? WordSpacing;
        internal float? LineSpacing;
        internal float? ParagraphSpacing;
        internal bool? EnableWordWrapping;
        internal TextOverflowModes? OverflowMode;

        public Outline Outline;
        public Underlay Underlay;
    }
}