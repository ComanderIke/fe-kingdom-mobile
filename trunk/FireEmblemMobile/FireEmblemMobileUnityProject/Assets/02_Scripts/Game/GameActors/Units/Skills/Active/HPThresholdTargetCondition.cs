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
        public bool useFixedHp;
        public int fixedHp;
        public bool useDexAsHp;
        public bool checkRevivalStones;
        public int revivalStones;

        int GetScaledDexValue(Unit caster)
        {
            return caster.Stats.CombinedAttributes().DEX;
        }
        public override bool CanTarget(Unit caster, Unit target)
        {
            Unit compareUnit = caster;
            if (checkTargetInsteadofCaster)
                compareUnit = target;
            switch (compareType)
            {
                case CompareNumbersType.Equal:
                    if (checkRevivalStones)
                    {
                        return compareUnit.RevivalStones == revivalStones;
                    }
                    if (useDexAsHp)
                    {
                        return compareUnit.Hp - GetScaledDexValue(caster) == 0;
                    }
                    if (useFixedHp)
                    {
                        return compareUnit.Hp - fixedHp == 0;
                    }
                    return Math.Abs(compareUnit.Hp / (compareUnit.MaxHp * 1.0f) - hpThreshold) < 0.01;
                case CompareNumbersType.NotEqual:
                    if (useDexAsHp)
                    {
                        return compareUnit.Hp - GetScaledDexValue(caster) != 0;
                    }
                    if (useFixedHp)
                    {
                        return compareUnit.Hp - fixedHp != 0;
                    }
                    return Math.Abs(compareUnit.Hp / (compareUnit.MaxHp * 1.0f) - hpThreshold) > 0.01;
                case CompareNumbersType.Higher:
                    if (useDexAsHp)
                    {
                        return compareUnit.Hp - GetScaledDexValue(caster) > 0;
                    }
                    if (useFixedHp)
                    {
                        return compareUnit.Hp - fixedHp > 0;
                    }
                    return compareUnit.Hp / (compareUnit.MaxHp * 1.0f) > hpThreshold;
                case CompareNumbersType.Lower:
                    if (useDexAsHp)
                    {
                        return compareUnit.Hp - GetScaledDexValue(caster) < 0;
                    }
                    if (useFixedHp)
                    {
                        return compareUnit.Hp - fixedHp < 0;
                    }
                    return compareUnit.Hp / (compareUnit.MaxHp * 1.0f) < hpThreshold;
            }

            return false;
        }
    }
}