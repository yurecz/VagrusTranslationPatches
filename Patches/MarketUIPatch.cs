using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(MarketUI))]
    internal class MarketUIPatch
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(MarketUI __instance, GameObject ___noTradeOffers)
        {
            __instance.btnCargoText.text = "Goods";
            ___noTradeOffers.AddComponent<UIElementTranslator>();

        }

        //[HarmonyPatch("FindComponents")]
        //[HarmonyPostfix]
        //public static void FindComponents_Postfix(
        //    Tooltip ___tooltip,
        //    Toggle ___btnEAll,
        //    Toggle ___btnEArms,
        //    Toggle ___btnESorcery,
        //    Toggle ___btnECargo,
        //    Toggle ___btnETools,
        //    Toggle ___btnEHarnesses,
        //    Toggle ___btnEMiscellaneous,
        //    Toggle ___btnENavigation,
        //    Toggle ___btnEAnimals,
        //    Toggle ___btnEOffense,
        //    Toggle ___btnEDefense
        //)
        //{
        //    var tooltip = ___tooltip;
        //    Type type = tooltip.GetType();
        //    MethodInfo methodInfo = type.GetMethod("AddLink");

        //    if (methodInfo == null)
        //    {

        //        return;

        //    }

        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Unknown, "All".FromDictionary(), tooltip.gameObject, ___btnEAll.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Arms, EquipmentCategory.Arms.ToString().FromDictionary(), tooltip.gameObject, ___btnEArms.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Sorcery, EquipmentCategory.Sorcery.ToString().FromDictionary(), tooltip.gameObject, ___btnESorcery.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Cargo, EquipmentCategory.Cargo.ToString().FromDictionary(), tooltip.gameObject, ___btnECargo.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Harnesses, EquipmentCategory.Harnesses.ToString().FromDictionary(), tooltip.gameObject, ___btnEHarnesses.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Tools, EquipmentCategory.Tools.ToString().FromDictionary(), tooltip.gameObject, ___btnETools.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Miscellaneous, EquipmentCategory.Miscellaneous.ToString().FromDictionary(), tooltip.gameObject, ___btnEMiscellaneous.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Navigation, EquipmentCategory.Navigation.ToString().FromDictionary(), tooltip.gameObject, ___btnENavigation.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Animals, EquipmentCategory.Animals.ToString().FromDictionary(), tooltip.gameObject, ___btnEAnimals.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Offense, EquipmentCategory.Offense.ToString().FromDictionary(), tooltip.gameObject, ___btnEOffense.gameObject, fix: true);
        //    tooltip.AddLink("market", "EquipmentFilter" + EquipmentCategory.Defense, EquipmentCategory.Defense.ToString().FromDictionary(), tooltip.gameObject, ___btnEDefense.gameObject, fix: true);
        //}
    }
}