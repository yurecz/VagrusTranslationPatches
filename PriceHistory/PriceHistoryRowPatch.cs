using HarmonyLib;
using System.Reflection;
using TMPro;
using Vagrus.UI;

namespace VagrusTranslationPatches.PriceHistory
{
    [HarmonyPatch(typeof(PriceHistoryRow))]
    internal class PriceHistoryRowPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void Update_Postfix(PriceHistoryRow __instance, TextMeshProUGUI ___name, PriceHistoryBox ___priceHistoryBox, Goods ___goods)
        {
            var priceHistoryBox = ___priceHistoryBox;
            var name = ___name;
            var goods = ___goods;
            var priceHistoryColumns = __instance.priceHistoryColumns;
            name.text = goods.GetName();
            int num = 0;
            Node node = null;
            Node settlementForColumn = priceHistoryBox.GetSettlementForColumn(0);
            LastMarket firstLastMarket = (settlementForColumn ? settlementForColumn.FindLastMarket(goods.GetID()) : null);
            foreach (PriceHistoryColumn priceHistoryColumn in priceHistoryColumns)
            {
                node = priceHistoryBox.GetSettlementForColumn(num);
                priceHistoryColumn.SetSettlement(node);
                LastMarket currentLastMarket = (node ? node.FindLastMarket(goods.GetID()) : null);
                int num2 = 0;
                int num3 = 0;
                if (currentLastMarket != null)
                {
                    bool contraband = GetContraband(goods, settlementForColumn, node);
                    if (firstLastMarket != null)
                    {
                        if (PriceHistoryBoxPatch.displayPriceToggle == 1)
                        {
                            num2 = currentLastMarket.buyPrice - firstLastMarket.buyPrice;
                            num3 = currentLastMarket.sellPrice - firstLastMarket.sellPrice;
                        } else if (PriceHistoryBoxPatch.displayPriceToggle == 2)
                        {
                            num3 = currentLastMarket.sellPrice - firstLastMarket.buyPrice;
                        }
                        else if (PriceHistoryBoxPatch.displayPriceToggle == 3)
                        {
                            num2 = firstLastMarket.sellPrice - currentLastMarket.buyPrice;
                        }
                    }
                    bool stackPrice = priceHistoryBox.IsStackPrice();
                    if (priceHistoryBox.IsDifferencePrices() && node != settlementForColumn)
                    {
                        if (firstLastMarket != null)
                        {
                            priceHistoryColumn.SetBuyPrice(num2, stackPrice, goods, num, difference: true, contraband);
                            priceHistoryColumn.SetSellPrice(num3, stackPrice, goods, num, difference: true, contraband);
                            if (PriceHistoryTweak.DiffPriceBorderColored)
                            {
                                UpdateCellBorders(__instance, priceHistoryColumn, num2, num3, num, difference: true, contraband);
                            }
                        }
                        else
                        {
                            priceHistoryColumn.EmptyCell();
                        }
                    }
                    else
                    {
                        priceHistoryColumn.SetBuyPrice(currentLastMarket.buyPrice, stackPrice, goods, num, difference: false, contraband);
                        priceHistoryColumn.SetSellPrice(currentLastMarket.sellPrice, stackPrice, goods, num, difference: false, contraband);
                        if (firstLastMarket != null && PriceHistoryTweak.FullPriceBorderColored)
                        {
                            UpdateCellBorders(__instance,priceHistoryColumn, num2, num3, num, difference: false, contraband);
                        }
                        else
                        {
                            priceHistoryColumn.SetBuyPriceBorder(PriceBorderType.Normal);
                            priceHistoryColumn.SetSellPriceBorder(PriceBorderType.Normal);
                        }
                    }
                }
                else
                {
                    priceHistoryColumn.EmptyCell();
                }
                num++;
            }
            UpdateGoodsBox(__instance);
        }

        private static bool GetContraband(Goods goods, Node source, Node dest)
        {
            if (!goods.IsContraband())
            {
                return false;
            }
            if (dest.GetContraband() > 1)
            {
                return false;
            }
            return true;
        }

        private static void UpdateCellBorders(PriceHistoryRow instance, PriceHistoryColumn priceHistoryColumn, int diffBuy, int diffSell, int columnidx, bool difference, bool contraband)
        {
            MethodInfo methodInfo = typeof(PriceHistoryRow).GetMethod("UpdateCellBorders", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { priceHistoryColumn, diffBuy, diffSell, columnidx, difference, contraband };
            methodInfo.Invoke(instance, parameters);
        }

        private static void UpdateGoodsBox(PriceHistoryRow instance)
        {
            MethodInfo methodInfo = typeof(PriceHistoryRow).GetMethod("UpdateGoodsBox", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] {  };
            methodInfo.Invoke(instance, parameters);
        }

    }
}