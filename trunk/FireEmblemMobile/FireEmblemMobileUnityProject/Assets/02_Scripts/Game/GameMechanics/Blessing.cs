using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Blessing", fileName = "Blessing")]
    public class Blessing : ScriptableObject
    {
        [FormerlySerializedAs("skill")] public SkillBP skillBp;
        public string effectDescription;
        public string name;
        public string Description;
        
    }
}