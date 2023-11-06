using System;
using System.Collections.Generic;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    public class ConditionPackage
    {
        public ConditionCompareType CompareType;
        public List<Condition> Conditions;

        public bool Valid(Unit unit, Unit target = null)
        {
            if (Conditions == null || Conditions.Count == 0)
                return true;
            switch (CompareType)
            {
                case ConditionCompareType.AND:
                    foreach (var condition in Conditions)
                    {
                        if (condition is SelfTargetCondition stc)
                        {
                            if (!stc.CanTarget(unit))
                                return false;
                        }
                        else if (condition is SingleTargetCondition sitc)
                        {
                            if (!sitc.CanTarget(unit, target))
                                return false;
                        }
                        
                    }

                    return true;
                    break;
                case ConditionCompareType.OR:
                    foreach (var condition in Conditions)
                    {
                        if (condition is SelfTargetCondition stc)
                        {
                            if (stc.CanTarget(unit))
                                return true;
                        }
                        else if (condition is SingleTargetCondition sitc)
                        {
                            if (sitc.CanTarget(unit, target))
                                return true;
                        }
                    }

                    return false;
                    break;
                case ConditionCompareType.XOR:
                    bool oneValid = false;
                    foreach (var condition in Conditions)
                    {
                        if (condition is SelfTargetCondition stc)
                        {
                            if (stc.CanTarget(unit))
                            {
                                if (oneValid == true)
                                    return false;
                                oneValid = true;
                            }
                        }
                        else if (condition is SingleTargetCondition sitc)
                        {
                            if (sitc.CanTarget(unit, target))
                            {
                                if (oneValid == true)
                                    return false;
                                oneValid = true;
                            }
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