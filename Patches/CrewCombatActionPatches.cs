using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CrewCombatAction))]
    internal class CrewCombatActionPatches
    {

        [HarmonyPatch(nameof(CrewCombatAction.FormatEntryTitle))]
        [HarmonyPostfix]
        public static void FormatEntryTitle_Postfix(CrewCombatAction __instance, ref string __result, ActionType ___type, bool additalic = false, bool showRFCost = false, int actionIdx = 0)
        {
            var type = ___type;
            string text = "";
            string text2 = (additalic ? "<i>" : "");
            string text3 = (additalic ? "</i>" : "");
            text = text + "<b>" + __instance.GetName() + "</b>";
            if (showRFCost)
            {
                __result = text + "  <sprite=\"icon_collector\" index=4>" + Game.game.crewCombat.GetActionRFCost(__instance, actionIdx);
                return;
            }
            if (__instance.GetMechanics() == ActionMechanics.CompCombat)
            {
                __result = text + " - " + text2 + Game.FromDictionary("Companion Combat") + text3;
                return;
            }
            __result = text + " - " + type.ToString().FromDictionary();
            return;
        }
    }
}