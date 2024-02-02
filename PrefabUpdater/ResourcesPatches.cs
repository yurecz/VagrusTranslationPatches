using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using VagrusTranslationPatches.Utils;
using System;
using Vagrus;

namespace VagrusTranslationPatches.PrefabUpdater
{
    struct Rule
    {
        public UpdateComponent updateComponent;
        public string[] elements;

        public Rule(UpdateComponent prefabType, string[] elements) : this()
        {
            this.elements = elements;
        }
    }
    public enum UpdateComponent
    {
        UIPrefabFontUpdater,
        UIPrefabObjectTranslator
    }

    [HarmonyPatch(typeof(Resources))]
    internal class ResourcesPatches
    {
        private static Dictionary<string, Rule> prefabRegistry = new Dictionary<string, Rule>();

        static ResourcesPatches()
        {
            AddPrefabToRegistry("CrewCombat/Prefab/CrewCombatUI", UpdateComponent.UIPrefabFontUpdater, new string[]{ "Odds", "Title"} );
            AddPrefabToRegistry("UI/Prefab/CrewCombatGoal", UpdateComponent.UIPrefabFontUpdater);
            AddPrefabToRegistry("UI/Prefab/CrewCombatGoalSmall", UpdateComponent.UIPrefabFontUpdater);
            AddPrefabToRegistry("CrewCombat/Prefab/ActionEntry", UpdateComponent.UIPrefabFontUpdater);
            AddPrefabToRegistry("CrewCombat/Prefab/ActionDropDown", UpdateComponent.UIPrefabFontUpdater);
            AddPrefabToRegistry("CrewCombat/Prefab/ItemOfferRow", UpdateComponent.UIPrefabFontUpdater);

            AddPrefabToRegistry("Nodes/Prefab/LineLiteCanvas", UpdateComponent.UIPrefabFontUpdater);
            AddPrefabToRegistry("Nodes/Prefab/LineCanvas", UpdateComponent.UIPrefabFontUpdater);

            //AddPrefabToRegistry("MainMenu/Prefab/MainMenuUI", UpdateComponent.UIPrefabFontUpdater);
            //AddPrefabToRegistry("MainMenu/Prefab/MainMenuUI_SS", UpdateComponent.UIPrefabFontUpdater);
            AddPrefabToRegistry("MainMenu/Prefab/SaveGameSlot", UpdateComponent.UIPrefabFontUpdater);
            AddPrefabToRegistry("MainMenu/Prefab/GameModeSlot", UpdateComponent.UIPrefabFontUpdater);
            AddPrefabToRegistry("MainMenu/Prefab/Credits", UpdateComponent.UIPrefabFontUpdater);
            AddPrefabToRegistry("MainMenu/Prefab/Extras", UpdateComponent.UIPrefabFontUpdater);
            //AddPrefabToRegistry("MainMenu/Prefab/SaveGamePlaceholder", UpdateComponent.UIPrefabFontUpdater);

            //AddPrefabToRegistry("UI/Prefab/ButtonMenu", UpdateComponent.UIPrefabFontUpdater); - не имеет смысла
            //AddPrefabToRegistry("UI/Prefab/ButtonNormal", UpdateComponent.UIPrefabFontUpdater); - не имеет смысла
        }

        private static void AddPrefabToRegistry(string path, UpdateComponent prefabType, string[] elements = null)
        {
            if (!prefabRegistry.ContainsKey(path))
            {
                prefabRegistry.Add(path, new Rule(prefabType, elements));
            }
        }

        [HarmonyPatch(nameof(Resources.Load), new System.Type[] { typeof(string), typeof(Type) })]
        [HarmonyPostfix]
        public static void LoadPostfix(string path, ref System.Object __result)
        {
            if (__result != null && __result is GameObject)
            {
                GameObject loadedGameObject = (GameObject)__result;

                if (prefabRegistry.TryGetValue(path, out Rule rule))
                {
                    switch (rule.updateComponent)
                    {
                        case UpdateComponent.UIPrefabFontUpdater:
                            if (rule.elements != null)
                            {
                                foreach (var subObjectName in rule.elements)
                                {
                                    var subObject = loadedGameObject.FindDeepChild(subObjectName);
                                    subObject.AddIfNotExistComponent<UIElementTranslator>();
                                }
                            }
                            loadedGameObject.UpdatePrefabFonts();
                            break;

                        case UpdateComponent.UIPrefabObjectTranslator:
                            loadedGameObject.UpdatePrefabFontsAndTranslation();
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                }
            }
        }
    }
}
