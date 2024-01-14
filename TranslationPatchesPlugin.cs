using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Linq;
using UnityEngine;
using VagrusTranslationPatches.Patches;
using VagrusTranslationPatches.Utils;


namespace VagrusTranslationPatches
{
    // TODO Review this file and update to your own requirements.

    [BepInPlugin(MyGUID, PluginName, VersionString)]
    public class TranslationPatchesPlugin : BaseUnityPlugin
    {
        // Mod specific details. MyGUID should be unique, and follow the reverse domain pattern
        // e.g.
        // com.mynameororg.pluginname
        // Version should be a valid version string.
        // e.g.
        // 1.0.0
        private const string MyGUID = "ru.Vagrus.TranslationPatches";
        private const string PluginName = "TranslationPatches";
        private const string VersionString = "0.3.0";

        // Config entry key strings
        // These will appear in the config file created by BepInEx and can also be used
        // by the OnSettingsChange event to determine which setting has changed.
        public static string FloatExampleKey = "Float Example Key";
        public static string IntExampleKey = "Int Example Key";
        public static string KeyboardShortcutRereadTranslationFilesKey = "Заново прочитать файлы перевода";

        // Configuration entries. Static, so can be accessed directly elsewhere in code via
        // e.g.
        // float myFloat = VagrusTestModePlugin.FloatExample.Value;
        // TODO Change this code or remove the code if not required.
        public static ConfigEntry<float> FloatExample;
        public static ConfigEntry<int> IntExample;
        public static ConfigEntry<KeyboardShortcut> KeyboardShortcutRereadTranslationFiles;

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

            // Add listeners methods to run if and when settings are changed by the player.
            //FloatExample.SettingChanged += ConfigSettingChanged;
            //IntExample.SettingChanged += ConfigSettingChanged;
            KeyboardShortcutRereadTranslationFiles.SettingChanged += ConfigSettingChanged;

            //new Translators();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loading...");
            Harmony.PatchAll();
            Logger.LogInfo($"PluginName: {PluginName}, VersionString: {VersionString} is loaded.");

            Log = Logger;

            SetGameFixedValues();
        }

        private static void SetGameFixedValues()
        {
            NewsTweak.Distance0Title = "Local".FromDictionary();

            NewsTweak.Distance1Title = "Neighboring".FromDictionary();

            NewsTweak.Distance2Title = "Nearby".FromDictionary();

            NewsTweak.Distance3Title = "Distant".FromDictionary();

            NewsTweak.Distance4Title = "Far away".FromDictionary();

            NewsTweak.Distance5Title = "Unbelievably far".FromDictionary();

            NewsTweak.Fresh0Title = "Fresh".FromDictionary();

            NewsTweak.Fresh1Title = "Recent".FromDictionary();

            NewsTweak.Fresh2Title = "Stale".FromDictionary();

            NewsTweak.Fresh3Title = "Outdated".FromDictionary();

            NewsTweak.Fresh4Title = "Very Old".FromDictionary();
        }

        /// <summary>
        /// Code executed every frame.
        /// </summary>
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
                        if (Game.GetActiveEvent() != null && gameEvent.curStep!=null)
                        {
                            gameEvent.eventUI.RemoveUnusedChoicesFromLog(-1);
                            gameEvent.SelectStep(gameEvent.curStep);
                            Logger.LogInfo("Событие "+ gameEvent.GetTitle()  + " обновлено.");
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
                    Logger.LogInfo("Файлы перевода непрочитаны, так как перевод не включен в настройках игры.");
                }
            }
        }

        /// <summary>
        /// Method to handle changes to configuration made by the player
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            // Check if null and return
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

            // Example Keyboard Shortcut setting changed handler
            if (settingChangedEventArgs.ChangedSetting.Definition.Key == KeyboardShortcutRereadTranslationFilesKey)
            {
                KeyboardShortcut newValue = (KeyboardShortcut)settingChangedEventArgs.ChangedSetting.BoxedValue;

                // TODO - Add your code here or remove this section if not required.
                // Code here to do something with the new value
            }
        }
    }
}
