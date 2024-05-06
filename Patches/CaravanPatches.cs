using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(Caravan))]
    internal class CaravanPatches
    {
        //[HarmonyPatch("Awake")]
        //[HarmonyPostfix]
        //public static void Awake_Postfix(Caravan __instance, GameObject ___ScoutUIPrefab)
        //{
        //    ___ScoutUIPrefab.UpdatePrefabFonts();

        //}

        [HarmonyPatch("FormatMoneyLyrgBross")]
        [HarmonyPostfix]
        private static void FormatMoneyLyrgBross_Postfix(Caravan __instance, ref string __result, int _money = int.MaxValue, bool showZero = true, bool showLeadZero = true, int digits = 2)
        {
            __instance.MoneyToCurrency(out var _, out var lyrg, out var bross, _money);
            string text = lyrg.ToString();
            string text2 = bross.ToString();
            string text3 = "";
            while (text2.Length < digits)
            {
                text2 += " ";
            }

            while (text.Length < digits)
            {
                text += " ";
            }

            if (bross != 0 || (showZero && text3.Length > 0) || (showLeadZero && text3.Length == 0))
            {
                text3 += __instance.GetMoneySprite(TextSprite.Bross, text2);
            }

            if (lyrg != 0 || (showZero && text3.Length > 0) || (showLeadZero && text3.Length == 0))
            {
                text3 += (text3.Length > 0?"  ":"") + __instance.GetMoneySprite(TextSprite.Lyrg, text);
            }

            __result = text3.Trim();
        }

        [HarmonyPatch("LiberateUpdateCall")]
        [HarmonyPostfix]
        private static void LiberateUpdateCall_Postfix(Caravan __instance, int qty)
        {
            var marketQtyUI = __instance.marketQtyUI;
            if ((bool)marketQtyUI)
            {
                TweakFunctions.GetObedienceForLiberate(qty, out var val);
                var text = "Gain %value% Obedience".FromDictionary();
                text = text.ReplaceToken("%value%", String.Sign(val));
                marketQtyUI.UpdateGeneralText(text);
            }
        }

        [HarmonyPatch("WhipUpdateCall")]
        [HarmonyPostfix]
        private static void WhipUpdateCall_Postfix(Caravan __instance, int qty)
        {
            var marketQtyUI = __instance.marketQtyUI;
            if ((bool)marketQtyUI)
            {
                var text = "";
                TweakFunctions.GetObedienceForDiscipline(qty, out var min, out var max);
                if (min != max)
                {
                    text = "Gain %min%-%max% Obedience".FromDictionary();
                    text = text.ReplaceToken("%min%", min);
                    text = text.ReplaceToken("%max%", max);
                } else
                {
                    text = "Gain %value% Obedience".FromDictionary();
                    text = text.ReplaceToken("%value%", String.Sign(min));
                }
                marketQtyUI.UpdateGeneralText(text);
            }
        }

        [HarmonyPatch("BeastButcherUpdateCall")]
        [HarmonyPostfix]
        private static void BeastButcherUpdateCall_Postfix(Caravan __instance, int qty)
        {
            var marketQtyUI = __instance.marketQtyUI;
            if (!(marketQtyUI == null))
            {
                TweakFunctions.GetSupplyForButcher(Prop.Beast, qty, out var min, out var max);
                var text = "Gain %min%-%max% Supplies".FromDictionary();
                text = text.ReplaceToken("%min%", min);
                text = text.ReplaceToken("%max%", max);
                marketQtyUI.UpdateGeneralText(text);
            }
        }

        [HarmonyPatch("MountButcherUpdateCall")]
        [HarmonyPostfix]
        private static void MountButcherUpdateCall_Postfix(Caravan __instance, int qty)
        {
            var marketQtyUI = __instance.marketQtyUI;
            if (!(marketQtyUI == null))
            {
                TweakFunctions.GetSupplyForButcher(Prop.Mount, qty, out var min, out var max);
                var text = "Gain %min%-%max% Supplies".FromDictionary();
                text = text.ReplaceToken("%min%", min);
                text = text.ReplaceToken("%max%", max);
                marketQtyUI.UpdateGeneralText(text);
            }
        }


        [HarmonyPatch("GetPropertyDetails")]
        [HarmonyPrefix]
        public static bool GetPropertyDetails_Prefix(Caravan __instance, ref string __result, Prop prop, bool total = true)
        {
            PropInstance propInstance = __instance.FindProperty(prop);
            if (propInstance == null)
            {
                Game.Exception("Property " + prop.ToString() + " not found.");
                __result = "";
                return false;
            }
            string text;
            if (propInstance.property.IsPropList(PropList.Status))
            {
                text = __instance.GetPropertyLevelName(prop, int.MaxValue, total).ToString();
            }
            else
            {
                int num = propInstance.Get(total);
                switch (prop)
                {
                    case Prop.Deputies:
                    case Prop.CCDeputies:
                        text = ((num == 0) ? "Not assigned".FromDictionary() : num.ToString());
                        break;
                    case Prop.CargoWorth:
                    case Prop.CargoWorthWOStash:
                    case Prop.EquipmentWorth:
                    case Prop.ItemWorth:
                    case Prop.ComitatusWorth:
                    case Prop.ComitatusWorthWOTasks:
                        text = __instance.FormatMoneySingle(num);
                        break;
                    default:
                        text = num.ToString();
                        break;
                }
            }
            if (propInstance.property.percentage)
            {
                text += "%";
            }
            if ((uint)(prop - 158) <= 1u)
            {
                text += " " + "assigned".FromDictionary();
            }
            __result = text;
            return false;
        }

    }
}