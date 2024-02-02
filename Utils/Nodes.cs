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
    public static class NodesUtils
    {
        public static int MaxProfitTo(this List<Node> sources, Node target, Goods goods)
        {
            var maxProfit = int.MinValue;
            foreach (Node source in sources)
            {
                var profit = target.Profit(source, goods);
                if (maxProfit < profit)
                {
                    maxProfit = profit;
                }
            }

            return maxProfit;
        }

        public static int MaxProfitFrom(this List<Node> targets, Node source, Goods goods)
        {
            var maxProfit = int.MinValue;
            foreach (Node target in targets)
            {
                var profit = target.Profit(source, goods);
                if (maxProfit < profit)
                {
                    maxProfit = profit;
                }
            }

            return maxProfit;
        }
    }
}
