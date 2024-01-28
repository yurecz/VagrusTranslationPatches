using HarmonyLib;
using System.Reflection;
using System.Reflection.Emit;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(PerkButtonUI))]
    internal class PerkButtonUIPatches
    {

        [HarmonyPatch(nameof(PerkButtonUI.RefreshTooltipText))]
        [HarmonyPostfix]
        public static void RefreshTooltipText_Postfix(PerkButtonUI __instance, int ___level, Perk ___perk, VirtualVagrus ___virtualVagrus, PerkButtonStatus ___status, PerkEntryUI ___perkEntryUI)
        {
            string text = "";
            var level = ___level;
            var perk = ___perk;
            var virtualVagrus = ___virtualVagrus;
            var status = ___status;
            var perkEntryUI = ___perkEntryUI;
            if (status != PerkButtonStatus.HiddenLevel)
            {
                text = perk.GetTooltip(level);
                int virtualLevel = perkEntryUI.GetVirtualLevel();
                string description = Tooltip.GetDescription("LeaderPerkCosts");
                int cost = perk.GetCost(virtualLevel, level, virtualVagrus.CountPerkTypeLevel(PerkType.Leadership));
                description = description.Replace("%insightcost%", cost.ToString() ?? "");
                if (GetResourcefulnessCost(perk) != 0 )
                    description = description.Replace("%RFcost%", GetResourcefulnessCost(perk).ToString() ?? "").Replace("%insightcost%", cost.ToString() ?? "");
                else
                    description = "Cost: %insightcost% Insight".FromDictionary().ReplaceToken("%insightcost%", cost.ToString() ?? "");

                if (perk.IsAvailable() && cost > 0 && (status == PerkButtonStatus.ActiveEmpty || status == PerkButtonStatus.InactiveEmpty))
                {
                    text = text + "\n\n" + description;
                }
                if (!perk.IsAvailable())
                {
                    text += "\n\n<i>"+"Not available".FromDictionary()+"</i>";
                }
            }

            MethodInfo methodInfo = typeof(PerkButtonUI).GetMethod("UpdateTooltip", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { text };
            text = (string)methodInfo.Invoke(__instance, parameters);
        }

        private static int GetResourcefulnessCost(Perk perk)
        {
            return perk.ID switch
            {
                "recHMo0COX7qsFqYW" => CombatTweak.AidResourceFulnessCost,
                "recypZxeOeq6DCD3v" => CombatTweak.BoostResourceFulnessCost,
                "recay1dJdNmjOm2vu" => CrewCombatTweak.CommandRFCost,
                "recrRxSdVQXk4KXyj" => CombatTweak.EmpowerResourceFulnessCost,
                "rec4PsnXVJYg0mgOx" => LeaderTweak.CostOfEncourage,
                "rec1PXIwdOV0sTwVl" => LeaderTweak.CostOfEnhance2,
                "recqJnIIJz4FRRMRB" => FactionTweak.GetTradeTaskRefreshRFCost(),
                "recfDIDLVumfV7XU1" => CombatTweak.InspireResourceFulnessCost,
                "recitURstYuWpGZrT" => CombatTweak.PrepareResourceFulnessCost,
                "recKgWdVmwDb6IcXj" => LeaderTweak.CostOfTraverse,
                _ => 0,
            };
        }
    }
}