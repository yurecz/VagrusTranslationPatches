using HarmonyLib;
using System.Collections.Generic;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(PerkSummaryHolderUI))]
    internal class PerkSummaryHolderUIPatches
    {

        [HarmonyPatch("GetTotalTooltip")]
        [HarmonyPostfix]
        public static void GetTotalTooltip_Postfix(PerkSummaryHolderUI __instance, ref string __result, Perk perk, int totalLevel, int highest)
        {
            string text = "";
            text += GetPerkHeader(perk);
            text = text + "Total Level".FromDictionary()+" " + totalLevel + "\n";
            if (highest > 0)
            {
                text = text + "Highest single:".FromDictionary()+" " + highest + "\n";
            }
            __result = text;
        }

        [HarmonyPatch("GetVagrusTooltip")]
        [HarmonyPostfix]
        public static void GetVagrusTooltip_Postfix(PerkSummaryHolderUI __instance, ref string __result, Perk perk, int vagrusLevel, Dictionary<Property, int> statuses)
        {
            string text = "";
            string text2 = "";
            int num = vagrusLevel;
            text += GetPerkHeader(perk);
            text = text + "Vagrus Total".FromDictionary()+" " + String.Sign(vagrusLevel) + "\n";
            foreach (KeyValuePair<Property, int> status in statuses)
            {
                num -= status.Value;
                text2 = text2 + "      " + String.Sign(status.Value) + " " + status.Key.GetName() + "\n";
            }
            if (vagrusLevel != 0 || statuses.Count != 0)
            {
                text = text + "      " + String.Sign(num) + " "+"Base".FromDictionary()+"\n";
            }
            __result = text + text2;
        }

        [HarmonyPatch("GetComitatusTooltip")]
        [HarmonyPostfix]
        public static void GetComitatusTooltip_Postfix(PerkSummaryHolderUI __instance, ref string __result, Perk perk, int comitatusLevel, Dictionary<Property, int> statuses, Dictionary<Equipment, int> equipments)
        {
            string text = "";
            text += GetPerkHeader(perk);
            text = text + "Comitatus Total".FromDictionary()+" " + String.Sign(comitatusLevel) + "\n";
            foreach (KeyValuePair<Equipment, int> equipment in equipments)
            {
                text = text + "      " + String.Sign(equipment.Value) + " " + equipment.Key.GetName() + "\n";
            }
            foreach (KeyValuePair<Property, int> status in statuses)
            {
                text = text + "      " + String.Sign(status.Value) + " " + status.Key.GetName() + "\n";
            }
            __result = text;
        }

        [HarmonyPatch("GetCompanionTooltip")]
        [HarmonyPostfix]
        public static void GetCompanionTooltip_Postfix(PerkSummaryHolderUI __instance, ref string __result, Perk perk, Dictionary<Character, int> companions, int highest)
        {
            string text = "";
            text += GetPerkHeader(perk);
            if (companions.Count == 0)
            {
                __result = text + "Companions Total".FromDictionary()+" 0\n";
                return;
            }
            int num = 0;
            string text2 = "";
            foreach (KeyValuePair<Character, int> companion in companions)
            {
                Character key = companion.Key;
                GameCharacter gameCharacter = key.GetGameCharacter();
                if (gameCharacter == null)
                {
                    continue;
                }
                num += companion.Value;
                int num2 = companion.Value;
                string text3 = "";
                text2 = text2 + key.GameName() + " " + String.Sign(companion.Value) + "\n";
                int count;
                Dictionary<Gear, int> dictionary = Game.game.caravan.gearHolder.FindGearsByGrantedPerk(perk, key, out count);
                int count2;
                Dictionary<Property, int> dictionary2 = gameCharacter.FindStatusesByGrantedPerk(perk, out count2);
                foreach (KeyValuePair<Gear, int> item in dictionary)
                {
                    num2 -= item.Value;
                    text3 = text3 + "      " + String.Sign(item.Value) + " " + item.Key.GetName() + "\n";
                }
                foreach (KeyValuePair<Property, int> item2 in dictionary2)
                {
                    num2 -= item2.Value;
                    text3 = text3 + "      " + String.Sign(item2.Value) + " " + item2.Key.GetName() + "\n";
                }
                text2 = text2 + "      " + String.Sign(num2) + " "+"Base".FromDictionary()+"\n";
                text2 += text3;
                if (dictionary.Count > 0 || dictionary2.Count > 0)
                {
                    text2 += "\n";
                }
            }
            text = text + "Companions Total".FromDictionary()+" " + String.Sign(num) + "\n\n";
            text += text2;
            if (highest > 0)
            {
                text = text + "\n"+"Highest single:".FromDictionary()+" " + highest + "\n";
            }
            __result = text;
        }

        private static string GetPerkHeader(Perk perk)
        {
            return "" + "<size="+ VisualTweak.TooltipHeaderSize + "%>" + perk.GetName() + "</size>\n\n";
        }
    }
}