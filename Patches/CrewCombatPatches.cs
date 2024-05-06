using HarmonyLib;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text.RegularExpressions;
using UnityEngine;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CrewCombat))]
    internal class CrewCombatPatches
    {

        [HarmonyPatch("WriteAmbushLog")]
        [HarmonyPrefix]
        public static bool  WriteAmbushLog_Postfix(
            CrewCombat __instance,
            string ___AmbushResults1,
            string ___AmbushResults2)
        {
            CombatLogHeader(__instance, "Ambush Phase".FromDictionary());
            __instance.CombatLog(___AmbushResults1);
            __instance.CombatLog(___AmbushResults2);
            __instance.CombatLog("<b>" + "End of Ambush Phase".FromDictionary() +" ------------------------------</b>");
            __instance.WriteCombatLog();

            return false;
        }

        public static void CombatLogHeader(CrewCombat __instance, string header, bool aftermathLogEnabled = false)
        {
            Type classType = typeof(CrewCombat);

            MethodInfo methodInfo = classType.GetMethod("CombatLogHeader", BindingFlags.NonPublic | BindingFlags.Instance);

            methodInfo.Invoke(__instance, new object[] { header, aftermathLogEnabled });

        }

        public static void ExecuteDMGRedistribution(CrewCombat __instance, ref int dmgToDistribute)
        {
            Type classType = typeof(CrewCombat);

            MethodInfo methodInfo = classType.GetMethod("ExecuteDMGRedistribution", BindingFlags.NonPublic | BindingFlags.Instance);

            methodInfo.Invoke(__instance, new object[] { dmgToDistribute });
        }

        [HarmonyPatch("DumpRoundCombatLog")]
        [HarmonyPrefix]
        public static bool DumpRoundCombatLog_Prefix(
            CrewCombat __instance, 
            string ___CriticalTestResultText, 
            string ___WardingText,
            string ___roundendtext,
            string ___MoraleText1,
            string ___MoraleText2,
            string ___MoraleText3,
            string ___ConfidenceText1,
            string ___ConfidenceText2,
            string ___ConfidenceText3,
            bool aftermathLogEnabled = false)
        {
            var CriticalTestResultText = ___CriticalTestResultText;

            var WardingText = ___WardingText;
            var roundendtext = ___roundendtext;

            var MoraleText1 = ___MoraleText1;
            var MoraleText2 = ___MoraleText2;
            var MoraleText3 = ___MoraleText3;
            var ConfidenceText1 = ___ConfidenceText1;
            var ConfidenceText2 = ___ConfidenceText2;
            var ConfidenceText3 = ___ConfidenceText3;
            var log = "";
            if (__instance.IsAftermath())
            {
                __instance.CombatLogDebug(string.Format("{0} aftermath rounds executed".FromDictionary(), __instance.GetRound() - 4), aftermathLogEnabled);
            }
            __instance.CombatLog(CriticalTestResultText);
            if (WardingText != "")
            {
                __instance.CombatLog(WardingText, aftermathLogEnabled: true);
            }
            int num = 0;
            foreach (CrewCombatGroup group in __instance.groups)
            {
                string whoInflicts = (group.IsPlayer() ? "The Enemy" : "Your Crew");
                var groupPostfix = !group.elite ? "" : "(" + (group.leader ? "Leader".FromDictionary() : "Elite".FromDictionary()) + ")";
                if (group.stat.damageCount > 0)
                {
                    if (group.IsPlayer())
                    {
                        log = string.Format("<b>The Enemy</b> inflicts <color={0}>{1} DMG</color> to <b>{2}{3}</b>".FromDictionary(), VisualTweak.Red, group.stat.damageCount, (group.GetName() + (group.GetNotDeadOrTakenCount() > 1 && group.stat.killCount > 1 ? "s" : "")).FromDictionary(), groupPostfix);
                    } else
                    {
                        log = string.Format("<b>Your Crew</b> inflicts <color={0}>{1} DMG</color> to <b>{2}{3}</b>".FromDictionary(), VisualTweak.Red, group.stat.damageCount, group.GetName(), groupPostfix);
                    }
                    __instance.CombatLog(log, aftermathLogEnabled);
                }
                if (group.stat.killCount > 0)
                {
                    string groupPostfix2 = group.elite ? ", " + "causing additional grit damage".FromDictionary() : "";
                    string numberOfWounded = (group.IsCharacter() && group.IsPlayer()) ? "" : group.stat.killCount.ToString();
                    if (group.IsPlayer())
                    {
                        __instance.CombatLog(string.Format("<b>The Enemy</b> severely wounds <color={0}>{1}</color><b>{2}{3}{4}</b>".FromDictionary(), VisualTweak.Red, numberOfWounded, group.GetName().FromDictionary(), groupPostfix, groupPostfix2), aftermathLogEnabled);
                    } else
                    {
                        __instance.CombatLog(string.Format("<b>Your Crew</b> severely wounds <color={0}>{1}</color><b>{2}{3}{4}</b>".FromDictionary(), VisualTweak.Red, numberOfWounded, group.GetName(), groupPostfix,groupPostfix2), aftermathLogEnabled);
                    }
                }
                if (group.stat.killCount + group.stat.takenCount > 0)
                {
                    num += (group.IsPlayer() ? (group.stat.killCount + group.stat.takenCount) : 0);
                }
            }
            if ((float)num > 0.1f * (float)__instance.GetCrewCount(CrewSide.Player, Prop.None, onlyFightingCrew: false))
            {
                int num2 = Mathf.RoundToInt(Mathf.Min(3f, 6f * (float)num / (float)__instance.GetCrewCount(CrewSide.Player, Prop.None, onlyFightingCrew: false)));
                Game.game.caravan.AddProperty(Prop.Morale, -num2);
                __instance.CombatLog(string.Format("{0} Morale due to your casualties".FromDictionary(), String.Sign(-num2)));
            }
            if (__instance.roundStat.propList.Count > 0 || __instance.roundStat.cargoList.Count > 0)
            {
                __instance.CombatLog("<b>" + (__instance.IsEnemyAttacker() ? ("The Enemy" + ((__instance.HasAttribute("recnAcHsmQshE5uCC") || __instance.HasAttribute("recnAcHsmQshE5uCC")) ? " ravages" : " grabs")) : "Your Crew grabs").FromDictionary() + "</b>");
            }
            foreach (PropQty prop in __instance.roundStat.propList)
            {
                __instance.CombatLog(prop.qty.FormatNumberByNomen(Game.game.caravan.FindProperty(prop.prop).GetName(),true), aftermathLogEnabled);
            }
            foreach (GoodsQty cargo in __instance.roundStat.cargoList)
            {
                __instance.CombatLog(cargo.qty.FormatNumberByNomen(cargo.goods.GetName(true),true), aftermathLogEnabled);
            }
            if (!__instance.IsAppeaseSuccessful())
            {
                __instance.CombatLog($"<b>{ "End of round".FromDictionary() } ------------------------------</b>", aftermathLogEnabled: true);
            }
            __instance.CombatLog(roundendtext, aftermathLogEnabled: true);
            if (MoraleText1 != "")
            {
                __instance.CombatLog(MoraleText1, aftermathLogEnabled: true);
            }
            if (MoraleText2 != "")
            {
                __instance.CombatLog(MoraleText2, aftermathLogEnabled: true);
            }
            if (MoraleText3 != "")
            {
                __instance.CombatLog(MoraleText3, aftermathLogEnabled: true);
            }
            if (ConfidenceText1 != "")
            {
                __instance.CombatLog(ConfidenceText1, aftermathLogEnabled: true);
            }
            if (ConfidenceText2 != "")
            {
                __instance.CombatLog(ConfidenceText2, aftermathLogEnabled: true);
            }
            if (ConfidenceText3 != "")
            {
                __instance.CombatLog(ConfidenceText3, aftermathLogEnabled: true);
            }
            __instance.WriteCombatLog();

            return false;
        }

        [HarmonyPatch("ExecuteWarding")]
        [HarmonyPrefix]
        public static bool ExecuteWarding_Prefix(CrewCombat __instance, ref string ___WardingText, ref int damage, ref int ward, CrewSide side = CrewSide.Unknown)
        {
            if (side != 0 && ward >= 1)
            {
                int num = 0;
                num = Mathf.Min(damage, ward);
                damage -= num;
                ward -= num;
                if (num > 0)
                {
                    ___WardingText = ___WardingText + string.Format(("{0} DMG successfully warded off by " + ((side == CrewSide.Player) ? "your Fighting Crew" : "the Enemy")).FromDictionary(),num) + "\n";
                }
            }

            return false;
        }

        [HarmonyPatch("ExecuteTargetWarding")]
        [HarmonyPrefix]
        public static bool ExecuteTargetWarding_prefix(CrewCombat __instance, ref string ___WardingText, string target,ref int ___beastWard, ref int ___cargoWard, ref int ___crewWard)
        {
            int dmgToDistribute = 0;
            if (target == "recpjS1O15WGBP6gI" && ___beastWard > 0)
            {
                dmgToDistribute = Mathf.Min(___beastWard, __instance.accruedTargetDamage);
                if (dmgToDistribute > 0)
                {
                    ___WardingText = ___WardingText + dmgToDistribute + " DMG against Beasts absorbed by " + (__instance.IsPlayerDefender() ? "your Fighting Crew" : "the Enemy") + "\n";
                }
                __instance.accruedTargetDamage -= dmgToDistribute;
                ___beastWard -= dmgToDistribute;
            }
            else
            {
                switch (target)
                {
                    case "recyXqWtN2agsAZbC":
                    case "recGVTHkSnwJnshSr":
                    case "recDxvpAcQVkVqTYV":
                        if (___crewWard > 0)
                        {
                            dmgToDistribute = Mathf.Min(___crewWard, __instance.accruedTargetDamage);
                            if (target == "recyXqWtN2agsAZbC")
                            {
                                if (dmgToDistribute > 0)
                                {
                                    ___WardingText = ___WardingText + dmgToDistribute + " DMG against Crew absorbed by " + (__instance.IsPlayerDefender() ? "your Fighting Crew" : "the Enemy") + "\n";
                                }
                                else if (target == "recDxvpAcQVkVqTYV")
                                {
                                    if (dmgToDistribute > 0)
                                    {
                                        ___WardingText = ___WardingText + dmgToDistribute + " DMG against Passengers absorbed by " + (__instance.IsPlayerDefender() ? "your Fighting Crew" : "the Enemy") + "\n";
                                    }
                                    else if (target == "recGVTHkSnwJnshSr" && dmgToDistribute > 0)
                                    {
                                        ___WardingText = ___WardingText + dmgToDistribute + " DMG to free Slaves absorbed by " + (__instance.IsPlayerDefender() ? "your Fighting Crew" : "the Enemy") + "\n";
                                    }
                                }
                            }
                            __instance.accruedTargetDamage -= dmgToDistribute;
                            ___crewWard -= dmgToDistribute;
                            break;
                        }
                        goto default;
                    default:
                        if (target == "reclLHhWav7yD6BxC" && ___cargoWard > 0)
                        {
                            dmgToDistribute = Mathf.Min(___cargoWard, __instance.accruedTargetDamage);
                            if (dmgToDistribute > 0)
                            {
                                ___WardingText = ___WardingText + dmgToDistribute + " DMG against Cargo absorbed by " + (__instance.IsPlayerDefender() ? "your Fighting Crew" : "the Enemy") + "\n";
                            }
                            __instance.accruedTargetDamage -= dmgToDistribute;
                            ___cargoWard -= dmgToDistribute;
                        }
                        break;
                }
            }
            if (dmgToDistribute > 0)
            {
                ExecuteDMGRedistribution(__instance, ref dmgToDistribute);
            }

            return false;
        }

        [HarmonyPatch("DumpHealCombatLog")]
        [HarmonyPrefix]
        public static bool DumpHealCombatLog_Prefix(CrewCombat __instance)
        {
            foreach (CrewCombatGroup group in __instance.groups)
            {
                if (group.stat.healFromDeadCount > 0)
                {
                    if (group.IsCharacter())
                    {
                        __instance.CombatLog(string.Format("Severely wounded <b>{0}</b> healed".FromDictionary(), group.GetName().FromDictionary()));
                    }
                    else
                    {
                        __instance.CombatLog(string.Format("<color={0}>{1}</color> severely wounded <b>{2}</b> healed".FromDictionary(), VisualTweak.Green, group.stat.healFromDeadCount, group.GetName().FromDictionary()));
                    }
                }
                if (group.stat.healFromWoundedCount > 0)
                {
                    if (group.IsCharacter())
                    {
                        __instance.CombatLog(string.Format("Wounded <b>{0}</b> healed".FromDictionary(), group.GetName()));
                    }
                    else
                    {
                        __instance.CombatLog(string.Format("<color={0}>{1}</color> wounded <b>{2}</b> healed".FromDictionary(), VisualTweak.Green, group.stat.healFromWoundedCount, group.GetName().FromDictionary()));
                    }
                }
                if (group.stat.killDuringHeal > 0)
                {
                    __instance.CombatLog(string.Format("<color={0}>{1}</color> wounded <b>{2}</b> could not be saved".FromDictionary(), VisualTweak.Red, group.stat.killDuringHeal, group.GetName().FromDictionary()));
                }
            }
            __instance.WriteCombatLog();

            return false;
        }

        [HarmonyPatch("ExecuteHealStatus")]
        [HarmonyPrefix]
        public static bool ExecuteHealStatus_Prefix(CrewCombat __instance, CrewCombatLoot ___crewCombatLoot)
        {
            CombatLogHeader(__instance,"Healing".FromDictionary());
            __instance.WriteCombatLog();
            if (Game.IsAnyTutorial())
            {
                Game.game.caravan.ShowTutorialCrewCombatHeal();
            }
            ___crewCombatLoot.GenerateSlaveLoot();
            if (__instance.IsDealActive(DealWithType.Enemy) || __instance.IsDealActive(DealWithType.Slaves))
            {
                __instance.crewCombatFightUI.BuildDeals();
            }

            return false;
        }

        public static float GetWorthRate(CrewCombat __instance)
        {
            Type classType = typeof(CrewCombat);

            MethodInfo methodInfo = classType.GetMethod("GetWorthRate", BindingFlags.NonPublic | BindingFlags.Instance);

            return (float)methodInfo.Invoke(__instance, new object[] { });
        }

        [HarmonyPatch("SurrenderTest")]
        [HarmonyPrefix]
        public static bool SurrenderTest_Prefix(CrewCombat __instance, ref bool __result, ref string ___ConfidenceText1, ref string ___ConfidenceText2, ref string ___ConfidenceText3)
        {
            if (__instance.IsPlayerDefender())
            {
                if (__instance.IsFleeFight())
                {
                    __result = true;
                    return false;
                }
                __result = false;
                return false;
            }
            if (__instance.HasAttribute("recMsiuLwsnxil4dA"))
            {
                __result = false;
                return false;
            }
            int value = Mathf.RoundToInt(Mathf.Pow(GetWorthRate(__instance) * (float)Game.game.caravan.GetProperty(Prop.StrengthRate) / 100f, 0.5f) * 100f + Mathf.Min((__instance.GetCSDefenseRate(fightingCrew: true) - 1f) * 100f, 100f) + (float)Game.game.caravan.GetProperty(Prop.Grit) - 50f);
            __instance.CombatLogDebug("<b>SurrenderTest chance</b> = WorthR(" + GetWorthRate(__instance) + ")*SR(" + (float)Game.game.caravan.GetProperty(Prop.StrengthRate) / 100f + ")+ CSR/CSP(" + Mathf.Min((__instance.GetCSDefenseRate(fightingCrew: true) - 1f) * 100f, 100f) + ")+ Grit(" + Game.game.caravan.GetProperty(Prop.Grit) + ")- 50");
            value = Mathf.Clamp(value, 0, 100);
            int result;
            bool flag = Game.Test(value, out result);
            ___ConfidenceText1 = string.Format("<b>Confidence check </b>({0} to {1})".FromDictionary(),result,value);
            if (__instance.gameEvent != null || !__instance.IsOverwhelmed(CrewSide.Enemy) || __instance.HasPlayerAttribute("recIylqNd2SVjjoJb") || __instance.template.surrenderNotAllowed)
            {
                ___ConfidenceText2 = ("<b>The Enemy " + (flag ? "Continues the Fight" : "tries to Flee</b>")).FromDictionary();
                if (!flag)
                {
                    __instance.scheduledAbort = new CrewCombatAbort(AbortReason.Flee, CrewSide.Enemy);
                }
            }
            else
            {
                ___ConfidenceText2 = ("<b>The Enemy " + (flag ? "Continues the Fight" : "Surrenders</b>")).FromDictionary();
                if (!flag)
                {
                    __instance.scheduledAbort = new CrewCombatAbort(AbortReason.Surrender, CrewSide.Enemy);
                }
            }
            __result = !flag;
            return false;
        }

    }
}