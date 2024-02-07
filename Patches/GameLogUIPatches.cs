using HarmonyLib;
using System;
using System.Reflection;

namespace VagrusTranslationPatches.Patches
{
    public static class GameLogUIPatches
    {
        public static EnhancedScrollController scroller(this GameLogUI instance)
        {
            Type myType = typeof(GameLogUI);
            FieldInfo privateFieldInfo = myType.GetField("scroller", BindingFlags.NonPublic | BindingFlags.Instance);

             return (EnhancedScrollController)privateFieldInfo.GetValue(instance);
        }
    }
}