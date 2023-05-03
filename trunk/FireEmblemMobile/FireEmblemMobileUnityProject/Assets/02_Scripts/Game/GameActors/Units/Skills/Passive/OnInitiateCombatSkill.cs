using System;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
   
    public class OnInitiateCombatSkill:PassiveSkill
    {
        public Attributes BonusAttributes;
        public BonusStats BonusStats;
        public OnInitiateCombatSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescr) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
        }

        public override void BindSkill(Unit unit)
        {
            // unit.BattleComponent.OnEndCombat += ReactToEndCombat;
            // unit.BattleComponent.OnInitiateCombat += ReactToInitiateCombat;
        }
        public override void UnbindSkill(Unit unit)
        {
            // unit.BattleComponent.OnEndCombat -= ReactToEndCombat;
            // unit.BattleComponent.OnInitiateCombat -= ReactToInitiateCombat;
        }
        private void ReactToEndCombat(Unit unit)
        {
            //unit.Stats.BonusAttributes.IncreaseAttribute(BonusAttributes.DEF, ;
            //unit.BattleComponent.BattleStats.BonusStats.Hit -= BonusStats.Hit;
        }
        private void ReactToInitiateCombat(Unit unit)
        {
            //unit.Stats.BonusAttributes.DEX += BonusAttributes.DEX;
          //  unit.BattleComponent.BattleStats.BonusStats.Hit += BonusStats.Hit;
        }
    }
}