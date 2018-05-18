using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.AI
{
    public enum GoalType
    {
        ATTACK
    }
    public class Goal
    {
        public GoalType Type { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int Priority { get; set; }
        public List<Unit> AssignedUnits { get; set; }
        public List<Unit> PotentialUnits { get; set; }

        public Goal(GoalType type, int x, int y, WeightSet w)
        {
            Type = type;
            this.x = x;
            this.y = y;
            AssignedUnits = new List<Unit>();
            PotentialUnits = new List<Unit>();
        }
        public void AssignUnitResourceSuitability(Unit unit, WeightSet weightSet)
        {
            PotentialUnits.Add(unit);
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
                unit.AIGoals.Add(this);
            }
        }
    }
}
