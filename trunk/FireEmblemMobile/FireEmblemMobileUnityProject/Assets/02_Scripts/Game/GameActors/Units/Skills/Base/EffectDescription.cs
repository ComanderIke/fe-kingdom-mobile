using System;

namespace Game.GameActors.Units.Skills.Base
{
    [Serializable]
    public class EffectDescription:IEquatable<EffectDescription>
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
        

        public bool Equals(EffectDescription other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return String.Equals(label,other.label) && string.Equals(value,other.value) && string.Equals(upgValue,other.upgValue);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EffectDescription)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(label, value, upgValue);
        }
    }
}