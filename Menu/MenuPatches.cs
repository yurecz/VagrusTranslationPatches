using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch]
    internal class MenuPatches
    {

        static Vector2 shift = new Vector2(100f,0);

        [HarmonyPatch(typeof(KeybindUI), "InitializeKeybind")]
        [HarmonyPostfix]
        public static void InitializeKeybind_Postfix(KeybindUI __instance, Toggle ___btnPrimary, TextMeshProUGUI ___title)
        {
            //___btnPrimary.transform.localPosition += shift;
            ___title.rectTransform.sizeDelta += new Vector2(200f, 0);

            //___KeybindPrefab.FindDeep("KeyBindButtonPrimary").GetComponent<RectTransform>().localPosition += shift;
            //___KeybindPrefab.FindDeep("KeyBindButtonSecondary").GetComponent<RectTransform>().localPosition += shift;
        }

        [HarmonyPatch(typeof(CheckboxUI), "Initialize")]
        [HarmonyPostfix]
        public static void Initialize_Postfix(CheckboxUI __instance, GameObject ___mark, string subtitle)
        {
            if (string.IsNullOrEmpty(subtitle))
            {
                ___mark.transform.parent.localPosition += new Vector3(200f, 0, 0);
            }
        }
    }
}