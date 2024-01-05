using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using Vagrus;
using Vagrus.UI;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CharUI2))]
    internal class CharUI2Patch
    {
        [HarmonyPatch("ApplyCharacter")]
        [HarmonyPostfix]
        public static void ApplyCharacter_Postfix(CharUI2 __instance, ICharacterDisplay character)
        {
            var labelVitalityDurability = Traverse.Create(__instance).Field("labelVitalityDurability").GetValue() as TextMeshProUGUI;

            if (character == null) {
               // labelVitalityDurability.text = "";
            }
        }

    }
}
