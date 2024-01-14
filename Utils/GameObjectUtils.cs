using AOT;
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
    }

}
