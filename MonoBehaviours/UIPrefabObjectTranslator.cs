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
    public class UIPrefabObjectTranslator : MonoBehaviour
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

        public List<KeyUIPair> keyUIPairs = new List<KeyUIPair>();

        private void Awake()
        {
            Game.AddLanguageChangeListener(TranslateUIElements);
        }

        private void Start()
        {
            FindAllText();
        }
        private UIPrefabObjectTranslator FindAllText()
        {
            keyUIPairs.Clear();

            TranslationPatchesPlugin.Log.LogInfo("Registered UIPrefabObjectTranslator" + base.gameObject.GetFullName());
            
            TextMeshProUGUI[] componentsInChildren = base.transform.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
            foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
            {
                if (!textMeshProUGUI.gameObject.HasComponent<UIElementTranslator>())
                    keyUIPairs.Add(new KeyUIPair
                    {
                        originalText = textMeshProUGUI.text,
                        textMesh = textMeshProUGUI,                       
                        firstFont = textMeshProUGUI.font,
                        noScaling = (textMeshProUGUI.GetComponent<ContentSizeFitter>() != null)
                    });
            }

            return this;
        }

        public void TranslateUIElements()
        {
            foreach (KeyUIPair keyUIPair in keyUIPairs)
            {
                if (keyUIPair.textMesh == null)
                {
                    continue;
                }

                //keyUIPair.textMesh.text = (keyUIPair.noScaling ? value : Game.GetScaledLocalizedText(keyUIPair.textMesh, keyUIPair.originalText, value));
                keyUIPair.textMesh.text = keyUIPair.originalText.ToLower().Trim('\n', ' ').FromDictionary();
                keyUIPair.textMesh.richText = true;

                FontUtils.Update(keyUIPair.textMesh, keyUIPair.firstFont, "UIObjectTranslator");
            }
        }

        private void OnDestroy()
        {
            Game.RemoveLanguageChangeListener(TranslateUIElements);
        }
    }
}
