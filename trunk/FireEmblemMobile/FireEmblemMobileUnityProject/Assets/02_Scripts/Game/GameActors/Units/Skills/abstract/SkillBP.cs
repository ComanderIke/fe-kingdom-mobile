using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public abstract class SkillBP:  ScriptableObject
    {
        public Sprite Icon;
        public GameObject AnimationObject;
        public int Cooldown;
        public int MaxLevel = 1;
        public string Description;
        public string[] UpgradeDescriptions;
        public string Name;

        public abstract Skill Create();
    }
}