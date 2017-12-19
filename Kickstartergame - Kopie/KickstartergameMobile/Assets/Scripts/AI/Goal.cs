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
        public List<LivingObject> AssignedUnits { get; set; }
        public List<LivingObject> PotentialUnits { get; set; }

        public Goal(GoalType type, int x, int y, WeightSet w)
        {
            Type = type;
            this.x = x;
            this.y = y;
            AssignedUnits = new List<LivingObject>();
            PotentialUnits = new List<LivingObject>();
        }
        public void AssignUnitResourceSuitability(LivingObject unit, WeightSet weightSet)
        {
            PotentialUnits.Add(unit);
        }
        public bool HasSufficientRessources()
        {
            return PotentialUnits.Count >= 1;
        }
        public void AssignGoalRessources()
        {
            foreach (LivingObject unit in PotentialUnits)
            {
                AssignedUnits.Add(unit);
                unit.AIGoals.Add(this);
            }
        }
    }
}
