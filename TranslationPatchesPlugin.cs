using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using VagrusTranslationPatches.FontUpdater;
using VagrusTranslationPatches.Patches;
using VagrusTranslationPatches.Utils;


namespace VagrusTranslationPatches
{

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class TranslationPatchesPlugin : BaseUnityPlugin
    {
        private const string MyGUID = "ru.Vagrus.TranslationPatches";
        private const string PluginName = "TranslationPatches";
        private const string VersionString = "0.7.0";

        // Config entry key strings
        // These will appear in the config file created by BepInEx and can also be used
        // by the OnSettingsChange event to determine which setting has changed.
        public static string FloatExampleKey = "Float Example Key";
        public static string IntExampleKey = "Int Example Key";
        public static string KeyboardShortcutRereadTranslationFilesKey = "Заново прочитать файлы перевода";
        public static string KeyboardShortcutRereadFontsFilesKey = "Заново обновить шрифты";
        public static string KeyboardShortcutEnableFontHelperKey = "Включить подсказку шрифта в игре";
        public static string KeyboardShortcutEnableCopyTextToClipboardKey = "Скопировать текст";
        public static string KeyboardShortcutEnableCopyPathToClipboardKey = "Скопировать путь";
        // Configuration entries. Static, so can be accessed directly elsewhere in code via
        // e.g.
        // float myFloat = VagrusTestModePlugin.FloatExample.Value;
        // TODO Change this code or remove the code if not required.
        public static ConfigEntry<float> FloatExample;
        public static ConfigEntry<int> IntExample;
        public static ConfigEntry<KeyboardShortcut> KeyboardShortcutRereadTranslationFiles;
        public static ConfigEntry<KeyboardShortcut> KeyboardShortcutRereadFontsFiles;
        public static ConfigEntry<KeyboardShortcut> KeyboardShortcutEnableFontHelper;
        public static ConfigEntry<KeyboardShortcut> KeyboardShortcutEnableCopyTextToClipboard;
        public static ConfigEntry<KeyboardShortcut> KeyboardShortcutEnableCopyPathToClipboard;

        private static readonly Harmony Harmony = new Harmony(MyGUID);
        public static ManualLogSource Log = new ManualLogSource(PluginName);

        /// <summary>
        /// Initialise the configuration settings and patch methods
        /// </summary>
        private void Awake()
        {
            // Float configuration setting example
            //FloatExample = Config.Bind("General",    // The section under which the option is shown
            //    FloatExampleKey,                            // The key of the configuration option
            //    1.0f,                            // The default value
            //    new ConfigDescription("Example float configuration setting.",         // Description that appears in Configuration Manager
            //        new AcceptableValueRange<float>(0.0f, 10.0f)));     // Acceptable range, enabled slider and validation in Configuration Manager

            // Int setting example
            //IntExample = Config.Bind("General",
            //    IntExampleKey,
            //    1,
            //    new ConfigDescription("Example int configuration setting.",
            //        new AcceptableValueRange<int>(0, 10)));

            // Keyboard shortcut setting example
            KeyboardShortcutRereadTranslationFiles = Config.Bind("Перевод",
                KeyboardShortcutRereadTranslationFilesKey,
                new KeyboardShortcut(KeyCode.R, KeyCode.LeftControl));

            KeyboardShortcutRereadFontsFiles = Config.Bind("Шрифты",
                KeyboardShortcutRereadFontsFilesKey,
                new KeyboardShortcut(KeyCode.A, KeyCode.LeftControl));

            KeyboardShortcutEnableFontHelper = Config.Bind("Общие",
                KeyboardShortcutEnableFontHelperKey,
                new KeyboardShortcut(KeyCode.T, KeyCode.LeftControl));

            KeyboardShortcutEnableCopyTextToClipboard = Config.Bind("Перевод",
                KeyboardShortcutEnableCopyTextToClipboardKey,
                new KeyboardShortcut(KeyCode.C, KeyCode.LeftControl));

            KeyboardShortcutEnableCopyPathToClipboard = Config.Bind("Шрифты",
                KeyboardShortcutEnableCopyPathToClipboardKey,
                new KeyboardShortcut(KeyCode.P, KeyCode.LeftControl));

            KeyboardShortcutRereadTranslationFiles.SettingChanged += ConfigSettingChanged;
            KeyboardShortcutRereadFontsFiles.SettingChanged += ConfigSettingChanged;
            KeyboardShortcutEnableFontHelper.SettingChanged += ConfigSettingChanged;
            KeyboardShortcutEnableCopyTextToClipboard.SettingChanged += ConfigSettingChanged;
            KeyboardShortcutEnableCopyPathToClipboard.SettingChanged += ConfigSettingChanged;

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();

            GameObject patchObjectGO = new GameObject("PatchObject");
            PatchObject patchObject = patchObjectGO.AddComponent<PatchObject>();

            GameObject fontInspectorObject = new GameObject("FontInspector");
            var fontInspector = fontInspectorObject.AddComponent<FontInspect>();

            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            Log = Logger;

        }
        private void Update()
        {
            if (TranslationPatchesPlugin.KeyboardShortcutRereadTranslationFiles.Value.IsDown())
            {
                if (GeneralSettings.EnableLanguage)
                {
                    var game = Game.game;
                    int id = game.settings.pubSettings.language;
                    long packId = game.settings.pubSettings.languagePackId;
                    Game.game.settings.SetLanguage(id, packId, false);
                    Game.AcceptLanguage(true);
                    if (Game.game.eventUIIsON)
                    {
                        var gameEvent = Game.GetActiveEvent();
                        if (Game.GetActiveEvent() != null && gameEvent.curStep != null)
                        {
                            gameEvent.eventUI.RemoveUnusedChoicesFromLog(-1);
                            gameEvent.SelectStep(gameEvent.curStep);
                            Logger.LogInfo("Событие " + gameEvent.GetTitle() + " обновлено.");
                        }
                    }
                    TutorialUtils.UpdateTutorialText();
                    Game.game.caravan.InvalidateUI();
                    if (game.caravan.bookUI != null && game.caravan.bookUI.IsTop() && game.caravan.IsOpenBookUI(BookType.Any))
                    {
                        var bookType = game.caravan.bookUI.bookType;
                        game.caravan.CloseBookUI();
                        game.caravan.OpenBookUI(bookType, false, null, false);
                    }

                    SetGameFixedValues();

                    Logger.LogInfo("Файлы перевода заново прочитаны.");
                }
                else
                {
                    Logger.LogInfo("Файлы перевода непрочитаны, так как перевод не включён в настройках игры.");
                }
            }

            if (TranslationPatchesPlugin.KeyboardShortcutRereadFontsFiles.Value.IsDown())
            {
                FontUtils.ReplacementRecords.Clear();
                FontUtils.LoadFonts();
                PrefabsList.UpdatePrefabs();

                PatchObject.instance.UpdatePrefab("Assets/Addressables/Glossary/Prefab/GlossaryUI_prefab");
                PatchObject.instance.UpdatePrefab("Assets/Addressables/DLC-ICONS/DLC-ICON");

                onFontsRefresh.Invoke();

                Logger.LogInfo("Шрифты обновлены");

            }

            if (TranslationPatchesPlugin.KeyboardShortcutEnableFontHelper.Value.IsDown())
            {
                isFontHelperEnabled = !isFontHelperEnabled;
            }

            if (TranslationPatchesPlugin.KeyboardShortcutEnableCopyTextToClipboard.Value.IsDown())
            {
               if (FontInspect.instance.FontInspectorCanvas.enabled) {
                    GUIUtility.systemCopyBuffer = FontInspect.instance.SelectedText;
                }
            }

            if (TranslationPatchesPlugin.KeyboardShortcutEnableCopyPathToClipboard.Value.IsDown())
            {
                if (FontInspect.instance.FontInspectorCanvas.enabled)
                {
                    GUIUtility.systemCopyBuffer = FontInspect.instance.PrefabName;
                }
            }

        }

        public static bool isFontHelperEnabled = false;
        public static UnityEvent onFontsRefresh = new UnityEvent();

        /// Method to handle changes to configuration made by the player
        private void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            if (settingChangedEventArgs == null)
            {
                return;
            }

            // Example Float Shortcut setting changed handler
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == FloatExampleKey)
            {
                // TODO - Add your code here or remove this section if not required.
                // Code here to do something with the new value
            }

