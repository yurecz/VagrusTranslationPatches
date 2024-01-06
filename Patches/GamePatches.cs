using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Vagrus;
using Vagrus.UI;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(Game))]
    internal class GamePatches
    {
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

    }
}
