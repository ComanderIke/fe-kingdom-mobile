using System;
using UnityEngine;

namespace Game.AI
{
    //public GoalWeightSet()
    //{
    //    TENSION_FAKTOR = 1;
    //    VULNERABILITY_FAKTOR = 1;
    //    SOURCE_ATTACK_FAKTOR = 1;
    //    SOURCE_HP_FAKTOR = 1;
    //    SOURCE_DISTANCE_FAKTOR = 1;
    //    TARGET_DISTANCE_FAKTOR = 10;
    //    ATTACK_TARGET_BONUS = 5;
    //}
    [Serializable]
    public class GoalWeightSet
    {
        [SerializeField] private float attackTargetBonus = default;
        [SerializeField] private float sourceAttackKoefficient = default;
        [SerializeField] private float sourceDistanceKoefficient = default;
        [SerializeField] private float sourceHpKoefficient = default;
        [SerializeField] private float targetDistanceKoefficient = default;
        [SerializeField] private float tensionKoefficient = default;
        [SerializeField] private float vulnerabilityKoefficient = default;

        public float SourceDistanceKoefficient => sourceDistanceKoefficient;

        public float TargetDistanceKoefficient => targetDistanceKoefficient;

        public float AttackTargetBonus => attackTargetBonus;

        public float TensionKoefficient => tensionKoefficient;

        public float SourceAttackKoefficient => sourceAttackKoefficient;

        public float SourceHpKoefficient => sourceHpKoefficient;

        public float VulnerabilityKoefficient => vulnerabilityKoefficient;
    }
}