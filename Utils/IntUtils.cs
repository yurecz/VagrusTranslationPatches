using UnityEngine;

namespace VagrusTranslationPatches.Utils
{
    /// <summary>
    /// Static utilities class for common functions and properties to be used within your mod code
    /// </summary>
    public static class IntUtils
    {
        /// <summary>
        /// Example static method to return Players current location / transform
        /// </summary>
        /// <returns></returns>
        public static string FormatNumberByNomen(this int number, string nomen)
        {
            int lastDigit = number % 10;
            string formattedText = "";
            if (number == 1)
            {
                formattedText = $"{number} {Game.FromDictionary(nomen)}";
            }
            else if (number > 4 && number <= 20)
            {
                formattedText = $"{number} {Game.FromDictionary(nomen+"s")}";
            }
            else if (lastDigit > 1 && lastDigit <= 4)
            {
                formattedText = $"{number} {Game.FromDictionary(nomen+"") ?? Game.FromDictionary(nomen + "s")}";
            }
            else
            {
                formattedText = $"{number} {Game.FromDictionary(nomen + "s")}";
            }

            return formattedText;
        }
    }
}
