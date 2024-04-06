using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Vagrus;
using Vagrus.UI;
using VagrusTranslationPatches.Utils;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(EventUI))]
    internal class EventUIPatch
    {

        //[HarmonyTranspiler]
        //[HarmonyPatch("Awake")]
        //[HarmonyPostfix]
        //static void Awake_Postfix(EventUI __instance)
        //{
        //    __instance.instance.AddOnceRecursiveComponent<UIFontUpdater>();
        //    var eventChoice = Resources.Load<EventChoiceButton>("Event/Prefab/EventChoiceButton");
        //    eventChoice.gameObject.AddOnceRecursiveComponent<UIFontUpdater>();
        //}

        //[HarmonyTranspiler]
        //[HarmonyPatch("GetDependencyIcons")]
        //static IEnumerable<CodeInstruction> GetDependencyIconsTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
        //{
        //    foreach (var instruction in instructions)
        //    {
        //        if (instruction.opcode == OpCodes.Ldstr && (string)instruction.operand == " required")

        //            yield return new CodeInstruction(OpCodes.Ldstr, " требуется");
        //        else
        //            yield return instruction;
        //    }
        //}

        //[HarmonyTranspiler]
        //[HarmonyPatch("SelectStep")]
        //static IEnumerable<CodeInstruction> SelectStepTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
        //{
        //    foreach (var instruction in instructions)
        //    {
        //        if (instruction.opcode == OpCodes.Ldstr && (string)instruction.operand == " or ")

        //            yield return new CodeInstruction(OpCodes.Ldstr, " или ");
        //        else
        //            yield return instruction;
        //    }
        //}

        [HarmonyPatch("GetChoiceActionIcons")]
        [HarmonyPrefix]
        public static bool GetChoiceActionIcons_Prefix(EventStep step, ChoiceAction action, List<ChoiceIcon> ___choiceIcons, ref int ___choicelinkidx, ref Tooltip ___tooltip, out int start, out int end)
        {
            var choiceIcons = ___choiceIcons;
            var choicelinkidx = ___choicelinkidx;
            var tooltip = ___tooltip;
            start = choiceIcons.Count;
            foreach (PropertyAction property in action.properties)
            {
                if (property.marketBlock == null || property.marketBlock.mode == PriceMode.None || !property.property || (property.hide && !Game.IsTest()))
                {
                    continue;
                }

                Crew crew = Crew.FindByProperty(property.property.prop);
                if (!(!crew))
                {
                    var flag = property.marketBlock.qty > 0;
                    choicelinkidx++;
                    string subject = Tooltip.GetDescription(flag ? "BuyAction" : "SellAction");
                    int marketPrice = crew.GetMarketPrice(property.marketBlock);
                    int replace = Mathf.Abs(property.marketBlock.qty);
                    int priceRate = property.marketBlock.priceRate;
                    var assessment = flag ? EventsUIUtils.GetEvaluateBuyPrice(priceRate) : EventsUIUtils.GetEvaluateSellPrice(priceRate);
                    String.Replace(ref subject, "%qty%", replace);
                    String.Replace(ref subject, "%name%", property.property.GetTitle());
                    String.Replace(ref subject, "%stack%", "");
                    String.Replace(ref subject, "%money%", Game.game.caravan.FormatMoney(marketPrice));
                    String.Replace(ref subject, "%actqty%", Game.game.caravan.GetProperty(property.property.prop));
                    String.Replace(ref subject, "%guestimate%", assessment);
                    String.Replace(ref subject, "%cargospace%", "");
                    EventUI.FormatTooltipAfterDays(ref subject, property.afterdays);
                    ChoiceIcon choiceIcon = new ChoiceIcon(EventSprite.CrewIcon, "actlink_" + choicelinkidx);
                    choiceIcons.Add(choiceIcon);
                    tooltip.AddLink("event", choiceIcon.linkid, subject);
                }
            }

            end = choiceIcons.Count;

            return false;
        }
    }
}
