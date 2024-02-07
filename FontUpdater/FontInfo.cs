using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.EventSystems;
using VagrusTranslationPatches.Utils;
using System;
using static VagrusTranslationPatches.MonoBehaviours.UIPrefabFontUpdater;
using static UnityEngine.GraphicsBuffer;

namespace VagrusTranslationPatches.PrefabUpdater
{

    public class FontInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TextMeshProUGUI textMeshPro;
        //private BoxCollider2D boxCollider;
        private Canvas tooltipCanvas;
        private TextMeshProUGUI tooltipText;
        

        // You can adjust these parameters to customize the hover effect
        public Color hoverColor = Color.blue;
        public float hoverScaleMultiplier = 1.1f;

        private Color originalColor;
        private Vector3 originalScale;
        
        [SerializeField]
        public string prefabName;

        void Awake()
        {
            TranslationPatchesPlugin.onFontsRefresh.AddListener(UpdateFonts);
        }
        void Start()
        {
            // Get the TextMeshPro component
            textMeshPro = GetComponent<TextMeshProUGUI>();

            // Add a BoxCollider for mouse events
            //boxCollider = gameObject.AddComponent<BoxCollider2D>();

            // Create a canvas and text for the tooltip
            CreateTooltip(textMeshPro);

            // Store original color and scale
            originalColor = textMeshPro.color;
            originalScale = transform.localScale;
        }

        void CreateTooltip(TextMeshProUGUI textMeshPro)
        {
            // Create a canvas for the tooltip
            GameObject tooltipObject = new GameObject("InfoCanvas");
            tooltipCanvas = tooltipObject.AddComponent<Canvas>();
            tooltipCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            tooltipCanvas.transform.SetParent(textMeshPro.transform);
            tooltipObject.AddComponent<CanvasScaler>();
            tooltipObject.AddComponent<GraphicRaycaster>();
            tooltipCanvas.sortingOrder = BaseUI.GetTooltipOrder() + 1;

            // Create a TextMeshPro component for the tooltip
            GameObject textObject = new GameObject("InfoText");
            textObject.transform.SetParent(tooltipObject.transform,true);
            textObject.transform.localPosition = Vector2.zero;
                        
            tooltipText = textObject.AddComponent<TextMeshProUGUI>();
            tooltipText.rectTransform.sizeDelta = new Vector2(400, 50);
            tooltipText.color = Color.white;
            tooltipText.outlineColor = Color.black;
            tooltipText.outlineWidth = 0.2f;
            tooltipText.materialForRendering.EnableKeyword(ShaderUtilities.Keyword_Outline);
            tooltipText.fontSize = 20;
            tooltipText.enableWordWrapping = true;
            tooltipText.alignment = TextAlignmentOptions.MidlineLeft;

            // Set the canvas to not be initially active
            tooltipCanvas.enabled = false;
        }

        void OnMouseEnter()
        {
            // Change the appearance when the mouse hovers
            textMeshPro.color = hoverColor;
            //transform.localScale = originalScale * hoverScaleMultiplier;

            // Show the tooltip with the object's name
            ShowTooltip();
        }

        void OnMouseExit()
        {
            // Restore the original appearance when the mouse exits
            textMeshPro.color = originalColor;
            transform.localScale = originalScale;

            // Hide the tooltip
            HideTooltip();
        }

        void ShowTooltip()
        {
            tooltipText.text = $"Path: {this.prefabName}\nFont: {this.textMeshPro.font.name}";//gameObject.GetFullName();
            var replacementRecords = FontUtils.FindRelevantReplacementRecords(this.textMeshPro.font.name, this.prefabName);
            foreach(var record in replacementRecords)
            {
                tooltipText.text += $"\nRegEx: {record.Key.TargetRegEx}";
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                tooltipCanvas.transform as RectTransform,
                UnityEngine.Input.mousePosition,
                null,
                out Vector2 localPoint
            );

            tooltipCanvas.transform.position = tooltipCanvas.transform.TransformPoint(localPoint);
            tooltipCanvas.transform.localPosition = Vector2.zero;
            tooltipCanvas.enabled = true;
        }

        void HideTooltip()
        {
            // Hide the tooltip
            tooltipCanvas.enabled = false;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            textMeshPro.color = hoverColor;
            ShowTooltip();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            textMeshPro.color = originalColor;
            //transform.localScale = originalScale;

            HideTooltip();
        }

        private void UpdateFonts()
        {
                FontUtils.Update(textMeshPro, null, prefabName);
        }

        private void OnDestroy()
        {
            TranslationPatchesPlugin.onFontsRefresh.RemoveListener(UpdateFonts);
        }

    }

}
