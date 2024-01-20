using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(CombatCharacter))]
    internal class CombatCharacterPatches
    {
        [HarmonyPatch(nameof(CombatCharacter.FormatEffects))]
        [HarmonyPatch(new Type[] { typeof(List<EffectProp>), typeof(Colorize), typeof(string), typeof(EffectCategory), typeof(GameCharacter), typeof(CombatCharacter) })]
        [HarmonyPostfix]
        public static void FormatEffects_Postfix1(ref string __result, List<EffectProp> effects, Colorize colorize = Colorize.None, string separator = ", ", EffectCategory category = EffectCategory.None, GameCharacter gchar = null, CombatCharacter selectedCharacter = null)
        {
            if (effects.Count == 0)
            {
                __result = "";
                return;
            }
            bool flag = colorize == Colorize.Tooltip;
            bool flag2 = false;
            int num = 65536;
            int num2 = 0;
            List<EffectRow> list = new List<EffectRow>();
            foreach (EffectProp effect in effects)
            {
                if (category != 0 && effect.category != category)
                {
                    continue;
                }
                if (effect.linkTo == EffectLink.Cover && selectedCharacter != null)
                {
                    foreach (ActiveEffect activeEffect in selectedCharacter.activeEffects)
                    {
                        if (activeEffect.effect.ID == "recz0AtTiKUCT2NVs")
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                string text = "";
                if (effect.subcategory == EffectSubCategory.Cleanse)
                {
                    text = text + "Cleanse".FromDictionary() + " " + effect.FormatCategory().FromDictionary();
                }
                else if (effect.subcategory == EffectSubCategory.Taunt)
                {
                    text = text + "Taunt".FromDictionary() + " " + effect.FormatCategory().FromDictionary();
                }
                else if (effect.category == EffectCategory.DOT)
                {
                    if (flag)
                    {
                        text = text + CombatUI.GetSprite(CombatSprite.DOT, "gray") + " ";
                    }
                    text = text + String.FormatModValue(effect.mod, 1, colorize, flipColor: false, verbose: true, showsign: false, 1f, effect.subcategory) + " " + effect.resisttype.ToString().FromDictionary() + " " + "damage".FromDictionary();
                }
                else if (effect.category == EffectCategory.HOT)
                {
                    if (flag)
                    {
                        text = text + CombatUI.GetSprite(CombatSprite.DOT, "gray") + " ";
                    }
                    text = text + String.FormatModValue(effect.mod, 1, colorize, flipColor: false, verbose: true, showsign: false, 1f, effect.subcategory) + ((effect.stat == EffectStat.VIT) ? " " + "heal".FromDictionary() : " " + "power".FromDictionary());
                }
                else if (effect.category == EffectCategory.Imped)
                {
                    if (flag)
                    {
                        text = text + CombatUI.GetSprite(CombatSprite.Impediment, "gray") + " ";
                    }
                    text += effect.subcategory.ToString().FromDictionary();
                }
                else if (effect.category == EffectCategory.Buff && effect.subcategory == EffectSubCategory.Enchanted)
                {
                    if (flag)
                    {
                        text = text + CombatUI.GetSprite(CombatSprite.Buff, "gray") + " ";
                    }
                    text = effect.subcategory.ToString();
                }
                else if (effect.category == EffectCategory.Buff && effect.subcategory == EffectSubCategory.HitsBackOnBlock && effect.linkTo != 0)
                {
                    text = "Upon a successful Block".FromDictionary() + ", " + "strikes back in melee range".FromDictionary();
                }
                else if (effect.category == EffectCategory.Buff && effect.subcategory == EffectSubCategory.Immunity)
                {
                    if (flag)
                    {
                        text = text + CombatUI.GetSprite(CombatSprite.Buff, "gray") + " ";
                    }
                    text += "Immune to".FromDictionary() + " ";
                    foreach (EffectImmunityType immunitytype in effect.immunitytypes)
                    {
                        text = ((immunitytype != EffectImmunityType.NonMagicalAttack) ? (text + immunitytype.ToString().FromDictionary()) : (text + "Nonmagical Attack".FromDictionary()));
                    }
                }
                else if (effect.category == EffectCategory.Buff && effect.subcategory == EffectSubCategory.Transformed)
                {
                    text = "Transformed".FromDictionary();
                }
                else
                {
                    string text2 = ((effect.subcategory == EffectSubCategory.None) ? effect.resisttype.ToString().FromDictionary() : effect.subcategory.ToString().FromDictionary());
                    string text3 = ((effect.stat == EffectStat.None) ? (effect.category.ToString().FromDictionary() + "(" + text2 + ")") : effect.effect.property.GetTitle());
                    if (flag)
                    {
                        CombatSprite cs = CombatSprite.Unknown;
                        if (effect.subcategory == EffectSubCategory.Cleanse)
                        {
                            cs = CombatSprite.Cleanse;
                        }
                        else if (effect.category == EffectCategory.Buff)
                        {
                            cs = CombatSprite.Buff;
                        }
                        else if (effect.category == EffectCategory.Debuff)
                        {
                            cs = CombatSprite.Debuff;
                        }
                        text = text + CombatUI.GetSprite(cs, "gray") + " ";
                    }
                    if (effect != null)
                    {
                        text = text + String.FormatModValue(effect.mod, 1, colorize, flipColor: false, verbose: true, showsign: true, flag2 ? 0.5f : 1f, effect.subcategory) + " " + text3;
                    }
                }
                int duration = effect.GetDuration(gchar);
                if (flag)
                {
                    list.Add(new EffectRow(effect, "<b>" + text + "</b>", duration));
                }
                else
                {
                    list.Add(new EffectRow(effect, text, duration));
                }
                if (duration < num)
                {
                    num = duration;
                }
                if (duration > num2)
                {
                    num2 = duration;
                }
            }
            string text4 = "";
            if (!flag && num == num2)
            {
                foreach (EffectRow item in list)
                {
                    if (text4.Length > 0)
                    {
                        text4 += separator;
                    }
                    text4 += item.row;
                }
                if (num2 > 0)
                {
                    text4 = text4 + " " + Game.FromDictionary("for") + " " + FormatRemainTurns(num2);
                }
            }
            else
            {
                foreach (EffectRow item2 in list)
                {
                    if (text4.Length > 0)
                    {
                        text4 += separator;
                    }
                    text4 += item2.row;
                    if (item2.turn > 0)
                    {
                        text4 = text4 + (flag ? ("<color=" + Tooltip.gray + "> (") : " " + Game.FromDictionary("for") + " ") + FormatRemainTurns(item2.turn, item2.effect) + (flag ? ")</color>" : "");
                        if (flag2)
                        {
                            text4 = text4 + "<color=" + Tooltip.gray + "> " + Tooltip.GetDescription("MarksmanEffect") + "</color>";
                        }
                    }
                }
            }
            __result = text4;
        }

        [HarmonyPatch(nameof(CombatCharacter.FormatEffects))]
        [HarmonyPatch(new Type[] { typeof(List<ActiveEffect>), typeof(Colorize), typeof(string), typeof(CombatCharacter), typeof(CombatCharacter) })]
        [HarmonyPostfix]
        public static void FormatEffects_Postfix2(ref string __result, List<ActiveEffect> acteffects, Colorize colorize = Colorize.None, string separator = ", ", CombatCharacter selectedCharacter = null, CombatCharacter tauntedByCharacter = null)
        {
            if (acteffects.Count == 0 || (acteffects.Count == 1 && acteffects[0].effect.subcategory == EffectSubCategory.Taunt && tauntedByCharacter == null))
            {
                __result = "";
                return;
            }
            bool flag = colorize == Colorize.Tooltip;
            bool flag2 = false;
            int num = 65536;
            int num2 = 0;
            List<EffectRow> list = new List<EffectRow>();
            foreach (ActiveEffect acteffect in acteffects)
            {
                EffectProp effect = acteffect.effect;
                if (effect.subcategory == EffectSubCategory.Taunt && tauntedByCharacter == null)
                {
                    continue;
                }
                if (effect.linkTo == EffectLink.Cover && selectedCharacter != null)
                {
                    foreach (ActiveEffect activeEffect in selectedCharacter.activeEffects)
                    {
                        if (activeEffect.effect.ID == "recz0AtTiKUCT2NVs")
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                string text = "";
                if (effect.category == EffectCategory.DOT)
                {
                    if (flag)
                    {
                        text = text + CombatUI.GetResistIcon(effect.resisttype, "gray") + " ";
                    }
                    string text2 = "";
                    text2 = ((effect.stat == EffectStat.VIT) ? "damage".FromDictionary() : ((effect.stat != EffectStat.POW) ? (effect.stat.ToString() + " "+"drained".FromDictionary()) : "POW drained".FromDictionary()));
                    text = text + String.FormatModValue(effect.mod, 1, colorize, flipColor: false, verbose: true, showsign: false, 1f, effect.subcategory) + " " + effect.resisttype.ToString() + " " + text2;
                }
                else if (effect.category == EffectCategory.HOT)
                {
                    text = String.FormatModValue(effect.mod, 1, colorize, flipColor: false, verbose: true, showsign: true, 1f, effect.subcategory) + " " + effect.stat.ToString().FromDictionary();
                }
                else if (effect.category == EffectCategory.Imped)
                {
                    text = effect.subcategory.ToString().FromDictionary();
                }
                else if (effect.category == EffectCategory.Buff && effect.subcategory == EffectSubCategory.Enchanted)
                {
                    text = effect.subcategory.ToString().FromDictionary();
                }
                else if (effect.category == EffectCategory.Buff && effect.subcategory == EffectSubCategory.HitsBackOnBlock && effect.linkTo != 0)
                {
                    text = "Upon a successful Block".FromDictionary()+", "+"strikes back in melee range".FromDictionary();
                }
                else if (effect.category == EffectCategory.Buff && effect.subcategory == EffectSubCategory.Immunity)
                {
                    text = "Immune to".FromDictionary() + " ";
                    foreach (EffectImmunityType immunitytype in effect.immunitytypes)
                    {
                        text = ((immunitytype != EffectImmunityType.NonMagicalAttack) ? (text + immunitytype.ToString().FromDictionary()) : (text + "Nonmagical Attack".FromDictionary()));
                    }
                }
                else if (effect.category == EffectCategory.Buff && effect.subcategory == EffectSubCategory.Transformed)
                {
                    text = "Transformed".FromDictionary();
                }
                else if (effect.category == EffectCategory.Buff && effect.stat == EffectStat.None && effect.effect != null)
                {
                    text = effect.effect.GetTitle();
                }
                else
                {
                    string text3 = ((effect.subcategory == EffectSubCategory.None) ? effect.resisttype.ToString().FromDictionary() : effect.subcategory.ToString().FromDictionary());
                    string text4 = ((effect.stat == EffectStat.None) ? (effect.category.ToString().FromDictionary() + "(" + text3 + ")") : effect.effect.property.GetTitle());
                    if (effect.subcategory == EffectSubCategory.Taunt)
                    {
                        if (tauntedByCharacter != null)
                        {
                            text = "Taunted by".FromDictionary()+" " + tauntedByCharacter.GameName().ToString();
                        }
                    }
                    else
                    {
                        text = String.FormatModValue(effect.mod, 1, colorize, flipColor: false, verbose: true, showsign: true, flag2 ? 0.5f : 1f, effect.subcategory) + " " + text4;
                    }
                }
                int remainTurns = acteffect.remainTurns;
                if (flag)
                {
                    list.Add(new EffectRow(effect, "<b>" + text + "</b>", remainTurns));
                }
                else
                {
                    list.Add(new EffectRow(effect, text, remainTurns));
                }
                if (remainTurns < num)
                {
                    num = remainTurns;
                }
                if (remainTurns > num2)
                {
                    num2 = remainTurns;
                }
            }
            string text5 = "";
            if (!flag && num == num2)
            {
                foreach (EffectRow item in list)
                {
                    if (text5.Length > 0)
                    {
                        text5 += separator;
                    }
                    text5 += item.row;
                }
                text5 = text5 + " " + Game.FromDictionary("for") + " " + FormatRemainTurns(num2);
            }
            else
            {
                foreach (EffectRow item2 in list)
                {
                    if (text5.Length > 0)
                    {
                        text5 += separator;
                    }
                    text5 += item2.row;
                    text5 = text5 + (flag ? ("<color=" + Tooltip.gray + "> (") : (" " + Game.FromDictionary("for") + " ")) + FormatRemainTurns(item2.turn, item2.effect) + (flag ? ")</color>" : "");
                    if (flag2)
                    {
                        text5 = text5 + "<color=" + Tooltip.gray + "> " + Tooltip.GetDescription("MarksmanEffect") + "</color>";
                    }
                }
            }
            __result = text5;
        }

        //private static string FormatRemainTurns(int turns, EffectProp effect = null)
        //{
        //    MethodInfo methodInfo = typeof(CombatCharacter).GetMethod("FormatRemainTurns", BindingFlags.Public | BindingFlags.Static);
        //    var parameters = new object[] { turns, effect };
        //    return (string)methodInfo.Invoke(null, parameters);
        //}

        public static string FormatRemainTurns(int turns, EffectProp effect = null)
        {
            if (effect != null && effect.linkTo == EffectLink.Cover)
            {
                return Game.FromDictionary("on Line of Sight attacks");
            }

            if (turns >= EffectProp.InfiniteTurns)
            {
                return Game.FromDictionary("the current combat encounter");
            }

            if (turns > 0)
            {
                return turns.FormatNumberByNomen("round");
            }

            return Game.FromDictionary("this round");
        }
    }
}