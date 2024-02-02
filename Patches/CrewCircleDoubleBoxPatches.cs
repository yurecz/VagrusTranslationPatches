using HarmonyLib;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{
    [HarmonyPatch(typeof(CrewCircleDoubleBox))]
    internal class CrewCircleDoubleBoxPatches
    {
        [HarmonyPatch("FormatStrengthTooltip")]
        [HarmonyPostfix]
        public static void FormatStrengthTooltip_Postfix(CrewCircleDoubleBox __instance, ref string __result, CrewCombatGroup crewCombatGroup)
        {
            string text = "";
            int combatStrength = crewCombatGroup.GetCombatStrength();
            int combatDefense = crewCombatGroup.GetCombatDefense();
            if (crewCombatGroup.IsCharacter())
            {
                if (crewCombatGroup.side == CrewSide.Player)
                {
                    text = text + HeaderSizeTooltip("Combat Strength:".FromDictionary() + " " + combatStrength) + "\n";
                    var tmp = Game.game.caravan.GetPropertyTooltip(Prop.CompanionCS, showMax: true, onlyEffects: true).Trim();
                    if (tmp != "")
                    {
                        text = text + tmp + "\n\n";
                    }
                    text = text + HeaderSizeTooltip("Combat Defense:".FromDictionary() + " " + combatDefense) + "\n";
                    tmp = Game.game.caravan.GetPropertyTooltip(Prop.CompanionDef, showMax: true, onlyEffects: true).Trim();
                    if (tmp != "")
                    {
                        text = text + tmp + "\n\n";
                    }
                }
                else
                {
                    text = text + HeaderSizeTooltip("Combat Strength:".FromDictionary() + " " + combatStrength) + "\n";
                    text = text + HeaderSizeTooltip("Combat Defense:".FromDictionary() + " " + combatDefense) + "\n";
                    text += "\n";
                    text = text + ((!crewCombatGroup.IsElite()) ? "Units:" : (crewCombatGroup.IsLeader() ? "Leader Units:" : "Elite Units:")).FromDictionary() + "\n";
                    if (crewCombatGroup.IsElite())
                    {
                        text = text + HeaderSizeTooltip("Combat Strength:".FromDictionary() + " " + crewCombatGroup.gchar.GetCombatStrength(elite: true)) + "\n";
                        string text2 = Game.game.caravan.GetPropertyTooltip(Prop.EnemyEliteCS, showMax: true, onlyEffects: true).Trim();
                        if (text2.Length > 0)
                        {
                            text = text + text2 + "\n\n";
                        }
                        text = text + HeaderSizeTooltip("Combat Defense:".FromDictionary() + " " + crewCombatGroup.gchar.GetDefense(elite: true)) + "\n";
                        text2 = Game.game.caravan.GetPropertyTooltip(Prop.EnemyEliteDef, showMax: true, onlyEffects: true).Trim();
                        if (text2.Length > 0)
                        {
                            text = text + text2 + "\n\n";
                        }
                    }
                    else
                    {
                        text = text + HeaderSizeTooltip("Combat Strength:".FromDictionary() + " " + crewCombatGroup.gchar.GetCombatStrength()) + "\n";
                        string text2 = Game.game.caravan.GetPropertyTooltip(Prop.EnemyCS, showMax: true, onlyEffects: true).Trim();
                        if (text2.Length > 0)
                        {
                            text = text + text2 + "\n\n";
                        }
                        text = text + HeaderSizeTooltip("Combat Defense:".FromDictionary() + " " + crewCombatGroup.gchar.GetDefense()) + "\n";
                        text2 = Game.game.caravan.GetPropertyTooltip(Prop.EnemyDef, showMax: true, onlyEffects: true).Trim();
                        if (text2.Length > 0)
                        {
                            text = text + text2 + "\n\n";
                        }
                    }
                }
            }
            else if (crewCombatGroup.IsCrew())
            {
                text = text + HeaderSizeTooltip("Combat Strength:".FromDictionary() + " " + combatStrength) + "\n";
                text = text + HeaderSizeTooltip("Combat Defense:".FromDictionary() + " " + combatDefense) + "\n";
                text += "\n";
                text += "Units:".FromDictionary()+"\n";
                if (crewCombatGroup.side == CrewSide.Player)
                {
                    Prop prop = Property.ToCombatStrength(crewCombatGroup.prop);
                    if (prop != 0)
                    {
                        text = text + Game.game.caravan.GetPropertyTooltip(prop) + "\n\n";
                    }
                    Prop prop2 = Property.ToDefense(crewCombatGroup.prop);
                    if (prop2 != 0)
                    {
                        text = text + Game.game.caravan.GetPropertyTooltip(prop2) + "\n\n";
                    }
                }
                else if (crewCombatGroup.IsElite())
                {
                    text = text + Game.game.caravan.GetPropertyTooltip(Prop.EnemyEliteCS) + "\n\n";
                    text = text + Game.game.caravan.GetPropertyTooltip(Prop.EnemyEliteDef) + "\n\n";
                }
                else
                {
                    text = text + Game.game.caravan.GetPropertyTooltip(Prop.EnemyCS) + "\n\n";
                    text = text + Game.game.caravan.GetPropertyTooltip(Prop.EnemyDef) + "\n\n";
                }
            }
            __result = text;
        }

        private static string HeaderSizeTooltip(string str)
        {
            return "<b><size=" + VisualTweak.TooltipHeaderSize + "%>" + str + "</size></b>";
        }
    }
    
}