using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public enum GoalType
    {
        ATTACK
    }
    public class Goal
    {
        public GoalType Type { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Priority { get; set; }
        public List<Unit> AssignedUnits { get; set; }
        public List<Unit> PotentialUnits { get; set; }

        public Goal(GoalType type, int x, int y)
        {
            Type = type;
            this.X = x;
            this.Y = y;
            AssignedUnits = new List<Unit>();
            PotentialUnits = new List<Unit>();
        }
        public void AssignUnitResourceSuitability(Unit unit, WeightSet weightSet)
        {
            Unit leastSuitabilityUnit = GetUnitWithLeastSuitability();
            if(IsSuitabilityHigher(unit, leastSuitabilityUnit))
                PotentialUnits.Add(unit);
        }
        private Unit GetUnitWithLeastSuitability()
        {
            Unit least = null;
            float leastSuitability = float.MaxValue;
            foreach(Unit u in PotentialUnits)
            {
                float suitability = CalculateSuitability(u);
                if ( suitability < leastSuitability)
                {
                    least = u;
                    leastSuitability = suitability;
                }
            }
            return least;
        }
        private float CalculateSuitability(Unit unit)
        {
            WeightSet ws = unit.Agent.WeightSet;
            float suitability = 0;
            int distance1 = unit.GridPosition.GetManhattenDistance(X, Y);
            suitability -= distance1 * ws.GOAL.TARGET_DISTANCE_FAKTOR;
            return suitability;
        }
        private bool IsSuitabilityHigher(Unit unit1, Unit unit2)
        {
            return CalculateSuitability(unit1) > CalculateSuitability(unit2);
        }
        public bool HasSufficientRessources()
        {
            return PotentialUnits.Count >= 1;
        }
        public void AssignGoalRessources()
        {
            foreach (Unit unit in PotentialUnits)
            {
                AssignedUnits.Add(unit);
                unit.Agent.AIGoals.Add(this);
            }
        }
    }
}
