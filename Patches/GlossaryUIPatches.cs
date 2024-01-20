using HarmonyLib;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(GlossaryUI))]
    internal class GlossaryUIPatches
    {
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        public static void Start_Postfix(GlossaryUI __instance)
        {
            var gameObject = __instance.gameObject;
            if (gameObject != null && !gameObject.HasComponent<UIFontUpdater>())
            {
                gameObject.AddComponent<UIFontUpdater>();
            }
        }
    }
}