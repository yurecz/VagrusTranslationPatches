using HarmonyLib;
using System;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vagrus.UI;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(EventChoiceButton))]
    internal class EventChoiceButtonPatch
    {
        [HarmonyPatch("CreateEndButton")]
        [HarmonyPostfix]
        public static void Refresh_Postfix(EventChoiceButton __result)
        {
            var choiceText = Traverse.Create(__result).Field("choiceText").GetValue() as TextMeshProUGUI;
            choiceText.text = "<size=46><b><smallcaps>" + "End".FromDictionary() + "</smallcaps></b></size>";
        }

        [HarmonyPatch("CreateItemBoxes")]
        [HarmonyPrefix]
        public static bool CreateItemBoxes_Prefix(EventChoiceButton __instance, EventStep step, ChoiceAction action, Caravan caravan, Tooltip tooltip, Transform ___itemBoxParent)
        {
            var ChoiceIndex = __instance.ChoiceIndex;
            var itemBoxParent = ___itemBoxParent;
            int iconsCounter = 0;
            int totalPrice = 0;
            bool flag = false;
            var assessment = "";
            foreach (GoodsAction goodsAction in action.goods)
            {
                if (goodsAction.marketBlock != null && !(!goodsAction.goods) && goodsAction.marketBlock.mode != PriceMode.None && (!goodsAction.hide || Game.IsTest()))
                {
                    iconsCounter++;
                    flag = goodsAction.marketBlock.qty > 0;
                    string text = Tooltip.GetDescription(flag ? "BuyAction" : "SellAction");
                    int marketPrice = goodsAction.goods.GetMarketPrice(goodsAction.marketBlock);
                    int num5 = Mathf.Abs(goodsAction.marketBlock.qty);
                    totalPrice += marketPrice;
                    int priceRate = goodsAction.marketBlock.priceRate;
                    assessment = flag ? EventsUIUtils.GetEvaluateBuyPrice(priceRate) : EventsUIUtils.GetEvaluateSellPrice(priceRate);
                    int num6 = Mathf.CeilToInt((float)num5 / (float)goodsAction.goods.GetStackSize());
                    String.Replace(ref text, "%qty%", num5);
                    String.Replace(ref text, "%name%", goodsAction.goods.GetName(false));
                    String.Replace(ref text, "%stack%", "(" + num6.FormatNumberByNomen("stack")+")");
                    String.Replace(ref text, "%money%", caravan.FormatMoney(marketPrice, true, true, 3, ""));
                    String.Replace(ref text, "%actqty%", caravan.cargo.CountGoodsQty(goodsAction.goods, true, false, false));
                    String.Replace(ref text, "%guestimate%", assessment);
                    String.Replace(ref text, "%cargospace%", string.Format("and have {0}".FromDictionary(),caravan.cargo.GetFreeSlots().FormatNumberByNomen("free cargo slot")));
                    text = goodsAction.goods.GetTooltip(true) + "\n\n" + text;
                    EventUI.FormatTooltipAfterDays(ref text, goodsAction.afterdays);
                    AddEquipmentBox(__instance, num5, goodsAction.goods.iconLoader, tooltip, "choiceItem_" + ChoiceIndex.ToString() + "_" + iconsCounter.ToString(), text);
                }
            }
            string text4 = "";
            string text5 = "";
            foreach (ItemAction itemAction in action.items)
            {
                if (itemAction.marketBlock != null && !(!itemAction.item) && itemAction.marketBlock.mode != PriceMode.None && (!itemAction.hide || Game.IsTest()))
                {
                    iconsCounter++;
                    flag = itemAction.marketBlock.qty > 0;
                    text5 = Tooltip.GetDescription(flag ? "BuyAction" : "SellAction");
                    int marketPrice2 = itemAction.item.GetMarketPrice(itemAction.marketBlock);
                    int num7 = Mathf.Abs(itemAction.marketBlock.qty);
                    totalPrice += marketPrice2;
                    int priceRate2 = itemAction.marketBlock.priceRate;
                    assessment = flag ? EventsUIUtils.GetEvaluateBuyPrice(priceRate2) : EventsUIUtils.GetEvaluateSellPrice(priceRate2);
                    String.Replace(ref text5, "%qty%", num7);
                    String.Replace(ref text5, "%name%", itemAction.item.GetName());
                    String.Replace(ref text5, "%stack%", "");
                    String.Replace(ref text5, "%money%", caravan.FormatMoney(marketPrice2, true, true, 3, ""));
                    String.Replace(ref text5, "%actqty%", caravan.itemHolder.GetCount(itemAction.item));
                    String.Replace(ref text5, "%guestimate%", assessment);
                    String.Replace(ref text5, "%cargospace%", "");
                    text5 = itemAction.item.GetTooltip() + "\n\n" + text5;
                    EventUI.FormatTooltipAfterDays(ref text5, itemAction.afterdays);
                    if (num7 > 1)
                    {
                        text4 = string.Concat(new string[]
                        {
                            text4,
                            num7.ToString(),
                            " <voffset=-5><size=150%><sprite=\"item_collector\" index=",
                            itemAction.item.iconIndex.ToString(),
                            "></size></voffset> "
                        });
                    }
                    else
                    {
                        text4 = text4 + "<voffset=-5><size=150%><sprite=\"item_collector\" index=" + itemAction.item.iconIndex.ToString() + "></size></voffset> ";
                    }
                }
            }
            if (!string.IsNullOrEmpty(text4))
            {
                AddItemText(__instance, text4.Trim(), tooltip, "choiceItem_" + ChoiceIndex.ToString() + "_" + iconsCounter.ToString(), text5);
            }
            foreach (EquipmentAction equipmentAction in action.equipments)
            {
                if (equipmentAction.marketBlock != null && !(!equipmentAction.equipment) && equipmentAction.marketBlock.mode != PriceMode.None && (!equipmentAction.hide || Game.IsTest()))
                {
                    iconsCounter++;
                    flag = equipmentAction.marketBlock.qty > 0;
                    string text8 = Tooltip.GetDescription(flag ? "BuyAction" : "SellAction");
                    int marketPrice3 = equipmentAction.equipment.GetMarketPrice(equipmentAction.marketBlock);
                    int num8 = Mathf.Abs(equipmentAction.marketBlock.qty);
                    totalPrice += marketPrice3;
                    int priceRate3 = equipmentAction.marketBlock.priceRate;
                    assessment = flag ? EventsUIUtils.GetEvaluateBuyPrice(marketPrice3) : EventsUIUtils.GetEvaluateSellPrice(marketPrice3);
                    String.Replace(ref text8, "%qty%", num8);
                    String.Replace(ref text8, "%name%", equipmentAction.equipment.GetName());
                    String.Replace(ref text8, "%stack%", "");
                    String.Replace(ref text8, "%money%", caravan.FormatMoney(marketPrice3, true, true, 3, ""));
                    String.Replace(ref text8, "%actqty%", caravan.equipmentHolder.GetCount(equipmentAction.equipment));
                    String.Replace(ref text8, "%guestimate%", assessment);
                    String.Replace(ref text8, "%cargospace%", "");
                    text8 = equipmentAction.equipment.GetTooltip(true, true, null) + "\n\n" + text8;
                    EventUI.FormatTooltipAfterDays(ref text8, equipmentAction.afterdays);
                    AddEquipmentBox(__instance, num8, equipmentAction.equipment.iconLoader, tooltip, "choiceItem_" + ChoiceIndex.ToString() + "_" + iconsCounter.ToString(), text8);
                }
            }
            foreach (GearAction gearAction in action.gears)
            {
                if (gearAction.marketBlock != null && !(!gearAction.gear) && gearAction.marketBlock.mode != PriceMode.None && (!gearAction.hide || Game.IsTest()))
                {
                    iconsCounter++;
                    flag = gearAction.marketBlock.qty > 0;
                    string text11 = Tooltip.GetDescription(flag ? "BuyAction" : "SellAction");
                    int marketPrice4 = gearAction.gear.GetMarketPrice(gearAction.marketBlock);
                    int num9 = Mathf.Abs(gearAction.marketBlock.qty);
                    totalPrice += marketPrice4;
                    int priceRate4 = gearAction.marketBlock.priceRate;
                    assessment = flag ? EventsUIUtils.GetEvaluateBuyPrice(marketPrice4) : EventsUIUtils.GetEvaluateSellPrice(marketPrice4);
                    String.Replace(ref text11, "%qty%", num9);
                    String.Replace(ref text11, "%name%", gearAction.gear.GetName());
                    String.Replace(ref text11, "%stack%", "");
                    String.Replace(ref text11, "%money%", caravan.FormatMoney(marketPrice4, true, true, 3, ""));
                    String.Replace(ref text11, "%actqty%", caravan.gearHolder.GetCount(gearAction.gear));
                    String.Replace(ref text11, "%guestimate%", assessment);
                    String.Replace(ref text11, "%cargospace%", "");
                    text11 = gearAction.gear.GetTooltip(null, null) + "\n\n" + text11;
                    EventUI.FormatTooltipAfterDays(ref text11, gearAction.afterdays);
                    AddEquipmentBox(__instance, num9, gearAction.gear.iconLoader, tooltip, "choiceItem_" + ChoiceIndex.ToString() + "_" + iconsCounter.ToString(), text11);
                }
            }
            if (totalPrice != 0)
            {
                RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(Resources.Load<RectTransform>(flag ? "Event/Prefab/ChoiceTotalPriceBuy" : "Event/Prefab/ChoiceTotalPriceSell"));
                rectTransform.SetParent(itemBoxParent);
                rectTransform.localScale = Vector3.one;
                rectTransform.GetComponentInChildren<TextMeshProUGUI>().text = "  " + caravan.FormatMoney(totalPrice, false, false, 3, "");
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 100f);
                if (flag)
                {
                    rectTransform.SetAsFirstSibling();
                }
            }

            return false;
        }

        public static void AddEquipmentBox(EventChoiceButton __instance, int quantity, OnDemandAssetLoader<Sprite, Image> iconLoader, Tooltip tooltip, string tooltipID, string tooltipString)
        {
            Type classType = typeof(EventChoiceButton);

            MethodInfo methodInfo = classType.GetMethod("AddEquipmentBox", BindingFlags.NonPublic | BindingFlags.Instance);

            methodInfo.Invoke(__instance, new object[] { quantity, iconLoader, tooltip, tooltipID, tooltipString });
        }

        public static void AddItemText(EventChoiceButton __instance, string text, Tooltip tooltip, string tooltipID, string tooltipString)
        {
            Type classType = typeof(EventChoiceButton);

            MethodInfo methodInfo = classType.GetMethod("AddItemText", BindingFlags.NonPublic | BindingFlags.Instance);

            methodInfo.Invoke(__instance, new object[] { text, tooltip, tooltipID, tooltipString });

        }
    }
}