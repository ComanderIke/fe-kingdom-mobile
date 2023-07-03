﻿using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using LostGrace;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
   
    public class DuringCombatEffect:PassiveSkill
    {
        public Attributes BonusAttributes;
        public BonusStats BonusStats;
        public DuringCombatEffect(string Name, string description, Sprite icon, GameObject animationObject, int tier,string[] upgradeDescr) : base(Name, description, icon, animationObject, tier,upgradeDescr)
        {
        }

        public override List<EffectDescription> GetEffectDescription()
        {
            throw new NotImplementedException();
        }

        public override void BindSkill(Unit unit)
        {
            // unit.BattleComponent.OnEndCombat += ReactToEndCombat;
            // unit.BattleComponent.OnCombat += ReactToCombat;
        }
        public override void UnbindSkill(Unit unit)
        {
            // unit.BattleComponent.OnEndCombat -= ReactToEndCombat;
            // unit.BattleComponent.OnCombat -= ReactToCombat;
        }
        private void ReactToEndCombat(Unit unit)
        {
            //unit.Stats.BonusAttributes.DEX -= BonusAttributes.DEF;
            //unit.BattleComponent.BattleStats.BonusStats.Hit -= BonusStats.Hit;
        }
        private void ReactToCombat(Unit unit)
        {
           // unit.Stats.BonusAttributes.DEX += BonusAttributes.DEX;
           // unit.BattleComponent.BattleStats.BonusStats.Hit += BonusStats.Hit;
        }
    }
}