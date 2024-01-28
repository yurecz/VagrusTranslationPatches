using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vagrus;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.MonoBehaviours
{
    public class UIObjectTranslator : MonoBehaviour
    {
        [Serializable]
        public class KeyUIPair
        {
            public bool noScaling;

            public string originalText;

            public TextMeshProUGUI textMesh;

            [ReadOnly]
            public TMP_FontAsset firstFont;
        }

        private List<KeyUIPair> keyUIPairs = new List<KeyUIPair>();

        private void Awake()
        {
            Game.AddLanguageChangeListener(TranslateUIElements);
        }

        private void Start()
        {
            StartCoroutine(WaitThenFind());
            IEnumerator WaitThenFind()
            {
                yield return null;
                FindAllText();
                TranslateUIElements();
                Game.AddLanguageChangeListener(TranslateUIElements);

            }
        }

        private void FindAllText()
        {
            keyUIPairs.Clear();

            TranslationPatchesPlugin.Log.LogInfo("Registered UIObjectTranslator on " + base.gameObject.GetFullName());
            
            TextMeshProUGUI[] componentsInChildren = base.transform.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
            foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
            {
                    keyUIPairs.Add(new KeyUIPair
                    {
                        originalText = textMeshProUGUI.text,
                        textMesh = textMeshProUGUI,                       
                        firstFont = textMeshProUGUI.font,
                        noScaling = (textMeshProUGUI.GetComponent<ContentSizeFitter>() != null)
                    });
            }
        }

        public void TranslateUIElements()
        {
               foreach (KeyUIPair keyUIPair in keyUIPairs)
            {
                if (keyUIPair.textMesh == null)
                {
                    continue;
                }

                if (Game.Dictionary.TryGetValue(keyUIPair.originalText.ToLower().Trim('\n', ' '), out var value))
                {
                    keyUIPair.textMesh.text = (keyUIPair.noScaling ? value : Game.GetScaledLocalizedText(keyUIPair.textMesh, keyUIPair.originalText, value));
                    keyUIPair.textMesh.richText = true;
                }
                else
                {
                    keyUIPair.textMesh.text = keyUIPair.originalText;
                }
                FontUtils.Update(keyUIPair.textMesh, keyUIPair.firstFont, "UIObjectTranslator");
            }
        }

        private void OnDestroy()
        {
            Game.RemoveLanguageChangeListener(TranslateUIElements);
        }
    }
}
