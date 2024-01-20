using HarmonyLib;
using System.Reflection.Emit;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(Skill))]
    internal class SkillPatches
    {

        [HarmonyPatch(nameof(Skill.FormatDependency))]
        [HarmonyPostfix]
        public static void FormatDependency_PostFix(ref string __result, Skill __instance, GameCharacter gchar = null, int overwriteProwess = -1)
        {
            string text = "";
            string text2 = "";
            if (__instance.level > 1)
            {
                string text3 = Tooltip.gray;
                int num = ((__instance.level == 3) ? 6 : 3);
                if (gchar != null)
                {
                    text3 = ((((overwriteProwess != -1) ? overwriteProwess : gchar.GetProperty(Prop.Prowess, true)) >= num) ? Tooltip.green : Tooltip.red);
                }
                text2 = string.Concat(new string[]
                {
                text2,
                "\n<color=",
                text3,
                ">"+"Prowess".FromDictionary()+" ",
                String.ToRoman(num),
                "</color>"
                });
            }
            if (__instance.dependencies.Count > 0)
            {
                text = string.Concat(new string[]
                {
                text,
                "\n\n<i><color=",
                Tooltip.gray,
                ">"+"Requirements:".FromDictionary()+"</color>",
                text2
                });
                foreach (PerkLevelDependency perkLevelDependency in __instance.dependencies)
                {
                    foreach (Perk perk in perkLevelDependency.perks)
                    {
                        string text4 = Tooltip.gray;
                        if (gchar != null)
                        {
                            bool flag = gchar.perks.Find(perk, perkLevelDependency.level, true);
                            bool flag2 = gchar.perks.Find(perk, perkLevelDependency.level, false);
                            text4 = (flag ? Tooltip.green : (flag2 ? Tooltip.orange : Tooltip.red));
                        }
                        text = string.Concat(new string[]
                        {
                        text,
                        "\n<color=",
                        text4,
                        ">",
                        perk.GetNameAndLevel(perkLevelDependency.level),
                        "</color>"
                        });
                    }
                }
                text += "</i>";
            }
            __result = text;
        }

        [HarmonyPatch(nameof(Skill.FormatDependencyVirtual))]
        [HarmonyPostfix]
        public static void FormatDependencyVirtual_PostFix(ref string __result, Skill __instance, GameCharacter gchar, VirtualPerk virtualPerk)
        {
            string text = "";
            string text2 = "";
            if (__instance.level > 1)
            {
                string text3 = Tooltip.gray;
                int num = ((__instance.level == 3) ? 6 : 3);
                if (gchar != null)
                {
                    text3 = ((virtualPerk.prowess >= num) ? Tooltip.green : Tooltip.red);
                }
                text2 = text2 + "\n<color=" + text3 + ">"+"Prowess".FromDictionary()+" " + String.ToRoman(num) + "</color>";
            }
            if (__instance.dependencies.Count > 0)
            {
                text = text + "\n\n<i><color=" + Tooltip.gray + ">"+"Requirements:".FromDictionary()+"</color>" + text2;
                foreach (PerkLevelDependency dependency in __instance.dependencies)
                {
                    foreach (Perk perk in dependency.perks)
                    {
                        string text4 = Tooltip.red;
                        if (virtualPerk.LoadPerkLevel(perk) >= dependency.level)
                        {
                            text4 = Tooltip.green;
                        }
                        text = text + "\n<color=" + text4 + ">" + perk.GetNameAndLevel(dependency.level) + "</color>";
                    }
                }
                text += "</i>";
            }
            __result = text;
        }
    }
}