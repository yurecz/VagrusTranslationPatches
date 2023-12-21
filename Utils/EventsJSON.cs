using System;
using System.Collections.Generic;

namespace VagrusTranslationPatches.Utils
{
    [Serializable]
    public struct EventJSON
    {
        public string uid;

        public string title;

        public string description;

        public StepJSON[] steps;
    }

    [Serializable]
    public struct StepJSON
    {
        public string uid;

        public string description;

        public ChoiceJSON[] choices;

        public bool Reviewed;
    }

    [Serializable]
    public struct ChoiceJSON
    {
        public int id;

        public string description;

        public bool Reviewed;
    }

    [Serializable]
    public class EventsJSON
    {
        public EventJSON[] events;
    }

}
