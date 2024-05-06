using HarmonyLib;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CrewCombatFightUI))]
    internal class CrewCombatFightUIPatches
    {
        [HarmonyPatch("UpdateAttributes")]
        [HarmonyPostfix]
        public static void UpdateAttributes_Postfix(CrewCombatFightUI __instance, CrewCombat ___crewCombat, FlexibleStat ___flexibleStatPlayer, FlexibleStat ___flexibleStatEnemy, CrewSide side)
        {
            string text = "";
            string text2 = "";
            foreach (CrewCombatAttribute crewCombatAttribute in ___crewCombat.SortAttributes(___crewCombat.GetAttributes(side)))
            {
                if (text.Length > 0)
                {
                    text += ", ";
                }
                text += crewCombatAttribute.GetTitle();
                text2 = text2 + crewCombatAttribute.GetTooltip(false) + "\n\n";
            }
            if (side == CrewSide.Player)
            {
                ___flexibleStatPlayer.Set(Game.FromDictionary("Your Attributes"), text, "", text2);
                return;
            }
            if (side == CrewSide.Enemy)
            {
                ___flexibleStatEnemy.Set(Game.FromDictionary("Enemy Attributes"), text, "", text2);
            }
        }

        [HarmonyPatch("UpdatePlayerStats")]
        [HarmonyPostfix]
        public static void UpdatePlayerStats_Postfix(CrewCombatFightUI __instance, CrewCombat ___crewCombat, List<FlexibleStat> ___playerStats, float ___playerStatWidth)
        {
            var crewCombat = ___crewCombat;
            var playerStatWidth = ___playerStatWidth;
            var playerStats = ___playerStats;

            float num = 0.7f;
            playerStats[1].PlaceRight(playerStats[0]);
            playerStats[1].SetValuePlacement(0.65f);
            playerStats[2].PlaceBottom(playerStats[0]);
            playerStats[3].PlaceBottom(playerStats[1]);
            playerStats[3].SetValuePlacement(0.65f);
            int num2 = 0;
            List<Prop> list = new List<Prop>();
            list = ((!crewCombat.IsFlee()) ? new List<Prop>
        {
            Prop.Vigor,
            Prop.Morale,
            Prop.CCDeputies,
            Prop.Obedience
        } : new List<Prop>
        {
            Prop.Flee,
            Prop.Cargo,
            Prop.CCDeputies,
            Prop.Obedience
        });
            if ((crewCombat.IsFight() || crewCombat.IsFleeFight()) && crewCombat.IsEnemyAttacker())
            {
                playerStats[4].PlaceBottom(playerStats[2]);
                playerStats[4].SetWidth(playerStatWidth * num);
                playerStats[5].PlaceRight(playerStats[4]);
                playerStats[5].SetWidth(playerStatWidth * num);
                playerStats[6].PlaceRight(playerStats[5]);
                playerStats[6].SetWidth(playerStatWidth * num);
                if (crewCombat.HasAttribute("recuUgfy7PXSLHDyv"))
                {
                    list.Add(Prop.Supplies);
                    list.Add(Prop.CrewMountBeast);
                }
                else if (crewCombat.HasAttribute("recVN3xhxic8sDT26"))
                {
                    list.Add(Prop.Supplies);
                    list.Add(Prop.CrewTotal);
                    list.Add(Prop.CrewMountBeast);
                }
                else if (crewCombat.HasAttribute("recyXqWtN2agsAZbC"))
                {
                    list.Add(Prop.CrewTotal);
                }
                else if (crewCombat.HasAttribute("reclLHhWav7yD6BxC"))
                {
                    list.Add(Prop.Cargo);
                    list.Add(Prop.CargoWorth);
                }
                else if (crewCombat.HasAttribute("recpjS1O15WGBP6gI"))
                {
                    list.Add(Prop.CrewMountBeast);
                }
                else if (crewCombat.HasAttribute("recIylqNd2SVjjoJb"))
                {
                    list.Add(Prop.CrewTotal);
                }
                else if (crewCombat.HasAttribute("recGVTHkSnwJnshSr"))
                {
                    list.Add(Prop.Slave);
                }
            }
            else if (crewCombat.IsHeal())
            {
                playerStats[4].PlaceBottom(playerStats[2]);
                playerStats[4].SetWidth(playerStatWidth * 2f);
            }
            foreach (Prop item in list)
            {
                if (num2 == playerStats.Count)
                {
                    break;
                }
                PropInstance propInstance = Game.game.caravan.FindProperty(item);
                playerStats[num2++].Set(propInstance.GetTitle(), Game.game.caravan.GetPropertyDetails(item), propInstance.GetDescription(), Game.game.caravan.GetPropertyTooltip(propInstance));
            }
            if ((crewCombat.IsFight() || crewCombat.IsFleeFight()) && crewCombat.IsEnemyDefender())
            {
                playerStats[4].PlaceBottom(playerStats[2]);
                playerStats[4].SetWidth(playerStatWidth * 2f);
                playerStats[4].SetValuePlacement(0.25f);
                CrewCombatAttribute mandatoryAttribute = crewCombat.GetMandatoryAttribute(CrewSide.Player);
                string valtext = (mandatoryAttribute ? mandatoryAttribute.GetName() : Game.FromDictionary("Unknown"));
                Game.game.caravan.FormatMoney(crewCombat.startDemand);
                playerStats[num2++].Set(Game.FromDictionary("Goal"), valtext);
            }
            else if (crewCombat.IsHeal())
            {
                playerStats[num2++].Set(Game.FromDictionary("Medkit"), Item.GetMedkits().ToString(), Item.GetMedkit().GetDescription());
            }
            while (num2 < playerStats.Count)
            {
                playerStats[num2++].Set();
            }
        }

    [HarmonyPatch("UpdateEnemyStats")]
    [HarmonyPostfix]
    public static void UpdateEnemyStats_PostFix(CrewCombatFightUI __instance, CrewCombat ___crewCombat, List<FlexibleStat> ___enemyStats, float ___enemyStatWidth)
    {
        var crewCombat = ___crewCombat;
        var enemyStatWidth = ___enemyStatWidth;
        var enemyStats = ___enemyStats;

        enemyStats[0].SetValuePlacement(0.7f);
        enemyStats[1].PlaceRight(enemyStats[0]);
        enemyStats[1].SetValuePlacement(0.7f);
        enemyStats[2].PlaceRight(enemyStats[1]);
        enemyStats[2].SetValuePlacement(0.7f);
        int num = 0;
        
            List<Prop> list = new List<Prop>();
        if (crewCombat.IsFight())
        {
            list = ((!crewCombat.IsPlayerAttacker()) ? new List<Prop>
            {
                Prop.CraveEnemy,
                Prop.Grit,
                Prop.StrengthRate
            } : new List<Prop>
            {
                Prop.WorthEnemy,
                Prop.Grit,
                Prop.StrengthRate
            });
        }
        else if (crewCombat.IsFlee() || crewCombat.IsFleeFight())
        {
            list = new List<Prop>
            {
                Prop.CraveEnemy,
                Prop.ChaseEnemy,
                Prop.StrengthRate
            };
        }
        else if (crewCombat.IsSack())
        {
            list = new List<Prop>
            {
                Prop.WorthEnemy,
                Prop.FleeEnemy,
                Prop.StrengthRate
            };
        }
        else
        {
            crewCombat.IsHeal();
        }
        foreach (Prop item in list)
        {
            if (num == enemyStats.Count)
            {
                break;
            }
            PropInstance propInstance = Game.game.caravan.FindProperty(item);
            enemyStats[num++].Set(propInstance.GetTitle(), Game.game.caravan.GetPropertyDetails(item), propInstance.GetDescription(), Game.game.caravan.GetPropertyTooltip(propInstance));
        }
        if (crewCombat.IsFight() || crewCombat.IsFleeFight())
        {
            enemyStats[3].PlaceBottom(enemyStats[0]);
            enemyStats[3].SetWidth(enemyStatWidth * 3f);
            enemyStats[3].SetValuePlacement(0.7f);
            string itemRow = crewCombat.enemyProp.GetItemRow();
            enemyStats[num++].Set("Items".FromDictionary(), (itemRow.Length > 0) ? itemRow : "None".FromDictionary(), "", crewCombat.enemyProp.GetItemTooltip());
            if (crewCombat.IsEnemyDefender())
            {
                if (crewCombat.HasAttribute("recyXqWtN2agsAZbC"))
                {
                    enemyStats[4].PlaceBottom(enemyStats[3]);
                    int crewCount = crewCombat.GetCrewCount(CrewSide.Enemy);
                    enemyStats[num++].Set("Crew".FromDictionary(), crewCount.ToString());
                }
                else if (crewCombat.HasAttribute("reclLHhWav7yD6BxC"))
                {
                    enemyStats[4].PlaceBottom(enemyStats[3]);
                    enemyStats[4].SetWidth(enemyStatWidth * 3f);
                    enemyStats[4].SetValuePlacement(0.7f);
                    string cargoRow = crewCombat.enemyProp.GetCargoRow();
                    enemyStats[num++].Set("Cargo".FromDictionary(), (cargoRow.Length > 0) ? cargoRow : "None".FromDictionary(), "", crewCombat.enemyProp.GetCargoTooltip());
                }
                else if (crewCombat.HasAttribute("recpjS1O15WGBP6gI"))
                {
                    enemyStats[4].PlaceBottom(enemyStats[3]);
                    enemyStats[num++].Set("Beasts".FromDictionary(), (crewCombat.enemyProp.mounts + crewCombat.enemyProp.beasts).ToString());
                }
                else if (crewCombat.HasAttribute("recGVTHkSnwJnshSr"))
                {
                    enemyStats[4].PlaceBottom(enemyStats[3]);
                    enemyStats[num++].Set("Slaves".FromDictionary(), crewCombat.enemyProp.slaves.ToString());
                }
            }
        }
        while (num < enemyStats.Count)
        {
            enemyStats[num++].Set();
        }
    }

    }
}