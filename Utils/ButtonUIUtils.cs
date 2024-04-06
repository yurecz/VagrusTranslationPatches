using System;
using System.Reflection;
using UnityEngine;

namespace VagrusTranslationPatches.Utils
{
    public static class ButtonUIUtils
    {
        public static void SetHoverEnabled(this ButtonUI instance, bool value)
        {
            Type classType = typeof(ButtonUI);
            FieldInfo fieldInfo = classType.GetField("hoverEnabled", BindingFlags.NonPublic | BindingFlags.Instance);
            fieldInfo.SetValue(instance, value);
        }
    }
}
