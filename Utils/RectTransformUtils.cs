using UnityEngine;

namespace VagrusTranslationPatches.Utils
{
    public static class RectTransformUtils
    {
            public static void RestoreRectTransform(this RectTransform rectTransform, GameObject gameObject)
            {
                if (gameObject == null) return;
                var rectTransformRef = gameObject.GetComponent<RectTransform>();

                rectTransform.localPosition = rectTransformRef.localPosition;
                rectTransform.localScale = rectTransformRef.localScale;
                rectTransform.sizeDelta = rectTransformRef.sizeDelta;
                rectTransform.anchorMin = rectTransformRef.anchorMin;
                rectTransform.anchorMax = rectTransformRef.anchorMax;
                rectTransform.anchoredPosition = rectTransformRef.anchoredPosition;
                rectTransform.pivot = rectTransformRef.pivot;
            }
    }
}