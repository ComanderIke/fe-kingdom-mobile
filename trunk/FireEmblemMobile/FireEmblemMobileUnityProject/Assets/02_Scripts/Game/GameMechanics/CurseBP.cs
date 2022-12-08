using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Curse", fileName = "Curse")]
    public class CurseBP : ScriptableObject
    {
        [FormerlySerializedAs("skill")] public SkillBP skillBp;
        public string name;
        public string Description;
        public int tier=3;
        //faith / 2 minimum 3

        public Curse Create()
        {
            return new Curse(skillBp.Create(), name, Description, tier);
        }

    }
}