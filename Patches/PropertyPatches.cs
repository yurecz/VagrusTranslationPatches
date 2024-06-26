﻿using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Security.Cryptography;
using UnityEngine;
using VagrusTranslationPatches.Utils;
using static UnityEngine.GraphicsBuffer;

namespace VagrusTranslationPatches.Patches
{
    // TODO Review this file and update to your own requirements.

    [HarmonyPatch(typeof(Property))]
    internal class PropertyPatches
    {
        [HarmonyPatch("FormatDescriptionByProperty")]
        [HarmonyPostfix]
        public static void FormatDescriptionByProperty_Postfix(Property __instance, ref string __result)
        {
            Caravan caravan = Game.game.caravan;
            var prop = __instance.prop;
            string cargoCapacity = (Property.ToCargo(prop) != 0) ? caravan.GetProperty(Property.ToCargo(prop)).ToString() : "";
            var template = __instance.GetDescriptionTemplate();
            switch (prop)
            {
                case Prop.Mount:
                    {
                        Crew crew3 = Crew.FindByProperty(Prop.Mount);
                        Crew crew4 = Crew.FindByProperty(Prop.Outrider);
                        if ((bool)crew3 || (bool)crew4)
                        {
                            template = template.Replace("%cargo%", cargoCapacity);
                            __result = template.Replace("%type%", caravan.GetBeastType().ToString().FromDictionary());
                        }
                        break;
                    }
                case Prop.Beast:
                    if ((bool)Crew.FindByProperty(Prop.Beast))
                    {
                        template = template.Replace("%cargo%", cargoCapacity);
                        __result = template.Replace("%type%", caravan.GetBeastType().ToString().FromDictionary());
                    }
                    break;
                case Prop.Money:
                    {
                        caravan.MoneyToCurrency(out var changer, out var lyrg, out var bross);
                        __result = bross.FormatNumberByNomen("bross") + " " + lyrg.FormatNumberByNomen("lyrg") + " " + changer.FormatNumberByNomen("changer");
                        break;
                    }

            }
        }

        [HarmonyPatch("GetStatusTooltip")]
        [HarmonyPostfix]
        public static void  GetStatusTooltip_postfix(Property __instance, ref string __result)
        {
            string text = "<b><size=" + VisualTweak.TooltipHeaderSize + "%>" + __instance.GetTitle() + "</size></b> (" + __instance.target.ToString().FromDictionary() + ")";

            __result = text + "\n" + __result.RemoveFirstLineFromString();
        }
    }
}
