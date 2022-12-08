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

    public void DecreaseDuration()
    {
        throw new System.NotImplementedException();
    }

    public int GetDuration(int faith)
    {
        throw new System.NotImplementedException();
    }

    public override void Apply(Unit unit)
    {
        unit.Stats.BonusAttributes.IncreaseAttribute(value, attributeType);
    }
}