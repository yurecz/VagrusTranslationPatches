using System.Collections.Generic;
using System.Linq;
using BepInEx;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VagrusTranslationPatches.PrefabUpdater;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.FontUpdater
{

    public class FontInspect : MonoBehaviour
    {
        public static FontInspect instance;

        public Canvas FontInspectorCanvas;
        public string SelectedText;
        public string PrefabName;
        private GameObject Holder;
        private TextMeshProUGUI InformationText;
        private Image BackgroundImage;

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

        void Start()
        {
            CreateTooltip();
        }

        void CreateTooltip()
        {
            FontInspectorCanvas = gameObject.AddComponent<Canvas>();
            FontInspectorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            FontInspectorCanvas.sortingOrder = 32767;
            FontInspectorCanvas.sortingLayerID = 30;
            gameObject.AddComponent<CanvasScaler>();
            var raycaster = gameObject.AddComponent<GraphicRaycaster>();


            Holder = new GameObject("Holder");
            Holder.transform.SetParent(gameObject.transform);

            BackgroundImage = Holder.AddComponent<Image>();

            GameObject informationObject = new GameObject("Information");
            informationObject.transform.SetParent(Holder.transform);

            InformationText = informationObject.AddComponent<TextMeshProUGUI>();
            InformationText.color = Color.black;
            //tooltipText.outlineColor = Color.black;
            //tooltipText.outlineWidth = 0.2f;
            //tooltipText.materialForRendering.EnableKeyword(ShaderUtilities.Keyword_Outline);
            InformationText.fontSize = 20;
            InformationText.enableWordWrapping = false;
            InformationText.horizontalAlignment = HorizontalAlignmentOptions.Left;
            InformationText.verticalAlignment = VerticalAlignmentOptions.Top;
            InformationText.rectTransform.localPosition = Vector2.zero;
            FontInspectorCanvas.enabled = false;
        }


        private static string _hoverText = string.Empty;

        public static GameTextInfo selected;

        void Update()
        {

            if (!TranslationPatchesPlugin.isFontHelperEnabled)
            {
                FontInspectorCanvas.enabled = false;
                return;
            }

            InformationText.text = "";

            var camera = Camera.main;
            var canvasHits = DoCanvas(camera);

            if (canvasHits.Count > 0)
            {

                var currentIndex = canvasHits.IndexOf(selected);
                if (UnityInput.Current.GetMouseButtonDown(2))
                {
                    currentIndex = (currentIndex + 1) % canvasHits.Count;
                    selected = canvasHits[currentIndex];

                } else if (currentIndex == -1 )
                {
                    selected = canvasHits[0];
                    currentIndex = 1;
                }

                InformationText.text += $"Selected: {currentIndex + 1}/{canvasHits.Count}";
            }
            else selected = null;

            if (selected != null)
            {

                float screenWidth = Screen.width;
                float screenHeight = Screen.height;

                Vector3 mousePosition = Input.mousePosition;
                Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 10f));

                FontInspectorCanvas.enabled = true;
                InformationText.text += "\n"+selected.GetTextInfo();
                SelectedText = selected.textMesh.text;
                PrefabName = selected.prefabName;
                InformationText.rectTransform.sizeDelta = new Vector2(InformationText.preferredWidth, InformationText.preferredHeight);
                BackgroundImage.rectTransform.sizeDelta = InformationText.rectTransform.sizeDelta + 10*Vector2.one;


                float shiftX = 5 + BackgroundImage.rectTransform.sizeDelta.x / 2;
                float shiftY = 20 + BackgroundImage.rectTransform.sizeDelta.y / 2;


                if (mousePosition.x > screenWidth / 2)
                {
                    shiftX *= -1;
                }

                if (mousePosition.y > screenHeight / 2)
                {
                    shiftY *= -1;
                }

                Holder.transform.position = mousePosition + new Vector3(shiftX, shiftY, 0);
                //Clipboard.Text = selected.textMesh.text;

            }
            else
            {
                FontInspectorCanvas.enabled = false;
                _hoverText = string.Empty;
                SelectedText = null;
                PrefabName = null;
                //Clipboard.Text = "";
            }
        }
        private static List<GameTextInfo> DoCanvas(Camera camera)
        {
            Vector2 mousePosition = UnityInput.Current.mousePosition;

            var hits = UnityEngine.Object.FindObjectsOfType<GameTextInfo>().Where(rt =>
            {
                if (rt!=null && rt.gameObject.activeInHierarchy && rt.textMesh!=null)// && rt.textMesh.rectTransform.sizeDelta.x != 0 && rt.textMesh.rectTransform.sizeDelta.y != 0 ) 
                {
                    var canvas = rt.GetComponentInParent<Canvas>();
                    if (canvas != null && canvas.enabled && rt.GetComponentsInParent<CanvasGroup>().All(x => x.alpha > 0.1f))
                    {
                        var cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera != null ? canvas.worldCamera : camera;
                        return RectTransformUtility.RectangleContainsScreenPoint(rt.textMesh.rectTransform, mousePosition, cam);
                    }
                    return true;
                }
                return false;
            });

            if (hits != null && hits.Count()>0)
            {
                return hits.OrderBy(x =>

                    {
                        var canvas = x.GetComponentInParent<Canvas>();
                        //return x.textMesh.rectTransform.sizeDelta.sqrMagnitude
                        if (canvas != null && canvas.enabled)
                        {

                        }
                        return 0;
                    }
                
                ).ToList();
            }
            else
            {
                return new List<GameTextInfo>();
            }

        }
    }

}
