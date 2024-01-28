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
    public static class NodeUtils
    {
        public static int Profit(this Node target, Node source, Goods goods)
        {
            LastMarket targetLastMarket = target ? target.FindLastMarket(goods.GetID()) : null;
            LastMarket sourceLastMarket = source ? source.FindLastMarket(goods.GetID()) : null;
            if (targetLastMarket == null || sourceLastMarket == null)
            {
                return 0;
            } else
                return targetLastMarket.sellPrice - sourceLastMarket.buyPrice;

        }
    }
}
