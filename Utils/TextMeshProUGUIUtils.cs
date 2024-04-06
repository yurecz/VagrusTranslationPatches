using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using VagrusTranslationPatches.PrefabUpdater;

namespace VagrusTranslationPatches.Utils
{
    public static  class TextMeshProUGUIUtils
    {
        public static TextMeshProUGUI AddInfo(this TextMeshProUGUI component)
        {
            var fontInfo = component.gameObject.AddComponent<GameTextInfo>();
            //fontInfo()
            return component;
        }
    }
}
