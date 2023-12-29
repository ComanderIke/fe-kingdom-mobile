using _02_Scripts.Game.GUI.Utility;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/PartyModifier", fileName = "MetaUpgrade1")]
public class PartyModifierMetaUpgradeMixin : MetaUpgradeMixin
{
    public int[] value;
    public PartyModifierType ModifierType;
    public SerializableDictionary<PartyFlag, bool> flags;
}

public enum PartyFlag
    {
        PartyMemberAfterArea1,
        PartyMemberAfterArea2,
        PartyMemberAfterArea3,
        PartyMemberAfterArea4,
        PartyMemberAfterArea5,
        PartyMemberAfterArea6
    
    }
    public enum PartyModifierType
    {
        PartyStartSize,
        PartyMaxSize,
        StorageSize,
        ConvoySize
    }
