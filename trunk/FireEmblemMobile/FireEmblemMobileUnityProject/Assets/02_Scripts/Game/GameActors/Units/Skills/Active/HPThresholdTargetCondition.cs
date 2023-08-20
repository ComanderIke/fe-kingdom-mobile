using System;
using UnityEngine;

namespace Game.GameActors.Units.Skills
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Skills/TargetConditions/HpThreshold", fileName = "HpThreshold")]
    public class HPThresholdTargetCondition : SingleTargetCondition
    {
        
        public float hpThreshold;
        public CompareNumbersType compareType;
        public bool checkTargetInsteadofCaster;
        public override bool CanTarget(Unit caster, Unit target)
        {
            Unit compareUnit = caster;
            if (checkTargetInsteadofCaster)
                compareUnit = target;
            switch (compareType)
            {
                case CompareNumbersType.Equal:
                    return Math.Abs(compareUnit.Hp / (compareUnit.MaxHp * 1.0f) - hpThreshold) < 0.01;break;
                case CompareNumbersType.NotEqual:
                    return Math.Abs(compareUnit.Hp / (compareUnit.MaxHp * 1.0f) - hpThreshold) > 0.01;break;
                case CompareNumbersType.Higher:
                    return compareUnit.Hp / (compareUnit.MaxHp * 1.0f) > hpThreshold;break;
                case CompareNumbersType.Lower:
                    return compareUnit.Hp / (compareUnit.MaxHp * 1.0f) < hpThreshold;break;
            }

            return false;
        }
    }
}