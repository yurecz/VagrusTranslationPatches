using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(Gear))]
    internal class GearPatches
    {
        [HarmonyPatch("GetTooltip")]
        [HarmonyPostfix]
        public static void GetTooltip_Postfix(Gear __instance, ref string __result, Character selcharacter = null, Character mountcharacter = null)
        {
            string text = string.Concat(new string[]
            {
            "<b><size=",
            VisualTweak.TooltipHeaderSize.ToString(),
            "%>",
            __instance.GetName(),
            "</size></b>"
            });
            if (__instance.effectsOld.Count > 0)
            {
                text = text + "\n" + ImpactBlock.ToString(__instance.effectsOld, Colorize.Tooltip);
            }
            string text2 = __instance.GetDescription();
            if (text2.Length > 0)
            {
                text = text + "\n\n" + text2;
            }
            if (__instance.requirements.Length > 0 || __instance.perkRequirements.Count > 0)
            {
                text = text + "\n\n<color=" + Tooltip.gray + ">"+Game.FromDictionary("Requirements:")+"</color>";
                if (__instance.requirements.Length > 0)
                {
                    text = text + "\n" + __instance.requirements;
                }
                foreach (Perk perk in __instance.perkRequirements)
                {
                    string text3 = ((selcharacter != null && Game.game.leader.GetPerkLevelCompanion(perk, selcharacter, false, false, true) > 0) ? Tooltip.green : Tooltip.red);
                    text = string.Concat(new string[]
                    {
                    text,
                    "\n<color=",
                    text3,
                    ">",
                    perk.GetName(),
                    "</color>"
                    });
                }
            }
            if (__instance.perkGranted.Count > 0 || __instance.perkGrantedLevel2.Count > 0)
            {
                text = text + "\n\n<color=" + Tooltip.gray + ">"+Game.FromDictionary("Perks Granted:")+"</color>";
                foreach (Perk perk2 in __instance.perkGranted)
                {
                    text = text + "\n" + String.ColorizeSigned("+1 " + perk2.GetName(), Colorize.Tooltip);
                }
                foreach (Perk perk3 in __instance.perkGrantedLevel2)
                {
                    text = text + "\n" + String.ColorizeSigned("+2 " + perk3.GetName(), Colorize.Tooltip);
                }
            }
            if (mountcharacter)
            {
                text = string.Concat(new string[]
                {
                text,
                "\n\n<color=",
                Tooltip.red,
                "><b>*"+"In use by".FromDictionary()+" ",
                mountcharacter.GameName(false),
                "</b></color>"
                });
            }
            __result = text;
        }
    }
}