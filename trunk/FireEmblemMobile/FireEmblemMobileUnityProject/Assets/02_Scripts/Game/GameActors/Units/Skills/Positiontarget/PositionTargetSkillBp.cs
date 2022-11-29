﻿using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units.Numbers;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName="GameData/Skills/PositionTarget", fileName = "PositionTargetSkill")]
    public class PositionTargetSkillBp : SkillBP
    {
        public int power;
        public int range;
        public int size;
        
        public SkillTargetArea targetArea;
        public bool rooted;
        
        public override Skill Create()
        {
            return new PositionTargetSkill(Name, Description, Icon, AnimationObject,Cooldown,UpgradeDescriptions,power, range, size, targetArea, rooted);
        }
    }
    
}