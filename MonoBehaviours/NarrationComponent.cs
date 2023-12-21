using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace VagrusTranslationPatches.MonoBehaviours
{
    internal class NarrationComponent : MonoBehaviour
    {

        delegate void Callback(AudioSource audioSource);

        private ValueTuple<string, AudioSource> narration;

        public void Awake()
        {
            this.narration = ValueTuple.Create("", this.gameObject.AddComponent<AudioSource>());
            TranslationPatchesPlugin.Log.LogMessage("Нарратив загружен.");
        }

        private IEnumerator LoadNarration(string narrationID, Callback callback)
        {
            AudioSource audioSource = this.narration.Item2;
            List<LangPackData> list;
            if (narrationID != this.narration.Item1 && Game.LoadedLanguagePacks.TryGetValue("ru", out list))
            {
                long currentLanguagePackID = Game.GetCurrentLanguagePackID(false);
                LangPackData langPackData = list.Find((LangPackData item) => item.ID == currentLanguagePackID) ?? list[0];
                string text = ((langPackData.ID == -1L) ? Path.Combine(langPackData.InstallPath, "reviewed") : Path.Combine(langPackData.InstallPath, "lang"));
                if (Directory.Exists(text))
                {
                    string text2 = Path.Combine(text, narrationID);
                    if (File.Exists(text2))
                    {
                        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(text2, AudioType.MPEG);
                        yield return www.SendWebRequest();
                        if (www.result != UnityWebRequest.Result.ProtocolError)
                        {
                            audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                        }
                    }
                }
            }
            audioSource.volume = 1f;
            callback(audioSource);
            yield break;
        }

        public void StopNarration()
        {
            if (this.narration.Item2.isPlaying)
            {
                StartCoroutine(Sound.game.sound.FadeAudioSOurceVolume(this.narration.Item2, 0f, 3f, 0f));
            }
        }

        public void PlayNarration(string narrationId, float delay = 0f, bool loop = false, float volume = 1f, float pan = 0f)
        {
            StartCoroutine(this.LoadNarration(narrationId, delegate (AudioSource audioSource)
            {
                Game.game.sound.PlaySfx(audioSource, AudioGroup.Narration, delay, loop, volume, pan);
            }));
        }
    }
}
