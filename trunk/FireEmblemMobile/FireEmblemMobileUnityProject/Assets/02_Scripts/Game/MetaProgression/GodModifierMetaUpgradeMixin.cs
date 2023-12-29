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
        throw new System.NotImplementedException();
    }
}
public enum GodModifierType
{
    BondExp,
    MaxBondLevelINT
}