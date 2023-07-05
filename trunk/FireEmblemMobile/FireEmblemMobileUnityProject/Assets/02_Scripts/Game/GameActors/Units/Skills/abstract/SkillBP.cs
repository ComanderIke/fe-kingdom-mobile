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
        public int MaxLevel = 1;
        public string Description;
        public string Name;
        public int Tier;

        public virtual Skill Create()
        {
            return new Skill(Name, Description, Icon, Tier);
        }
    }
}