using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace Game.GameActors.Units.Humans
{
    [System.Serializable]
    public class SkillManager : ICloneable
    {
        [SerializeField]
        public List<Skill> Skills;

        
        public SkillManager(SkillManager sm)
        {
            Skills = new List<Skill>();
            foreach (var skill in sm.Skills)
            {
                Skills.Add(skill);
            }
        }

        public bool HasSkill<T>()
        {
            return Skills.OfType<T>().Any();
        }

        public T GetSkill<T>()
        {
            foreach (var s in Skills.OfType<T>())
                return (T) Convert.ChangeType(s, typeof(T));
            return default;
        }

        public object Clone()
        {
            var clone = (SkillManager)MemberwiseClone();
            clone.Skills = new List<Skill>();
            foreach (Skill skill in Skills)
            {
                clone.Skills.Add(skill);
            }
            return clone;
        }
    }
}