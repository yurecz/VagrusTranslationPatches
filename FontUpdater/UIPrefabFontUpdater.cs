using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Vagrus;
using VagrusTranslationPatches.PrefabUpdater;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.MonoBehaviours
{
    public class UIPrefabFontUpdater : MonoBehaviour
    {
        [Serializable]
        public class TextComponent
        {
            public TextMeshProUGUI textMesh;
            public string prefabName;
        }

        public List<TextComponent> textComponents = new List<TextComponent>();

        private void Awake()
        {
            TranslationPatchesPlugin.onFontsRefresh.AddListener(UpdateFonts);
        }

        private void Start()
        {
               // FindAllText();
        }

        private void FindAllText()
        {
            textComponents.Clear();

            //TranslationPatchesPlugin.Log.LogInfo("Registered UIPrefabFontUpdater on " + base.gameObject.GetFullName());
            
            TextMeshProUGUI[] componentsInChildren = base.transform.GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true);
            foreach (TextMeshProUGUI textMeshProUGUI in componentsInChildren)
            {
                if (!textMeshProUGUI.gameObject.HasComponent<FontInfo>())
                {
                    var prefabName = textMeshProUGUI.gameObject.GetFullName();
                    textComponents.Add(new TextComponent
                    {
                        textMesh = textMeshProUGUI,
                        prefabName = prefabName
                    });
                    var fontInfo = textMeshProUGUI.gameObject.AddComponent<FontInfo>();
                    fontInfo.prefabName = prefabName;
                }

            }
        }

        private void UpdateFonts()
        {
            foreach (TextComponent textComponent in textComponents)
            {
                FontUtils.Update(textComponent.textMesh, null, "UIPrefabFontUpdater");
            }
        }

        private void OnDestroy()
        {
            TranslationPatchesPlugin.onFontsRefresh.RemoveListener(UpdateFonts);
        }
    }
}
