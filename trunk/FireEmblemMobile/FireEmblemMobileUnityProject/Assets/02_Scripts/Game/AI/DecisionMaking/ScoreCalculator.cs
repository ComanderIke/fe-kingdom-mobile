using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid.GridPathFinding;
using Game.Mechanics;
using UnityEngine;

namespace Game.AI
{
    public class ScoreCalculator
    {
        IPathFinder pathFinder;
        private Dictionary<Goal, MovementPath> pathsToGoals;
        private Goal nearestGoal;
        private const float notOnPathMalus = 50.0f;
        private int goalsDistance = 0;

        public ScoreCalculator(IPathFinder pathFinder)
        {
            this.pathFinder = pathFinder;
            pathsToGoals = new Dictionary<Goal, MovementPath>();
        }

        public int GetDistanceToClosestEnemy(IAIAgent agent)
        {
            int shortestDistance = int.MaxValue;
            foreach (var unit in from faction in agent.Faction.GetOpponentFactions()
                     from unit in faction.Units
                     where unit.IsAlive()
                     select unit)
            {
                var path = pathFinder.FindPath(agent.GridComponent.GridPosition.X, agent.GridComponent.GridPosition.Y, unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y, agent);
                if (path.GetLength() < shortestDistance)
                {
                    shortestDistance = path.GetLength();
                }
            }

            return shortestDistance;
        }
        public void InitCurrentLocationScore(IAIAgent agent)
        {
            pathsToGoals.Clear();
            goalsDistance = 0;
            nearestGoal = null;
            float nearestLength = float.MaxValue;
            Debug.Log("Unit: "+agent);
            if (agent.AIComponent.AIGoals.Count == 0)
                Debug.Log("Goals are NULL===============================");
            foreach (Goal g in agent.AIComponent.AIGoals)
            {
                Debug.Log("Goal: " + g.X + " " + g.Y);
                if (agent is IGridActor actor)
                {
                    var path = pathFinder.FindPath(agent.GridComponent.GridPosition.X,
                        agent.GridComponent.GridPosition.Y, g.X, g.Y,
                        actor);
                    if (path != null)
                    {
                        pathsToGoals.Add(g, path);
                        goalsDistance += path.GetLength();
                        if (path.GetLength() < nearestLength)
                        {
                            nearestLength = path.GetLength();
                            nearestGoal = g;
                        }
                       
                        Debug.Log(" Distance:"+path.GetLength());
                    }
                    else
                    {
                        float dist = notOnPathMalus + GetManhattenDistance(g.X, g.Y, agent.GridComponent.GridPosition.X,
                            agent.GridComponent.GridPosition.Y);
                        if (dist < nearestLength)
                        {
                            nearestLength = dist;
                            nearestGoal = g;
                        }
                    }
                }
            }
        }

        private static int GetManhattenDistance(int x, int y, int x2, int y2)
        {
            return Mathf.Abs(x - x2) + Mathf.Abs(y - y2);
        }
        public float ScoreLocationForCharacter(Vector2Int location, IAIAgent agent)
        {
            float score = 0;
            WeightSet w = agent.AIComponent.WeightSet;
            if (agent.GridComponent.GridPosition.IsSamePosition(location))
            {
                score = w.StayOnTile;
            }
            score += GetDistanceToGoalImprovement(agent, new Vector2Int(location.x, location.y)) ;
            Debug.Log("LocationScore: " + score+ " "+location);
            return score;
        }
        private float GetDistanceToGoalImprovement(IAIAgent agent, Vector2Int newLoc)
        {
            float newDistance=0;
            if (nearestGoal == null)
            {
                Debug.Log("Goal is null!!!");
                return 0;
            }

            Goal g = nearestGoal;
            Debug.Log("Current Goal: " +g.X+" "+g.Y);
            int pathIndex = -1;
            if (pathsToGoals.ContainsKey(g))
            {
                pathIndex = pathsToGoals[g].GetIndex(new Step(newLoc.x, newLoc.y));
            }
            if (pathIndex == -1)
            {
                newDistance = notOnPathMalus+GetManhattenDistance(newLoc.x ,newLoc.y, g.X, g.Y);
                Debug.Log(newLoc+" Not on Path: "+newDistance);
            }
            else
            {
                Debug.Log(newLoc+" Path: "+g+" Index:"+ pathIndex+ " Length: "+pathsToGoals[g].GetLength());
                newDistance = pathIndex+1;
            }
            
            // return factor representing improvement in position, protect vs divide by 0
            return (goalsDistance > 0) ? (1f - newDistance / goalsDistance) : (1f - newDistance);
        }
        public float ScoreAttackForUnit(IBattleActor attacker,WeightSet aiWeightSet, IAttackableTarget defender)
        {
            
            WeightSet w = aiWeightSet;
            float ret = w.AttackStartWeight;//start with 10 so enemies will also attack if they get more dmg themselves(Most players like Enemies attacks even if the attack doesn't do much)
            /*if (attacker.goalID == Goal.GoalType.ATTACK && attacker.goalTarget == new Vector2(location.x, location.z))
            {
                ret += w.ATTACK_GOAL.ATTACK_TARGET_BONUS;
            }*/
            // will this kill the target?
            if (attacker.BattleComponent.BattleStats.CanKillTarget(defender,1.0f))
            {
                // bonus is 10, + current HP (do not want to reward too much overkill)
                ret += w.AttackKillBonusWeight + defender.Hp * w.AttackKillHpMultiplier;
            }
            else
            {
                ret += attacker.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(defender) * w.DealtDamageMultiplier;//we value dealt damage higher than received damage
            }

            return ret;
        }

        public int GetDistanceToEnemy(IAIAgent unit, IGridActor enemyAgent)
        {
            return GetDistanceToEnemy(new Vector2Int(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y), enemyAgent, unit);
        }
        public MovementPath GetPathToEnemy(IAIAgent unit, IGridObject enemyAgent)
        {
            //TODO (SPECIAL NOTE: if enemy cannot be directly pathed to, unit paths toward closest tile from which they can attack. Choose by highest tile priority value if there is a tie. This only applies when chasing enemies.)
            var path = pathFinder.FindPath(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y, enemyAgent.GridComponent.GridPosition.X, enemyAgent.GridComponent.GridPosition.Y, unit);
            return path;
            
        }
        public int GetDistanceToEnemy(Vector2Int startPos, IGridObject enemyAgent, IGridActor unit)
        {
            var path = pathFinder.FindPath(startPos.x, startPos.y, enemyAgent.GridComponent.GridPosition.X, enemyAgent.GridComponent.GridPosition.Y, unit);
            if (path!=null)
            {
                return path.GetLength()-1;
            }

            return int.MaxValue;
        }
    }
    
}