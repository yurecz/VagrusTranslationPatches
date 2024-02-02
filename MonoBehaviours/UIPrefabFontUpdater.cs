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
    public class UIPrefabFontUpdater : MonoBehaviour
    {
        [Serializable]
        public class KeyUIPair
        {
            public TextMeshProUGUI textMesh;
        }

        private List<KeyUIPair> keyUIPairs = new List<KeyUIPair>();

        private void Awake()
        {
            Game.AddLanguageChangeListener(UpdateFonts);
        }

        private void Start()
        {
                FindAllText();
        }

        private void FindAllText()
        {
            keyUIPairs.Clear();

            TranslationPatchesPlugin.Log.LogInfo("Registered UIPrefabFontUpdater on " + base.gameObject.GetFullName());
            
            TextMeshProUGUI[] componentsInChildren = base.transform.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
            foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
            {
                if(!textMeshProUGUI.gameObject.HasComponent<UIElementTranslator>())
                    keyUIPairs.Add(new KeyUIPair
                    {
                        textMesh = textMeshProUGUI,                       
                    });
            }
        }

        private void UpdateFonts()
        {
               foreach (KeyUIPair keyUIPair in keyUIPairs)
            {
                if (keyUIPair.textMesh == null)
                {
                    continue;
                }

                FontUtils.Update(keyUIPair.textMesh, null, "UIPrefabFontUpdater");
            }
        }

        private void OnDestroy()
        {
            Game.RemoveLanguageChangeListener(UpdateFonts);
        }
    }
}
