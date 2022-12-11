using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public class Ressurect : PassiveSkill
    {

        private Unit owner;
        [SerializeField] float hpRegPercentage;

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public Ressurect(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, int tier,string[] upgradeDescr, float hpRegPercentage) : base(Name, description, icon, animationObject, cooldown, tier,upgradeDescr)
        {
            this.hpRegPercentage = hpRegPercentage;
        }
        public override List<EffectDescription> GetEffectDescription()
        {
            var list = new List<EffectDescription>();
            list.Add(new EffectDescription("Regenerated HP: ", hpRegPercentage.ToString()+"%"));
            return list;
        }
        public override void BindSkill(Unit unit)
        {
            this.owner = unit;
            unit.OnLethalDamage += ReactToBeforeDeath;
        }
        public override void UnbindSkill(Unit unit)
        {
            unit.OnLethalDamage -= ReactToBeforeDeath;
            this.owner = null;
        }
        private void ReactToBeforeDeath()
        {
            Debug.Log("TODO Event Before Death but after damage received");
            owner.Hp = 0;
            owner.Heal((int)hpRegPercentage*owner.MaxHp);
        }
      
    }
}