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

            var proficiencyImage = __instance.gameObject.FindDeep("PerkUI/Image (1)").GetComponent<RectTransform>();
            proficiencyImage.sizeDelta = new Vector2(400, proficiencyImage.sizeDelta.y);

            var insightImage = __instance.gameObject.FindDeep("PerkUI/Image[6]").GetComponent<RectTransform>();
            insightImage.sizeDelta = new Vector2(400, insightImage.sizeDelta.y);
            insightImage.localPosition = new Vector2(-500, insightImage.localPosition.y);

            var insightValue = __instance.gameObject.FindDeep("PerkUI/InsightValue").GetComponent<RectTransform>();
            insightValue.localPosition = new Vector2(-350, insightValue.localPosition.y);
            var insightValue2 = __instance.gameObject.FindDeep("PerkUI/InsightValue").GetComponent<TextMeshProUGUI>();
            insightValue2.verticalAlignment = VerticalAlignmentOptions.Middle;

            var insightTitle = __instance.gameObject.FindDeep("PerkUI/InsightTitle").GetComponent<RectTransform>();
            insightTitle.sizeDelta = new Vector2(300, insightTitle.sizeDelta.y);
            insightTitle.localPosition = new Vector2(-530, insightTitle.localPosition.y);

            var insightTitle2 = __instance.gameObject.FindDeep("PerkUI/InsightTitle").GetComponent<TextMeshProUGUI>();
            insightTitle2.verticalAlignment = VerticalAlignmentOptions.Middle;

            var proficiencyTitle = __instance.gameObject.FindDeep("PerkUI/ProficiencyTitle").GetComponent<RectTransform>();
            proficiencyTitle.sizeDelta = new Vector2(300, proficiencyImage.sizeDelta.y);

            var proficiencyTitle2 = __instance.gameObject.FindDeep("PerkUI/ProficiencyTitle").GetComponent<TextMeshProUGUI>();
            proficiencyTitle2.verticalAlignment = VerticalAlignmentOptions.Middle;
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
