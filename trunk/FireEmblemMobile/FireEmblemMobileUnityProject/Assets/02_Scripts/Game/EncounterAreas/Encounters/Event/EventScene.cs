using System;
using System.Collections.Generic;

namespace Game.EncounterAreas.Encounters.Event
{
    [Serializable]
    public class EventScene
    {
        public List<ResponseOption> textOptions;
        public string MainText;
        public EventScene(string mainText, List<ResponseOption> textOptions)
        {
            this.MainText = mainText;
            this.textOptions = textOptions;
        }
    }
}