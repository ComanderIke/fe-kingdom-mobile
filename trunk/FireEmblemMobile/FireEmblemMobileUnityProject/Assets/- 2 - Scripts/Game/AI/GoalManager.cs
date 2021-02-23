﻿using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using UnityEngine;

namespace Game.AI
{
    public class GoalManager
    {
        private List<Goal> currentGoals;
        private Faction player;

        public GoalManager(Faction player)
        {
            this.player = player;
            currentGoals = new List<Goal>();
        }
        public void PrepareGoals()
        {
            var units = player.GetActiveUnits();

            //Debug.Log("TODO:  Move Camera To Moving Character");

            ResetGoals(units);
            CreateGoalsForAllEnemyUnits();

            // add all our units as potential resources for every goal
            AssignUnitsAsGoalResources();
        }
        private void ResetGoals(IEnumerable<Unit> units)
        {
            currentGoals.Clear();

            foreach (var u in units) u.AIComponent.AIGoals.Clear();
        }
        private void CreateGoalsForAllEnemyUnits()
        {
            foreach (var unit in from faction in player.GetOpponentFactions()
                from unit in faction.Units
                where unit.IsAlive()
                select unit)
                currentGoals.Add(new Goal(GoalType.Attack, unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y));
        }
        private void AssignUnitsAsGoalResources()
        {
            var cnt = 0;
            foreach (var g in currentGoals)
            {
                foreach (var u in player.Units.Where(u => u.IsAlive() && u.AIComponent.AIGoals.Count <= 4))
                    g.AssignUnitResourceSuitability(u, u.AIComponent.WeightSet);

                // Does goal have enough potential resources
                if (g.HasSufficientResources() || cnt == currentGoals.Count - 1) //Assign last goal even if not enough resources
                    // Assign resources to goal
                    g.AssignGoalResources();

                cnt++;
            }
        }
    }
}