using HarmonyLib;
using System.Linq;
using System.Security.Cryptography;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(Faction))]
    internal class FactionPatches
    {
        [HarmonyPatch(nameof(Faction.GetTierDescription))]
        [HarmonyPostfix]
        public static void GetTierDescription_Postfix(Faction __instance, ref string __result,int tier)
        {
            string text = "";
            foreach (FactionReward reward in __instance.GetRewards(tier))
            {
                foreach (Container container in reward.GetContainers())
                {
                    text = text + "- " + container.GetDescription() + "\n";
                }
            }
            string text2 = "?";
            foreach (TierDescription tierDescription in __instance.tierDescriptions)
            {
                if (tierDescription.tier == tier)
                {
                    var description = "";
                    var descriptions = tierDescription.description.Split(new char[] { '\n' });

                    for (int i = 0; i < descriptions.Length; i++)
                    {
                        descriptions[i] = descriptions[i].TrimEnd('.', ' ');
                        descriptions[i] = descriptions[i].FromDictionary(true);
                    }

                    description = string.Join("\n", descriptions);

                    text2 = Game.FormatText(description, makeCodexLinks: false);

                    break;
                }
            }

            string text3 = text + text2;
            text3 = text3.Replace("\n- ", "\n<sprite=\"icon_collector\" index=52>");
            if (text3.StartsWith("- "))
            {
                text3 = "<sprite=\"icon_collector\" index=52>" + text3.Substring(2);
            }
            __result = text3;
        }
    }
}