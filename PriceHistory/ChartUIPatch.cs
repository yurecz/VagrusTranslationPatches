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

            if (sellProfitButton != null) { return; }

            var headerDisplayPrice = ___priceHistoryBoxPrefab.FindDeepChild("HeaderDisplayPrice");

            var templateButton = headerDisplayPrice.FindDeepChild("ButtonFull");
            if (templateButton != null)
            {
                sellProfitButton = GameObject.Instantiate(templateButton);
                sellProfitButton.name = "SellProfit";
                sellProfitButton.transform.SetParent(templateButton.transform.parent, false);
                sellProfitButton.transform.localPosition = templateButton.transform.localPosition - new Vector3(0, shiftY, 0);
                sellProfitButton.transform.Find("Holder/Title").GetComponent<TextMeshProUGUI>().text = "Sell Profit";
            }

            templateButton = headerDisplayPrice.FindDeepChild("ButtonDifference");
            if (templateButton != null)
            {
                buyProfitButton = GameObject.Instantiate(templateButton);
                buyProfitButton.name = "BuyProfit";
                buyProfitButton.transform.SetParent(templateButton.transform.parent, false);
                buyProfitButton.transform.localPosition = templateButton.transform.localPosition - new Vector3(0, shiftY, 0);
                buyProfitButton.transform.Find("Holder/Title").GetComponent<TextMeshProUGUI>().text = "Buy Profit";
            }

            var group = headerDisplayPrice.FindDeepChild("Group");
            if (group != null)
            {
                var groupRect = group.GetComponent<RectTransform>();
                groupRect.sizeDelta += new Vector2(0, shiftY);
                group.transform.localPosition -= new Vector3(0, shiftY / 2f, 0);
            }

            var headerSortBy = ___priceHistoryBoxPrefab.FindDeepChild("HeaderSortBy");
            var buttonDistance = headerSortBy.FindDeepChild("ButtonDistance");

            if (buttonDistance != null)
            {
                GameObject sortBySellProfitButton = GameObject.Instantiate(buttonDistance);
                sortBySellProfitButton.name = "SortBySellProfit";
                sortBySellProfitButton.transform.SetParent(buttonDistance.transform.parent, false);
                sortBySellProfitButton.transform.localPosition = buttonDistance.transform.localPosition - new Vector3(0, shiftY, 0);
                sortBySellProfitButton.transform.Find("Holder/Title").GetComponent<TextMeshProUGUI>().text = "Sell Profit";
            }

            var buttonName = headerSortBy.FindDeepChild("ButtonName");

            if (buttonName != null)
            {
                GameObject sortByBuyProfitButton = GameObject.Instantiate(buttonName);
                sortByBuyProfitButton.name = "SortByBuyProfit";
                sortByBuyProfitButton.transform.SetParent(buttonName.transform.parent, false);
                sortByBuyProfitButton.transform.localPosition = buttonName.transform.localPosition - new Vector3(0, shiftY, 0);
                sortByBuyProfitButton.transform.Find("Holder/Title").GetComponent<TextMeshProUGUI>().text = "Buy Profit";
            }

            group = headerSortBy.FindDeepChild("Group");
            if (group != null)
            {
                var groupRect = group.GetComponent<RectTransform>();
                groupRect.sizeDelta += new Vector2(0, shiftY);
                group.transform.localPosition -= new Vector3(0, shiftY / 2f, 0);
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