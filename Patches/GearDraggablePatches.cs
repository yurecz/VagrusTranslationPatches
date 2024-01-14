using HarmonyLib;
using Vagrus.UI;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(GearDraggable))]
    internal class GearDraggablePatches
    {
        [HarmonyPatch("GetGearText")]
        [HarmonyPostfix]
        public static void GetGearText_Postfix(ref string __result, UniGear ___gear)
        {
            if (___gear.mountChar != null)
            {
                __result = ___gear.GetName() + "\n<color=#a00>*" + "In use by".FromDictionary() + " " + ___gear.mountChar.GameName(false) + "</color>";
            }
        }
    }
}