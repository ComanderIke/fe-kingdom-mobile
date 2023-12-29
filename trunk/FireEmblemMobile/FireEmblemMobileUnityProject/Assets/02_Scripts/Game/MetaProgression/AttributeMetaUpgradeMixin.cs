using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Units.Numbers;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/Attribute", fileName = "MetaUpgrade1")]
public class AttributeMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<AttributeType, int> attributes;
    public override void Activate(int level)
    {
        throw new System.NotImplementedException();
    }
}