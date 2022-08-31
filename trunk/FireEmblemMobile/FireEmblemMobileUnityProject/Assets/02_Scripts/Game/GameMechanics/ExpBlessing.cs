using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    public class ExpBlessing : Blessing
    {
        [SerializeField] private float expMul = 1.2f;
        private Unit owner;
        public void Init(Unit unit)
        {
            this.owner = unit;
            unit.OnExpGained += AddExtraExp;
        }

        public void AddExtraExp(int expBefore, int expGained)
        {
            float extraexp=(expGained * expMul) - expGained;
            //TODO STACKOVERFLOW
            //owner.ExperienceManager.AddExp(extraexp);
            
        }

    }
}