using System.Collections.Generic;
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

        public GridSystem GridSystem;
        public AIUnitAction ChooseBestMove(IEnumerable<Unit> units)
        {
            if (GridSystem == null)
                GridSystem=GridGameManager.Instance.GetSystem<GridSystem>();
            var bestScore = float.MinValue;
            AIUnitAction bestAction = null;

            foreach (var u in units)
            {
                var action = CalculateBestMove(u);
                if (action.Score > bestScore)
                {
                    bestScore = action.Score;
                    bestAction = action;
                }
            }

            return bestAction;
            
        }
        private AIUnitAction CalculateBestMove(IAIAgent unit)
        {
            
            var currentBestScore = float.MinValue;// always want to do something
            var currentBestMoveLocation = new Vector2Int(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y); //By default stay on the same position
            UnitAction unitAction = UnitAction.Wait;

            Vector2 startLoc = new Vector2(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
            Unit currentBestTarget = null;
            var moveLocs = GridSystem.GridLogic.GetMoveLocations((IGridActor)unit);

            foreach (var loc in moveLocs)
            {
//                Debug.Log("Possible Location: "+loc);
                float locScore = ScoreCalculater.ScoreLocationForCharacter(loc, unit);
                var targets =GridSystem.GridLogic.GetAttackTargets((IGridActor)unit, loc.x, loc.y);
              //  Debug.Log("Targets: "+targets.Count);
                foreach (var gridActor in targets)
                {
                   // Debug.Log("Possible Target: "+gridActor);
                    var t = (Unit) gridActor;
                    float attackscore = ScoreCalculater.ScoreAttackForUnit((IBattleActor)unit, unit.AIComponent.WeightSet,loc, t);
                    if ((locScore + attackscore) > currentBestScore)
                    {
                        currentBestMoveLocation = loc;
                        currentBestScore = locScore + attackscore;
                        currentBestTarget = t;
                        unitAction = UnitAction.Attack;
                    }
                }
                if (locScore > currentBestScore)
                {
                    currentBestMoveLocation = loc;
                    currentBestScore = locScore;
                    unitAction = UnitAction.Wait;
                }
            }
           // Debug.Log("Score : "+currentBestScore);
            var action = new AIUnitAction {Performer= (Unit)unit, Score = currentBestScore, Location = currentBestMoveLocation, Target = currentBestTarget, UnitAction = unitAction};
            
            return action;
        }

        

       
    }
}