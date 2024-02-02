using HarmonyLib;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CrewCombatFlag))]
    internal class CrewCombatFlagPatches
    {
        [HarmonyPatch("FormatStatTooltip")]
        [HarmonyPostfix]
        public static void FormatStatTooltip_Postfix(ref string __result, PropInstance prop1, PropInstance prop2, PropInstance prop3, CrewSide ___side, CrewCombat ___crewCombat, bool ___player)
        {
            CrewSide side = ___side;
            CrewCombat crewCombat = ___crewCombat;
            bool player = ___player;

            string text = $"{prop1.GetTooltip(35, rightAlign: true)}{prop2.GetTooltip((side == CrewSide.Player) ? 45 : 48, rightAlign: true)}{prop3.GetTooltip(47, rightAlign: true)}\n";

            string baseDMGTooltip = Game.game.caravan.FindProperty(player ? Prop.PlayerBaseDMG : Prop.EnemyBaseDMG).GetTooltip(0, rightAlign: true);
            string generalTooltip = (side != CrewSide.Player)
                ? FormatGeneralTooltip("Impervious Ward".FromDictionary(), Mathf.RoundToInt(crewCombat.GetWard(side)))
                : FormatGeneralTooltip("Enchanted Damage".FromDictionary(), Mathf.RoundToInt(crewCombat.GetEnchantedDMG(side)));

            string baseWardTooltip = Game.game.caravan.FindProperty(player ? Prop.PlayerBaseWard : Prop.EnemyBaseWard).GetTooltip(0, rightAlign: true);

            __result = $"{text}{baseDMGTooltip}{generalTooltip}{baseWardTooltip}";
        }

        private static string FormatGeneralTooltip(string title, int val)
        {
            return $"<b><size={VisualTweak.TooltipHeaderSize}%>{title}<indent=80%>{val}</indent></size></b>\n";
        }
    }
}
