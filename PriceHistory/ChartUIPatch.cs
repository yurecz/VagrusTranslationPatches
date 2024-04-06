using HarmonyLib;
using TMPro;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.PriceHistory
{

    [HarmonyPatch(typeof(ChartUI))]
    internal class ChartUIPatch
    {
        public static GameObject sellProfitButton;
        public static GameObject buyProfitButton;

        [HarmonyPatch("LoadResources")]
        [HarmonyPostfix]
        public static void LoadResources_Postfix(ChartUI __instance, GameObject ___priceHistoryBoxPrefab)
        {
            var shiftY = 64f;
            var shiftX = 190.5f;
            var deltaX = 70f;

            if (___priceHistoryBoxPrefab.FindDeepChild("SellProfit") != null) return;

            var headerDisplayPrice = ___priceHistoryBoxPrefab.FindDeepChild("HeaderDisplayPrice");

            var headerDisplayPriceTitle = headerDisplayPrice.FindDeepChild("Title");
            headerDisplayPriceTitle.transform.localPosition += new Vector3(350f, 60f, 0);

            var sellProfitButton = headerDisplayPrice.CloneButton("ButtonFull","SellProfit", "Sell Profit", shiftX, 0, deltaX);

            var buyProfitButton = headerDisplayPrice.CloneButton("ButtonFull", "BuyProfit", "Buy Profit", shiftX, -shiftY, deltaX);

            var buttonDifference = headerDisplayPrice.FindDeepChild("ButtonDifference");
            buttonDifference.transform.localPosition += new Vector3(-shiftX, -shiftY, 0);

            var buttonStack = headerDisplayPrice.FindDeepChild("ButtonStack");
            buttonStack.transform.localPosition += new Vector3(deltaX, 0, 0);

            var group = headerDisplayPrice.FindDeepChild("Group");
            if (group != null)
            {
                var groupRect = group.GetComponent<RectTransform>();
                groupRect.sizeDelta += new Vector2(deltaX, shiftY);
                group.transform.localPosition += new Vector3(deltaX / 2, -shiftY / 2f, 0);
            }

            var headerSortBy = ___priceHistoryBoxPrefab.FindDeepChild("HeaderSortBy");

            var headerSortByTitle = headerSortBy.FindDeepChild("Title");
            headerSortByTitle.transform.localPosition += new Vector3(400f, 60f, 0);

            var sortBySellProfit = headerSortBy.CloneButton("ButtonDistance", "SortBySellProfit", "Sell Profit", 2*shiftX, 0, deltaX);

            var sortByBuyProfit = headerSortBy.CloneButton("ButtonDistance", "SortByBuyProfit", "Buy Profit", 2*shiftX, -shiftY, deltaX);

            var realmGateButton = headerSortBy.FindDeepChild("RealmGateButton");
            realmGateButton.transform.localPosition += new Vector3(-60f, shiftY, 0);

            var buttonDistance = headerSortBy.FindDeepChild("ButtonName");
            buttonDistance.transform.localPosition += new Vector3(-2*shiftX, -shiftY, 0);

            group = headerSortBy.FindDeepChild("Group");
            if (group != null)
            {
                var groupRect = group.GetComponent<RectTransform>();
                groupRect.sizeDelta += new Vector2(deltaX, shiftY);
                group.transform.localPosition += new Vector3(deltaX / 2, -shiftY / 2f, 0);
            }

            var codexUI = Resources.Load("UI/Book/Codex/Prefab/CodexUI") as GameObject;
            var textInputPrefab = codexUI.FindDeepChild("TextInput");
            if (textInputPrefab != null)
            {
                var textInput = GameObject.Instantiate(textInputPrefab);
                textInput.name = "GoodsNameFilter";
                var goodsFilter = ___priceHistoryBoxPrefab.FindDeepChild("GoodsFilter");
                textInput.transform.SetParent(goodsFilter.transform, false);
                textInput.transform.localPosition = new Vector3(-260, 124, 0);

            }
        }

    }
}