using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;
using UnityEngine;

namespace Game.AI
{
    public class DecisionMaker
    {
        private readonly IGridInformation gridInfo;
        private ScoreCalculator scoreCalculator;
        private List<IAIAgent> moveOrderList;


        public DecisionMaker(IGridInformation gridInfo, IPathFinder pathFinder)
        {
            this.gridInfo = gridInfo;
            scoreCalculator = new ScoreCalculator(pathFinder);
        }


        private void InitTargets(IEnumerable<IAIAgent> units)
        {
           
            foreach (var unit in units)
            {
                var minDistance = int.MaxValue;
                unit.AIComponent.ClosestTarget = null;
                unit.AIComponent.Targets.Clear();
                foreach (var enemyAgent in from faction in unit.Faction.GetOpponentFactions()
                         from enemy in faction.Units
                         where enemy.IsAlive()
                         select enemy)
                {
                    var AITarget = new AITarget();
                    AITarget.Actor = enemyAgent;
                    AITarget.Distance = scoreCalculator.GetDistanceToEnemy(unit, enemyAgent);
                    unit.AIComponent.Targets.Add(AITarget);
                    if ( AITarget.Distance > minDistance)
                    {
                        minDistance = AITarget.Distance;
                        unit.AIComponent.ClosestTarget = AITarget;
                    }
                }
            }
        }

        private List<IAIAgent> CreateMoveOrderList(IEnumerable<IAIAgent> units)
        {
            var moveOrderList = new List<IAIAgent>();
            foreach (var unit in units)
            {
                moveOrderList.Add(unit);
            }

            moveOrderList.Sort(new UnitComparer());
            return moveOrderList;
        }

        public AIUnitAction ChooseBestMovementAction()
        {
            AIUnitAction bestAction = new AIUnitAction();
            moveOrderList.RemoveAll(unit => unit.TurnStateManager.HasMoved);
            IAIAgent unit = moveOrderList.First();
            Debug.Log("First Unit in MoveOrderList: " + unit);
            var chaseTarget = ChooseChaseTarget(unit);
            if (chaseTarget == null)
                Debug.Log("Chase Target = null!");
            else
            {
                Vector2Int location = ChooseBestLocationToChaseTarget(unit, chaseTarget);
                return new AIUnitAction(0, location, null, UnitActionType.Wait, (Unit)unit);
            }

            return bestAction;
        }

        private Vector2Int ChooseBestLocationToChaseTarget(IAIAgent unit, AITarget chaseTarget)
        {
            var moveLocs = gridInfo.GetMoveLocations(unit);
            int minDistance = int.MaxValue;
            Vector2Int bestloc = new Vector2Int(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
            foreach (var loc in moveLocs)
            {
                int distanceToTarget = scoreCalculator.GetDistanceToEnemy(loc, chaseTarget.Actor, unit);
                if (distanceToTarget < minDistance)
                {
                    bestloc = loc;
                    minDistance = distanceToTarget;
                }
            }

            return bestloc;
        }

        private AITarget ChooseChaseTarget(IAIAgent agent)
        {
            int MaxPrioValue = int.MinValue;
            AITarget target = null;
            foreach (var unit in from faction in agent.Faction.GetOpponentFactions()
                     from unit in faction.Units
                     where unit.IsAlive()
                     select unit)
            {
                Debug.Log("Opponent Unit: " + unit);
                int dmg = unit.BattleComponent.BattleStats.GetTotalDamageAgainstTarget(unit);
                //Take Terrain into Account instead of using path.getLength)
                int turnRange = agent.AIComponent.GetTarget(unit).Distance / agent.MovementRange;
                Debug.Log("TurnRange: " + turnRange);
                var prioValue = dmg - 2 * turnRange;
                Debug.Log("PrioValue: " + prioValue);
                if (prioValue > MaxPrioValue)
                {
                    MaxPrioValue = prioValue;
                    target = agent.AIComponent.GetTarget(unit);
                }
            }

            return target;
        }
        // public AIUnitAction ChooseBestAction(IEnumerable<Unit> units)
        // {
        //     var bestScore = float.MinValue;
        //     AIUnitAction bestAction = new AIUnitAction();
        //
        //     foreach (var u in units)
        //     {
        //         scoreCalculator.InitCurrentLocationScore(u);
        //         var action = CalculateBestAction(u);
        //         if (action.Score >= bestScore)
        //         {
        //             bestScore = action.Score;
        //             bestAction = action;
        //         }
        //     }
        //
        //     return bestAction;
        // }

        private AIUnitAction CreateMoveAction(IAIAgent unit, Vector2Int loc, float locScore)
        {
            return new AIUnitAction(locScore, loc, null, UnitActionType.Wait, (Unit)unit);
        }

        private AIUnitAction CalculateBestAttackAction(IAIAgent unit, AIUnitAction bestAction, Vector2Int loc,
            List<IGridObject> targets)
        {
            foreach (var gridActor in targets)
            {
                float attackscore = scoreCalculator.ScoreAttackForUnit((IBattleActor)unit, unit.AIComponent.WeightSet,
                    (IAttackableTarget)gridActor);
                if (attackscore > bestAction.Score)
                {
                    bestAction = new AIUnitAction(attackscore, loc, gridActor, UnitActionType.Attack, (Unit)unit);
                }
            }

            return bestAction;
        }

        private AIUnitAction CreateDefaultAction(IAIAgent unit)
        {
            var bestAction = new AIUnitAction();
            bestAction.Score = float.MinValue; // always want to do something
            bestAction.Location = new Vector2Int(unit.GridComponent.GridPosition.X,
                unit.GridComponent.GridPosition.Y); // by default stay on the same position
            bestAction.Target = null;
            bestAction.UnitActionType = UnitActionType.Wait; // default action is wait;
            bestAction.Performer = (Unit)unit;
            return bestAction;
        }


        public void InitTurnData(IEnumerable<IAIAgent> units)
        {
            InitTargets(units);
            moveOrderList = CreateMoveOrderList(units);
        }
    }
}