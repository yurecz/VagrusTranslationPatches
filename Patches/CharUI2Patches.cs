using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Vagrus;
using Vagrus.UI;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CharUI2))]
    internal class CharUI2Patches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void ApplyCharacter_Postfix(
        CharUI2 __instance,
        Transform ___perkPrefab
        )
        {
            ___perkPrefab.gameObject.UpdatePrefabFonts();   
        }

        [HarmonyPatch("ApplyCharacter")]
        [HarmonyPostfix]
        public static void ApplyCharacter_Postfix(
            CharUI2 __instance, 
            ICharacterDisplay character, 
            TextMeshProUGUI ___labelName, 
            TextMeshProUGUI ___labelVitalityDurability,
            TextMeshProUGUI ___labelArmorHardness,
            TextMeshProUGUI ___labelLoyalty
        )
        {

            var resistances = __instance.transform.Find("CharacterSheet/Res/Resistances").GetComponent<TextMeshProUGUI>();
            resistances.text = Game.FromDictionary("Resistances");

            ___labelArmorHardness.text = Game.FromDictionary(___labelArmorHardness.text);
            ___labelVitalityDurability.text = Game.FromDictionary(___labelVitalityDurability.text);

            if (character == null) {
                ___labelName.text = Game.FromDictionary("No Companions");
                ___labelLoyalty.text = "";
            }
        }

    }
}
