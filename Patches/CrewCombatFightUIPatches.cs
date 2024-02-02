using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CrewCombatFightUI))]
    internal class CrewCombatFightUIPatches
    {
        [HarmonyPatch("UpdateAttributes")]
        [HarmonyPostfix]
        public static void UpdateAttributes_Postfix(CrewCombatFightUI __instance, CrewCombat ___crewCombat, FlexibleStat ___flexibleStatPlayer, FlexibleStat ___flexibleStatEnemy, CrewSide side)
        {
            string text = "";
            string text2 = "";
            foreach (CrewCombatAttribute crewCombatAttribute in ___crewCombat.SortAttributes(___crewCombat.GetAttributes(side)))
            {
                if (text.Length > 0)
                {
                    text += ", ";
                }
                text += crewCombatAttribute.GetTitle();
                text2 = text2 + crewCombatAttribute.GetTooltip(false) + "\n\n";
            }
            if (side == CrewSide.Player)
            {
                ___flexibleStatPlayer.Set(Game.FromDictionary("Your Attributes"), text, "", text2);
                return;
            }
            if (side == CrewSide.Enemy)
            {
                ___flexibleStatEnemy.Set(Game.FromDictionary("Enemy Attributes"), text, "", text2);
            }
        }
    }
}