            // Example Int Shortcut setting changed handler
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == IntExampleKey)
            {
                // TODO - Add your code here or remove this section if not required.
                // Code here to do something with the new value
            }

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == KeyboardShortcutRereadTranslationFilesKey)
            {
                KeyboardShortcut newValue = (KeyboardShortcut)settingChangedEventArgs.ChangedSetting.BoxedValue;
            }

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == KeyboardShortcutRereadFontsFilesKey)
            {
                KeyboardShortcut newValue = (KeyboardShortcut)settingChangedEventArgs.ChangedSetting.BoxedValue;
            }

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == KeyboardShortcutEnableFontHelperKey)
            {
                KeyboardShortcut newValue = (KeyboardShortcut)settingChangedEventArgs.ChangedSetting.BoxedValue;
            }

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == KeyboardShortcutEnableCopyTextToClipboardKey)
            {
                KeyboardShortcut newValue = (KeyboardShortcut)settingChangedEventArgs.ChangedSetting.BoxedValue;
            }

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == KeyboardShortcutEnableCopyPathToClipboardKey)
            {
                KeyboardShortcut newValue = (KeyboardShortcut)settingChangedEventArgs.ChangedSetting.BoxedValue;
            }

        }

        public void Dispose()
        {
            Harmony.UnpatchSelf();
        }
        public static void SetGameFixedValues()
        {
            NewsTweak.Distance0Title = "Local".FromDictionary(true);

            NewsTweak.Distance1Title = "Neighboring".FromDictionary(true);

            NewsTweak.Distance2Title = "Nearby".FromDictionary(true);

            NewsTweak.Distance3Title = "Distant".FromDictionary(true);

            NewsTweak.Distance4Title = "Far away".FromDictionary(true);

            NewsTweak.Distance5Title = "Unbelievably far".FromDictionary(true);

            NewsTweak.Fresh0Title = "Fresh".FromDictionary(true);

            NewsTweak.Fresh1Title = "Recent".FromDictionary(true);

            NewsTweak.Fresh2Title = "Stale".FromDictionary(true);

            NewsTweak.Fresh3Title = "Outdated".FromDictionary(true);

            NewsTweak.Fresh4Title = "Very Old".FromDictionary(true);

            HuntingTweak.HuntingForagingTitle = "Acquire Supplies".FromDictionary(true);

            HuntingTweak.OddJobsTitle = "Odd Jobs".FromDictionary(true);

            BookUI.notesCloseQuestion = "Any changes you have done will get lost.\n Are you sure?".FromDictionary(true);
    }

    }
}
