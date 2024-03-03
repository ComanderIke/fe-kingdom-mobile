using System;
using Game.GameActors.Units.Skills.Base;
using Game.GUI.Skills;
using UnityEngine;

namespace Game.GameActors.Units.Humans
{
    [System.Serializable]
    public class SkillTreeEntry
    {
        public int levelRequirement;

        public int row;
        // public int[] statRequirements;
        //public Skill[] skillRequirements;
        [SerializeField] private SkillBp skillBp;
        public SkillState SkillState = SkillState.NotLearnable;
        [NonSerialized]public SkillTree tree;

        private Skill skill;
        public Skill Skill
        {
            get
            {
                if (skill == null&&skillBp!=null)
                    skill = skillBp.Create();
                return skill;
            }
        }



    }
}