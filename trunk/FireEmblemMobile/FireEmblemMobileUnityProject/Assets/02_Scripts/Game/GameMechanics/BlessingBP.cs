using Game.GameActors.Units.Skills;
using UnityEngine;
using UnityEngine.Serialization;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameData/Blessing", fileName = "Blessing")]
    public class BlessingBP : ScriptableObject
    {
        [FormerlySerializedAs("skill")] public SkillBP skillBp;
        public string blessingName;
        public string Description;
        public int tier=3;
       //faith / 2 minimum 3

        public Blessing Create()
        {
            return new Blessing(blessingName, Description, skillBp.Icon, skillBp.AnimationObject, tier, skillBp.UpgradeDescriptions);
        }

    }
}