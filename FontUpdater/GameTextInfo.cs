using UnityEngine;
using TMPro;
using System.Linq;
using VagrusTranslationPatches.FontUpdater;

namespace VagrusTranslationPatches.PrefabUpdater
{

    public class GameTextInfo : MonoBehaviour
    {
        public TextMeshProUGUI textMesh;
        

        public Color hoverColor = Color.blue;
        public float hoverScaleMultiplier = 1.1f;

        public Color originalColor;
        private Vector3 originalScale;
        
        [SerializeField]
        public string prefabName;

        public Vector2 tooltipSize = new Vector2(800, 200);


        void Awake()
        {
            TranslationPatchesPlugin.onFontsRefresh.AddListener(UpdateFonts);
        }
        void Start()
        {
            textMesh = GetComponent<TextMeshProUGUI>();

            originalColor = textMesh.color;
            originalScale = transform.localScale;
        }

        public string GetTextInfo()
        {
            var text = "";
            text = $"Path: {this.prefabName}\nFont: {this.textMesh.font.name}";//gameObject.GetFullName();
            var replacementRecords = FontUtils.FindRelevantReplacementRecords(this.textMesh.font.name, this.prefabName);

            text += $"\nSize: {textMesh.fontSize}, {textMesh.horizontalAlignment.ToString()}, {textMesh.verticalAlignment.ToString()}";
            text += $"\nCS: {textMesh.characterSpacing}em, WS: {textMesh.wordSpacing}em, LS: {textMesh.lineSpacing}em, PS: {textMesh.paragraphSpacing}em";
            foreach (var record in replacementRecords.Reverse())
            {
                text += $"\nRank:{record.Value} - {record.Key.Comment} RegEx: {record.Key.TargetRegEx}";
            }
            return text;
        }

        private void UpdateFonts()
        {
                FontUtils.Update(textMesh, null, prefabName);
        }

        private void OnDestroy()
        {
            TranslationPatchesPlugin.onFontsRefresh.RemoveListener(UpdateFonts);
        }

        public bool selected = false;
        void Update()
        {

            if (TranslationPatchesPlugin.isFontHelperEnabled)
            {
                if (FontInspect.selected == this)
                {
                    textMesh.color = hoverColor;
                } else
                {
                    textMesh.color = originalColor;
                }
            }
        }

    }
}
