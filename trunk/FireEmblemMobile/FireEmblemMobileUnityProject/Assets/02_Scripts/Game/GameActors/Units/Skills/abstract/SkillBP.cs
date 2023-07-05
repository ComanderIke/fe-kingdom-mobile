using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(fileName = "Skill", menuName = "GameData/Skills/Skill")]
    public class SkillBp:  ScriptableObject
    {
        public Sprite Icon;
        public string Description;
        public string Name;
        public int Tier;
        public List<PassiveSkillMixin> passiveMixins;
        public ActiveSkillMixin activeMixin;

        public virtual Skill Create()
        {
            return new Skill(Name, Description, Icon, Tier, passiveMixins, activeMixin);
        }
    }
}