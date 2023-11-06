using System;
using System.Collections.Generic;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    public class ConditionBigPackage
    {
        public ConditionCompareType CompareType;
        public List<ConditionPackage> Conditions;

        public bool Valid(Unit unit, Unit target=null)
        {
            if (Conditions == null || Conditions.Count == 0)
                return true;
            switch (CompareType)
            {
                case ConditionCompareType.AND:
                    foreach (var conditionPackage in Conditions)
                    {
                        if (!conditionPackage.Valid(unit, target))
                            return false;
                    }

                    return true;
                    break;
                case ConditionCompareType.OR:
                    foreach (var conditionPackage in Conditions)
                    {
                        if (conditionPackage.Valid(unit, target))
                            return true;
                    }

                    return false;
                    break;
                case ConditionCompareType.XOR:
                    bool oneValid = false;
                    foreach (var conditionPackage in Conditions)
                    {
                        if (conditionPackage.Valid(unit, target))
                        {
                            if (oneValid == true)
                                return false;
                            oneValid = true;
                        }

                        return oneValid;
                    }

                    return false;
                    break;
                
            }

            return true;
        }
    }
}