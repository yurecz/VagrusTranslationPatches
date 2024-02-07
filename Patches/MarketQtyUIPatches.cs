using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using Vagrus;
using Vagrus.UI;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(MarketQtyUI))]

    internal class MarketQtyUIPatches
    {
        //[HarmonyPatch("LoadResources")]
        //[HarmonyPostfix]
        //public static void LoadResources_Postfix(MarketQtyUI __instance, GameObject ___prefab)
        //{
        //    ___prefab.AddOnceRecursiveComponent<UIFontUpdater>();
        //}

        [HarmonyPatch("UpdateMarketValues")]
        [HarmonyPostfix]
        public static void UpdateMarketValues_Postfix(MarketQtyUI __instance,
            CargoAction ___cargoAction,
            TextMeshProUGUI ___priceText,
            TextMeshProUGUI ___leftCargoText,
            TextMeshProUGUI ___rightCargoText,
            TextMeshProUGUI ___supplyDaysText,
            int ___qty,
            int ___price
            )
        {
            var cargoAction = ___cargoAction;
            var goodsDrag = __instance.goodsDrag;
            var priceText = ___priceText;
            var leftCargoText = ___leftCargoText;
            var rightCargoText = ___rightCargoText;
            var supplyDaysText = ___supplyDaysText;
            var qty = ___qty;
            var price = ___price;

            Goods goods = null;
            if (cargoAction == CargoAction.DragBuy && goodsDrag.marketRow.product.category == MarketCategory.Cargo)
            {
                goods = goodsDrag.marketRow.product.goods;
            }
            else if (cargoAction == CargoAction.DragSell && goodsDrag.goodsBox != null)
            {
                goods = goodsDrag.goodsBox.slot.goods;
            }
            else if (cargoAction == CargoAction.MassBuy || cargoAction == CargoAction.MassSell)
            {
                goods = goodsDrag.marketRow.product.goods;
            }
            if (cargoAction == CargoAction.Move || cargoAction == CargoAction.Offer || cargoAction == CargoAction.CancelOffer)
            {
                priceText.text = "";
            }
            else
            {
                priceText.text = Game.game.caravan.FormatMoney(qty * price, showZero: true, showLeadZero: false) + " " + "(Avg".FromDictionary() + " " + Game.game.caravan.FormatMoney(price, showZero: true, showLeadZero: false) + ")";
            }
            bool flag = false;
            if ((bool)goods)
            {
                int num = qty / goods.qtyPerSlot;
                if (qty % goods.qtyPerSlot > 0)
                {
                    num++;
                }
                int freeQtyForGoods = Game.game.caravan.cargo.GetFreeQtyForGoods(goods);
                int num2 = freeQtyForGoods / goods.qtyPerSlot;
                if (freeQtyForGoods % goods.qtyPerSlot > 0)
                {
                    num2++;
                }
                flag = num2 == 0;
                leftCargoText.text = num.ToString();
                rightCargoText.text = num2.ToString();
                if (goods.IsSupply())
                {
                    int supply = qty * Goods.StackOfSupply;
                    supplyDaysText.text = "(" + ((cargoAction == CargoAction.DragBuy || cargoAction == CargoAction.MassBuy) ? "+" : "-") + Game.game.caravan.GetSupplyDays(supply).FormatNumberByNomen("day") + ".)";
                }
                else
                {
                    supplyDaysText.text = "";
                }
            }
            else
            {
                leftCargoText.text = "";
                rightCargoText.text = "";
                supplyDaysText.text = "";
            }
        }
    }
}
