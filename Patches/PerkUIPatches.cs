using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(PerkUI))]
    internal class PerkUIPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(PerkUI __instance)
        {
            var gameObject = __instance.gameObject;
            if (gameObject != null && !gameObject.HasComponent<UIFontUpdater>())
            {
                gameObject.AddComponent<UIFontUpdater>();
            }
            
            var prefabPerkSingleRow = Resources.Load("UI/Prefab/PerkSingleRow") as GameObject;

            if (prefabPerkSingleRow != null && !prefabPerkSingleRow.HasComponent<UIFontUpdater>())
            {
                prefabPerkSingleRow.AddComponent<UIFontUpdater>();
            }
        }

        [HarmonyPatch("UpdateCombatAbilitiesOld")]
        [HarmonyPostfix]
        public static void UpdateCombatAbilitiesOld_PostFix( List<Skill> ___resetCombatAbilities, GameCharacter ___gchar, VirtualPerk ___virtualPerk, Tooltip ___tooltip, int ___maxCombatAbilityLevel)
        {
            var resetCombatAbilities = ___resetCombatAbilities;
            var gchar = ___gchar;
            var virtualPerk = ___virtualPerk;
            var tooltip = ___tooltip;
            var maxCombatAbilityLevel = ___maxCombatAbilityLevel;

            int i = 0;
            for (int count = gchar.GetBaseSkills().Count; i < count; i++)
            {
                Skill skill = resetCombatAbilities[i];
                var levels = gchar.GetLevelsOfSkill(skill);
                int currentLevel = levels.level;
                for (int j = 0; j < maxCombatAbilityLevel; j++)
                {
                    int num = j + 1;
                    var skillLevel = gchar.GetGivenLevelOfSkill(skill, num);
                    int perkCost = -1;
                    if (skillLevel != null)
                    {
                        string text = skillLevel.GetTooltipVirtual(gchar.character, virtualPerk);
                        perkCost = LeaderTweak.GetPerkCost(Perk.GetTerrifying(), currentLevel, num, 0, virtualPerk.GetCombatSkillsTraits());
                        if (perkCost > 0 && num > currentLevel)
                        {
                            text = text + "\n\n[" + "Costs %perkCost%".FromDictionary().Replace("%perkCost%", perkCost.FormatNumberByNomen("proficiency point")) + "]";
                        }
                        tooltip.UpdateLink("perk_combatability" + i + j, text);
                    }
                }
            }
        }
    }
}