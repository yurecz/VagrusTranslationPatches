using HarmonyLib;
using System.Runtime.InteropServices.WindowsRuntime;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(Passenger))]
    internal class PassengerPatches
    {
        [HarmonyPatch(nameof(Passenger.GetDeliverMessage))]
        [HarmonyPostfix]
        public static void GetDeliverMessage_Postfix(Passenger __instance, ref string __result)
        {
            if (__instance.HasDestination())
            {
                __result = Game.FormatText(string.Concat(new string[]
                    {
                        "Deliver to".FromDictionary(),
                        " *",
                        __instance.GetTargetName(),
                        "*: ",
                        __instance.GetDeliverName()
                    }),
                false, false);
                return;
            }
            __result = Game.FormatText("Travel with".FromDictionary() + " " + __instance.GetDeliverName() + " " + "until they are ready to depart".FromDictionary(), false, false);
        }
    }
}