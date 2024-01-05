using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using Vagrus;

namespace VagrusTranslationPatches.Patches
{
    internal class Translators
    {

        public static Dictionary<TextMeshProUGUI, UIElementTranslator> translators = new Dictionary<TextMeshProUGUI, UIElementTranslator>();
        
        public static Boolean InTranslation = false;

        public Translators()
        {

            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);

        }

        private void OnTextChanged(UnityEngine.Object obj)
        {
            if (translators.TryGetValue((TextMeshProUGUI)obj, out var translator))
            {
                if (!InTranslation)
                {
                    InTranslation = true;
                    var KeyText = translator.KeyText;
                    var NoScaling = translator.NoScaling;
                    var IsTextTemplate = translator.IsTextTemplate;
                    var textMesh = translator.textMesh;
                    string value;
                    if (IsTextTemplate)
                    {
                        TextTemplate textTemplate = TextTemplate.FindByName(KeyText);
                        textMesh.text = (NoScaling ? textTemplate.GetText() : Game.GetScaledLocalizedText(textMesh, textTemplate.text, textTemplate.GetText()));
                        textMesh.richText = true;
                    }
                    else if (Game.Dictionary.TryGetValue(KeyText.ToLower().Trim('\n', ' '), out value))
                    {
                        textMesh.text = (NoScaling ? value : Game.GetScaledLocalizedText(textMesh, KeyText, value));
                        textMesh.richText = true;
                    }
                    else
                    {
                        textMesh.text = KeyText;
                    }

                    InTranslation = false;

                }

            }
        }
    }
}
