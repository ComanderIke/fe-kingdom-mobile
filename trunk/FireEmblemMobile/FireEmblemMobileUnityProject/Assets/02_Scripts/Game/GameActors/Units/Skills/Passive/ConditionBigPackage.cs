using System;
using System.Collections.Generic;
using Game.Grid;
using Game.Grid.Tiles;

namespace Game.GameActors.Units.Skills.Passive
{
    [Serializable]
    public class ConditionBigPackage
    {
        public ConditionCompareType CompareType;
        public List<ConditionPackage> Conditions;

      
        public bool Valid(Unit unit, Unit target=null, Tile tile=null)
        {
            if (Conditions == null || Conditions.Count == 0)
                return true;
            switch (CompareType)
            {
                case ConditionCompareType.AND:
                    foreach (var conditionPackage in Conditions)
                    {
                        if (!conditionPackage.Valid(unit, target, tile))
                            return false;
                    }
                    return true;
                case ConditionCompareType.OR:
                    foreach (var conditionPackage in Conditions)
                    {
                        if (conditionPackage.Valid(unit, target, tile))
                            return true;
                    }
                    return false;
                case ConditionCompareType.XOR:
                    bool oneValid = false;
                    foreach (var conditionPackage in Conditions)
                    {
                        if (conditionPackage.Valid(unit, target, tile))
                        {
                            if (oneValid)
                                return false;
                            oneValid = true;
                        }
                       
                    }
                    return oneValid;
            }
            return true;
        }
    }
}