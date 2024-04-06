using Newtonsoft.Json.Linq;
using System.Reflection;
using UnityEngine;

namespace VagrusTranslationPatches.Utils
{
    public static class PriceHistoryColumnPatches
    {

        public static Node GetSettlement(this PriceHistoryColumn instance)
        {
            FieldInfo fieldInfo = typeof(PriceHistoryColumn).GetField("node", BindingFlags.NonPublic | BindingFlags.Instance);
            return (Node)fieldInfo.GetValue(instance);
        }
    }
}
