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
    [HarmonyPatch(typeof(SkillDisplay))]
    internal class SkillDisplayPatches
    {
        [HarmonyPatch("ApplySkill")]
        [HarmonyPostfix]
        public static void ApplySkill_Postfix(
            SkillDisplay __instance,
            GameCharacter ___gchar,
            Skill ___baseSkill,
            TextMeshProUGUI ___labelName,
            TextMeshProUGUI ___labelDmgHeal
        )
        {
            ___labelDmgHeal.text = Game.FromDictionary(___labelDmgHeal.text);
            if (___gchar == null || ___baseSkill == null) { 
                ___labelName.text = Game.FromDictionary("No Skill");
            }
        }

    }
}
