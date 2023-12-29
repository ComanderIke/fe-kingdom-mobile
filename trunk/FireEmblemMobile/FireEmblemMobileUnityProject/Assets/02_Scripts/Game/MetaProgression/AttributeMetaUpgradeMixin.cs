using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using Game.GameActors.Units.Numbers;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/Attribute", fileName = "MetaUpgrade1")]
public class AttributeMetaUpgradeMixin : MetaUpgradeMixin
{
    public SerializableDictionary<AttributeType, int> attributes;
    public override void Activate(int level)
    {
        foreach (KeyValuePair<AttributeType, int> keyValuePair in attributes)
        {
            switch (keyValuePair.Key)
            {
                case AttributeType.AGI: Debug.Log("TODO Add stats ONLY to player Units");
                    //Either change all player UnitBPs directly or add the stats on unit creation.
                    //When starting the party AND when getting units later in the run those need to be added.
                    //Basically Initialize Player Unit Method where extra stats/equipment will be applied.
                    //Do it the same way as stat increases for Harder Difficulties? or not(they could just use different blueprints)
                    break;
            }
        }
    }
}