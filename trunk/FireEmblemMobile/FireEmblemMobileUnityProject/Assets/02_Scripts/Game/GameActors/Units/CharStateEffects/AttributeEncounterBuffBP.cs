using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Numbers;
using UnityEngine;


public class AttributeEncounterBuffBP : EncounterBasedBuffBP
{
    [SerializeField] private int value;
    [SerializeField] private AttributeType attributeType;
    public override EncounterBasedBuff Create()
    {
        return new AttributeEncounterBuff(duration,value,attributeType);
    }
}