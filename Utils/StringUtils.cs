using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace VagrusTranslationPatches.Utils
{
    public static class StringExtension
    {
        public static string FirstLetterUpperCase(this string str)
        {
            return str[0].ToString().ToUpper() + str.Substring(1);
        }

        public static string FromDictionary(this string str,bool showWarning = true)
        {
            var text = Game.FromDictionary(str);
            if (!string.IsNullOrEmpty(str) && showWarning && text == str)
            {
                TranslationPatchesPlugin.Log.LogWarning("Missing in dictionary.csv the translation for text:" + str);
                return str;
            }
            return text;
        }

        public static string FromDictionaryScaled(this string str)
        {
            var newText = str.FromDictionary();
            if (newText != str)
            {
                return Game.GetScaledLocalizedText(str, str.FromDictionary());
            } else
            {
                return newText;
            }
        }
        public static string GetScaledLocalizedText(this string str, string original)
        {
            return Game.GetScaledLocalizedText(original, str);
        }

        public static Color32 HexToColor32(this string hex)
        {
            // Remove any leading '#' character
            hex = hex.TrimStart('#');

            // Parse hexadecimal values for red, green, and blue
            byte r = System.Convert.ToByte(hex.Substring(0, 2), 16);
            byte g = System.Convert.ToByte(hex.Substring(2, 2), 16);
            byte b = System.Convert.ToByte(hex.Substring(4, 2), 16);

            // Create and return the Color32 object
            return new Color32(r, g, b, 255); // Alpha is set to 255 for fully opaque
        }

        public static string ReplaceToken<T>(this string str, string token, T value)
        {
            // Convert the value to a string
            string text = value.ToString();

            // Replace the token with the string representation of the value
            str = str.Replace(token, text);

            return str;
        }

        public static string RemoveFirstLineFromString(this string input)
        {
            // Split the input string into lines
            string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            // Check if there is more than one line
            if (lines.Length > 1)
            {
                // Join the lines (excluding the first one) to form the result string
                return string.Join(Environment.NewLine, lines, 1, lines.Length - 1);
            }
            else
            {
                // If there is only one line or the input is empty, return an empty string
                return string.Empty;
            }
        }
    }
}
