using Epic.OnlineServices.Auth;
using Epic.OnlineServices.UserInfo;
using LingvoNET;
using System;
using UnityEngine;

namespace VagrusTranslationPatches.Utils
{
    public static class IntUtils
    {
        public static string FormatNumberByNomen(this int number, string nomen,bool withSign = false)
        {
            int module = Math.Abs(number);
            string stringNumber = withSign? String.Sign(number):number.ToString();
            int lastDigit = module % 10;
            int last2Digit = module % 100;
            string formattedText;
            if (last2Digit > 4 && last2Digit <= 20) 
            {
                formattedText = $"{stringNumber} {Game.FromDictionary(nomen + "5") ?? Game.FromDictionary(nomen + "s")}";
            }
            else if (lastDigit == 1)
            {
                formattedText = $"{stringNumber} {Game.FromDictionary(nomen)}";
            }
            else if (lastDigit > 1 && lastDigit <= 4)
            {
                formattedText = $"{stringNumber} {Game.FromDictionary(nomen + "2") ?? Game.FromDictionary(nomen + "s")}";
            }
            else
            {
                formattedText = $"{stringNumber} {Game.FromDictionary(nomen + "5") ?? Game.FromDictionary(nomen + "s")}";
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

        public static string ToDaysLeftText(this int number)
        {
            string formattedText = "";
            if (number == 0)
            {
                formattedText = "is today".FromDictionary();
            }
            else
            {
                formattedText = string.Format("{0} left".FromDictionary(),number.FormatNumberByNomen("day"));
            }

            return formattedText.ToLower();
        }

        public static string ToInDaysText(this int number)
        {
            string formattedText = "";
            if (number == 0)
            {
                formattedText = "is today".FromDictionary();
            }
            else
            {
                formattedText = string.Format("in {0}".FromDictionary(), number.FormatNumberByNomen("day"));
            }

            return formattedText.ToLower();
        }

        public static string ToDaysAgoText(this int number,bool roughly = false)
        {
            string formattedText = "";
            if (number == 0)
            {
                formattedText = $"<b>{"today".FromDictionary()}</b>";
            }
            else
            {
                var value = number;
                formattedText = $"<b>{value.FormatNumberByNomen("day")}</b> {"ago".FromDictionary()}";
                if (roughly)
                {
                    value = number / 360;
                    if (value >= 1)
                    {
                        formattedText = $"<b>{value.FormatNumberByNomen("year")}</b> {"ago".FromDictionary()}";
                    }
                    else
                    {
                        value = number / 30;
                        if (value >= 3)
                        {
                            formattedText = $"<b>{value.FormatNumberByNomen("month")}</b> {"ago".FromDictionary()}";
                        }
                    }

                }
            }

            return formattedText.ToLower();
        }

    }

}
