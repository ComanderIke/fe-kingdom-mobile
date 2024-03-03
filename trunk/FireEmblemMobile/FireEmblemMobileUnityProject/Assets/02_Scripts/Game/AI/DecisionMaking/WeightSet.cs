using UnityEngine;

namespace Game.AI.DecisionMaking
{
    //public WeightSet()
    //{
    //    STAY = -1;
    //    ALLY_UNIT_MULT = 1;
    //    HEAL_ALMOST_FULL_HP = 1;
    //    HEAL_LOW_HP = 1;
    //    HEAL_ALMOST_FULL_HP = 1;
    //    ATTACK_START_WEIGHT = 10;//start with 10 so enemies will also attack if they get more dmg themselves(Most players like Enemies attacks even if the attack doesn't do much)
    //    ATTACK_KILL_HP_MULT = 1f;
    //    ATTACK_KILL_BONUS_WEIGHT = 10;
    //    ATTACK_OWN_DEATH_HP_MULT = 0.5f;
    //    ATTACK_OWN_DEATH_WEIGHT = 3;
    //    RECEIVED_DAMAGE_MULT = 0.5f;
    //    DEALT_DAMAGE_MULT = 2.0f;
    //    ATTACK_GOAL = new GoalWeightSet();
    //}
    [CreateAssetMenu(menuName = "GameData/AI/WeightSet", fileName = "WeightSet")]
    public class WeightSet : ScriptableObject
    {
        [SerializeField] private float allyUnitMultiplier = default;
        [SerializeField] private int attackKillBonusWeight = default;
        [SerializeField] private float attackKillHpMultiplier = default;
        [SerializeField] private float attackOwnDeathHpMultiplier = default;
        [SerializeField] private int attackOwnDeathWeight = default;
        [SerializeField] private int attackStartWeight = default;
        [SerializeField] private float dealtDamageMultiplier = default;
        [SerializeField] private int healAlmostFullHp = default;
        [SerializeField] private int healHalfHp = default;
        [SerializeField] private int healLowHp = default;
        [SerializeField] private float receivedDamageMultiplier = default;
        [SerializeField] private float stayOnTile = default;

        public float AllyUnitMultiplier => allyUnitMultiplier;

        public int AttackKillBonusWeight => attackKillBonusWeight;

        public float AttackKillHpMultiplier => attackKillHpMultiplier;

        public float AttackOwnDeathHpMultiplier => attackOwnDeathHpMultiplier;

        public int AttackOwnDeathWeight => attackOwnDeathWeight;

        public int AttackStartWeight => attackStartWeight;

        public float DealtDamageMultiplier => dealtDamageMultiplier;

        public int HealAlmostFullHp => healAlmostFullHp;

        public int HealHalfHp => healHalfHp;

        public int HealLowHp => healLowHp;

        public float ReceivedDamageMultiplier => receivedDamageMultiplier;

        public float StayOnTile => stayOnTile;
        
    }
}