using HarmonyLib;
using Vagrus.Data;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(CrewCombatOpenUI))]
    internal class CrewCombatOpenUIPatches
    {
        [HarmonyPatch(nameof(CrewCombatOpenUI.FormatWardedLoot))]
        [HarmonyPostfix]
        public static void FormatWardedLoot_Postfix(CrewCombatOpenUI __instance, ref string __result, CrewCombatAttribute _attribute, string separator = ", ")
        {
            var caravan = Game.game.caravan;
            string src = "";
            string iD = _attribute.GetID();
            bool flag = iD == "reciE4qAEIvLJ2Gsp";
            if (caravan.GetProperty(Prop.Beast) > 0 && (flag || iD == "recl9BxYioeONTzHf"))
            {
                String.SeparateList(ref src, caravan.GetProperty(Prop.Beast).FormatNumberByNomen("beast of burden"), separator);
            }
            if (caravan.GetProperty(Prop.Mount) > 0 && (flag || iD == "recl9BxYioeONTzHf"))
            {
                String.SeparateList(ref src, caravan.GetProperty(Prop.Mount).FormatNumberByNomen("mount"), separator);
            }
            if (caravan.GetProperty(Prop.Crew) > 0 && (flag || iD == "recKPa74oTqLA9jhi"))
            {
                String.SeparateList(ref src, caravan.GetProperty(Prop.Crew).FormatNumberByNomen("comes"), separator);
            }
            if (caravan.GetProperty(Prop.CrewArmedCompanion) > 0 && iD == "recQUQbVQYrnucDLo")
            {
                String.SeparateList(ref src, caravan.GetProperty(Prop.CrewArmedCompanion).FormatNumberByNomen("fighting crew"), separator);
            }
            if (flag || iD == "receNoDsWky3xhOv7")
            {
                foreach (GoodsQty goodsQty in caravan.cargo.GetGoodsQtyList())
                {
                    if (goodsQty != null && goodsQty.goods != null && goodsQty.qty != 0)
                    {
                        String.SeparateList(ref src, goodsQty.qty + " " + goodsQty.goods.GetName(), separator);
                    }
                }
            }
            if (src.Length == 0)
            {
                src = "Nothing".FromDictionary();
            }
            __result = src;
        }
        [HarmonyPatch(nameof(CrewCombatOpenUI.FormatPotentialLoot))]
        [HarmonyPostfix]
        public static void FormatPotentialLoot(CrewCombatOpenUI __instance, ref string __result, CrewCombatAttribute attribute, CrewCombat ___crewCombat, string separator = ", ")
        {
            var caravan = Game.game.caravan;
            var crewCombat = ___crewCombat;
            string src = "";
            EnemyProp enemyProp = ___crewCombat.enemyProp;
            if (attribute == AirTableItem<CrewCombatAttribute>.FindByUID("recIylqNd2SVjjoJb"))
            {
                String.SeparateList(ref src, enemyProp.crew.FormatNumberByNomen("foe"), separator);
            }
            if (enemyProp.beasts > 0 && crewCombat.CanLoot(attribute, Prop.Beast))
            {
                String.SeparateList(ref src, enemyProp.beasts.FormatNumberByNomen("beast of burden"), separator);
            }
            if (enemyProp.mounts > 0 && crewCombat.CanLoot(attribute, Prop.Mount))
            {
                String.SeparateList(ref src, enemyProp.mounts.FormatNumberByNomen("mount"), separator);
            }
            if (enemyProp.crew > 0 && crewCombat.CanLoot(attribute, Prop.Crew))
            {
                String.SeparateList(ref src, enemyProp.crew.FormatNumberByNomen("foe"), separator);
            }
            if (enemyProp.slaves > 0 && crewCombat.CanLoot(attribute, Prop.Slave))
            {
                String.SeparateList(ref src, enemyProp.slaves.FormatNumberByNomen("slave"), separator);
            }
            foreach (GoodsQty cargoQty in enemyProp.GetCargoQtyList())
            {
                if (cargoQty.goods.IsSupply() && crewCombat.CanLoot(attribute, Prop.Supply))
                {
                    String.SeparateList(ref src, cargoQty.qty + " " + cargoQty.goods.GetName(), separator);
                }
            }
            foreach (GoodsQty cargoQty2 in enemyProp.GetCargoQtyList())
            {
                if (!cargoQty2.goods.IsSupply() && crewCombat.CanLoot(attribute, Prop.Cargo))
                {
                    String.SeparateList(ref src, cargoQty2.qty + " " + cargoQty2.goods.GetName(), separator);
                }
            }
            if (src.Length == 0)
            {
                src = "Nothing".FromDictionary();
            }
            __result = src;
        }
    }
}