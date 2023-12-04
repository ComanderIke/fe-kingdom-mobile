using System;
using System.Collections.Generic;
using _02_Scripts.Game.GUI.Utility;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    public abstract class SkillMixin : ScriptableObject
    {
        public const int MAXLEVEL = 5;
        [NonSerialized]public Skill skill;
        [SerializeField]public SerializableDictionary<BlessingBP,SynergieEffects> synergies;

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
        protected BlessingBP GetKey(Unit user)
        {
            if (user.Blessing == null)
                return null;
            foreach (var key in synergies.Keys)
            {
                if (key.Name == user.Blessing.Name)
                    return key;
            }

            return null;
        }
        protected bool HasSynergy(Unit user)
        {
            foreach (var key in synergies.Keys)
            {
                if (key.Name == user.Blessing.Name)
                    return true;
            }

            return false;
        }
       
    }
}