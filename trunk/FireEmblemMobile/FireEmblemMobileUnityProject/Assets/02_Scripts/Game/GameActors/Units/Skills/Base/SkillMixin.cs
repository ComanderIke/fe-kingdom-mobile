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
            skill.skillTransferData = null;
        }

        public virtual void UnbindFromUnit(Unit unit, Skill skill)
        {
            skill.skillTransferData = null;
            this.skill = null;//TODO is this right?
            
        }
       
    }
}