using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;
using Random = System.Random;

namespace LostGrace
{
    public class Miracle : PassiveSkill
    {

        private Unit owner;
        [SerializeField] float procChance;

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public Miracle(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr, float procChance) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
            this.procChance = procChance;
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
            if(UnityEngine.Random.value<=procChance)
                owner.Hp = 1;
        }
      
    }
    
    public class SkillActivation : PassiveSkill
    {

        private Unit owner;
        [SerializeField] float procChance;

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public SkillActivation(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr, float procChance) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
            this.procChance = procChance;
        }
        public override void BindSkill(Unit unit)
        {
            this.owner = unit;
            unit.BonusSkillProcChance += procChance;
        }
        public override void UnbindSkill(Unit unit)
        {
            unit.BonusSkillProcChance -= procChance;
            this.owner = null;
        }
      
    }
}