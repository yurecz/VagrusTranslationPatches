using HarmonyLib;
using TMPro;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch()]
    internal class CampPatches
    {

        [HarmonyPatch(typeof(Camp), "DefenceMansioText")]
        [HarmonyPostfix]
        public static void DefenceMansioText_Postfix(Camp __instance, ref string __result, int defence)
        {
            if (!__instance.IsSettlementCamp())
            {
                __result = "";
                return;
            }
            string result = "";
            int defenceCost = __instance.GetDefenceCost(defence);
            int restLevel = Game.game.caravan.curNode.GetRestLevel();
            if (__instance.campMode == CampMode.Settlement && restLevel < defence)
            {
                result = "\n\n<i><color=" + Tooltip.red + ">"+"Not an available option in this Mansio".FromDictionary()+"</color></i>";
            }
            else if (Game.game.caravan.GetMoney() < defenceCost)
            {
                result = "\n\n<i><color=" + Tooltip.red + ">"+"Not enough coins".FromDictionary()+".</color></i>";
            }
            __result = result;
        }

        [HarmonyPatch(typeof(Camp), "DefenceOrderVirtualImpact")]
        [HarmonyPrefix]
        public static bool DefenceOrderVirtualImpact(Camp __instance, int defmode, bool show, out string guardposting, out string vigor)
        {
            guardposting = "";
            vigor = ((defmode == 2) ? "No bonuses; No penalties".FromDictionary() : "");
            if (__instance.IsSettlementCamp())
            {
                return false;
            }
            int unDefendedLevel = TweakFunctions.GetUnDefendedLevel(false);
            bool flag = TweakFunctions.IsGeneralCampEnabled(defmode, false);
            bool flag2 = TweakFunctions.IsGeneralCampEnabled(defmode, true);
            if (defmode != 1)
            {
                if (defmode - 2 > 1)
                {
                    return false;
                }
                if (flag2 && !flag)
                {
                    PropQty propQty = new PropQty(Prop.Vigor, -GeneralSettings.GeneralCampVigorCost);
                    if (show)
                    {
                        guardposting = "<b>" + ("Post " + ((defmode == 3) ? "extra " : "") + "guards (insufficient armed crew)").FromDictionary() + "</b>";
                        vigor = global::String.Sign(propQty.qty, true) + " " + BaseUI.game.caravan.FindProperty(propQty.prop).GetTitle(false);
                        return false;
                    }
                    Camp.game.caravan.AddProperty(propQty.prop, propQty.qty, false);
                    return false;
                }
                else if (flag)
                {
                    if (show)
                    {
                        guardposting = "<b>" + ("Post " + ((defmode == 3) ? "extra " : "") + "guards (sufficient armed crew)").FromDictionary() + "</b>";
                        return false;
                    }
                    return false;
                }
                else
                {
                    if (show)
                    {
                        guardposting = "<b>" + ("Post " + ((defmode == 3) ? "extra " : "") + "guards").FromDictionary() + "</b>";
                        return false;
                    }
                    return false;
                }
            }
            else if (unDefendedLevel == 3)
            {
                if (show)
                {
                    guardposting = "<b>" + "Unable to post guards".FromDictionary() + "</b>";
                    return false;
                }
                return false;
            }
            else if (unDefendedLevel <= 1)
            {
                PropQty propQty2 = new PropQty(Prop.Vigor, GeneralSettings.GeneralCampVigorCost);
                if (show)
                {
                    guardposting = "<b>" + "Post no guards".FromDictionary() + "</b>";
                    vigor = global::String.Sign(propQty2.qty, true) + " " + BaseUI.game.caravan.FindProperty(propQty2.prop).GetTitle(false);
                    return false;
                }
                Camp.game.caravan.AddProperty(propQty2.prop, propQty2.qty, false);
                return false;
            }
            else
            {
                if (show)
                {
                    guardposting = "<b>" + "Post no guards".FromDictionary() + "</b>";
                    return false;
                }
                return false;
            }
        }
    }
}