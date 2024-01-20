using HarmonyLib;
using UnityEngine;
using Vagrus.Data;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(GameCharacter))]
    internal class GameCharacterPatches
    {
        [HarmonyPatch(nameof(GameCharacter.Heal))]
        [HarmonyPrefix]
        public static bool Awake_Prefix(GameCharacter __instance, ref string __result, CriticalTestResult result, bool healBoost, ref int medKitUsed)
        {
            string text = "";
            int num = __instance.GetVitality();
            Health health = __instance.GetHealth();
            if (__instance.Heal(result, healBoost, ref medKitUsed) == "") 
            { 
                __result = ""; 
                return false;
            }
            
            Health health2 = __instance.GetHealth();
            var healthDelta = __instance.GetVitality() - num;
            string currentHealthText = ((health != health2 && health != Health.AnyHurt && health2 != Health.MarkedToKill && health2 != Health.Maximum && health2 != Health.AnyHurt) ? health2.ToString() : "");
            string characterName = ((__instance.GameName(shortName: true) == string.Empty) ? __instance.GameName() : __instance.GameName(shortName: true));
            if (healthDelta != 0 || currentHealthText.Length > 0)
            {
                string color = ((healthDelta < 0) ? VisualTweak.Red : VisualTweak.Green);
                string text5 = "";
                string text6 = "";
                if (healthDelta != 0)
                {
                    text5 = text5 + characterName + "<indent=70%><color=" + color + ">" + String.Sign(healthDelta) + "</color></indent> vit\n";
                }
                else if (currentHealthText.Length > 0)
                {
                    text5 += characterName;
                }
                if (currentHealthText.Length > 0)
                {
                    if (text5.Length < 13)
                    {
                        text5 = text5 + ((health2 < health) ? " healed" : " fallen") + "\n";
                        text6 = text6 + "to <i>" + currentHealthText + "</i>\n";
                    }
                    else
                    {
                        text5 += ((healthDelta != 0) ? "" : "\n");
                        text6 = text6 + ((health2 < health) ? "healed" : "fallen") + ((currentHealthText.Length > 12) ? "\n" : " ") + "to <i>" + currentHealthText + "</i>\n";
                    }
                }
                if (health != Health.AnyHurt && (health2 == Health.AnyHurt || health2 == Health.Maximum) && health > health2)
                {
                    if (text5.Length < 13)
                    {
                        text5 += " healed\n";
                        text6 = text6 + "from <i>" + health.ToString() + "</i>\n";
                    }
                    else
                    {
                        text5 += ((healthDelta != 0) ? "" : "\n");
                        text6 = text6 + "healed from <i>" + health.ToString() + "</i>\n";
                    }
                }
                text += text5;
                text += text6;
            }
            if (health2 == Health.MarkedToKill)
            {
                text = text + characterName + " has\ndied of " + __instance.character.GetCharacterPronoun() + " wounds.";
            }
            if (text.Length > 0)
            {
                text += "<sprite=\"result_heal_row\" index=0>";
            }
            __result = text;
            return false;
        }
    }
}