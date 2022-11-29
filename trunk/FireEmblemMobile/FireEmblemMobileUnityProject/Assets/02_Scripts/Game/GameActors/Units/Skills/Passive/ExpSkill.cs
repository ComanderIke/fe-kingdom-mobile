using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using UnityEngine;

namespace LostGrace
{
    public class ExpSkill : PassiveSkill
    {
        [SerializeField] private float expMul = 1.2f;
        private Unit owner;
        public void Init(Unit unit)
        {
            this.owner = unit;
            // unit.ExperienceManager.ExpGained += AddExtraExp;
        }

        public void AddExtraExp(int expBefore, int expGained)
        {
            float extraexp=(expGained * expMul) - expGained;
            //TODO STACKOVERFLOW
            //owner.ExperienceManager.AddExp(extraexp);
            
        }

        public override bool CanTargetCharacters()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDamage(Unit user, bool justToShow)
        {
            throw new System.NotImplementedException();
        }

        public ExpSkill(string Name, string description, Sprite icon, GameObject animationObject, int cooldown, string[] upgradeDescr, float expMul) : base(Name, description, icon, animationObject, cooldown, upgradeDescr)
        {
            this.expMul = expMul;
        }
    }
}