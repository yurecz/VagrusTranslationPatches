using HarmonyLib;
using UnityEngine.Rendering;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(TaskInstance))]
    internal class TaskInstancePatches
    {

        [HarmonyPatch("FormatTradeTitle")]
        [HarmonyPostfix]
        public static void FormatTradeTitle_Postfix(TaskInstance __instance, ref string __result)
        {
                string text = "";
                switch (__instance.tradeTask.type)
                {
                    case TradeTaskType.DistributionRun:
                        text = "Distribute for";
                        break;
                    case TradeTaskType.ProcurementJob:
                        text = "Procure Goods for";
                        break;
                    case TradeTaskType.SupplyRun:
                        text = "Supply Run for";
                        break;
                    case TradeTaskType.RestockErrand:
                        text = "Restock for";
                        break;
                }
            __result =  text.FromDictionary() + " " + __instance.faction.GetName(false);
        }

        [HarmonyPatch("FormatMercenaryTitle")]
        [HarmonyPostfix]
        public static void FormatMercenaryTitle_Postfix(TaskInstance __instance, ref string __result)
        {
            __result = (__instance.mercenaryTask.type.ToString() + " for").FromDictionary() + " " + __instance.faction.GetName();
        }

        [HarmonyPatch("FormatExplorationTitle")]
        [HarmonyPostfix]
        public static void FormatExplorationTitle_Postfix(TaskInstance __instance, ref string __result)
        {
            __result = (__instance.explorationTask.type.ToString() + " for").FromDictionary() + " " + __instance.faction.GetName();
        }

        [HarmonyPatch("FormatRegularTooltip")]
        [HarmonyPostfix]
        public static void FormatRegularTooltip_Postfix(TaskInstance __instance, ref string __result)
        {
            string reputationSymbol = "<sprite=\"icon_collector\" index=24>";
            string result = "";
            int reputation = __instance.GetFactionReputation();
            int level = __instance.GetLevel();
            result = result + "<b><size=" + VisualTweak.TooltipHeaderSize + "%>" + __instance.FormatTitle() + "</size></b>\n\n";
            if (__instance.IsTradeTask())
            {
                result = result + "<i><color=" + Tooltip.gray + ">" + "Deliver to".FromDictionary() + " " + __instance.destNode.GetName() + " :</color></i>\n\n";
                result = result + "<i><color=" + Tooltip.gray + ">" + __instance.FormatGoodsList() + "</color></i>\n\n";
            }
            else if (__instance.IsMercenaryTask())
            {
                MercenaryTaskType type = __instance.mercenaryTask.type;
                if (__instance.encounterTarget > __instance.numberOfEncounters)
                {
                    if (__instance.encounterTarget != 1 + __instance.numberOfEncounters)
                    {
                        _ = VisualTweak.Red;
                    }
                    else
                    {
                        _ = VisualTweak.Orange;
                    }
                }
                else
                {
                    _ = VisualTweak.Green;
                }
                bool flag = __instance.GetMarkerNode() != null;
                bool flag2 = __instance.destNode != null;
                bool flag3 = flag && flag2 && __instance.destNode == __instance.GetMarkerNode();
                if (flag && flag2 && !flag3)
                {
                    result += __instance.GetDeliverText(__instance.GetMarkerNode(), 1, nearBy: true);
                    result += __instance.GetDeliverText(__instance.destNode, 2);
                }
                else if (flag)
                {
                    result += __instance.GetDeliverText(__instance.GetMarkerNode(), 3, nearBy: true);
                }
                else if (flag2)
                {
                    result += __instance.GetDeliverText(__instance.destNode, 3);
                }
                if (flag || flag2)
                {
                    result += "\n";
                }
                if (__instance.csDefRange != null)
                {
                    string color = "";
                    int factionBottomIconIndex = TaskTweak.GetFactionBottomIconIndex(__instance, out color, tooltip: true);
                    result = result + "Encounter Difficulty:".FromDictionary() + " <color=" + color + "><sprite=\"icon_collector\" index=" + factionBottomIconIndex + "></color> ~" + __instance.csDefRange.GetMiddleInt() + "</b>\n";
                }
                result = ((type != MercenaryTaskType.Escort && type != MercenaryTaskType.Deliver && (type != MercenaryTaskType.Defend || __instance.numberOfEncounters <= 1)) ? (result + "Required Combat Encounters:".FromDictionary() + " " + __instance.encounterTarget + "\n") : (result + "Potential Combat Encounters:".FromDictionary() + " " + __instance.encounterTarget + "\n"));
                if (__instance.passenger == null && __instance.IsActive() && __instance.GetStatus() != TaskStatus.Ready && (type == MercenaryTaskType.Defend || type == MercenaryTaskType.Conquer))
                {
                    int num3 = ((__instance.GetLevel() < 4) ? (5 * (__instance.GetLevel() + 1)) : ((__instance.GetLevel() == 4) ? 30 : 40));
                    int num4 = ((__instance.GetStatus() != TaskStatus.Assigned) ? num3 : 0);
                    string text3 = ((num3 <= num4) ? VisualTweak.Green : VisualTweak.Red);
                    result = result + "<color=" + VisualTweak.Gold + ">"+"Bring Reinforcements (optional)".FromDictionary()+"</color>\n";
                    result = result + "<b><color=" + text3 + ">" + num4 + "</color>/" + num3 + "</b>\n";
                }
                else if (__instance.passenger != null)
                {
                    int num5 = __instance.passengersTarget;
                    int passengers = __instance.passenger.GetPassengers();
                    if (num5 > passengers)
                    {
                        _ = VisualTweak.Red;
                    }
                    else
                    {
                        _ = VisualTweak.Green;
                    }
                    
                    result = string.Concat(new string[]
                {
                    result,
                    (type == MercenaryTaskType.Capture) ? "Captured targets".FromDictionary() : ((type == MercenaryTaskType.Abduct) ? "Taken hostages".FromDictionary() : ((type == MercenaryTaskType.Rescue) ? "Freed Hostages".FromDictionary() : "Passengers".FromDictionary())),
                    " to deliver: ",
                    num5.ToString(),
                    "\n"
                });
                }
                int stackCount = __instance.GetStackCount();
                if (stackCount > 0)
                {
                    result = result + "<color=" + VisualTweak.Gold + ">"+"Cargo slot".FromDictionary() + "</color>\n";
                    result += stackCount;
                }
            }
            result = result + "\n<b><color=" + Tooltip.gold + ">"+"Rewards".FromDictionary()+"</color></b>\n";
            result = result + Game.game.caravan.FormatMoney(__instance.GetPayment(), showZero: false) + "\n";
            if (reputation != 0)
            {
                result = result + reputationSymbol + String.Sign(reputation) + "\n";
            }
            result += "\n";
            if (__instance.IsPassengerTask())
            {
                __result = result;
                return;
            }
            bool flag4 = __instance.GetOpposingFactionReputation() < 0;
            bool flag5 = __instance.GetOpposingFaction() != null && __instance.GetOpposingFaction().IsImpervious();
            if (flag4 || flag5 || __instance.HasContraband())
            {
                result = result + "<b><color=" + Tooltip.red + ">"+"Risks".FromDictionary() +"</color></b>\n";
                if (flag5)
                {
                    result += "Impervious Enemy".FromDictionary() + "\n";
                }
                if (flag4)
                {
                    result = result + "Faction Opponent:".FromDictionary() + " " + __instance.GetOpposingFactionForRep().GetName() + "\n";
                }
                if (__instance.HasContraband())
                {
                    result += "Contraband".FromDictionary() + "\n";
                }
                result += "\n";
            }
            string text4 = (__instance.CanTake() ? Tooltip.green : Tooltip.red);
            string text5 = "";
            if (level > 0)
            {
                text5 = text5 + "Faction Reputation Tier".FromDictionary() + " " + String.ToRoman(level) + "\n";
            }
            if (__instance.taskType == TaskType.Trade)
            {
                text5 = text5 + "Free Cargo Slots".FromDictionary() + " " + __instance.GetStackCount() + "\n";
            }
            if (text5.Length > 0)
            {
                result = result + "<b><color=" + text4 + ">" + "Requirements".FromDictionary() + "</color></b>\n";
                result += text5;
            }
            __result = result;
        }

        [HarmonyPatch("GetDeliverText")]
        [HarmonyPostfix]
        public static void GetDeliverText_Postfix(TaskInstance __instance, ref string __result, Node targetNode, int step, bool nearBy = false)
        {
            string text = "";
            string targetName = ((targetNode == null) ? "target missing" : targetNode.GetLocationName(nearBy, forceRegionName: false, prepositionIN: true));
            if (__instance.IsMercenaryTask())
            {
                switch (__instance.mercenaryTask.type)
                {
                    case MercenaryTaskType.Escort:
                        {
                            PassengerUse passengerUse = __instance.mercenaryTask.GetPassengerUse();
                            if (step == 1 && (passengerUse == PassengerUse.AddReady || passengerUse == PassengerUse.AddPrepReady || passengerUse == PassengerUse.AddPrepReadyRemoveReady))
                            {
                                text += "Pick up from".FromDictionary();
                            }
                            else
                            {
                                text += ((step == 2) ? "Then escort them to".FromDictionary() : "Escort to".FromDictionary());
                            }

                            text = text + " <b>" + targetName + "</b> " + ((step == 2) ? "" : (" <b>" + ((__instance.passenger == null) ? "your passengers".FromDictionary() : __instance.passenger.GetDeliverName()))) + "</b>" + "\n";
                            break;
                        }
                    case MercenaryTaskType.Eliminate:
                        text = "Eliminate your targets".FromDictionary() + " <b>" + targetName + "</b>\n";
                        break;
                    case MercenaryTaskType.Abduct:
                    case MercenaryTaskType.Rescue:
                    case MercenaryTaskType.Capture:
                        text = ((step == 1) ? ((__instance.mercenaryTask.type.ToString() + " your targets").FromDictionary()) : "Then bring them to".FromDictionary()) + " <b>" + targetName + "</b>\n";
                        break;
                    case MercenaryTaskType.Pillage:
                        text = (__instance.mercenaryTask.type.ToString() + " your targets").FromDictionary() + " <b>" + targetName + "</b>\n";
                        break;
                    default:
                        text = (__instance.mercenaryTask.type.ToString() + " your target").FromDictionary() + " <b>" + targetName + "</b>\n";
                        break;
                }
            }
            else if (__instance.goodsList != null)
            {
                text = text + "Deliver to".FromDictionary()+" <b>" + targetName + "</b>:\n";

                foreach (GoodsQty goods in __instance.goodsList)
                {
                    text = text + "              <b>" + goods.GetQty() + " " + goods.GetName() + "</b>\n";
                }
            }

            __result = text;
        }

        [HarmonyPatch("GetPenalties")]
        [HarmonyPostfix]
        public static void GetPenalties(TaskInstance __instance, ref string __result)
        {
            if (__instance.goodsList == null)
            {
                __result = "";
                return;
            }
            string text = "<sprite=\"icon_collector\" index=26>";
            string text2 = "";
            foreach (GoodsQty goodsQty in __instance.goodsList)
            {
                string text3 = Game.game.caravan.FormatMoney(goodsQty.GetValue(), true, false, 3);
                text2 = text2 + text + text3 + "\n";
            }
            __result = text2;
        }

    }
}