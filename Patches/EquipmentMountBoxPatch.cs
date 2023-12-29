using HarmonyLib;
using System.Reflection;
using TMPro;
using UnityEngine;
using Vagrus.UI;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(EquipmentMountBox))]
    internal class EquipmentMountBoxPatch
    {
        [HarmonyPatch("UpdateContents")]
        [HarmonyPostfix]
        public static void UpdateContents_Postfix(EquipmentMountBox __instance)
        {
            Equipment mountEquipment = Game.game.caravan.equipmentHolder.GetMountEquipment(__instance.category, __instance.slotIndex);
            var unlocked = Traverse.Create(__instance).Field("unlocked").GetValue<bool>();
            var tooltip = Traverse.Create(__instance).Field("tooltip").GetValue<Tooltip>();
            var game = Game.game;
            if (unlocked)
            {
                EquipmentComponent equipmentComponent = __instance.containingComponent;
                if (((equipmentComponent != null) ? equipmentComponent.GetGoodsDrag() : null) == null)
                {
                    string text = (mountEquipment ? mountEquipment.GetTooltip(true, true, null) : __instance.category.ToString().FromDictionary());
                    MethodInfo methodInfo = typeof(EquipmentMountBox).GetMethod("TooltipLink", BindingFlags.NonPublic | BindingFlags.Instance);
                    var parameters = new object[] { };
                    var tooltipLink = (string)methodInfo.Invoke(__instance, parameters);
                    tooltip.UpdateLink(tooltipLink, text);
                }
            }
        }
    }
}