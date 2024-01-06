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
    internal class CharUI2Patches
    {
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

            if (character == null) {
                ___labelVitalityDurability.text = Game.FromDictionary("Vitality");
                ___labelName.text = Game.FromDictionary("No Companions");
                ___labelArmorHardness.text = Game.FromDictionary("Armor");
                ___labelLoyalty.text = "";
            }
        }

    }
}
