using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills.Base;
using Game.GUI.Utility;
using UnityEngine;

namespace Game.MetaProgression
{
    [CreateAssetMenu(menuName = "GameData/Upgrades/MetaUpgrade/AttributeGrowth", fileName = "MetaUpgrade1")]
    public class AttributeGrowthMetaUpgradeMixin : MetaUpgradeMixin
    {
        public SerializableDictionary<AttributeType, int> attributes;
        public override void Activate(int level)
        {
            GameConfig.BonusAttributeGrowths = new Attributes();
            foreach (KeyValuePair<AttributeType, int> keyValuePair in attributes)
            {
                GameConfig.BonusAttributeGrowths.IncreaseAttribute(keyValuePair.Value, keyValuePair.Key);
                // switch (keyValuePair.Key)
                // {
                //     case AttributeType.AGI: Debug.Log("TODO Add stats ONLY to player Units");
                //         //Either change all player UnitBPs directly or add the stats on unit creation.
                //         //When starting the party AND when getting units later in the run those need to be added.
                //         //Basically Initialize Player Unit Method where extra stats/equipment will be applied.
                //         //Do it the same way as stat increases for Harder Difficulties? or not(they could just use different blueprints)
                //         break;
                // }
            }
        }

        public override IEnumerable<EffectDescription> GetEffectDescriptions(int level)
        {
            var list = new List<EffectDescription>();
            foreach (var entry in attributes)
            {
                list.Add(new EffectDescription(""+entry.Key+" Growth", ((entry.Value>0)?"+":"")+entry.Value+"%", ((entry.Value>0)?"+":"")+entry.Value+"%"));
            }
       
            return list;
        }
    }
}