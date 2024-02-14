using System.Collections.Generic;
using System.Linq;
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
                case PartyFlag.Area1:
                    Player.Instance.Flags.PartyMemberAfterArea1 = valuePair.Value;
                    break;
                case PartyFlag.Area2:
                    Player.Instance.Flags.PartyMemberAfterArea2 = valuePair.Value;
                    break;
                case PartyFlag.Area3:
                    Player.Instance.Flags.PartyMemberAfterArea3 = valuePair.Value;
                    break;
                case PartyFlag.Area4:
                    Player.Instance.Flags.PartyMemberAfterArea4 = valuePair.Value;
                    break;
                case PartyFlag.Area5:
                    Player.Instance.Flags.PartyMemberAfterArea5 = valuePair.Value;
                    break;
                case PartyFlag.Area6:
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
        if(flags.Length==0)
            list.Add(new EffectDescription(""+ModifierType.EnumToString()+":", "+"+value[level], ""+value[upgLevel]));
        else
            foreach (var flag in flags[level])
            {
                list.Add(new EffectDescription(""+flag.Key.EnumToString()+":", "+"+value[0], ""+value[0]));
            }

       
        return list;
    }
}

public enum PartyFlag
    {
        Area1,
        Area2,
        Area3,
        Area4,
        Area5,
        Area6,
        Area7
    
    }
    public enum PartyModifierType
    {
        PartyStartSize,
        PartyMaxSize,
        StorageSize,
        ConvoySize
    }
