using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace VagrusTranslationPatches.Utils
{
    public static class PriceHistoryRowUtils
    {
        public static Goods GetGoods(this PriceHistoryRow priceHistoryRow)
        {

            FieldInfo field = typeof(PriceHistoryRow).GetField("goods", BindingFlags.NonPublic | BindingFlags.Instance);
            return (Goods)field.GetValue(priceHistoryRow);
        }
    }
}
