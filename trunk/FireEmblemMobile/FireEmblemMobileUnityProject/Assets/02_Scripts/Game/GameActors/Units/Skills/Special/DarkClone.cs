using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Special
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/DarkClone", fileName = "DarkClone")]
    public class DarkClone : SelfTargetSkill
    {

        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }
        
    }
}