using System;
using Game.GameActors.Units.CharStateEffects;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Debuff
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/EnemyTargetDebuff", fileName = "EnemyDebuff")]
    public class EnemyTargetDebuff : SingleTargetSkill
    {
        [SerializeField]
        public CharStateEffects.Debuff appliedDebuff;
        public override int GetDamage(Unit user, bool justToShow)
        {
            return 0;
        }

        public override bool CanTarget(Unit user, Unit target)
        {
            return user.Faction.Id != target.Faction.Id;
        }
    }
}