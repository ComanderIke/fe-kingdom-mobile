using System;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Buffs
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/AllyTargetBuff", fileName = "AllyTargetBuff")]
    public class AllyTargetBuff : SingleTargetSkill
    {
        [SerializeField]
        public Buff appliedBuff;
        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return user.Faction.Id == target.Faction.Id;
        }
    }
}