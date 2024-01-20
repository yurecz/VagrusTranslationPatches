using HarmonyLib;
using Vagrus.Data;
using System.IO;
using VagrusTranslationPatches.Utils;
using Newtonsoft.Json;

namespace VagrusTranslationPatches.Patches
{

    [HarmonyPatch(typeof(PoolLoader))]
    public class PoolLoaderPatches
    {


        [HarmonyPatch("LoadLocalization")]
        [HarmonyPostfix]
        public static void LoadLocalization_Postfix()
        {
            TranslationPatchesPlugin.SetGameFixedValues();
        }

        [HarmonyPatch(nameof(PoolLoader.LoadEventTranslationFromFile))]
        [HarmonyPostfix]
        public static void LoadEventTranslationFromFile_Postfix(ref bool __result, string path, bool secondary = false)
        {
            if (__result)
            {

                string json = File.ReadAllText(path);

                var eventsJSON = JsonConvert.DeserializeObject<EventsJSON>(json);
                if (eventsJSON.events != null) {
                    for (int i = 0; i < eventsJSON.events.Length; i++)
                    {
                        GameEvent gameEvent = AirTableItem<GameEvent>.FindByUID(eventsJSON.events[i].uid);
                        if (gameEvent != null)
                        {
                            if (eventsJSON.events[i].title != "")
                            {
                                gameEvent.modtitle = "<b>" + eventsJSON.events[i].title + "</b>";
                            }
                            gameEvent.description = eventsJSON.events[i].description ?? gameEvent.description;
                        }
                    }
                }
            }
        }
    }
}