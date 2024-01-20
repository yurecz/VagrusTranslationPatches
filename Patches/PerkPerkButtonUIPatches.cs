using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(PerkPerkButtonUI))]
    internal class PerkPerkButtonUIPatches
    {

        [HarmonyPatch(nameof(PerkPerkButtonUI.RefreshTooltipText))]
        [HarmonyPostfix]
        public static void RefreshTooltipText_Postfix(PerkPerkButtonUI __instance, Perk ___perk, int ___level,PerkPerkEntryUI ___perkEntryUI, VirtualPerk ___virtualPerk)
        {
            string text = "";
            if (__instance.GetStatus() != PerkButtonStatus.HiddenLevel)
            {
                text = new PerkEntry(___perk, ___level).GetTooltip(___virtualPerk.gchar.character, ___virtualPerk);
                int virtualLevel = ___perkEntryUI.GetVirtualLevel();
                int perkCost = LeaderTweak.GetPerkCost(___perk, virtualLevel, ___level, 0);
                if (___perk.type == PerkType.CombatTrait)
                {
                    perkCost = LeaderTweak.GetPerkCost(___perk, virtualLevel, ___level, 0, ___virtualPerk.GetCombatSkillsTraits());
                }
                if (___perk.IsAvailable() && perkCost > 0 && (__instance.GetStatus() == PerkButtonStatus.ActiveEmpty || __instance.GetStatus() == PerkButtonStatus.InactiveEmpty))
                {
                    text = text + "\n\n["+"Costs %perkCost%".FromDictionary().Replace("%perkCost%",perkCost.FormatNumberByNomen("proficiency point")) +"]";
                }
                if (!___perk.IsAvailable())
                {
                    text += "\n\n<i>"+"Not available".FromDictionary()+"</i>";
                }
            }

            MethodInfo methodInfo = typeof(PerkPerkButtonUI).GetMethod("UpdateTooltip", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { text };
            methodInfo.Invoke(__instance, parameters);

        }
    }
}