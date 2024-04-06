using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vagrus.Data;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.PriceHistory
{

    [HarmonyPatch(typeof(PriceHistoryBox))]
    internal class PriceHistoryBoxPatch
    {
        private static ButtonUI sellProfitButton;
        private static ButtonUI buyProfitButton;
        private static ButtonUI sortBySellProfitButton;
        private static ButtonUI sortByBuyProfitButton;
        private static TextInputUI textInput;
        public static int displayPriceToggle = 0;
        public static int sortByToggle = 0;
        public static PriceHistoryBox priceHistoryBoxInstance;


        private static void ToggleDisplayPrice(int idx)
        {
            displayPriceToggle = idx;
            BuildPriceHistoryRows();
        }

        private static void ToggleSortByGroup(int idx)
        {
            sortByToggle = idx;
            FieldInfo field = typeof(PriceHistoryBox).GetField("leftSettlementIdx", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(priceHistoryBoxInstance,0);
            BuildPriceHistoryRows();
        }

        private static void ToggleCustomGroup(int idx)
        {
            PriceHistoryBox.collectionMode = idx != 1;
            BuildPriceHistoryRows();
        }

        private static void ToggleCollectionGroup(int collection)
        {
            PriceHistoryBox.collectionIdx = collection - 1;
            BuildPriceHistoryRows();
        }
        public static void ExtSearch(string search)
        {
            textInput.UpdateText(search);
            FilterGoods(search);
        }

        private static void FilterGoods(string search)
        {
            BuildPriceHistoryRows();
        }

        [HarmonyPatch("SetFirstRowSettlement")]
        [HarmonyPostfix]
        public static void SetFirstRowSettlement(Node node)
        {
            if (!PriceHistoryBox.collectionMode)
            {
                BuildPriceHistoryRows();
            }
        }

        [HarmonyPatch("IsDifferencePrices")]
        [HarmonyPostfix]
        public static void IsDifferencePrices_Postfix(ref bool __result)
        {
            __result = displayPriceToggle != 0;
        }

        [HarmonyPatch("BuildPriceHistoryRows")]
        [HarmonyPrefix]
        public static bool BuildPriceHistoryRows_Prefix(
            PriceHistoryBox __instance,
            GameObject ___headerDisplayPrice,
             GameObject ___headerSortBy,
            ButtonUI ___buttonFull, 
            ButtonUI ___buttonDifference,
            ButtonUI ___buttonDistance,
            ButtonUI ___buttonDate,
            ButtonUI ___buttonName,
            ButtonUI ___buttonSorted,
            ButtonUI ___buttonCustom,
            List<ButtonUI> ___buttonCollections,
        GameObject ___instance,
            List<PriceHistoryRow> ___priceHistoryRows,
            Scroller ___scroller)
        {

            priceHistoryBoxInstance = __instance;
            var headerDisplayPrice = ___headerDisplayPrice;
            var buttonFull = ___buttonFull;
            var buttonDifference = ___buttonDifference;
            var buttonDistance = ___buttonDistance;
            var buttonDate = ___buttonDate;
            var buttonName = ___buttonName;
            var gamObject = ___instance;
            var buttonSorted = ___buttonSorted;
            var buttonCustom = ___buttonCustom;
            var buttonCollections = ___buttonCollections;

            buttonFull.Select(displayPriceToggle == 0);
            buttonFull.SetClickMethod(ToggleDisplayPrice, 0);
            
            buttonDifference.Select(displayPriceToggle == 1);
            buttonDifference.SetClickMethod(ToggleDisplayPrice, 1);

            sellProfitButton = headerDisplayPrice.transform.Find("SellProfit").GetComponent<ButtonUI>();
            sellProfitButton.SetToggleMode(toggleMode: true, "DisplayPriceGroup");
            sellProfitButton.SetClickMethod(ToggleDisplayPrice, 2);
            sellProfitButton.InitializeTooltip("pricehistory", fixtooltip: true);
            sellProfitButton.UpdateTooltip("When selected this option displays potential profit if sold.".FromDictionary());
            sellProfitButton.Select(displayPriceToggle==2);
            sellProfitButton.GetComponent<ButtonUI>().SetHoverEnabled(true);

            buyProfitButton = headerDisplayPrice.transform.Find("BuyProfit").GetComponent<ButtonUI>();
            buyProfitButton.SetToggleMode(toggleMode: true, "DisplayPriceGroup");
            buyProfitButton.SetClickMethod(ToggleDisplayPrice, 3);
            buyProfitButton.InitializeTooltip("pricehistory", fixtooltip: true);
            buyProfitButton.UpdateTooltip("When selected this option displays potential profit if bought.".FromDictionary());
            buyProfitButton.Select(displayPriceToggle==3);
            buyProfitButton.GetComponent<ButtonUI>().SetHoverEnabled(true);

            buttonDistance.Select(sortByToggle == 0);
            buttonDistance.SetClickMethod(ToggleSortByGroup, 0);

            buttonName.Select(sortByToggle == 1);
            buttonName.SetClickMethod(ToggleSortByGroup, 1);

            buttonDate.Select(sortByToggle == 2);
            buttonDate.SetClickMethod(ToggleSortByGroup, 2);

            buttonSorted.SetClickMethod(ToggleCustomGroup, 1);
            buttonCustom.SetClickMethod(ToggleCustomGroup, 2);
            for (int i = 0; i < buttonCollections.Count(); i++) { 
                var buttonCollection = buttonCollections[i];
                buttonCollection.SetClickMethod(ToggleCollectionGroup, i+1);
            }   

            sortBySellProfitButton = ___headerSortBy.transform.Find("SortBySellProfit").GetComponent<ButtonUI>();
            sortBySellProfitButton.SetToggleMode(toggleMode: true, "SortByGroup");
            sortBySellProfitButton.SetClickMethod(ToggleSortByGroup, 3);
            sortBySellProfitButton.InitializeTooltip("pricehistory", fixtooltip: true);
            sortBySellProfitButton.UpdateTooltip("When selected this option arrange places/goods by sell profit.".FromDictionary());
            sortBySellProfitButton.Select(sortByToggle == 3);
            sortBySellProfitButton.GetComponent<ButtonUI>().SetHoverEnabled(true);

            sortByBuyProfitButton = ___headerSortBy.transform.Find("SortByBuyProfit").GetComponent<ButtonUI>();
            sortByBuyProfitButton.SetToggleMode(toggleMode: true, "SortByGroup");
            sortByBuyProfitButton.SetClickMethod(ToggleSortByGroup, 4);
            sortByBuyProfitButton.InitializeTooltip("pricehistory", fixtooltip: true);
            sortByBuyProfitButton.UpdateTooltip("When selected this option arrange places/goods by buy profit.".FromDictionary());
            sortByBuyProfitButton.Select(sortByToggle == 4);
            sortByBuyProfitButton.GetComponent<ButtonUI>().SetHoverEnabled(true);

            var goodsFilter = gamObject.FindDeepChild("GoodsFilter");
            textInput = goodsFilter.FindDeepChild("GoodsNameFilter").GetComponent<TextInputUI>();
            textInput.SetSubmitMethod(FilterGoods);

            priceHistoryBoxInstance.DestroyPriceHistoryRows();
            int num = 0;
            List<Goods> list = new List<Goods>();
            foreach (Goods item2 in AirTableItem<Goods>.GetAll())
            {
                if (!item2.IsCollection() && (PriceHistoryBox.goodsCatGroup == GoodsCatGroup.All || item2.GetCategoryGroup() == PriceHistoryBox.goodsCatGroup))
                {
                    list.Add(item2);
                }
            }

            list = list.Where((Goods a) => Regex.IsMatch(a.GetName(),textInput.GetText())).ToList();

            list.Sort((Goods a, Goods b) => a.GetName().CompareTo(b.GetName()));

            var firstColumnSettlement = __instance.firstRowSettlement;
            var settlementList = __instance.GetAvailableSettlements(firstColumnSettlement); ;

            if (sortByToggle == 3)
            {
                list.Sort((Goods a, Goods b) =>
                {
                    return -1 * settlementList.MaxProfitFrom(firstColumnSettlement, a).CompareTo(settlementList.MaxProfitFrom(firstColumnSettlement, b));
                });
            }
            else if (sortByToggle == 4)
            {
                list.Sort((Goods a, Goods b) =>
                {
                    return -1 * settlementList.MaxProfitTo(firstColumnSettlement, a).CompareTo(settlementList.MaxProfitTo(firstColumnSettlement, b));
                });

            }

            foreach (Goods item3 in list)
            {
                    PriceHistoryRow item = CreatePriceHistoryRow(item3, num);
                    ___priceHistoryRows.Add(item);
                    num++;
            }
            UpdateAdjust();
            ___scroller.ScrollToTop();
            ___scroller.UpdateHeight((float)num * 195f);
            return false;
        }

        [HarmonyPatch("UpdateSettlementList")]
        [HarmonyPrefix]
        public static bool UpdateSettlementList_Prefix(PriceHistoryBox __instance, ref List<Node> ___settlementList, ref List<PriceHistoryRow> ___priceHistoryRows)
        {

            var priceHistoryRows = ___priceHistoryRows;

            if (PriceHistoryBox.collectionMode)
            {
                return false; 
            }

            var firstColumnSettlement = __instance.firstRowSettlement;
            var settlementList = __instance.GetAvailableSettlements(firstColumnSettlement); ;

            if (sortByToggle == 0)
            {
                if (Game.game.caravan.chartUI != null && Game.game.caravan.chartUI.priceHistoryBox != null)
                {
                    settlementList.Sort((Node a, Node b) => a.GetMovementDistanceFrom(firstColumnSettlement, Game.game.caravan.chartUI.priceHistoryBox.realmGateFactorButtonOn).CompareTo(b.GetMovementDistanceFrom(firstColumnSettlement, Game.game.caravan.chartUI.priceHistoryBox.realmGateFactorButtonOn)));
                }
            }
            else if (sortByToggle == 1)
            {
                settlementList.Sort((Node a, Node b) => a.GetName().CompareTo(b.GetName()));
            }
            else if (sortByToggle == 2)
            {
                settlementList.Sort((Node a, Node b) => b.GetLastKnownMarketPriceDay().CompareTo(a.GetLastKnownMarketPriceDay()));
            }
            else if (sortByToggle == 3)
            {
                //foreach(var settlement in settlementList)
                //{
                //    TranslationPatchesPlugin.Log.LogWarning($"Sorting By Sell Profit {settlement.GetName()}: { priceHistoryRows.MaxProfit(settlement, firstColumnSettlement)}");
                //}
                settlementList.Sort((Node a, Node b) =>
                {
                    return -1* priceHistoryRows.MaxProfit(a, firstColumnSettlement).CompareTo(priceHistoryRows.MaxProfit(b, firstColumnSettlement));
                });
                                        
            }
            else if (sortByToggle == 4)
            {
                //foreach (var settlement in settlementList)
                //{
                //    TranslationPatchesPlugin.Log.LogWarning($"Sorting By Buy Profit {settlement.GetName()}: {priceHistoryRows.MaxProfit(firstColumnSettlement, settlement)}");
                //}
                settlementList.Sort((Node a, Node b) =>
                {
                    return -1* priceHistoryRows.MaxProfit(firstColumnSettlement, a).CompareTo(priceHistoryRows.MaxProfit(firstColumnSettlement, b));
                });

            }

            ___settlementList = settlementList;
            return false;
        }

            private static void UpdateAdjust()
        {
            MethodInfo methodInfo = typeof(PriceHistoryBox).GetMethod("UpdateAdjust", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { };
            methodInfo.Invoke(priceHistoryBoxInstance, parameters);
        }

        private static void BuildPriceHistoryRows()
        {
            MethodInfo methodInfo = typeof(PriceHistoryBox).GetMethod("BuildPriceHistoryRows", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { };
            methodInfo.Invoke(priceHistoryBoxInstance, parameters);
        }

        private static void UpdateMatrix()
        {
            MethodInfo methodInfo = typeof(PriceHistoryBox).GetMethod("UpdateMatrix", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { };
            methodInfo.Invoke(priceHistoryBoxInstance, parameters);
        }

        private static PriceHistoryRow CreatePriceHistoryRow(Goods goods, int idx)
        {
            MethodInfo methodInfo = typeof(PriceHistoryBox).GetMethod("CreatePriceHistoryRow", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { goods, idx };
            return (PriceHistoryRow)methodInfo.Invoke(priceHistoryBoxInstance, parameters);
        }
    }
}