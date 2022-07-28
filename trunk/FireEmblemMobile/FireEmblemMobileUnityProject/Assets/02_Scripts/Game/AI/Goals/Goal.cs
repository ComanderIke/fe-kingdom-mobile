using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.AI
{
    public class Goal
    {
        private GoalType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Priority { get; set; }
        private List<Unit> AsignedUnits { get; set; }
        private List<Unit> PotentialUnits { get; set; }
        public Goal(GoalType type, int x, int y)
        {
            Type = type;
            X = x;
            Y = y;
            AsignedUnits = new List<Unit>();
            PotentialUnits = new List<Unit>();
        }

      

        public void AssignUnitResourceSuitability(Unit unit, WeightSet weightSet)
        {
            var leastSuitabilityUnit = GetUnitWithLeastSuitability();
            if (leastSuitabilityUnit==null||IsSuitabilityHigher(unit, leastSuitabilityUnit))
                PotentialUnits.Add(unit);
        }

        private Unit GetUnitWithLeastSuitability()
        {
            Unit least = null;
            var leastSuitability = float.MaxValue;
            foreach (var u in PotentialUnits)
            {
                float suitability = CalculateSuitability(u);
                if (suitability < leastSuitability)
                {
                    least = u;
                    leastSuitability = suitability;
                }
            }

            return least;
        }

        private float CalculateSuitability(Unit unit)
        {

            var ws = unit.AIComponent.WeightSet;
            float suitability = 0;
            int distance1 = ManhattanDistance(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
            suitability -= distance1 * ws.GoalWeightSet.TargetDistanceKoefficient;
            return suitability;
        }

        private int ManhattanDistance(int x, int y)
        {
            return Math.Abs(X - x) + Math.Abs(Y - y);
        }

        private bool IsSuitabilityHigher(Unit unit1, Unit unit2)
        {
            return CalculateSuitability(unit1) > CalculateSuitability(unit2);
        }

        public bool HasSufficientResources()
        {
            return PotentialUnits.Count >= 1;
        }

        public void AssignGoalResources()
        {
            foreach (var unit in PotentialUnits)
            {
                AsignedUnits.Add(unit);
                unit.AIComponent.AIGoals.Add(this);
            }
        }
    }
}