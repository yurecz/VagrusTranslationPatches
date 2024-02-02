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
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(Caravan __instance, GameObject ___ScoutUIPrefab)
        {
            ___ScoutUIPrefab.UpdatePrefabFonts();
            ___ScoutUIPrefab.AddIfNotExistComponent<UIObjectTranslator>();

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
    }
}