using UnityEngine;

namespace Game.GameActors.Units.Skills.Passive
{
    [System.Serializable]
    public class AttackEffects
    {
        [SerializeField] private bool cancelAttack = false;
        [SerializeField] bool bonusAttack = false;
        [SerializeField] bool luna = false;
        [SerializeField] bool sol = false;

        public void ApplyPositives(AttackEffects attackEffects)
        {
            if(!cancelAttack)
                cancelAttack = attackEffects.cancelAttack;
            if(!bonusAttack)
                bonusAttack = attackEffects.bonusAttack;
            if(!luna)
                luna = attackEffects.luna;
            if(!sol)
                sol = attackEffects.sol;
        }
    }
}