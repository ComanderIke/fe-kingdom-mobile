using System.Collections.Generic;
using Game.GameActors.Units.Skills.Active;
using Game.GameActors.Units.Skills.Base;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.GameActors.Units.Skills.EffectMixins
{
    [CreateAssetMenu(menuName = "GameData/Skills/Effectmixin/BattleModifier", fileName = "BattleModifierSkillEffect")]
    public class BattleModifierSkillEffectMixin : SelfTargetSkillEffectMixin
    {
        public bool excessHitToCrit = false;
        public bool movementToDmg = false;
        public bool movementToCrit = false;
        public bool movementToAS = false;
        public bool desperationEffect = false;
        public bool vantage;
        [FormerlySerializedAs("doMagicDamage")] public bool dealMagicDamage;
        public int[] multiplier;

        public override void Activate(Unit user, int level)
        {
            Debug.Log("ACTIVATE BATTLE MODIFIER");
            user.BattleComponent.BattleStats.ExcessHitToCrit = excessHitToCrit;
            user.BattleComponent.BattleStats.MovementToDmg = movementToDmg;
            user.BattleComponent.BattleStats.MovementToAS = movementToAS;
            user.BattleComponent.BattleStats.MovementToCrit = movementToCrit;
            user.BattleComponent.BattleStats.DealMagicDamage = dealMagicDamage;
            if(movementToDmg)
                user.BattleComponent.BattleStats.MovementToDmgMultiplier = multiplier[level];
            if(movementToAS)
                user.BattleComponent.BattleStats.MovementToASMultiplier = multiplier[level];
            if(movementToCrit)
                user.BattleComponent.BattleStats.MovementToCritMultiplier = multiplier[level];
        }
        

        public override void Deactivate(Unit user, int skillLevel)
        {
            Debug.Log("DEACTIVATE BATTLE MODIFIER");
            user.BattleComponent.BattleStats.ExcessHitToCrit = false;
            user.BattleComponent.BattleStats.MovementToDmg = false;
            user.BattleComponent.BattleStats.MovementToAS = false;
            user.BattleComponent.BattleStats.MovementToCrit = false;
            user.BattleComponent.BattleStats.DealMagicDamage = false;
            user.BattleComponent.BattleStats.MovementToDmgMultiplier = 1;
            user.BattleComponent.BattleStats.MovementToASMultiplier = 1;
            user.BattleComponent.BattleStats.MovementToCritMultiplier = 1;
        }


        public override List<EffectDescription> GetEffectDescription(Unit caster,int level)
        {
            var list = new List<EffectDescription>();
            if (level < multiplier.Length)
            {
                string upg = ""+multiplier[level];
                if (level + 1 < multiplier.Length)
                    upg = ""+multiplier[level + 1];
                list.Add(new EffectDescription("Multiplier", ""+multiplier[level], upg));
            }
            return list;
        }

      
    }
}