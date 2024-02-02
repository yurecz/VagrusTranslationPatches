using HarmonyLib;
using Vagrus.Data;
using System.IO;
using VagrusTranslationPatches.Utils;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

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

        [HarmonyPatch("LoadAsync")]
        [HarmonyPostfix]
        public static void LoadAsync_Postfix(ref IEnumerator __result, Action onLoadedCallback = null, Action<float> progressUpdater = null)
        {

            var myEnumerator = new SimplePostfixEnumerator() { enumerator = __result, progressUpdater = progressUpdater };
            myEnumerator.actions.Add(() =>
            {
                var dropDownButtonPrefab = Resources.Load<GameObject>("UI/Buttons/Prefab/DropDownButton");
                var fieldInfo = typeof(DropDownEntry).GetField("buttonPrefab", BindingFlags.NonPublic | BindingFlags.Static);
                fieldInfo.SetValue(null, dropDownButtonPrefab);
                dropDownButtonPrefab.UpdatePrefabFonts();
            });

            __result = myEnumerator.GetEnumerator();
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

    public class SimplePostfixEnumerator : IEnumerable
    {
        public IEnumerator enumerator;
        public List<Action> actions = new List<Action>();
        public Action<float> progressUpdater;
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator GetEnumerator()
        {
            // you can call "action" at any time here but you need to call MoveNext until it returns false
            while (enumerator!=null && enumerator.MoveNext())
            {
                yield return enumerator.Current;
            } ;

            int i = 0;
            foreach (var action in actions)
            {
                i++;
                progressUpdater(i / actions.Count());
                yield return action;
            }
        }
    }
}