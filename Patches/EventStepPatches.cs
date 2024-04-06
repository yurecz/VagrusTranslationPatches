using HarmonyLib;
using System;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(EventStep))]
    internal class EventStepPatches
    {
        [HarmonyPatch("ExecuteAction", new Type[] { typeof(PerkAction), typeof(bool), typeof(bool) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal })]
        [HarmonyPrefix]
        public static bool ExecutePerkAction_Prefix(EventStep __instance, ref bool __result, PerkAction perkAction, ref bool actionlogged, bool simulate = false)
        {
            var gameEvent = __instance.gameEvent;

            bool flag = EventStep.game.ScheduleAction(perkAction, perkAction.afterdays, simulate);
            if (simulate)
            {
                __result = flag;
                return false;
            }
            if (!perkAction.hide || Game.IsTest())
            {
                string text = string.Format((perkAction.remove ? "you have lost perk {0}" : "you have gained perk {0}").FromDictionary(), perkAction.perk.GetName());
                text = FormatAfterDays(text, perkAction.afterdays).FirstLetterUpperCase();
                gameEvent.AddLog(perkAction.hide ? String.TestBlock(text) : text);
                actionlogged = true;
            }
            __result = false;
            return false;
        }

        [HarmonyPatch("ExecuteAction", new Type[] { typeof(ItemAction), typeof(bool), typeof(bool) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal })]
        [HarmonyPrefix]
        public static bool ExecuteItemAction_Prefix(EventStep __instance, ref bool __result, ItemAction itemAction,  ref bool actionlogged, bool simulate = false)
        {
            var gameEvent = __instance.gameEvent;

            itemAction.rvalue = Game.GetRandomIncludeMax(itemAction.value, itemAction.value2);
            MarketBlock marketBlock = itemAction.marketBlock;
            if (marketBlock != null && marketBlock.mode != PriceMode.None)
            {
                bool flag = EventStep.game.ScheduleAction(itemAction, 0, simulate);
                if (simulate)
                {
                    __result = flag;
                    return false;
                }
                if (!itemAction.hide || Game.IsTest())
                {
                    int marketPrice = itemAction.item.GetMarketPrice(marketBlock);
                    int num = Mathf.Abs(marketBlock.qty);
                    string text = "";
                    if (marketBlock.qty > 0)
                    {
                        text = string.Format("Bought: {0}({1}) for {2}".FromDictionary(), itemAction.item.GetName(), num, Game.game.caravan.FormatMoney(marketPrice));
                    }
                    else
                    {
                        text = string.Format("Sold: {0}({1}) for {2}".FromDictionary(), itemAction.item.GetName(), num, Game.game.caravan.FormatMoney(marketPrice));
                    }
                    gameEvent.AddLog(itemAction.hide ? String.TestBlock(text) : text);
                    actionlogged = true;
                }
                __result = false;
                return false;
            }
            else
            {
                bool flag = EventStep.game.ScheduleAction(itemAction, itemAction.afterdays, simulate);
                if (simulate)
                {
                    __result = flag;
                    return false;
                }
                if (!itemAction.hide || Game.IsTest())
                {
                    if (itemAction.type == ValueType.AddValue)
                    {
                        if (itemAction.rvalue == 0)
                        {
                            __result = false;
                            return false;
                        }
                        string text2 = String.Sign(itemAction.rvalue, true) + " " + itemAction.item.GetName();
                        text2 = FormatAfterDays(text2, itemAction.afterdays);
                        gameEvent.AddLog(itemAction.hide ? String.TestBlock(text2, false) : text2);
                        actionlogged = true;
                    }
                    else if (itemAction.type == ValueType.AddPercent)
                    {
                        string text2 = String.Sign(itemAction.rvalue, true) + "% " + itemAction.item.GetName();
                        text2 = FormatAfterDays(text2, itemAction.afterdays);
                        gameEvent.AddLog(itemAction.hide ? String.TestBlock(text2, false) : text2);
                        actionlogged = true;
                    }
                }
                __result = false;
                return false;
            }

        }

        [HarmonyPatch("ExecuteAction", new Type[] { typeof(PropertyAction), typeof(bool), typeof(bool) }, new ArgumentType[] { ArgumentType.Normal, ArgumentType.Ref, ArgumentType.Normal })]
        [HarmonyPrefix]
        public static bool ExecutePropertyAction_Prefix(EventStep __instance, ref bool __result, PropertyAction propertyAction, ref bool actionlogged, ref bool simulate)
        {
            var game = Game.game;
            var gameEvent = __instance.gameEvent;

            switch (propertyAction.scriptNodeType)
            {
                case ScriptNodeType.Current:
                    propertyAction.node = game.caravan.curNode;
                    break;
                case ScriptNodeType.CurrentOutpost:
                    if (game.caravan.CurrentOutpostNode == null)
                    {
                        Debug.LogWarning("No node set to 'currentoutpost'!");
                        __result = false;
                        return false;
                    }
                    propertyAction.node = game.caravan.CurrentOutpostNode;
                    break;
            }
            Outpost outpost = ((propertyAction.node != null) ? propertyAction.node.outpostData : null);
            if (propertyAction.node != null && outpost == null)
            {
                Debug.LogWarning("No outpost data on: " + propertyAction.node.name + "! Can't set properties on a node where no outpost exists!");
                __result = false;
                return false;
            }
            if (propertyAction.type == ValueType.AddPercent)
            {
                int num = propertyAction.value / Mathf.Abs(propertyAction.value);
                int num2 = outpost?.GetProperty(propertyAction.property.prop) ?? game.caravan.GetProperty(propertyAction.property.prop);
                int num3 = Mathf.RoundToInt((float)num2 * ((float)propertyAction.value / 100f));
                if (num2 > 0 && num3 == 0)
                {
                    num3 = num2 * num;
                }
                if (num3 != 0)
                {
                    if (propertyAction.minchange == 0 && propertyAction.maxchange == 0)
                    {
                        propertyAction.rvalue = propertyAction.value;
                    }
                    else
                    {
                        int min = Mathf.Clamp(propertyAction.minchange, 0, num2);
                        propertyAction.rvalue = Mathf.Clamp(Mathf.Abs(num3), min, propertyAction.maxchange) * num;
                    }
                }
                else
                {
                    propertyAction.rvalue = 0;
                }
            }
            else
            {
                propertyAction.rvalue = Game.GetRandomIncludeMax(propertyAction.value, propertyAction.value2);
            }
            if (propertyAction.property.IsCrewCollection())
            {
                propertyAction.rprop = game.caravan.GetRandomCrewList(Property.ToPropList(propertyAction.property.prop), propertyAction.rvalue);
            }
            if (propertyAction.character != null)
            {
                propertyAction.rcharacter = (propertyAction.character.IsRandom() ? game.caravan.GetRandomCompanion() : propertyAction.character);
            }
            bool flag = false;
            MarketBlock marketBlock = propertyAction.marketBlock;
            if (marketBlock != null && marketBlock.mode != PriceMode.None)
            {
                flag = game.ScheduleAction(propertyAction, 0, simulate);
                if (simulate)
                {
                    __result = flag;
                    return false;
                }
                if (!propertyAction.hide || Game.IsTest())
                {
                    Crew crew = Crew.FindByProperty(propertyAction.property.prop);
                    int money = (crew ? crew.GetMarketPrice(marketBlock) : 0);
                    int num4 = Mathf.Abs(marketBlock.qty);
                    string text = "";
                    if (marketBlock.qty > 0)
                    {
                        text = string.Format("Bought: {0}({1}) for {2}".FromDictionary(), propertyAction.property.GetTitle(), num4, Game.game.caravan.FormatMoney(money));
                    }
                    else
                    {
                        text = string.Format("Sold: {0}({1}) for {2}".FromDictionary(), propertyAction.property.GetTitle(), num4, Game.game.caravan.FormatMoney(money));
                    } 
                    
                    gameEvent.AddLog(propertyAction.hide ? String.TestBlock(text) : text);
                    actionlogged = true;
                }
                __result = false;
                return false;
            }
            int val = 0;
            if (propertyAction.property.target != PropertyTarget.Companion && propertyAction.property.target != 0 && propertyAction.property.prop != 0)
            {
                val = outpost?.GetProperty(propertyAction.property.prop) ?? game.caravan.GetProperty(propertyAction.property.prop);
            }
            if ((bool)propertyAction.rcharacter && !propertyAction.rcharacter.IsIgnoreHeroselect() && !propertyAction.rcharacter.IsAll())
            {
                GameCharacter gameCharacter = game.caravan.GetGameCharacter(propertyAction.rcharacter);
                if (gameCharacter == null || (game.caravan.HasHeroSelect() && !game.caravan.IsCharacterHeroselect(gameCharacter)))
                {
                    __result = false;
                    return false;
                }
            }
            flag = game.ScheduleAction(propertyAction, propertyAction.afterdays, simulate);
            if (simulate)
            {
                __result = flag;
                return false;
            }
            if ((!propertyAction.property.hidden && !propertyAction.hide) || Game.IsTest())
            {
                string row = "";
                Prop prop = propertyAction.property.prop;
                if (propertyAction.property.type != PropertyType.Numeric)
                {
                    if (propertyAction.remove)
                    {
                        row = (propertyAction.property.type != PropertyType.Status) ? "?" : string.Format("You lost: {0}".FromDictionary(), propertyAction.property.GetTitle());
                    }
                    else
                    {
                        row = (propertyAction.property.type != PropertyType.Status) ? "?" : string.Format("You gained: {0}".FromDictionary(), propertyAction.property.GetTitle());
                    }
                }
                else
                {
                    string text2 = "";
                    string text3 = "";
                    switch (prop)
                    {
                        case Prop.Changer:
                            text2 = game.caravan.GetMoneySprite(TextSprite.Changer) + " ";
                            break;
                        case Prop.Lyrg:
                            text2 = game.caravan.GetMoneySprite(TextSprite.Lyrg) + " ";
                            break;
                        case Prop.Bross:
                            text2 = game.caravan.GetMoneySprite(TextSprite.Bross) + " ";
                            break;
                        case Prop.MoneyOutpost:
                        case Prop.Money:
                            text3 = game.caravan.FormatMoney(propertyAction.rvalue, showZero: false, showLeadZero: false) + ((prop == Prop.MoneyOutpost) ? " outpost money" : "");
                            break;
                    }
                    if (propertyAction.rprop != null && propertyAction.rprop.Count > 0)
                    {
                        foreach (PropQty item in propertyAction.rprop)
                        {
                            row = row + String.Sign(item.qty) + " " + item.prop.ToString() + "   ";
                        }
                    }
                    else if (propertyAction.type == ValueType.AddValue)
                    {
                        row = ((text3.Length <= 0) ? (String.Sign(propertyAction.rvalue) + " " + text2 + propertyAction.property.GetTitle()) : text3);
                    }
                    else if (propertyAction.type == ValueType.AddPercent)
                    {
                        if (EventTweak.ShowActionValuesInsteadOfPercentage && EventTweak.IsShowPercentageActionValue(prop))
                        {
                            if (propertyAction.rvalue != 0)
                            {
                                row = String.Sign(propertyAction.rvalue) + "% " + text2 + propertyAction.property.GetTitle();
                            }
                        }
                        else if (propertyAction.rvalue != 0)
                        {
                            row = String.Sign(Game.GetPercentOfValue(val, propertyAction.rvalue)) + " " + text2 + propertyAction.property.GetTitle();
                            row += String.TestBlock(" (" + String.Sign(propertyAction.rvalue) + "%)");
                        }
                    }
                }
                if (row.Length > 0)
                {
                    if ((bool)propertyAction.rcharacter)
                    {
                        row = row + " on " + propertyAction.rcharacter.GameName();
                    }
                    row = FormatAfterDays(row, propertyAction.afterdays);
                    gameEvent.AddLog((propertyAction.hide || propertyAction.property.hidden) ? String.TestBlock(row) : row);
                    actionlogged = true;
                }
            }
            __result = false;
            return false;
        }

      
        private static string FormatAfterDays(string text, int afterdays)
        {
            if (afterdays > 0)
            {
                return string.Format("After {0} {1}".FromDictionary(), afterdays.FormatNumberByNomen("day"),text);
            }
            return text;
        }
    }
}