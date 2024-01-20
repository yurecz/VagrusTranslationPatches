using HarmonyLib;
using System;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(Perk))]
    internal class PerkPatches
    {
        [HarmonyPatch(nameof(Perk.FormatDependency))]
        [HarmonyPatch(new Type[] { typeof(int),typeof(Character) })]
        [HarmonyPostfix]
        public static void FormatDependency_Postfix1(Perk __instance, ref string __result, int level, Character character = null)
        {
                string text = "";
                string text2 = "";
                GameCharacter gameCharacter = null;
                if (character != null)
                {
                    gameCharacter = character.GetGameCharacter();
                }
                if (__instance.prowessDependency > 1)
                {
                    string text3 = Tooltip.gray;
                    if (gameCharacter != null)
                    {
                        text3 = (__instance.IsProwessMet(gameCharacter) ? Tooltip.green : Tooltip.red);
                    }
                    text2 = text2 + "\n<color=" + text3 + ">"+"Prowess".FromDictionary()+" " + String.ToRoman(__instance.prowessDependency) + "</color>";
                }
                if (__instance.dependencies.Count > 0)
                {
                    foreach (PerkLevelDependency dependency in __instance.dependencies)
                    {
                        if (dependency.perklevel != level)
                        {
                            continue;
                        }
                        foreach (Perk perk in dependency.perks)
                        {
                            bool flag = false;
                            string text4 = Tooltip.gray;
                            if (gameCharacter != null)
                            {
                                bool num = gameCharacter.perks.Find(perk, dependency.level, baseOnly: true);
                                flag = gameCharacter.perks.Find(perk, dependency.level);
                                text4 = (num ? Tooltip.green : (flag ? Tooltip.orange : Tooltip.red));
                            }
                            text2 = text2 + "\n<color=" + text4 + ">" + perk.GetNameAndLevel(dependency.level) + "</color>";
                        }
                    }
                }
                if (text2.Length > 0)
                {
                    text = text + "\n\n<i><color=" + Tooltip.gray + ">" + Game.FromDictionary("Requirements:") + "</color>";
                    text += text2;
                    text += "</i>";
                }
            __result = text;
        }
        [HarmonyPatch(nameof(Perk.FormatDependency))]
        [HarmonyPatch(new Type[] { typeof(int), typeof(Character), typeof(VirtualPerk) })]
        [HarmonyPostfix]
        public static void FormatDependency_Postfix2(Perk __instance, ref string __result, int level, Character character, VirtualPerk virtualPerk)
        {
            string text = "";
            string text2 = "";
            GameCharacter gameCharacter = null;
            if (character != null)
            {
                gameCharacter = character.GetGameCharacter();
            }
            if (__instance.prowessDependency > 1)
            {
                string text3 = Tooltip.gray;
                if (gameCharacter != null)
                {
                    text3 = ((__instance.prowessDependency <= virtualPerk.prowess) ? Tooltip.green : Tooltip.red);
                }
                text2 = text2 + "\n<color=" + text3 + ">"+"Prowess".FromDictionary() +" " + String.ToRoman(__instance.prowessDependency) + "</color>";
            }
            if (__instance.dependencies.Count > 0)
            {
                foreach (PerkLevelDependency dependency in __instance.dependencies)
                {
                    if (dependency.perklevel != level)
                    {
                        continue;
                    }
                    bool flag = false;
                    string gray = Tooltip.gray;
                    foreach (Perk perk in dependency.perks)
                    {
                        flag = true;
                        if (virtualPerk.LoadPerkLevel(perk) < dependency.level)
                        {
                            flag = false;
                        }
                        gray = (flag ? Tooltip.green : Tooltip.red);
                        text2 = text2 + "\n<color=" + gray + ">" + perk.GetNameAndLevel(dependency.level) + "</color>";
                    }
                }
            }
            if (text2.Length > 0)
            {
                text = text + "\n\n<i><color=" + Tooltip.gray + ">"+"Requirements:".FromDictionary()+"</color>";
                text += text2;
                text += "</i>";
            }
            __result = text;
        }
    }
}