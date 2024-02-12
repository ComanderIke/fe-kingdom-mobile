using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Players;
using LostGrace;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/PartyModifier", fileName = "MetaUpgrade1")]
public class PartyModifierMetaUpgradeMixin : MetaUpgradeMixin
{
    public int[] value;
    public PartyModifierType ModifierType;
    public SerializableDictionary<PartyFlag, bool>[] flags;
    public override void Activate(int level)
    {
        switch (ModifierType)
        {
            case PartyModifierType.PartyMaxSize:
                Player.Instance.Party.MaxSize = value[level];
                break;
            case PartyModifierType.PartyStartSize:
                Player.Instance.Party.MaxSize = value[level];
                break;
            case PartyModifierType.ConvoySize:
                Player.Instance.Party.Convoy.MaxItems = value[level];
                break;
            case PartyModifierType.StorageSize:
                Player.Instance.Party.Storage.MaxItems = value[level];
                break;
            
        }

        foreach (KeyValuePair<PartyFlag, bool> valuePair in flags[level])
        {
            switch (valuePair.Key)
            {
                case PartyFlag.PartyMemberAfterArea1:
                    Player.Instance.Flags.PartyMemberAfterArea1 = valuePair.Value;
                    break;
                case PartyFlag.PartyMemberAfterArea2:
                    Player.Instance.Flags.PartyMemberAfterArea2 = valuePair.Value;
                    break;
                case PartyFlag.PartyMemberAfterArea3:
                    Player.Instance.Flags.PartyMemberAfterArea3 = valuePair.Value;
                    break;
                case PartyFlag.PartyMemberAfterArea4:
                    Player.Instance.Flags.PartyMemberAfterArea4 = valuePair.Value;
                    break;
                case PartyFlag.PartyMemberAfterArea5:
                    Player.Instance.Flags.PartyMemberAfterArea5 = valuePair.Value;
                    break;
                case PartyFlag.PartyMemberAfterArea6:
                    Player.Instance.Flags.PartyMemberAfterArea6 = valuePair.Value;
                    break;
            }
        }
    }
    public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        
        int upgLevel = level;
        if (upgLevel < value.Length-1)
            upgLevel++;
        list.Add(new EffectDescription(""+ModifierType, ""+value[level], ""+value[upgLevel]));

       
        return list;
    }
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
