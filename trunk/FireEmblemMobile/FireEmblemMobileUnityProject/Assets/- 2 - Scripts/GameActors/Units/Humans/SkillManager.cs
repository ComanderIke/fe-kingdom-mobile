using System;
using System.Collections.Generic;
using System.Linq;
using Assets.GameActors.Units.Skills;

namespace Assets.GameActors.Units.Humans
{
    public class SkillManager
    {
        public List<Skill> Skills;

        public SkillManager()
        {
            Skills = new List<Skill>();
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
    }
}