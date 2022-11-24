using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Special
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/DarkClone", fileName = "DarkClone")]
    public class DarkCloneSkillBP : SelfTargetSkillBp
    {

        public override Skill Create()
        {
            return new DarkClone();
        }
    }
}