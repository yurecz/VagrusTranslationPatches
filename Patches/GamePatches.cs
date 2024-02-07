using HarmonyLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Vagrus;
using Vagrus.UI;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(Game))]
    public static class GamePatches
    {

        //[HarmonyPatch("Awake")]
        //[HarmonyPostfix]
        //public static void Awake_Postfix()
        //{
        //   //TranslationPatchesPlugin.SetGameFixedValues();
        //}



        [HarmonyPatch("ShowLoadGame")]
        [HarmonyPostfix]
        public static void ShowLoadGame_Postfix(
            GameObject ___loadGameInstance,
            bool show
        )
        {

            if (show && ___loadGameInstance)
            {
                ___loadGameInstance.transform.Find("Holder/Back/Text").GetComponent<TextMeshProUGUI>().text = Game.FromDictionary("Loading...");
            }
        }

        [HarmonyPatch("UpdateCombatLoadingScreen")]
        [HarmonyPostfix]
        public static void UpdateCombatLoadingScreen_Postfix()
        {
            var combatLoading = Game.game.combatLoading;
            if (combatLoading != null)
            {
                GameObject obj = combatLoading.transform.Find("Holder").gameObject;
                _ = obj.transform.Find("Back/Skull").gameObject;
                TextMeshProUGUI component = obj.transform.Find("Back/LoadingText").GetComponent<TextMeshProUGUI>();
                component.text = Game.FromDictionary("Loading...");
            }
        }

        [HarmonyPatch("HasDLC")]
        [HarmonyPostfix]
        public static void HasDLC_Postfix(ref bool __result)
        {

            __result = true;
        }

        public static GameObject disclaimerInstance(this Game obj)
        {
            Type type = obj.GetType();

            FieldInfo privateFieldInfo = type.GetField("disclaimerInstance", BindingFlags.NonPublic | BindingFlags.Instance);
            return (GameObject)privateFieldInfo.GetValue(obj);
        }
    }
}
