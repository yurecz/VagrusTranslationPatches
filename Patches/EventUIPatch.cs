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

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(EventUI))]
    internal class EventUIPatch
    {

        [HarmonyTranspiler]
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        static void Awake_Postfix(EventUI __instance)
        {
            __instance.instance.AddOnceRecursiveComponent<UIFontUpdater>();
            var eventChoice = Resources.Load<EventChoiceButton>("Event/Prefab/EventChoiceButton");
            eventChoice.gameObject.AddOnceRecursiveComponent<UIFontUpdater>();
        }

        [HarmonyTranspiler]
        [HarmonyPatch("GetDependencyIcons")]
        static IEnumerable<CodeInstruction> GetDependencyIconsTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldstr && (string)instruction.operand == " required")

                    yield return new CodeInstruction(OpCodes.Ldstr, " требуется");
                else
                    yield return instruction;
            }
        }

        [HarmonyTranspiler]
        [HarmonyPatch("SelectStep")]
        static IEnumerable<CodeInstruction> SelectStepTranspiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Ldstr && (string)instruction.operand == " or ")

                    yield return new CodeInstruction(OpCodes.Ldstr, " или ");
                else
                    yield return instruction;
            }
        }
    }
}
