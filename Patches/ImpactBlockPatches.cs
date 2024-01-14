using HarmonyLib;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(ImpactBlock))]
    internal class ImpactBlockPatches
    {
        [HarmonyPatch("GetImpactPropertyName")]
        [HarmonyPostfix]
        public static void GetImpactPropertyName_Postfix(ImpactBlock __instance, ref string __result)
        {
            __result = Property.FindByName(__instance.prop.ToString()).GetTitle();
        }
    }
}