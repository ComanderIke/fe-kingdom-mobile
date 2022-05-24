using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/SingleCharacterTarget", fileName = "SingleCharacterTarget")]
    public class SingleCharacterTarget:SingleTargetSkill
    {
        public override int GetDamage(Unit user, bool justToShow)
        {
            return 1;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return true;
        }
    }
}