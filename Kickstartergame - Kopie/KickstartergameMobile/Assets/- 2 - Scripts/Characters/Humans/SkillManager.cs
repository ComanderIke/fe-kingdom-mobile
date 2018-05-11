using Assets.Scripts.Characters.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Characters
{
    public class SkillManager
    {
        public List<Skill> skills;

        public SkillManager()
        {
            skills = new List<Skill>();
        }

        public bool HasSkill<T>()
        {
            foreach (Skill s in skills)
            {
                if (s is T)
                {
                    return true;
                }
            }
            return false;
        }

        public T GetSkill<T>()
        {
            foreach (Skill s in skills)
            {
                if (s is T)
                {
                    return (T)Convert.ChangeType(s, typeof(T));
                }
            }
            return default(T);
        }
    }
}
