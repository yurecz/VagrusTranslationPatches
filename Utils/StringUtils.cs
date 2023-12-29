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

        public static string FromDictionary(this string str)
        {
            return Game.FromDictionary(str);
        }
    }
}
