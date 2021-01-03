namespace Game.AI
{
    public class Goal
    {
        //public Goal(GoalType type, int x, int y)
        //{
        //    Type = type;
        //    X = x;
        //    Y = y;
        //    AssignedUnits = new List<Unit>();
        //    PotentialUnits = new List<Unit>();
        //}

        //public GoalType Type { get; set; }
        //public int X { get; set; }
        //public int Y { get; set; }
        //public int Priority { get; set; }
        //public List<Unit> AssignedUnits { get; set; }
        //public List<Unit> PotentialUnits { get; set; }

        //public void AssignUnitResourceSuitability(Unit unit, WeightSet weightSet)
        //{
        //    var leastSuitabilityUnit = GetUnitWithLeastSuitability();
        //    if (IsSuitabilityHigher(unit, leastSuitabilityUnit))
        //        PotentialUnits.Add(unit);
        //}

        //private Unit GetUnitWithLeastSuitability()
        //{
        //    Unit least = null;
        //    var leastSuitability = float.MaxValue;
        //    foreach (var u in PotentialUnits)
        //    {
        //        float suitability = CalculateSuitability(u);
        //        if (suitability < leastSuitability)
        //        {
        //            least = u;
        //            leastSuitability = suitability;
        //        }
        //    }

        //    return least;
        //}

        //private float CalculateSuitability(Unit unit)
        //{
        //    var ws = unit.Agent.WeightSet;
        //    float suitability = 0;
        //    int distance1 = unit.GridPosition.GetManhattanDistance(X, Y);
        //    suitability -= distance1 * ws.GoalWeightSet.TargetDistanceKoefficient;
        //    return suitability;
        //}

        //private bool IsSuitabilityHigher(Unit unit1, Unit unit2)
        //{
        //    return CalculateSuitability(unit1) > CalculateSuitability(unit2);
        //}

        //public bool HasSufficientRessources()
        //{
        //    return PotentialUnits.Count >= 1;
        //}

        //public void AssignGoalRessources()
        //{
        //    foreach (var unit in PotentialUnits)
        //    {
        //        AssignedUnits.Add(unit);
        //        unit.Agent.AIGoals.Add(this);
        //    }
        //}
    }
}