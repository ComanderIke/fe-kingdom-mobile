using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public class HealPerNode : PassiveSkill
    {
        [SerializeField] private int hpRestored;
        private Unit owner;

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public HealPerNode(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescr, int hpRestored) : base(Name, description, icon, animationObject, cooldown,tier, upgradeDescr)
        {
            this.hpRestored = hpRestored;
        }
        public override List<EffectDescription> GetEffectDescription()
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("HP Restored: ", hpRestored.ToString()));
            return list;
        }
        public override void BindSkill(Unit unit)
        {
            this.owner = unit;
            //unit.OnNodeTravel += ReactToNodeTravel;
        }
        public override void UnbindSkill(Unit unit)
        {
            owner = null;
            //unit.OnNodeTravel -= ReactToNodeTravel;

        }

        void ReactToNodeTravel(EncounterNode node)
        {
            owner.Heal(hpRestored);
        }
      
    }
}