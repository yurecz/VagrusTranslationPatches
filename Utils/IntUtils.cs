using Epic.OnlineServices.Auth;
using Epic.OnlineServices.UserInfo;
using UnityEngine;

namespace VagrusTranslationPatches.Utils
{
    public static class IntUtils
    {
        public static string FormatNumberByNomen(this int number, string nomen)
        {
            int lastDigit = number % 10;
            int last2Digit = number % 100;
            string formattedText;
            if (last2Digit > 4 && last2Digit <= 20) 
            {
                formattedText = $"{number} {Game.FromDictionary(nomen + "5") ?? Game.FromDictionary(nomen + "s")}";
            }
            else if (lastDigit == 1)
            {
                formattedText = $"{number} {Game.FromDictionary(nomen)}";
            }
            else if (lastDigit > 1 && lastDigit <= 4)
            {
                formattedText = $"{number} {Game.FromDictionary(nomen + "2") ?? Game.FromDictionary(nomen + "s")}";
            }
            else
            {
                formattedText = $"{number} {Game.FromDictionary(nomen + "5") ?? Game.FromDictionary(nomen + "s")}";
            }

            return formattedText.ToLower();
        }

        public static string ToDueInText(this int number)
        {
            string formattedText = "";
            if (number == 0)
            {
                formattedText = "is today".FromDictionary();
            }
            else
            {
                formattedText = "is due in".FromDictionary() + " " + number.FormatNumberByNomen("day");
            }

            return formattedText.ToLower();
        }

        public static string ToDaysAgoText(this int number)
        {
            string formattedText = "";
            if (number == 0)
            {
                formattedText = "today".FromDictionary();
            }
            else
            {
                formattedText = number.FormatNumberByNomen("day") + " " + "ago".FromDictionary();
            }

            return formattedText.ToLower();
        }

    }

}
