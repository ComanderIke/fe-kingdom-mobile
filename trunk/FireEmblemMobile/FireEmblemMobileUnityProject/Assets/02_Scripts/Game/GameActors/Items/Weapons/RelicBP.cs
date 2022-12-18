using System;
using System.Linq;
using Game.GameActors.Items.Gems;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Passive;
using Game.GameResources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Relic", fileName = "Relic")]
    public class RelicBP : EquipableItemBP
    {
       
        [Header("RelicAttributes")] 
        public int slotCount=1;

      

        public override Item Create()
        {
            Debug.Log("TODO Do random generation");
            var attributes = new Attributes();
            Array values = Enum.GetValues(typeof(RelicPassiveEffectType));
            RelicPassiveEffectType passiveEffect = RelicPassiveEffectType.None;
            Skill activeSkill = null;
            if (rarity == 3)
            {
                Debug.Log("Choose Random Skill from Pool");
                activeSkill = GameBPData.Instance.GetRandomRelicSkill();
            }

            if (rarity >= 2)
            {
                Debug.Log("Generate Passive Effect");
                passiveEffect = (RelicPassiveEffectType)values.GetValue(Random.Range(0, values.Length));
            }

            if (rarity >= 1)
            {
                int attribute=Random.Range(0, 9);
                switch (attribute)
                {
                    case 0: 
                    attributes.IncreaseAttribute(Random.Range(1,4), AttributeType.STR);
                    break;
                    case 1: 
                        attributes.IncreaseAttribute(Random.Range(1,4), AttributeType.DEX);
                        break;
                    case 2: 
                        attributes.IncreaseAttribute(Random.Range(1,4), AttributeType.INT);
                        break;
                    case 3: 
                        attributes.IncreaseAttribute(Random.Range(1,4), AttributeType.AGI);
                        break;
                    case 4: 
                        attributes.IncreaseAttribute(Random.Range(1,4), AttributeType.CON);
                        break;
                    case 5: 
                        attributes.IncreaseAttribute(Random.Range(1,4), AttributeType.LCK);
                        break;
                    case 6: 
                        attributes.IncreaseAttribute(Random.Range(1,4), AttributeType.DEF);
                        break;
                    case 7: 
                        attributes.IncreaseAttribute(Random.Range(1,4), AttributeType.FTH);
                        break;
                }
               
            }
            return new Relic(name, description, cost, rarity,maxStack,sprite, equipmentSlotType, attributes, passiveEffect, activeSkill, slotCount);
        }
    }

    public enum RelicPassiveEffectType
    {
        None,
        Avoid,
        Crit,
        Regen
    }
    
}