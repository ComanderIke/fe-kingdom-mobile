using System.Collections.Generic;
using LostGrace;
using MoreMountains.Tools;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/BattleModifier", fileName = "BattleModifierSkillEffect")]
    public class BattleModifierSkillEffectMixin : SelfTargetSkillEffectMixin
    {
        public bool excessHitToCrit = false;
        public bool movementToDmg = false;
        public bool desperationEffect = false;
        public bool vantage;

        public override void Activate(Unit user, int level)
        {
            Debug.Log("ACTIVATE BATTLE MODIFIER");
            user.BattleComponent.BattleStats.ExcessHitToCrit = excessHitToCrit;
            user.BattleComponent.BattleStats.MovementToDmg = movementToDmg;
        }
        

        public override void Deactivate(Unit user, int skillLevel)
        {
            Debug.Log("DEACTIVATE BATTLE MODIFIER");
            user.BattleComponent.BattleStats.ExcessHitToCrit = false;
            user.BattleComponent.BattleStats.MovementToDmg = false;
        }


        public override List<EffectDescription> GetEffectDescription(int level)
        {
            return new List<EffectDescription>();
        }

      
    }
}