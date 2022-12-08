using Game.GameActors.Units.CharStateEffects;
using Game.GameActors.Units.Numbers;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/EncounterBuffs/AttributeBuff")]
public class AttributeEncounterBuffBP : EncounterBasedBuffBP
{
    [SerializeField] private int value;
    [SerializeField] private AttributeType attributeType;
    public override EncounterBasedBuff Create()
    {
        return new AttributeEncounterBuff(duration,value,attributeType);
    }
}