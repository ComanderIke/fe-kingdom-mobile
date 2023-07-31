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

        [SerializeField] private SkillBp skillBp;
      

        public override Item Create()
        {
            return new Relic(name, description, cost, rarity,maxStack,sprite,  skillBp.Create(), slotCount);
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