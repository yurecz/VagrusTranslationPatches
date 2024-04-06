using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Vagrus;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Patches;
using VagrusTranslationPatches.PrefabUpdater;
using static VagrusTranslationPatches.MonoBehaviours.UIPrefabFontUpdater;

namespace VagrusTranslationPatches.Utils
{
    public static class GameObjectUtils
    {
        public static string GetFullName(this GameObject gameObject)
        {
            var text = "";
            while (gameObject != null)
            {
                text = gameObject.name + "=>"+text;
                gameObject = gameObject.transform.parent!=null?gameObject.transform.parent.gameObject:null;
            }

            return text.Trim('>','=');
        }

        public static bool HasComponent<T>(this GameObject flag) where T : Component
        {
            return flag.GetComponent<T>() != null;
        }

        public static T AddIfNotExistComponent<T>(this GameObject gamObject) where T : Component
        {
            T result = null;
            if (gamObject != null && !gamObject.HasComponent<T>())
            {
                result = gamObject.AddComponent<T>();
            }

            return result;
        }

        public static void RemoveIfExistComponent<T>(this GameObject gamObject) where T : Component
        {
            if (gamObject != null && gamObject.HasComponent<T>())
            {
                UnityEngine.Object.DestroyImmediate(gamObject.GetComponent<T>());
            }
        }

        public static void RemoveEverywhereComponent<T>(this GameObject gamObject) where T : Component
        {
            Component component = gamObject.GetComponent<T>();

            if (component != null)
            {
                UnityEngine.Object.DestroyImmediate(component);
            }

            foreach (Transform child in gamObject.transform)
            {
                RemoveEverywhereComponent<T>(child.gameObject);
            }
        }

        public static void AddOnceRecursiveComponent<T>(this GameObject gamObject) where T : Component
        {
            if (!gamObject.HasComponent<T>())
            {
                gamObject.AddComponent<T>();
            }

            foreach (Transform child in gamObject.transform)
            {
                RemoveEverywhereComponent<T>(child.gameObject);
            }
        }

        public static GameObject FindDeepChild(this GameObject parent, string name)
        {
            foreach (Transform child in parent.transform)
            {
                if (child.name == name)
                {
                    return child.gameObject;
                }

                GameObject found = FindDeepChild(child.gameObject, name);
                if (found != null)
                {
                    return found;
                }
            }

            return null;
        }

        public static GameObject FindDeep(this GameObject parent, string path)
        {
            string[] objectNames = SplitString(path);
            var name = objectNames[0];
            var list = parent.FindAllSubObjectsWithName(name);

            foreach (GameObject obj in list)
            {
                if (objectNames.Length == 1)
                    return obj;

                GameObject result = FindDeep(obj, objectNames[1]);

                if (result != null)
                    return result;
            }

            return null;
        }

        static string[] SplitString(string input)
        {
            char separator = '/';
            int index = input.IndexOf(separator);

            if (index != -1)
            {
                string firstPart = input.Substring(0, index);
                string secondPart = input.Substring(index + 1);

                return new string[] { firstPart, secondPart };
            }
            else
            {
                return new string[] { input };
            }
        }

        static List<GameObject> FindAllSubObjectsWithName(this GameObject parent, string name)
        {
            List<GameObject> foundObjects = new List<GameObject>();
            FindAllSubObjectsWithNameRecursive(parent.transform, name, foundObjects);
            return foundObjects;
        }

        static void FindAllSubObjectsWithNameRecursive(Transform parent, string nameWithIndex, List<GameObject> foundObjects)
        {
            string name = ExtractNameFromIndex(nameWithIndex, out int index);
            foreach (Transform child in parent)
            {
                if (child.name == name && (index == -1 || child.GetSiblingIndex() == index))
                {                   
                    foundObjects.Add(child.gameObject);
                }

                FindAllSubObjectsWithNameRecursive(child, nameWithIndex, foundObjects);
            }
        }

