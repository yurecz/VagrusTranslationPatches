using HarmonyLib;
using TMPro;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch()]
    internal class CampUIPatches
    {
        [HarmonyPatch(typeof(CampUI),"UpdateUpkeep")]
        [HarmonyPostfix]
        public static void UpdateUpkeep_Postfix(CampUI __instance)
        {
           var unpaidDaysText = Game.game.camp.GetUnpaidDays().FormatNumberByNomen("day");
           var unpaidDays = Traverse.Create(__instance).Field("unpaidDays").GetValue() as TextMeshProUGUI;
           unpaidDays.text = unpaidDaysText;
        }

        [HarmonyPatch(typeof(CampUI), "UpdateDefenceTooltips")]
        [HarmonyPostfix]
        public static void UpdateDefenceTooltips_Postfix(CampUI __instance)
        {
            var tooltip = Traverse.Create(__instance).Field("tooltip").GetValue() as Tooltip;
            for (int i = 1; i <= 3; i++)
            {
                Impact impact = Impact.FindByName(CampUI.game.camp.campMode.ToString() + "Camp" + i.ToString());
                string text = CampUI.game.camp.GetDefenceDescription(impact.GetDescription(), i, true);
                int defenceCost = CampUI.game.camp.GetDefenceCost(i);
                if (defenceCost > 0)
                {
                    text = string.Concat(new string[]
                    {
                    text,
                    "\n",
                    Game.FromDictionary("Cost"),
                    ": ",
                    CampUI.game.caravan.FormatMoney(defenceCost, true, true, 3, "")
                    });
                }
                string dependencyText = impact.GetDependencyText(true);
                int guardCrewForRate = TweakFunctions.GetGuardCrewForRate(TweakFunctions.GetGeneralCampRequiredGuardRate(i, false));
                int num = ((i == 1) ? 0 : TweakFunctions.GetGuardCrewForRate(TweakFunctions.GetGeneralCampRequiredGuardRate(i, true)));
                int property = CampUI.game.caravan.GetProperty(Prop.CrewGuard, true);
                bool flag = TweakFunctions.IsGeneralCampEnabled(i, false);
                bool flag2 = TweakFunctions.IsGeneralCampEnabled(i, true);
                string text2 = string.Concat(new string[]
                {
                "<color=",
                flag ? Tooltip.green : Tooltip.red,
                ">",
                guardCrewForRate.ToString(),
                "</color>"
                });
                string text3 = (flag ? "" : string.Concat(new string[]
                {
                " or <color=",
                flag2 ? Tooltip.green : Tooltip.red,
                ">",
                num.ToString(),
                "</color> at the cost of ",
                global::String.ColorizeSigned("-" + GeneralSettings.GeneralCampVigorCost.ToString(), Colorize.Tooltip),
                " Vigor"
                }));
                if (guardCrewForRate > 0 && dependencyText.Length > 0)
                {
                    global::String.Replace(ref dependencyText, "%required1%", text2 ?? "");
                    global::String.Replace(ref dependencyText, "%required2%", text3 ?? "");
                    global::String.Replace(ref dependencyText, "%actual%", "(you have " + property.ToString() + ")");
                    if (dependencyText.Length > 0 && CampUI.game.camp.campMode == CampMode.General)
                    {
                        text = text + "\n" + dependencyText;
                    }
                }
                text += CampUI.game.camp.DefenceMansioText(i);
                tooltip.UpdateLink("ButtonDefence" + i.ToString(), text);
            }
        }
    }
}