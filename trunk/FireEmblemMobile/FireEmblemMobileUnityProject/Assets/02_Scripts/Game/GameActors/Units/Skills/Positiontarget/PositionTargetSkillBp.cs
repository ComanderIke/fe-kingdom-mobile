using System;
using System.Collections.Generic;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Players;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName="GameData/Skills/PositionTarget", fileName = "PositionTargetSkill")]
    public class PositionTargetSkillBp : ActivatedSkillBp
    {
        public int power;
        public int range;
        public int size;
        
        public SkillTargetArea targetArea;
        public EffectType effectType;
        public bool rooted;
        
        public override Skill Create()
        {
            return new PositionTargetSkill(Name, Description, Icon, AnimationObject,Cooldown,Tier, UpgradeDescriptions,hpCost,Uses,power, range, size, targetArea,effectType, rooted);
        }
    }
    
}