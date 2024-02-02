using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VagrusTranslationPatches.PriceHistory;

namespace VagrusTranslationPatches.Utils
{
    public static class PriceHistoryRows
    {
        public static int MaxProfit(this List<PriceHistoryRow> rows, Node target, Node source)
        {
            int maxProfit = int.MinValue;
            foreach (var row in rows)
            {
                var goods = row.GetGoods();
                var goodsResult = target.Profit(source, goods);
                if (goodsResult > maxProfit)
                {
                    maxProfit = goodsResult;
                }
            }

            //TranslationPatchesPlugin.Log.LogWarning($" profit from {source.GetName()} to {target.GetName()}:" + maxProfit);

            return maxProfit;
        }
    }
}
