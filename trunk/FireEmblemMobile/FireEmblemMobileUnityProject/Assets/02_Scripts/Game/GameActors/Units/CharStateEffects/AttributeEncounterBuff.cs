using Game.GameActors.Units.Numbers;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public class AttributeEncounterBuff : EncounterBasedBuff, ITemporaryEffect{
 
        [SerializeField] private AttributeType attributeType;
        [SerializeField] private int value;
        public AttributeEncounterBuff(int duration, int value, AttributeType attributeType):base (duration)
        {
            this.attributeType = attributeType;
            this.value = value;
        }

        public override void Apply(Unit unit)
        {
            unit.Stats.BonusAttributesFromEffects.IncreaseAttribute(value, attributeType);
        }
    }
}