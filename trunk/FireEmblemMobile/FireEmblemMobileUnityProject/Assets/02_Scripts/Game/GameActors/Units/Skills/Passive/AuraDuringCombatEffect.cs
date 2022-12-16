using System;
using System.Collections.Generic;
using Game.GameActors.Units.Numbers;
using Game.Mechanics.Battle;
using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
   
    public class AuraDuringCombatEffect:PassiveSkill
    {
      
        public BonusStats BonusStats;
        private List<Unit> inRangeAllies;
        private Unit owner;
        private int range;
        public AuraDuringCombatEffect(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescr, BonusStats bonusStats, int range) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
            this.BonusStats = bonusStats;
            this.range = range;
        }
        public override void BindSkill(Unit unit)
        {
            //#Option 1 check every time an ally unit moves and apply a virtual "Skill" on adjacent allies.
            //#Option 2 check adjacent units skills before combat and call "adjacent" skills manually from there.
            // This needs to be done then everytime an adjacent effect can apply (combat, start of turn, etc.)
            //#Option 3 OnAnyUnitMoved check if unit is adjacent to owner if yes apply bonus stats and save in a list.
            //if not check if unit was in the list if yes remove bonus stats.
            this.owner = unit;
            inRangeAllies = new List<Unit>();
            Unit.OnAnyUnitsCombat += OnAnyUnitsCombat;
            //check all adjacent allies and populate list
            
            
            //#Option 4 check If any unit started Battle then check if it is allied and adjacent. then apply buffs
            //Best way?
            
            //Option 5 OnAnyUnitDoesCombat check if adjacent then apply buffs.
        }
        public override void UnbindSkill(Unit unit)
        {
            Unit.OnAnyUnitsCombat -= OnAnyUnitsCombat;
           
            this.owner = null;
            inRangeAllies.Clear();
        }

        private void OnAnyUnitsCombatEnded(Unit unit)
        {
            if (inRangeAllies.Contains(unit))
            {
                inRangeAllies.Remove(unit);
                unit.BattleComponent.BattleStats.BonusStats.Decrease(BonusStats);
            }
        }
        private void OnAnyUnitsCombat(Unit unit)
        {
            //Check if unit is adjacent
            inRangeAllies.Clear();
            if (unit.GridComponent.IsInRange(owner.GridComponent, range))
            {
                inRangeAllies.Add(unit);
                unit.BattleComponent.BattleStats.BonusStats.Add(BonusStats);
            }
        }
       
    }
}