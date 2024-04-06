using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(PropertyStatus))]
    internal class PropertyStatusPatches
    {

        [HarmonyPatch(nameof(PropertyStatus.FormatDuration))]
        [HarmonyPostfix]
        public static void FormatDuration_Postfix(PropertyStatus __instance, ref string __result, bool timeicon = false)
        {
            string text = "";
            if (timeicon)
            {
                text += "<sprite=\"icon_collector\" index=40>";
            }
            if (__instance.expires == 0)
            {
                __result = text + "(" + "Persistent".FromDictionary() + ")";
                return;
            }
            int num = __instance.expires - Game.game.calendar.GetDayStamp();
            __result = text + "<b>" + num.FormatNumberByNomen("day") + "</b>";
        }
    }
}