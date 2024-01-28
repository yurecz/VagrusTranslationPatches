using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CargoEquipmentData))]
    internal class CargoEquipmentDataPatches
    {
        [HarmonyPatch("SetLeft")]
        [HarmonyPostfix]
        public static void SetLeft_Postfix(CargoEquipmentData __instance, EquipmentSlot slot, bool filled, bool mounted)
        {
            __instance.leftTitle = "<b>" + slot.equipment.GetName().FromDictionary() + "</b>";
            if (mounted)
                __instance.leftTitle += " <i>" + ("(" + "Equipped".FromDictionary() + ")").GetScaledLocalizedText("(Equipped)") + "</i>";
        }

        [HarmonyPatch("SetRight")]
        [HarmonyPostfix]
        public static void SetRight_Postfix(CargoEquipmentData __instance, EquipmentSlot slot, bool filled, bool mounted)
        {
            __instance.rightTitle = "<b>" + slot.equipment.GetName().FromDictionary() + "</b>";
            if (mounted)
                __instance.rightTitle += " <i>"+("(" + "Equipped".FromDictionary() + ")").GetScaledLocalizedText("(Equipped)") + "</i>";
        }

        //[HarmonyPatch("SetLeft")]
        //[HarmonyPostfix]
        //public static void SetLeft_Postfix(CargoEquipmentData __instance, EquipmentSlot slot, bool filled, bool mounted)
        //{
        //    if (mounted)
        //        __instance.leftTitle = "<b>" + slot.equipment.GetName().GetScaledLocalizedText(slot.equipment.title) + "</b> <i>" + ("(" + "Equipped".FromDictionary() + ")").GetScaledLocalizedText("(Equipped)") + "</i>";
        //    else
        //        __instance.leftTitle = "<b>" + slot.equipment.GetName().GetScaledLocalizedText(slot.equipment.title) + "</b>";
        //}

        //[HarmonyPatch("SetRight")]
        //[HarmonyPostfix]
        //public static void SetRight_Postfix(CargoEquipmentData __instance, EquipmentSlot slot, bool filled, bool mounted)
        //{
        //    if (mounted)
        //        __instance.rightTitle = "<b>" + slot.equipment.GetName().GetScaledLocalizedText(slot.equipment.title) + "</b> <i>" + ("(" + "Equipped".FromDictionary() + ")").GetScaledLocalizedText("(Equipped)") + "</i>";
        //    else
        //        __instance.rightTitle = "<b>" + slot.equipment.GetName().GetScaledLocalizedText(slot.equipment.title) + "</b>";
        //}

    }
}