using HarmonyLib;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VagrusTranslationPatches.Utils
{
    internal static class TutorialUtils
    {
        
        public static void UpdateTutorialText()
        {

            var tutorialUI = Game.game.caravan.tutorialUI;
            if (tutorialUI!=null)
            {
                var tutorial = tutorialUI.tutorial;
                bool controllerTutorial = tutorial.GetName().ToLower().Contains("_controller");
                var description = Traverse.Create(tutorialUI).Field("description").GetValue() as TextMeshProUGUI;
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)description.transform.parent);
                MethodInfo methodInfo = typeof(TutorialUI).GetMethod("IsNarration", BindingFlags.NonPublic | BindingFlags.Instance);
                var parameters = new object[] { };
                var isNarration = (bool)methodInfo.Invoke(tutorialUI, parameters);
                if (isNarration)
                {
                    description.text = Format(tutorialUI, tutorial.GetDescription(), controllerTutorial) + "\n";
                }
                else
                {
                    description.text = Format(tutorialUI, tutorial.GetDescription(), controllerTutorial) + "\n<color=" + Game.highLightColor + "><width=90%>" + Format(tutorialUI, tutorial.GetActionDescription(), controllerTutorial) + "</color>";
                }
                TranslationPatchesPlugin.Log.LogInfo("Руководство " + tutorial.GetName() + " обновлено.");
            }

        }

        private static string Format(TutorialUI tutorialUI, string text,bool controllerTutorial = false)
        {
            MethodInfo methodInfo = typeof(TutorialUI).GetMethod("FormatText", BindingFlags.NonPublic | BindingFlags.Instance);
            var parameters = new object[] { text, controllerTutorial };
            return (string)methodInfo.Invoke(tutorialUI, parameters);
        }

    }
}
