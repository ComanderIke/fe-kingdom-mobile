using System;

namespace LostGrace
{
    [Serializable]
    public class EffectDescription
    {
        public string label;
        public string value;
        public string upgValue;

        public EffectDescription(string label, string value,string upgValue)
        {
            this.label = label;
            this.value = value;
            this.upgValue = upgValue;
        }
    }
}