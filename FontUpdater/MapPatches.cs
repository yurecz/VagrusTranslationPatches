using HarmonyLib;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using Vagrus;
using VagrusTranslationPatches.MonoBehaviours;
using VagrusTranslationPatches.Patches;
using VagrusTranslationPatches.Utils;

namespace VagrusTranslationPatches.PrefabUpdater
{
    [HarmonyPatch(typeof(Map))]
    internal class MapPatches
    {
        [HarmonyPatch("Awake")]
        [HarmonyPrefix]
        public static bool Awake_Prefix(Map __instance)
        {

            LoadingScreen.Instance.gameObject.UpdatePrefabFonts();
            PatchObject.instance.UpdatePrefab("GameOver/Prefab/GameOverUI");

            PatchObject.instance.UpdatePrefab("UI/Prefab/TooltipCanvas");
            PatchObject.instance.UpdatePrefab("UI/Prefab/MessageBox");
            PatchObject.instance.UpdatePrefab("UI/Prefab/MessageBoxBig");

            //Intro screens
            PatchObject.instance.UpdatePrefab("Start/Disclaimer");
            PatchObject.instance.UpdatePrefab("Start/GamePhaseFirstQuote");
            PatchObject.instance.UpdatePrefab("Start/GamePhaseNewGameQuote");
            PatchObject.instance.UpdatePrefab("Start/GamePhaseVanillaQuote");

            //Main Menu
            PatchObject.instance.UpdatePrefab("MainMenu/Prefab/MainMenuUI_SS");
            var mainMenu = PatchObject.instance.UpdatePrefab("MainMenu/Prefab/MainMenuUI");
            mainMenu.FindDeep("CreditsTitle").AddComponent<UIElementTranslator>();
            mainMenu.FindDeep("ExtrasTitle").AddComponent<UIElementTranslator>();

            PatchObject.instance.UpdatePrefab("MainMenu/Prefab/SaveGameSlot");
            PatchObject.instance.UpdatePrefab("MainMenu/Prefab/GameModeSlot");
            PatchObject.instance.UpdatePrefab("MainMenu/Prefab/Credits").FindDeep("Credits").AddIfNotExistComponent<UIElementTranslator>();
            PatchObject.instance.UpdatePrefab("MainMenu/Prefab/Extras").FindDeep("DLC_Title").AddIfNotExistComponent<UIElementTranslator>();
            PatchObject.instance.UpdatePrefab("MainMenu/Prefab/SaveGamePlaceholder");

            PatchObject.instance.UpdatePrefab("UI/Prefab/Checkbox");
            PatchObject.instance.UpdatePrefab("UI/Prefab/HSlider");
            PatchObject.instance.UpdatePrefab("MainMenu/Prefab/Dropdown2");
            PatchObject.instance.UpdatePrefab("UI/MainMenuUI/KeybindLabel");
            PatchObject.instance.UpdatePrefab("MainMenu/Prefab/InfoLabel");
            PatchObject.instance.UpdatePrefab("UI/MainMenuUI/KeybindBox");

            var popupWindow = PatchObject.instance.UpdatePrefab("Assets/ControllerSupport/PopupWindow");
            popupWindow.FindDeep("Title_Background").AddIfNotExistComponent<UIElementTranslator>();
            popupWindow.FindDeep("SelectBackgroundButton/Text (TMP)").AddIfNotExistComponent<UIElementTranslator>();

            //Buttons
            PatchObject.instance.UpdatePrefab("UI/Prefab/ButtonMenu", "Holder");
            PatchObject.instance.UpdatePrefab("UI/Prefab/ButtonNormal", "Holder");
            PatchObject.instance.UpdatePrefab("UI/Buttons/Prefab/TextButtonCheck");

            //Map
            PatchObject.instance.UpdatePrefab("Nodes/Prefab/LineLiteCanvas");
            PatchObject.instance.UpdatePrefab("Nodes/Prefab/LineCanvas");
            PatchObject.instance.UpdatePrefab("Nodes/Prefab/RadialUI");
            PatchObject.instance.UpdatePrefab("Nodes/Prefab/NodeCanvas");

            PatchObject.instance.UpdatePrefab("UI/Prefab/HuntForageUI");
            PatchObject.instance.UpdatePrefab("Camp/Prefab/CampUI");
            PatchObject.instance.UpdatePrefab("Scouting/Prefab/ScoutUI");
            //PatchObject.instance.UpdatePrefab("Assets/ScoutResultRow");
            PatchObject.instance.UpdatePrefab("UI/Prefab/Goods");
            PatchObject.instance.UpdatePrefab("UI/Prefab/HFResultBox");


            //Chart
            PatchObject.instance.UpdatePrefab("Chart/Prefab/ChartUI");
            PatchObject.instance.UpdatePrefab("Chart/Prefab/POIBox");
            PatchObject.instance.UpdatePrefab("Chart/Prefab/PriceHistoryRow");
            PatchObject.instance.UpdatePrefab("Chart/Prefab/PriceHistoryBox");
            PatchObject.instance.UpdatePrefab("UI/Buttons/Prefab/DropDownButton");

            //Log
            PatchObject.instance.UpdatePrefab("UI/Prefab/GameLogUI");


            //CampFire on new game
            PatchObject.instance.UpdatePrefab("CampFire/Prefab/CampFireUI");
            PatchObject.instance.UpdatePrefab("CampFire/Prefab/CampFireUI_SS");
            PatchObject.instance.UpdatePrefab("VagrusCreation/Prefab/VagrusCreationUI");
            PatchObject.instance.UpdatePrefab("VagrusCreation/Prefab/VagrusBonusBlock");

            //LeaderUI
            PatchObject.instance.UpdatePrefab("Leader/Prefab/LeaderUI");
            PatchObject.instance.UpdatePrefab("Leader/Prefab/FactionSection");
            PatchObject.instance.UpdatePrefab("UI/Prefab/LeaderPerkSingleRow");
            PatchObject.instance.UpdatePrefab("UI/Prefab/LeaderPerkDoubleRow");
            PatchObject.instance.UpdatePrefab("Leader/Prefab/FactionThumb");
            PatchObject.instance.UpdatePrefab("UI/Prefab/StatusRow");

            //Calender
            PatchObject.instance.UpdatePrefab("UI/Prefab/CalendarUI");

            //Caravan
            PatchObject.instance.UpdatePrefab("Caravan/Prefab/MiniCaravanUI");
            PatchObject.instance.UpdatePrefab("UI/Prefab/PerkSingleRow");

            //BaseUI
            var baseUI = PatchObject.instance.UpdatePrefab("Caravan/BaseUI/BaseUI");
            foreach (var preset in baseUI.GetComponent<BaseUIEx>().SettlementPresets)
            {
                foreach (var tab in preset.Tabs)
                {
                    if (tab.TabPrefab != null)
                    {
                        var gameObject = tab.TabPrefab.gameObject;
                        gameObject.UpdatePrefabFonts();
                    }
                }
            }

            //CrewUI
            PatchObject.instance.UpdatePrefab("baseui/prefabs/EnduringEffectsRow");
            PatchObject.instance.UpdatePrefab("baseui/prefabs/CrewDetailsRow");
            PatchObject.instance.UpdatePrefab("UI/Prefab/Passenger");

            //MarketUI
            PatchObject.instance.UpdatePrefab("UI/MarketQty/Prefab/MarketQty");

            //StoryUI
            PatchObject.instance.UpdatePrefab("Settlement/Prefab/StoryElem2");

            //StatioUI

            //Events
            PatchObject.instance.UpdatePrefab("Event/Prefab/EventUI");
            PatchObject.instance.UpdatePrefab("Event/Prefab/EventUI-FactionTask");
            PatchObject.instance.UpdatePrefab("Event/Prefab/EventText");
            PatchObject.instance.UpdatePrefab("Event/Prefab/EventChoiceButton");

            //Crew combat
            var crewCombatUI = PatchObject.instance.UpdatePrefab("CrewCombat/Prefab/CrewCombatUI");
            crewCombatUI.FindDeep("Odds").AddIfNotExistComponent<UIElementTranslator>();
            crewCombatUI.FindDeep("Title").AddIfNotExistComponent<UIElementTranslator>();

            PatchObject.instance.UpdatePrefab("UI/Prefab/CrewCombatGoal");
            PatchObject.instance.UpdatePrefab("UI/Prefab/CrewCombatGoalSmall");
            PatchObject.instance.UpdatePrefab("CrewCombat/Prefab/ActionEntry");
            PatchObject.instance.UpdatePrefab("CrewCombat/Prefab/ActionDropDown");
            PatchObject.instance.UpdatePrefab("CrewCombat/Prefab/ItemOfferRow");

            PatchObject.instance.UpdatePrefab("CrewCombat/Prefab/LootBottom");
            PatchObject.instance.UpdatePrefab("CrewCombat/Prefab/LootTop");
            PatchObject.instance.UpdatePrefab("UI/Prefab/CrewCircleGroup");

            //BookUI
            var contractDetails = PatchObject.instance.UpdatePrefab("UI/Book/Prefab/ContractDetails");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/BookDivider");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/BookTextBlock");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/TaskCard");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/DeliverRow");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/FlavorBlock");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/RewardBlock");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/RumorBottomBlock");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/BookEntry");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/BookChapterBlock");
            PatchObject.instance.UpdatePrefab("UI/Book/Prefab/JournalNoteEditor");
            PatchObject.instance.UpdatePrefab("UI/Book/Codex/Prefab/CodexUI");
            PatchObject.instance.UpdatePrefab("UI/Book/Journal/Prefab/JournalUI");             

            return true;
        }
    }
}