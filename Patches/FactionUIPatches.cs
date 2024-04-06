using HarmonyLib;
using System;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(FactionUI))]
    internal class FactionUIPatches
    {
        [HarmonyPatch("GetDescription")]
        [HarmonyPostfix]
        public static void GetDescription_Postfix(FactionUI __instance, ref ValueTuple<string, string, string> __result, int dataIndex, TaskInstance taskInstance, FactionCategory ___category)
        {
            (string, string, string) result = ("", "", "");
            string gold = VisualTweak.Gold;
            string empty = "";
            var category = ___category;
            ref string item = ref empty;
            if (taskInstance.IsDebtTask())
            {
                item = ref result.Item1;
                item = item + "<color=" + VisualTweak.Red + "><b>" + Game.FromDictionary("Pay Debt") + " - " + taskInstance.faction.GetName() + "</b></color>\n";
                ref string item2 = ref result.Item2;
                item2 = item2 + TextTemplate.GetText(TextTemplateType.General, "TaskDebtUnavailable") + "\n";
                item = ref result.Item3;
                item = item + "<b><color=" + gold + ">" + Game.FromDictionary("Debt") + "</color></b> " + BaseUI.game.caravan.FormatMoney(taskInstance.faction.GetDebt(), showZero: false, showLeadZero: false);
                __result = result;
            }

            bool flag = false;
            if (taskInstance.IsMercenaryTask())
            {
                MercenaryTaskType type = taskInstance.mercenaryTask.type;
                if (((uint)(type - 3) <= 2u || type == MercenaryTaskType.Deliver) && taskInstance.GetStatus() == TaskStatus.PrepReady)
                {
                    flag = true;
                }
            }

            Node destinationNode = taskInstance.GetDestinationNode();
            Node node = (((bool)destinationNode && flag) ? null : taskInstance.GetMarkerNode(taskInstance.IsMercenaryTask()));
            string text = "?";
            if ((bool)node)
            {
                text = String.UppercaseFirst(node.GetLocationName(nearPoiName: true));
            }
            else if ((bool)destinationNode)
            {
                text = destinationNode.GetLocationName();
            }

            int num = 0;
            string text2 = "";
            switch (category)
            {
                case FactionCategory.Active:
                    num = taskInstance.GetExpireDays();
                    text2 = "<b>" + num.ToDaysLeftText() + "</b>";
                    break;
                case FactionCategory.Available:
                    num = taskInstance.GetInitialExpireDays();
                    text2 = "<b>" + num.ToInDaysText() + "</b>";
                    break;
            }

            int num2 = ((category == FactionCategory.Available) ? taskInstance.GetPayment() : taskInstance.GetActualPayment());
            int num3 = ((category != 0) ? taskInstance.GetActualPenalty() : 0);
            int num4 = num2 - num3;
            string text3 = "";
            text3 = ((num4 == 0) ? Game.FromDictionary("No payment") : ((num4 <= 0) ? ("<b><color=" + VisualTweak.Red + ">" + Game.FromDictionary("Penalty") + " -</color></b>" + BaseUI.game.caravan.FormatMoney(-num4, showZero: false, showLeadZero: false)) : ("<b><color=" + gold + ">" + Game.FromDictionary("Reward") + "</color></b> " + BaseUI.game.caravan.FormatMoney(num4, showZero: false, showLeadZero: false))));
            int stackCount = taskInstance.GetStackCount();
            bool flag2 = taskInstance.HasContraband();
            bool flag3 = category != 0 && taskInstance.GetMissingCargoUnits() > 0;
            int num5 = ((category == FactionCategory.Available) ? taskInstance.GetFactionReputation() : taskInstance.GetActualReputationReward());
            string text4 = "<sprite=\"icon_collector\" index=17>";
            string text5 = "<sprite=\"icon_collector\" index=22>";
            string text6 = "<sprite=\"icon_collector\" index=26>";
            string text7 = "<sprite=\"icon_collector\" index=30>";
            string text8 = "<sprite=\"icon_collector\" index=24>";
            string text9 = "<sprite=\"icon_collector\" index=43>";
            string text10 = "";
            if (taskInstance.IsTradeTask())
            {
                string text11 = "";
                text11 = ((!(category == FactionCategory.Active && flag3)) ? ("<i>" + text5 + text2 + " " + ((!flag2) ? text6 : text7) + stackCount + "</i>") : ("<color=" + VisualTweak.Red + "><i>" + text7 + Game.FromDictionary("missing cargo") + "</i></color>"));
                text10 = text11;
            }
            else if (taskInstance.IsMercenaryTask())
            {
                _ = taskInstance.mercenaryTask.type;
                int passengersTarget = taskInstance.passengersTarget;
                text10 = "<i>" + text5 + text2 + " </i>";
                if (passengersTarget > 0 && text10.Length < 25)
                {
                    text10 = text10 + "<i>" + text9 + passengersTarget + "</i>";
                }
            }

            string text12 = ((node != null) ? node.GetUID() : ((destinationNode != null) ? destinationNode.GetUID() : Game.FromDictionary("none")));
            ref string item3 = ref result.Item1;
            item3 = item3 + "<b>" + taskInstance.FormatTitle(Game.IsTest(), Game.IsTest()) + "</b>\n";
            item = ref result.Item2;
            item = item + "<link=dest_" + dataIndex + "_" + text12 + ">" + text4 + "<i>" + text + "</i></link> " + text10 + "\n";
            result.Item3 += text3;
            item = ref result.Item3;
            item = item + ", " + text8 + "<b>" + num5 + "</b>";
            __result = result;
        }

        [HarmonyPatch("GetButtonTakeActiveTooltip")]
        [HarmonyPostfix]
        public static void GetButtonTakeActiveTooltip_PostFix(FactionUI __instance, TaskInstance taskInstance, ref string __result)
        {
            string text = "";
            if ((bool)taskInstance.tradeTask)
            {
                text = text + string.Format("Take {0} to {1} in {2}".FromDictionary(), taskInstance.FormatGoodsList(), taskInstance.destNode.GetName(), taskInstance.GetInitialExpireDays().FormatNumberByNomen("day"));
                text = text + "\n\n";
                if (taskInstance.goodsList != null)
                {
                    text = text + "<b><color=" + Tooltip.orange + ">" + "Missing Cargo Penalty".FromDictionary() + "</color></b>\n";
                    text = text + "<i><color=" + Tooltip.gray + ">" + "You have to pay for missing cargo upon delivery:".FromDictionary() + "</color></i>\n";
                    foreach (GoodsQty goods in taskInstance.goodsList)
                    {
                        text = text + string.Format("for each {0} - {1}".FromDictionary(), goods.GetName(), BaseUI.game.caravan.FormatMoney(goods.GetValue())) + "\n";
                    }

                    text += "\n";
                }

                text = text + "<b><color=" + Tooltip.red + ">" + Game.FromDictionary("Failed Delivery Compensation") + "</color></b>\n";
                text += string.Format("<i><color={0}>{1} </color>{2}<color={3}> {4}, {5}.</color></i>", Tooltip.gray, Game.FromDictionary("If not delivered in"), taskInstance.GetLateDeliveryDays().FormatNumberByNomen("day"), Tooltip.gray, "", Game.FromDictionary("the whole cargo is deemed lost and has to be reimbursed"));
            }

            __result = text;
        }
    }
}