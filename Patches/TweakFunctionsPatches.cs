using HarmonyLib;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(TweakFunctions))]
    internal class TweakFunctionsPatches
    {

        [HarmonyPatch(nameof(TweakFunctions.RandomProp))]
        [HarmonyPrefix]
        public static bool RandomProp_Prefix(Prop prop, int chance, int qty = 1, string statusText = "due to a status", Outpost outpost = null)
            {
                chance = Mathf.Clamp(chance + (Game.IsTest() ? 50 : 0), 1, 100);
                bool flag = Game.Test(chance, realRandom: true);
                if (flag)
                {
                    if (outpost == null)
                    {
                        Game.game.caravan.AddProperty(prop, qty);
                        string text = TextTemplate.GetText(TextTemplateType.GameLog, "RandomProp");
                        string black = VisualTweak.Black;
                        black = ((qty <= 0) ? VisualTweak.VagrusRed : VisualTweak.VagrusGreen);
                        text = text.Replace("%quantity%", "<color=" + black + ">" + qty.ToString("+#;-#;0") + "</color>");
                        text = text.Replace("%property%", prop.ToString().FromDictionary());
                        text = text.Replace("%statustext%", statusText.FromDictionary());
                        GameLogUI.Add(GameLogType.People, text, "crew");
                    }
                    else
                    {
                        outpost.AddProperty(prop, qty);
                    }
                }

                return false;
            }

        }
    }