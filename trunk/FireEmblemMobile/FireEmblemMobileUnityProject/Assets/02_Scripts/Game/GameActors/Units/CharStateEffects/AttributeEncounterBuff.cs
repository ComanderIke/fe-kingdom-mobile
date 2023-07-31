using Game.GameActors.Units;
using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Numbers;
using LostGrace;
using UnityEngine;

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