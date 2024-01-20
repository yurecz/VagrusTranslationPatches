using Epic.OnlineServices;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vagrus;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(BookUI))]
    internal class BookUIPatches
    {

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        public static void Awake_Postfix(BookUI __instance, GameObject ___contractPrefab)
        {

            var contractPrefab = Resources.Load("UI/Book/Prefab/ContractDetails") as GameObject;
            if (contractPrefab != null && !contractPrefab.HasComponent<UIObjectTranslator>())
            {
                contractPrefab.AddComponent<UIObjectTranslator>();
            }

            var cardPrefab = Resources.Load("UI/Book/Prefab/TaskCard") as GameObject;
            if (cardPrefab != null && !cardPrefab.HasComponent<UIObjectTranslator>())
            {
                cardPrefab.AddComponent<UIObjectTranslator>();
            }

            var deliverPrefab = Resources.Load("UI/Book/Prefab/DeliverRow") as GameObject;
            if (deliverPrefab != null && !deliverPrefab.HasComponent<UIObjectTranslator>())
            {
                deliverPrefab.AddComponent<UIObjectTranslator>();
            }

            var flavorBlockPrefab = Resources.Load("UI/Book/Prefab/FlavorBlock") as GameObject;
            if (flavorBlockPrefab != null && !flavorBlockPrefab.HasComponent<UIObjectTranslator>())
            {
                flavorBlockPrefab.AddComponent<UIObjectTranslator>();
            }

            var rewardBlockPrefab = Resources.Load("UI/Book/Prefab/RewardBlock") as GameObject;
            if (rewardBlockPrefab != null && !rewardBlockPrefab.HasComponent<UIObjectTranslator>())
            {
                rewardBlockPrefab.AddComponent<UIObjectTranslator>();
            }

            var rumorBottomPrefab = Resources.Load("UI/Book/Prefab/RumorBottomBlock") as GameObject;
            if (rumorBottomPrefab != null && !rumorBottomPrefab.HasComponent<UIObjectTranslator>())
            {
                rumorBottomPrefab.AddComponent<UIObjectTranslator>();
            }

            var entryPrefab = Resources.Load("UI/Book/Prefab/BookEntry") as GameObject;
            if (entryPrefab != null && !entryPrefab.HasComponent<UIObjectTranslator>())
            {
                entryPrefab.AddComponent<UIObjectTranslator>();
            }

            var chapterPrefab = Resources.Load("UI/Book/Prefab/BookChapterBlock") as GameObject;
            if (chapterPrefab != null && !chapterPrefab.HasComponent<UIFontUpdater>())
            {
                chapterPrefab.AddComponent<UIFontUpdater>();
            }

            var noteEditorPrefab = Resources.Load("UI/Book/Prefab/JournalNoteEditor") as GameObject;
            if (noteEditorPrefab != null && !noteEditorPrefab.HasComponent<UIFontUpdater>())
            {
                noteEditorPrefab.AddComponent<UIFontUpdater>();
            }
        }

        [HarmonyPatch("GetRewardTopText")]
        [HarmonyPostfix]
        public static void GetRewardTopText_Postfix(BookUI __instance, ref string __result, TaskInstance taskInstance)
        {

            string text = "";
            bool isSuccess = taskInstance.IsSuccess();
            string color;

            DeliverTime deliveryTime = taskInstance.GetNextDeadline(out color, out var dueDays);
            if (taskInstance.IsActive())
            {
                var taskGoalName = (taskInstance.IsMercenaryTask() ? "completion" : "delivery");
                switch (deliveryTime)
                {
                    case DeliverTime.Fast:
                        {
                            text = $"<color={color}>" + $"Fast {taskGoalName} deadline".FromDictionary() + "</color> <b>" + dueDays.ToDueInText() + "</b>.";
                            break;
                        }
                    case DeliverTime.Ontime:
                        {
                            text = $"<color={color}>" + $"On time {taskGoalName} deadline".FromDictionary() + "</color> <b>" + dueDays.ToDueInText() + "</b>.";
                            break;
                        }
                    case DeliverTime.Late:
                        {
                            text = $"<color={color}>" + $"Late {taskGoalName} deadline".FromDictionary() + "</color> <b>" + dueDays.ToDueInText() + "</b>.";
                            break;
                        }
                }

            }
            else
            {
                var daysPassed = BookUI.game.calendar.GetDayStamp() - taskInstance.GetFinishedDaysStamp();
                var taskName = (taskInstance.IsMercenaryTask() ? "Task" : "Delivery");
                switch (deliveryTime)
                {

                    case DeliverTime.Fast:
                        {
                            text = $"{taskName} completed fast".FromDictionary() + " <b>" + daysPassed.ToDaysAgoText() + "</b>.";
                            break;
                        }
                    case DeliverTime.Ontime:
                        {
                            text = $"{taskName} completed on-time".FromDictionary() + " <b>" + daysPassed.ToDaysAgoText() + "</b>.";
                            break;
                        }
                    case DeliverTime.Late:
                        {
                            text = $"{taskName} completed late".FromDictionary() + " <b>" + daysPassed.ToDaysAgoText() + "</b>.";
                            break;
                        }
                    case DeliverTime.Failed:
                        {
                            text = $"{taskName} failed".FromDictionary() + " <b>" + daysPassed.ToDaysAgoText() + "</b>.";
                            break;
                        }
                }
            }
            __result = text;
        }

        [HarmonyPatch("GetTradeCardDescription")]
        [HarmonyPostfix]
        public static void GetTradeCardDescription_Postfix(BookUI __instance, TaskInstance taskInstance, TaskCard card, ref string __result)
        {

            string text = "";
            string color;
            int days;
            DeliverTime nextDeadline = taskInstance.GetNextDeadline(out color, out days);
            bool flag = nextDeadline == DeliverTime.Late || nextDeadline == DeliverTime.Failed;
            if (taskInstance.IsSuccess())
            {
                color = VisualTweak.Black;
            }
            bool flag2 = taskInstance.HasContraband();
            bool flag3 = flag2;
            int finishedDaysStamp = taskInstance.GetFinishedDaysStamp();
            int num = ((finishedDaysStamp > 0) ? (taskInstance.GetExpireDaysStamp() - finishedDaysStamp) : 0);
            int leftDays = ((num >= 0 && finishedDaysStamp > 0) ? num : taskInstance.GetExpireDays());
            int initialExpireDays = taskInstance.GetInitialExpireDays();
            int num3 = taskInstance.GetLateDeliveryDays() - initialExpireDays;
            int lateDeliveryRemainDays = taskInstance.GetLateDeliveryRemainDays();
            switch (card)
            {
                case TaskCard.CargoMissing:
                    if (taskInstance.IsActive())
                    {
                        int missingCargoUnits = taskInstance.GetMissingCargoUnits();
                        if (missingCargoUnits > 0)
                        {
                            text = text + "<color=" + VisualTweak.Gold + ">" + "Missing cargo".FromDictionary() + "</color>\n";
                            text = text + missingCargoUnits + " " + "units".FromDictionary() + "\n";
                        }
                    }
                    break;
                case TaskCard.Penalty:
                    if (taskInstance.IsActive())
                    {
                        int actualPenalty = taskInstance.GetActualPenalty();
                        if (actualPenalty > 0)
                        {
                            text = text + "<color=" + VisualTweak.Gold + ">" + "Penalties".FromDictionary() + "</color>\n";
                            text = text + Game.game.caravan.FormatMoney(actualPenalty, showZero: false, showLeadZero: false) + "\n";
                        }
                    }
                    break;
                case TaskCard.CargoSlots:
                    {
                        int stackCount = taskInstance.GetStackCount();
                        if (stackCount > 0)
                        {
                            text = text + "<color=" + VisualTweak.Gold + ">" + "Cargo slot".FromDictionary() + "</color>\n";
                            text += stackCount;
                        }
                        break;
                    }
                case TaskCard.Time:
                    text = text + "<color=" + VisualTweak.Gold + ">" + "Time left".FromDictionary() + "</color>\n";
                    text = text + "<b><color=" + color + ">" + leftDays + "</color>/" + initialExpireDays + " </b>" + "days".FromDictionary().ToLower() + "\n";
                    break;
                case TaskCard.Risk:
                    if (flag3)
                    {
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Risk".FromDictionary() + "</color>\n";
                        if (flag2)
                        {
                            text += "Contraband".FromDictionary() + "\n";
                        }
                    }
                    break;
                case TaskCard.LateDelivery:
                    if (flag)
                    {
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Late Delivery".FromDictionary() + "</color>\n";
                        text = text + "<b><color=" + color + ">" + lateDeliveryRemainDays + "</color>/" + num3 + "</b> " + "days".FromDictionary().ToLower() + "\n";
                    }
                    break;
            }
            __result = text;

        }

        [HarmonyPatch("GetMercenaryCardDescription")]
        [HarmonyPostfix]
        public static void GetMercenaryCardDescription_Postfix(BookUI __instance, TaskInstance taskInstance, TaskCard card, ref string __result)
        {
            string text = "";
            if (taskInstance.mercenaryTask == null)
            {
                __result = text;
            }
            string black;
            int num;
            DeliverTime nextDeadline = taskInstance.GetNextDeadline(out black, out num);
            bool flag = nextDeadline == DeliverTime.Late || nextDeadline == DeliverTime.Failed;
            if (taskInstance.IsSuccess())
            {
                black = VisualTweak.Black;
            }
            bool flag2 = taskInstance.GetOpposingFactionReputation() < 0;
            int finishedDaysStamp = taskInstance.GetFinishedDaysStamp();
            int num2 = ((finishedDaysStamp > 0) ? (taskInstance.GetExpireDaysStamp() - finishedDaysStamp) : 0);
            int num3 = ((num2 >= 0 && finishedDaysStamp > 0) ? num2 : taskInstance.GetExpireDays());
            int initialExpireDays = taskInstance.GetInitialExpireDays();
            int num4 = taskInstance.GetLateDeliveryDays() - initialExpireDays;
            int lateDeliveryRemainDays = taskInstance.GetLateDeliveryRemainDays();
            string text2 = ((taskInstance.encounterTarget <= taskInstance.numberOfEncounters) ? VisualTweak.Green : ((taskInstance.encounterTarget == 1 + taskInstance.numberOfEncounters) ? VisualTweak.Orange : VisualTweak.Red));
            MercenaryTaskType type = taskInstance.mercenaryTask.type;
            switch (card)
            {
                case TaskCard.CargoSlots:
                    {
                        int stackCount = taskInstance.GetStackCount();
                        if (stackCount > 0)
                        {
                            text = text + "<color=" + VisualTweak.Gold + ">" + "Cargo slot".FromDictionary() + "</color>\n";
                            text += stackCount.ToString();
                        }
                        break;
                    }
                case TaskCard.Encounter:
                    if (type == MercenaryTaskType.Escort || type == MercenaryTaskType.Deliver || (type == MercenaryTaskType.Defend && taskInstance.numberOfEncounters > 1))
                    {
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Potential Battles".FromDictionary() + "</color>\n";
                        text = string.Concat(new string[]
                        {
                    text,
                    "<b><color=",
                    text2,
                    ">",
                    taskInstance.numberOfEncounters.ToString(),
                    "</color>/",
                    taskInstance.encounterTarget.ToString(),
                    "</b>\n"
                        });
                    }
                    else
                    {
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Required Battles".FromDictionary() + "</color>\n";
                        text = string.Concat(new string[]
                        {
                    text,
                    "<b><color=",
                    text2,
                    ">",
                    taskInstance.numberOfEncounters.ToString(),
                    "</color>/",
                    taskInstance.encounterTarget.ToString(),
                    "</b>\n"
                        });
                    }
                    break;
                case TaskCard.EnemyType:
                    if (!flag2)
                    {
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Enemy Type".FromDictionary() + "</color>\n";
                        text = text + taskInstance.GetOpposingFaction().GetUnitType().FromDictionary() + "\n";
                    }
                    break;
                case TaskCard.LateDelivery:
                    if (flag)
                    {
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Late Completion".FromDictionary() + "</color>\n";
                        text = string.Concat(new string[]
                        {
                    text,
                    "<b><color=",
                    black,
                    ">",
                    lateDeliveryRemainDays.ToString(),
                    "</color>/",
                    num4.ToString(),
                    "</b> "+"days".FromDictionary()+"\n"
                        });
                    }
                    break;
                case TaskCard.Passengers:
                    if (taskInstance.passenger == null && taskInstance.IsActive() && taskInstance.GetStatus() != TaskStatus.Ready && (type == MercenaryTaskType.Defend || type == MercenaryTaskType.Conquer))
                    {
                        int reinforcementTarget = taskInstance.mercenaryTask.GetReinforcementTarget(taskInstance.GetLevel());
                        int num5 = ((taskInstance.GetStatus() == TaskStatus.Assigned) ? 0 : reinforcementTarget);
                        string text3 = ((reinforcementTarget <= num5) ? VisualTweak.Green : VisualTweak.Red);
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Reinforcements".FromDictionary() + "</color>\n";
                        text = string.Concat(new string[]
                        {
                    text,
                    "<b><color=",
                    text3,
                    ">",
                    num5.ToString(),
                    "</color>/",
                    reinforcementTarget.ToString(),
                    "</b>\n"
                        });
                    }
                    else if (taskInstance.passenger != null && taskInstance.IsActive() && type == MercenaryTaskType.Capture)
                    {
                        int passengersTarget = taskInstance.passengersTarget;
                        int passengers = taskInstance.passenger.GetPassengers();
                        string text4 = ((passengersTarget <= passengers) ? VisualTweak.Green : VisualTweak.Red);
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Capture".FromDictionary() + "</color>\n";
                        text += (Game.IsTest() ? ((taskInstance.passenger.taskInstance != null) ? taskInstance.passenger.taskInstance.GetUID() : "MISSING TI") : string.Concat(new string[]
                        {
                    "<b><color=",
                    text4,
                    ">",
                    passengers.ToString(),
                    "</color>/",
                    passengersTarget.ToString(),
                    "</b>\n"
                        }));
                    }
                    break;
                case TaskCard.Opponent:
                    if (flag2)
                    {
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Adversary".FromDictionary() + "</color>\n";
                        text = text + taskInstance.GetOpposingFactionForRep().GetName(false) + "\n";
                    }
                    break;
                case TaskCard.Risk:
                    {
                        bool flag3 = taskInstance.HasContraband();
                        if (taskInstance.GetOpposingFaction() != null && taskInstance.GetOpposingFaction().IsImpervious())
                        {
                            text = text + "<color=" + VisualTweak.Gold + ">" + "Risk".FromDictionary() + "</color>\n";
                            text += "Impervious Enemy".FromDictionary()+"\n";
                        }
                        else if (flag3)
                        {
                            text = text + "<color=" + VisualTweak.Gold + ">" + "Risk".FromDictionary() + "</color>\n";
                            text += "Contraband".FromDictionary() + "\n";
                        }
                        break;
                    }
                case TaskCard.Strength:
                    if (taskInstance.csDefRange != null)
                    {
                        string text5 = "";
                        int factionBottomIconIndex = TaskTweak.GetFactionBottomIconIndex(taskInstance, out text5, false);
                        text = text + "<color=" + VisualTweak.Gold + ">" + "Difficulty".FromDictionary() + "</color>\n";
                        text = string.Concat(new string[]
                        {
                    text,
                    "<b><color=",
                    text5,
                    "><sprite=\"icon_collector\" index=",
                    factionBottomIconIndex.ToString(),
                    "> ~",
                    taskInstance.csDefRange.GetMiddleInt().ToString(),
                    "</color></b>\n"
                        });
                    }
                    break;
                case TaskCard.Penalty:
                    if (taskInstance.IsActive())
                    {
                        int actualPenalty = taskInstance.GetActualPenalty();
                        if (actualPenalty > 0)
                        {
                            text = text + "<color=" + VisualTweak.Gold + ">" + "Penalties".FromDictionary() + "</color>\n";
                            text = text + BookUI.game.caravan.FormatMoney(actualPenalty, false, false, 3, "") + "\n";
                        }
                    }
                    break;
                case TaskCard.Time:
                    text = text + "<color=" + VisualTweak.Gold + ">" + "Time left".FromDictionary() + "</color>\n";
                    text = string.Concat(new string[]
                    {
                text,
                "<b><color=",
                black,
                ">",
                num3.ToString(),
                "</color>/",
                initialExpireDays.ToString(),
                "</b> "+"days".FromDictionary()+"\n"
                    });
                    break;
            }
            if (text.Length == 0 && taskInstance.passenger != null && taskInstance.passenger.GetPassengers() > 0)
            {
                MethodInfo methodInfo = typeof(BookUI).GetMethod("GetPassengerCardDescription", BindingFlags.NonPublic | BindingFlags.Instance);
                var parameters = new object[] { card, taskInstance };
                text = (string)methodInfo.Invoke(__instance, parameters);
            }
            __result = text;
        }

        [HarmonyPatch("GetPassengerCardDescription")]
        [HarmonyPostfix]
        public static void GetPassengerCardDescription_Postfix(BookUI __instance, TaskInstance taskInstance, TaskCard card, ref string __result)
        {
            string text = "";
            float workforceFloat = taskInstance.passenger.GetWorkforceFloat();
            float workforceNeedFloat = taskInstance.passenger.GetWorkforceNeedFloat();
            switch (card)
            {
                case TaskCard.Passengers:
                    {
                        int passengers = taskInstance.passenger.GetPassengers();
                        text = text + "<color=" + VisualTweak.Gold + ">"+"Passengers".FromDictionary()+"</color>\n";
                        text += passengers;
                        break;
                    }
                case TaskCard.Fighters:
                    {
                        int fighters = taskInstance.passenger.GetFighters();
                        text = text + "<color=" + VisualTweak.Gold + ">"+"Fighters".FromDictionary()+"</color>\n";
                        text += fighters;
                        break;
                    }
                case TaskCard.Consumption:
                    {
                        int consumption = taskInstance.passenger.GetConsumption();
                        text = text + "<color=" + VisualTweak.Gold + ">"+"Consumption".FromDictionary()+"</color>\n";
                        text += consumption;
                        break;
                    }
                case TaskCard.Workforce:
                    if (workforceFloat > 0f || workforceFloat == workforceNeedFloat)
                    {
                        text = text + "<color=" + VisualTweak.Gold + ">"+"Workforce".FromDictionary()+"</color>\n";
                        text += workforceFloat;
                    }
                    break;
                case TaskCard.WorkforceNeed:
                    if (workforceNeedFloat > 0f)
                    {
                        text = text + "<color=" + VisualTweak.Gold + ">"+"Workforce need".FromDictionary()+"</color>\n";
                        text += workforceNeedFloat;
                    }
                    break;
                case TaskCard.Time:
                    if (taskInstance.IsActive() && taskInstance.GetInitialExpireDays() > 0)
                    {
                        int expireDays = taskInstance.GetExpireDays();
                        int initialExpireDays = taskInstance.GetInitialExpireDays();
                        text = text + "<color=" + VisualTweak.Gold + ">"+"Time left".FromDictionary()+"</color>\n";
                        string text2 = (((float)expireDays / (float)initialExpireDays > 0.5f) ? VisualTweak.Green : (((float)expireDays / (float)initialExpireDays < 0.15f) ? VisualTweak.Red : VisualTweak.Orange));
                        text = text + "<b><color=" + text2 + ">" + expireDays + "</color>/" + initialExpireDays + "</b> "+"days".FromDictionary()+"\n";
                    }
                    break;
            }
            __result = text;
        }

        //[HarmonyPatch("CreateRightDivider")]
        //[HarmonyPostfix]
        //public static void CreateRightDivider_Postfix(BookUI __instance, GameObject ___rightHolderContent, GameObject ___dividerPrefab, ref float ___rightVOffset,string title,TaskInstance taskInstance = null)
        //{
        //    if (taskInstance != null)
        //    {
        //        ___rightVOffset += 35f;
        //    }
        //    GameObject obj = UnityEngine.Object.Instantiate(___dividerPrefab);
        //    obj.transform.SetParent(___rightHolderContent.transform);
        //    obj.transform.localPosition = new Vector3(0f, 0f - ___rightVOffset, 0f);
        //    obj.transform.localScale = Vector3.one;
        //    obj.transform.Find("Holder/Title").GetComponent<TextMeshProUGUI>().text = title;
        //    Image component = obj.transform.Find("Holder/FactionLeft").GetComponent<Image>();
        //    Image component2 = obj.transform.Find("Holder/FactionRight").GetComponent<Image>();
        //    if (taskInstance != null)
        //    {
        //        component.gameObject.SetActive(value: true);
        //        component2.gameObject.SetActive(value: true);
        //        Faction faction = taskInstance.GetFaction();
        //        Faction opposingFaction = taskInstance.GetOpposingFaction();
        //        component.sprite = null;
        //        component2.sprite = null;
        //        component.gameObject.SetActive(value: false);
        //        component2.gameObject.SetActive(value: false);
        //        faction?.iconLoader.SetAssetAsync(component, () => !Game.game.caravan.IsOpenJournalUI());
        //        opposingFaction?.iconLoader.SetAssetAsync(component2, () => !Game.game.caravan.IsOpenJournalUI());
        //    }
        //    else
        //    {
        //        component.gameObject.SetActive(value: false);
        //        component2.gameObject.SetActive(value: false);
        //    }
        //    obj.transform.Find("Holder/ButtonPlus").gameObject.SetActive(value: false);
        //    obj.transform.Find("Holder/ButtonMinus").gameObject.SetActive(value: false);
        //    obj.transform.Find("Holder/ButtonEditNote").gameObject.SetActive(value: false);
        //    ___rightVOffset += 95f;
        //}

        [HarmonyPatch("GetTaskHeader")]
        [HarmonyPostfix]
        public static void GetTaskHeader_PostFix(BookUI __instance, ref string __result, TaskInstance taskInstance)
        {
            int level = taskInstance.GetLevel();
            string text = "";
            text = ((level != 0) ? String.ToRoman(level) : "<sprite=\"icon_collector\" index=12>");
            string text2 = "";
            if (!taskInstance.IsActive())
            {
                text2 = (taskInstance.IsSuccess() ? (" - " + Game.FromDictionary("Completed")) : (" - " + Game.FromDictionary("Failed")));
            }
            else if (Game.IsTest())
            {
                text2 = " <color=" + VisualTweak.Red + ">(" + taskInstance.GetStatus().ToString() + ")</color>";
            }
            __result = Game.FromDictionary("Level") + " " + text + " " + ( taskInstance.GetTaskType().ToString() + " Task" ).FromDictionary() + text2;
        }

        [HarmonyPatch("CreateRightTextBlock")]
        [HarmonyPostfix]
        public static void CreateRightTextBlock_PostFix(BookUI __instance, ref BookTextBlock __result)
        {
            FontUtils.Update(__result.tm,null, "BookUI=>CreateRightTextBlock");
        }

        [HarmonyPatch("AddEntry")]
        [HarmonyPostfix]
        public static void AddEntry_PostFix(BookUI __instance, BookEntry bookEntry)
        {
            FontUtils.Update(bookEntry.titleLeft, null, "BookUI=>AddEntry");
            FontUtils.Update(bookEntry.titleRight, null, "BookUI=>AddEntry");
        }

        [HarmonyPatch("ClickEntry")]
        [HarmonyPatch(new Type[] { typeof(BookEntry) })]
        [HarmonyPostfix]
        public static void ClickEntry_PostFix(BookUI __instance, TextMeshProUGUI ___rightText)
        {
            FontUtils.Update(___rightText, null, "BookUI=>ClickEntry");
        }

        [HarmonyPatch("GetContractTime")]
        [HarmonyPostfix]
        public static void GetContractTime_PostFix(TaskInstance taskInstance, DeliverTime deliverTime, ref string __result)
        {
            string text = "";
            int deliverDays = TaskTweak.GetDeliverDays(deliverTime, taskInstance.GetInitialExpireDays());
            __result = text + "<b>" + (deliverTime == DeliverTime.Failed ? "+" : "") + "</b> " + deliverDays.FormatNumberByNomen("day");
        }

        [HarmonyPatch("GetContractMissingCargo")]
        [HarmonyPostfix]
        public static void GetContractMissingCargo_PostFix(TaskInstance taskInstance, DeliverTime deliverTime,ref string __result)
        {
            string text = "";
            int totalPenalty = taskInstance.GetTotalPenalty();
            if (deliverTime - DeliverTime.Fast > 2)
            {
                if (deliverTime == DeliverTime.Failed)
                {
                    text += BookUI.game.caravan.FormatMoneyLyrgBross(totalPenalty, false, false, 2);
                }
            }
            else
            {
                text = text + taskInstance.GetPenalties() + "/missing unit".FromDictionary();
            }
            __result = text;
        }

        [HarmonyPatch("GetJournalRegions")]
        [HarmonyPostfix]
        public static void GetJournalRegions(ref string __result, Journal journal)
        {
            string text = "";
            List<Region> regions = journal.GetRegions();
            if (regions.Count == 0)
            {
                __result = text;
                return;
            }
            foreach (Region item in regions)
            {
                if (text.Length > 0)
                {
                    text += ", ";
                }
                text = text + "<link=\"" + item.GetName() + "\"><u>" + item.GetName() + "</u></link>";
            }
            __result= "\n\n"+"Regions".FromDictionary()+": " + text;
        }

        [HarmonyPatch("UpdateRumorArchiveButton")]
        [HarmonyPostfix]
        public static void UpdateRumorArchiveButton(ButtonUI ___buttonArchive, Rumor rumor)
        {
            string title = (rumor.IsArchived() ? "Unarchive".FromDictionary() : "Archive".FromDictionary());
            ___buttonArchive.SetTitle(title);
        }


        [HarmonyPatch("UpdateCardOffset")]
        [HarmonyPrefix]
        public static bool UpdateCardOffset(TaskCard card, ref float ___rightVOffset, ref float ___rightHOffset)
        {
            ___rightHOffset += 500f;
            if (___rightHOffset > 500f)
            {
                ___rightHOffset = 0f;
                ___rightVOffset += 130f;
            }
            return false;
        }
    }
}