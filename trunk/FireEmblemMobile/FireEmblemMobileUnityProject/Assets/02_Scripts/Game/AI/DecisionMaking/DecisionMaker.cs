using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using Game.Mechanics.Commands;
using GameEngine;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Game.AI
{
    public class DecisionMaker
    {
        private readonly IGridInformation gridInfo;

        public DecisionMaker(IGridInformation gridInfo)
        {
            this.gridInfo = gridInfo;
        }

        public AIUnitAction ChooseBestAction(IEnumerable<Unit> units)
        {
            var bestScore = float.MinValue;
            AIUnitAction bestAction = new AIUnitAction();

            foreach (var u in units)
            {
                var action = CalculateBestAction(u);
                if (action.Score >= bestScore)
                {
                    bestScore = action.Score;
                    bestAction = action;
                }
            }

            return bestAction;
        }
        private AIUnitAction CalculateBestAction(IAIAgent unit)
        {
            var bestAction = CreateDefaultAction(unit);

            var moveLocs = gridInfo.GetMoveLocations((IGridActor)unit);

            foreach (var loc in moveLocs)
            {
                float locScore = ScoreCalculater.ScoreLocationForCharacter(loc, unit);
                
                bestAction = CalculateBestAttackAction(unit, bestAction, loc, locScore);
               
                if (locScore > bestAction.Score)
                {
                    bestAction = CreateMoveAction(unit, loc, locScore);
                }
            }

            return bestAction;
        }
        private AIUnitAction CreateMoveAction( IAIAgent unit, Vector2Int loc, float locScore)
        {
            return new AIUnitAction(locScore, loc, null, UnitActionType.Wait, (Unit)unit);
        }
        private AIUnitAction CalculateBestAttackAction(IAIAgent unit, AIUnitAction bestAction, Vector2Int loc, float locScore)
        {
            var targets = gridInfo.GetAttackTargets((IGridActor)unit, loc.x, loc.y);
            
            foreach (var gridActor in targets)
            {
                float attackscore = ScoreCalculater.ScoreAttackForUnit((IBattleActor)unit, unit.AIComponent.WeightSet, loc,
                    (IAttackableTarget)gridActor);
                var fullScore = locScore + attackscore;
                if (fullScore> bestAction.Score)
                {
                    bestAction = new AIUnitAction(fullScore,loc, gridActor, UnitActionType.Attack,(Unit)unit);
                }
            }

            return bestAction;
        }
        private static AIUnitAction CreateDefaultAction(IAIAgent unit)
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

       
    }
}