using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class SkillMixin : ScriptableObject
    {
        public const int MAXLEVEL = 5;
        [NonSerialized]public Skill skill;

        protected bool bound = false;

        public virtual void BindToUnit(Unit unit, Skill skill)
        {
            this.skill = skill;
            bound = true;
            if(skill.skillTransferData!=null)
                skill.skillTransferData.data = null;
        }

        public virtual void UnbindFromUnit(Unit unit, Skill skill)
        {
            if(skill.skillTransferData!=null)
                skill.skillTransferData.data = null;
            this.skill = null;//TODO is this right?
            bound = false;

        }
       
    }
}