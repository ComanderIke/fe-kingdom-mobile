using System;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Buffs
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/SelfTargetBuff", fileName = "SelfTargetBuff")]
    public class SelfTargetBuff:SingleTargetSkill
    {
        [SerializeField]
        public Buff appliedBuff;
        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return user == target;
        }
    }
}