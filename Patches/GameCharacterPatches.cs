using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using Vagrus.Data;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(GameCharacter))]
    internal class GameCharacterPatches
    {
        static MethodInfo healMethod = typeof(GameCharacter).GetMethod("Heal");

        [HarmonyPatch(nameof(GameCharacter.Heal))]
        [HarmonyPrefix]
        public static bool Heal_Prefix(GameCharacter __instance, out Tuple<int,Health> __state)
        {
            int num = __instance.GetVitality();
            Health health = __instance.GetHealth();
            __state = new Tuple<int, Health>(num, health);
            return true;
        }
        
        [HarmonyPatch(nameof(GameCharacter.Heal))]
        [HarmonyPostfix]
        public static void Heal_Postfix(GameCharacter __instance, ref string __result, CriticalTestResult result, bool healBoost, ref int medKitUsed, Tuple<int, Health> __state)
        {
            string text = "";
            int previouseHealth = __state.Item1;
            Health health = __state.Item2;
            if (__result == "") 
            { 
                return;
            }
            
            Health health2 = __instance.GetHealth();
            var healthDelta = __instance.GetVitality() - previouseHealth;
            string currentHealthText = ((health != health2 && health != Health.AnyHurt && health2 != Health.MarkedToKill && health2 != Health.Maximum && health2 != Health.AnyHurt) ? health2.ToString() : "");
            string characterName = ((__instance.GameName(shortName: true) == string.Empty) ? __instance.GameName() : __instance.GameName(shortName: true));
            if (healthDelta != 0 || currentHealthText.Length > 0)
            {
                string color = ((healthDelta < 0) ? VisualTweak.Red : VisualTweak.Green);
                string text5 = "";
                string text6 = "";
                if (healthDelta != 0)
                {
                    text5 = text5 + characterName + "<indent=50%><color=" + color + ">" + String.Sign(healthDelta) + "</color></indent> "+"vit".FromDictionary()+"\n";
                }
                else if (currentHealthText.Length > 0)
                {
                    text5 += characterName;
                }
                if (currentHealthText.Length > 0)
                {
                    text5 += ((healthDelta != 0) ? "" : "\n");
                    text6 = text6 + (((health2 < health) ? "healed" : "fallen") + ((currentHealthText.Length > 12) ? "\n" : " ") + "to <i>" + currentHealthText + "</i>").FromDictionary() + "\n";
                }
                if (health != Health.AnyHurt && (health2 == Health.AnyHurt || health2 == Health.Maximum) && health > health2)
                {
                    text5 += ((healthDelta != 0) ? "" : "\n");
                    text6 = text6 + ("healed from <i>" + health.ToString() + "</i>").FromDictionary()+"\n";
                }
                text += text5;
                text += text6;
            }
            if (health2 == Health.MarkedToKill)
            {
                text = text + characterName + "\n" + ("has died of " + __instance.character.GetCharacterPronoun() + " wounds").FromDictionary()+".";
            }
            if (text.Length > 0)
            {
                text += "<sprite=\"result_heal_row\" index=0>";
            }
            __result = text;
        }
    }
}