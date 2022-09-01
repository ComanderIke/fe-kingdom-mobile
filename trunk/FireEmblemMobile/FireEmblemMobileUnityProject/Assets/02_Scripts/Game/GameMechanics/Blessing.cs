using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Blessing", fileName = "Blessing")]
    public class Blessing : ScriptableObject
    {
        public Skill skill;
        public string effectDescription;
        public string name;
        public string Description;
        
    }
}