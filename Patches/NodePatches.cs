using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(Node))]
    internal class NodePatches
    {
        [HarmonyPatch(nameof(Node.GetLocationName))]
        [HarmonyPostfix]
        public static void GetLocationName_Postfix(Node __instance, string __result, bool nearPoiName = false, bool forceRegionName = false, bool prepositionIN = false)
        {
            var nearPoi = __instance.nearPoi;
            if (!forceRegionName && __instance.IsPOI())
            {
                __result = __instance.GetName();
            }

            if (nearPoiName && nearPoi != null && nearPoi.node != null && nearPoi.node.IsVisibleOnChart())
            {
                __result = TextTemplate.GetText(TextTemplateType.Market, "NearPOI").Replace("%node%", nearPoi.node.GetName());
            }

            if (nearPoiName && __instance.nearSettlement != null && __instance.nearSettlement.node != null && __instance.nearSettlement.movepoints < 30 + __instance.nearSettlement.node.GetSettlementLevel() * 2)
            {
                __result = TextTemplate.GetText(TextTemplateType.Market, "NearPOI").Replace("%node%", __instance.nearSettlement.node.GetName());
            }

            Region parentRegion = __instance.GetParentRegion();
            if ((bool)parentRegion)
            {
                __result = (prepositionIN ? "in region".FromDictionary() + " ": "") + parentRegion.GetName();
            }

            __result = "?";
        }
    }
}