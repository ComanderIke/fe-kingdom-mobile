using Game.GameActors.Units.Numbers;
using UnityEngine;

namespace Game.GameActors.Units.CharStateEffects
{
    public class AttributeEncounterBuffBP : EncounterBasedBuffBP
    {
        [SerializeField] private int value;
        [SerializeField] private AttributeType attributeType;
        public override EncounterBasedBuff Create()
        {
            return new AttributeEncounterBuff(duration,value,attributeType);
        }
    }
}