using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Blessing", fileName = "Blessing")]
    public class BlessingBP : ScriptableObject
    {
        [FormerlySerializedAs("skill")] public SkillBP skillBp;
        public string name;
        public string Description;
        public int tier=3;
       //faith / 2 minimum 3

        public Blessing Create()
        {
            return new Blessing(skillBp.Create(), name, Description, tier);
        }

    }
}