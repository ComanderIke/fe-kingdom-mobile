using System;
using System.Linq;
using Game.GameActors.Items.Gems;
using Game.GameActors.Items.Relics;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Numbers;
using Game.GameActors.Units.Skills;
using Game.GameActors.Units.Skills.Base;
using Game.GameActors.Units.Skills.Passive;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.GameActors.Items.Weapons
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Items/Relic", fileName = "Relic")]
    public class RelicBP : EquipableItemBP
    {

        [SerializeField] private SkillBp skillBp;
        [SerializeField] private GemBP startGem;

        public override Item Create()
        {
            Skill instSkill = null;
            if (skillBp != null)
                instSkill = skillBp.Create();
            Gem gem = null;
            if (startGem != null)
                 gem = (Gem)startGem.Create();
            return new Relic(name, description, cost, rarity,maxStack,sprite,  instSkill, gem);
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