        static string ExtractNameFromIndex(string nameWithIndex, out int index)
        {
            index = -1;

            int startIndex = nameWithIndex.IndexOf('[');
            int endIndex = nameWithIndex.IndexOf(']');

            if (startIndex != -1 && endIndex != -1 && endIndex > startIndex)
            {
                string name = nameWithIndex.Substring(0, startIndex);
                string indexString = nameWithIndex.Substring(startIndex + 1, endIndex - startIndex - 1);

                if (int.TryParse(indexString, out index))
                    return name;
            }

            return nameWithIndex;
        }

        public static void RestoreRectTransform(this GameObject gameObject,GameObject gameObjectRef)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.RestoreRectTransform(gameObjectRef);
        }
        public static void UpdatePrefabFonts(this GameObject prefab)
        {
            prefab.RemoveEverywhereComponent<UIFontUpdater>();
            FindAllTextAndChangeFontInPrefab(prefab);
            var scrollers = prefab.GetComponentsInChildren<EnhancedScrollController>(true);
            foreach (var scroller in scrollers)
            {
                var cellViewPrefabs = scroller.cellViewPrefabs() ?? new ScrollCellView[] { };
                foreach (var cellViewPrefab in cellViewPrefabs)
                {
                    cellViewPrefab.gameObject.UpdatePrefabFonts();
                }
            }
        }

        private static void FindAllTextAndChangeFontInPrefab(GameObject prefab)
        {
            TranslationPatchesPlugin.Log.LogMessage($"Updating prefab: {prefab.GetFullName()}");

            var textMeshes = prefab.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var textMesh in textMeshes)
            {
                var prefabName = textMesh.gameObject.GetFullName();
                var gameTextInfo = textMesh.gameObject.GetComponent<GameTextInfo>();
                if (gameTextInfo==null)
                {
                    gameTextInfo = textMesh.gameObject.AddComponent<GameTextInfo>();
                    gameTextInfo.prefabName = prefabName;
                }
                FontUtils.Update(textMesh, null, prefabName);
            }
            TranslationPatchesPlugin.Log.LogMessage($" Updated prefab: {prefab.GetFullName()} found {textMeshes.Count()} components");
        }

        public static GameObject Translate(this GameObject prefab)
        {
            TranslationPatchesPlugin.Log.LogMessage($"Translating prefab: {prefab.GetFullName()}");

            var textMeshes = prefab.transform.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var textMesh in textMeshes)
            {
                if (textMesh.gameObject.name != "InfoText")
                {
                    textMesh.text = textMesh.text.Trim('\n').FromDictionary(true);
                    textMesh.richText = true;
                }
            }

            TranslationPatchesPlugin.Log.LogMessage($" Translated prefab: {prefab.GetFullName()} found {textMeshes.Count()} components");
            return prefab;
        }

        public static GameObject CloneButton(this GameObject headerDisplayPrice, string originalButtonName, string newButtonName, string newButtonText, float shiftX, float shiftY, float deltaX)
        {
            var buttonFull = headerDisplayPrice.FindDeepChild(originalButtonName);
            if (buttonFull != null)
            {
                var clonedButton = GameObject.Instantiate(buttonFull);
                clonedButton.name = newButtonName;
                clonedButton.transform.SetParent(buttonFull.transform.parent, false);
                clonedButton.transform.localPosition = buttonFull.transform.localPosition + new Vector3(shiftX + deltaX / 2, shiftY, 0);
                clonedButton.FindDeep("Inactive").GetComponent<RectTransform>().sizeDelta += new Vector2(deltaX, 0);
                clonedButton.FindDeep("Selected").GetComponent<RectTransform>().sizeDelta += new Vector2(deltaX, 0);
                clonedButton.FindDeep("Hover").GetComponent<RectTransform>().sizeDelta += new Vector2(deltaX, 0);
                clonedButton.FindDeep("Normal").GetComponent<RectTransform>().sizeDelta += new Vector2(deltaX, 0);
                clonedButton.transform.Find("Holder/Title").GetComponent<TextMeshProUGUI>().text = newButtonText;
                clonedButton.GetComponent<ButtonUI>().SetHoverEnabled(true);
                return clonedButton;
            }

            return null;
        }

    }

}
