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
                case GodModifierType.MaxBondLevelINT:
                    keyValuePair.Key.MaxBondLevel = (int)percentage[level];
                    break;
            }
        }
    }
}
public enum GodModifierType
{
    BondExp,
    MaxBondLevelINT
}