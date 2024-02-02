using UnityEngine;
using TMPro;
using System.Collections;
using Vagrus;
using HarmonyLib;
using System.Collections.Generic;
using System;
using VagrusTranslationPatches.Utils;
using VagrusTranslationPatches;

namespace VagrusTranslationPatches
{
    public class PatchObject : MonoBehaviour
    {
        public static PatchObject instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {

        }

        // Static method to start the coroutine externally
        public void UpdateFonts(UIFontUpdater instance)
        {
            if (instance != null)
            {
                // Call the coroutine method on the PatchObject instance
                StartCoroutine(FindAllTextAndChangeFont(instance));
            }
            else
            {
                Debug.LogError("PatchObject priceHistoryBoxInstance not found.");
            }
        }

        private static IEnumerator FindAllTextAndChangeFont(UIFontUpdater instance)
        {
            if (instance == null) yield break;
            var keyUIPairs = Traverse.Create(instance).Field("keyPairs").GetValue() as List<UITranslator.KeyUIPair>;
            keyUIPairs.Clear();

            var textMeshes = instance.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var textMesh in textMeshes)
            {
                if (textMesh == null || keyUIPairs == null || instance == null) continue;

                try
                {
                    if (!textMesh.CompareTag("Translated"))
                    {
                        keyUIPairs.Add(new UITranslator.KeyUIPair
                        {
                            textMesh = textMesh,
                            firstFont = textMesh.font
                        });
                        FontUtils.Update(textMesh, null, "UIFontUpdater");
                    }

                }
                catch (Exception ex)
                {
                    Debug.LogError($"Exception caught: {ex.Message}");
                    yield break;
                }
                yield return null;
            }

            if (instance == null) yield break;
            keyUIPairs = Traverse.Create(instance).Field("keyPairs").GetValue() as List<UITranslator.KeyUIPair>;
            TranslationPatchesPlugin.Log.LogInfo($"UIFontUpdater on {instance.gameObject.GetFullName()} found: {keyUIPairs.Count} components");
            yield break;
        }

    }
}