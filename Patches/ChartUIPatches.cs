using HarmonyLib;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(ChartUI))]
    internal class ChartUIPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(ChartUI __instance)
        {
            var prefab = Resources.Load<GameObject>("Chart/Prefab/ChartUI");
            prefab.UpdatePrefabFonts();

            var poiboxPrefab = Resources.Load<GameObject>("Chart/Prefab/POIBox");
            poiboxPrefab.UpdatePrefabFonts();

            var priceHistoryRowPrefab = Resources.Load<GameObject>("Chart/Prefab/PriceHistoryRow");
            priceHistoryRowPrefab.UpdatePrefabFonts();

            var priceHistoryBoxPrefab = Resources.Load<GameObject>("Chart/Prefab/PriceHistoryBox");
            priceHistoryBoxPrefab.UpdatePrefabFonts();

        }


        [HarmonyPatch("GetNodeDistance")]
        [HarmonyPostfix]
        public static void GetNodeDistance_PostFix(ChartUI __instance,ref string __result,string ___travelDistance, Node ___prevDisNode, Node node, bool showWithPortal = false, bool calculateWithRealmGate = false, bool poiBox = false)
        {
            if (node == null || (node.IsPOI() && !Game.build.TestAccess(node.access)))
            {
                __result = "";
                return;
            }
            ___prevDisNode = node;
            int property = Game.game.caravan.GetProperty(Prop.MaxMove);
            Node curNode = Game.game.caravan.curNode;
            Node node2 = node;
            if (!poiBox)
            {
                curNode = Game.game.caravan.chartUI.priceHistoryBox.firstRowSettlement ?? Game.game.caravan.curNode;
            }
            else
            {
                POI pickedPOI = __instance.GetPickedPOI();
                curNode = Game.game.caravan.curNode;
                node2 = ((pickedPOI != null && pickedPOI.node != null) ? pickedPOI.node : Game.game.caravan.curNode);
            }
            int movementDistanceBetweenNodes = curNode.GetMovementDistanceBetweenNodes(node2, showWithPortal && calculateWithRealmGate);
            int num = movementDistanceBetweenNodes / Mathf.Max(8, property - 2, Mathf.Min(property, MarketTweak.MovementPointPerDays - 4));
            int num2 = movementDistanceBetweenNodes / Mathf.Max(property, MarketTweak.MovementPointPerDays);
            string subject = ChartTweak.POIDaysTravel;
            if (num == 0 && num2 == 0)
            {
                if (poiBox)
                {
                    __result = "";
                    return;
                }
            }
            ___travelDistance = ((num2 != num) ? num2 + "-" : "") + num.FormatNumberByNomen("day") + " " + "of travel".FromDictionary();
            __result = ___travelDistance;
        }

        [HarmonyPatch("GetMarkerInfo")]
        [HarmonyPostfix]
        public static void GetMarkerInfo_PostFix(ChartUI __instance, ref string __result, POIAreaMarker selectedMarker)
        {
            string text = "";
            if (selectedMarker != null)
            {
                text = text + __instance.GetNodeDistance(selectedMarker.Node, Game.game.caravan.RealmGateTravelUnlocked(), Game.game.caravan.RealmGateTravelUnlocked(), true) + "\n";
                if (selectedMarker.JournalMarker == null)
                {
                    Debug.LogWarning("valami nem jó itt");
                }
                else
                {
                    text = text + "<b><i>" + selectedMarker.JournalMarker.GetNodesInMarkerRange(selectedMarker.Node).FormatNumberByNomen("node") + "</i></b> "+"in the area".FromDictionary()+"\n";
                }
            }
            __result = text;
        }
    }
}