using System;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/AOE_Damage_Magic", fileName = "AOEDamageMagic")]
    public class AOEMagicSkillBP : PositionTargetSkillBp
    {
        private DamageType damageType;
      
    }
}