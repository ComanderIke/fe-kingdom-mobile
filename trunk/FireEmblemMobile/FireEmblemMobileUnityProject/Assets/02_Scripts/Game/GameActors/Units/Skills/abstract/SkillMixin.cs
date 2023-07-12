using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class SkillMixin : ScriptableObject
    {
        public const int MAXLEVEL = 5;
        [NonSerialized]public Skill skill;
     

        public virtual void BindToUnit(Unit unit, Skill skill)
        {
            this.skill = skill;
        }

        public virtual void UnbindFromUnit(Unit unit, Skill skill)
        {
            this.skill = this.skill;
        }
       
    }
}