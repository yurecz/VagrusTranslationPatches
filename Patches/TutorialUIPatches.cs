using HarmonyLib;
using System.ComponentModel;
using UnityEngine;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(TutorialUI))]
    internal class TutorialUIPatches
    {

        [HarmonyPatch(nameof(TutorialUI.CompenstateIfOutOFScreen))]
        [HarmonyPostfix]
        public static void CompenstateIfOutOFScreen_Postfix(TutorialUI __instance, RectTransform rect, GameObject ___holder, ref Vector2 ___boxOffset, bool ___visible, int ___resize)
        {
            if (!Tutorial.minimized && ___visible && ___resize > 2)
            {
                var bottomRight = ___holder.transform.Find("Back/BottomRight").GetComponent<RectTransform>().position;
                if (bottomRight.y <= 0)
                {
                    RectTransform component = ___holder.GetComponent<RectTransform>();
                    component.localPosition += new Vector3(0f, 10f, 0f);
                }
            }
        }
    }
}