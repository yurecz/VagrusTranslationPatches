using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(CrewUI))]
    internal class CrewUIPatches
    {
        [HarmonyPatch("LoadResources")]
        [HarmonyPostfix]
        public static void LoadResources_Postfix(CrewUI __instance, GameObject ___passengerPrefab, Transform ___crewDetailsRowPrefab,Transform ___effectRowPrefab)
        {
            ___passengerPrefab.UpdatePrefabFonts();
            ___crewDetailsRowPrefab.gameObject.UpdatePrefabFonts();
            ___effectRowPrefab.gameObject.UpdatePrefabFonts();
            var effectRowPrefab = Resources.Load("baseui/prefabs/EnduringEffectsRow") as GameObject;
            effectRowPrefab.UpdatePrefabFonts();
            var crewDetailsRowPrefab = Resources.Load("baseui/prefabs/CrewDetailsRow") as GameObject;
            crewDetailsRowPrefab.UpdatePrefabFonts();
            var passengerPrefab = Resources.Load("UI/Prefab/Passenger") as GameObject;
            passengerPrefab.UpdatePrefabFonts();
        }


            [HarmonyPatch("FindComponents")]
        [HarmonyPostfix]
        public static void FindComponents_Postfix(CrewUI __instance)
        {
            var unpaidDaysText = Game.game.camp.GetUnpaidDays().FormatNumberByNomen("day");
            var conditionRows = Traverse.Create(__instance).Field("conditionRows").GetValue() as Transform[];


            conditionRows[0].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.Vigilance).GetTitle(false);
            conditionRows[1].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.Ambush).GetTitle(false);
            conditionRows[2].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.Pacify).GetTitle(false);
            conditionRows[3].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.Settle).GetTitle(false);
            conditionRows[4].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.Flee).GetTitle(false);
            conditionRows[5].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.Chase).GetTitle(false);
            conditionRows[6].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.DisciplineModifier).GetTitle(false);
            conditionRows[7].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.HuntingBase).GetTitle(false);
            conditionRows[8].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.ForagingBase).GetTitle(false);
            conditionRows[9].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.HealingBase).GetTitle(false);
            conditionRows[10].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.SlaveWorkforce).GetTitle(false);
            conditionRows[11].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.WorkerWorkforce).GetTitle(false);
            conditionRows[12].Find("Label").gameObject.GetComponent<TextMeshProUGUI>().text = BaseUI.game.caravan.FindProperty(Prop.ScoutingBase).GetTitle(false);

            for (var i = 0; i < conditionRows.Length; i++)
            {
                TranslationPatchesPlugin.Log.LogMessage("CrewUI patched conditionRows:" + conditionRows[i].gameObject.GetFullName());
            }
        }
    }
}