using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    public abstract class SkillBP: ScriptableObject
    {
        
        public Sprite Icon;

        public GameObject AnimationObject;

        public int Cooldown;

        public int CooldownReduction { get; private set; } = 0;

        public int CurrentCooldown { get; private set; }
        public int MaxLevel = 1;

        public string Description;

        public int Level;

        public string Name;


   
    }
}