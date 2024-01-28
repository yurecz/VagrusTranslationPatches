using UnityEngine;

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

        public static void AddIfNotExistComponent<T>(this GameObject gamObject) where T : Component
        {
            if (gamObject != null && !gamObject.HasComponent<T>())
            {
                gamObject.AddComponent<T>();
            }
        }

        public static void RemoveIfExistComponent<T>(this GameObject gamObject) where T : Component
        {
            if (gamObject != null && gamObject.HasComponent<T>())
            {
                Object.Destroy(gamObject.GetComponent<T>());
            }
        }

        public static void RemoveEverywhereComponent<T>(this GameObject gamObject) where T : Component
        {
            Component component = gamObject.GetComponent<T>();

            if (component != null)
            {
                Object.Destroy(component);
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

        public static void RestoreRectTransform(this GameObject gameObject,GameObject gameObjectRef)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.RestoreRectTransform(gameObjectRef);
        }

    }

}
