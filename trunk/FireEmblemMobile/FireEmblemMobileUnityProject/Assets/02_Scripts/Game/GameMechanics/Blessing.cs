using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public abstract class Blessing : ScriptableObject
    {
        public Skill skill;
        public string effectDescription;
        public string name;
        public string Description;
        
    }
}