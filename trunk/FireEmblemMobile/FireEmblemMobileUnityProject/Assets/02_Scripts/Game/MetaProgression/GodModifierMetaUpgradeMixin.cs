using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using LostGrace;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/GodModifier", fileName = "MetaUpgrade1")]
public class GodModifierMetaUpgradeMixin: MetaUpgradeMixin
{
    public float[] percentage;
    public SerializableDictionary<God,GodModifierType> godModifierType;

    public override void Activate(int level)
    {
        foreach (KeyValuePair<God, GodModifierType> keyValuePair in godModifierType)
        {
            switch (keyValuePair.Value)
            {
                case GodModifierType.BondExp:
                    keyValuePair.Key.BondExpRate = percentage[level];
                    break;
            }
        }
    }
    
    public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
    {
        var list = new List<EffectDescription>();
        if (level >= percentage.Length)
            return list;
        int upgLevel = level;
        if (upgLevel < percentage.Length-1)
            upgLevel++;
        foreach (var entry in godModifierType)
        {
            //list.Add(new EffectDescription(""+entry.Key.name+": ", "", ""));
            list.Add(new EffectDescription("Bond exp: ","+"+(percentage[level]*100-100).ToString("0")+"%", "+"+(percentage[upgLevel]*100-100).ToString("0")+"%"));

          
        }

   
        
        return list;
    
    }
}
public enum GodModifierType
{
    BondExp
}