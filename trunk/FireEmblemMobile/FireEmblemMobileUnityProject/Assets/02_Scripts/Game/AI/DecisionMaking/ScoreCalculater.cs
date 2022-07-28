using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Mechanics;
using UnityEngine;

namespace Game.AI
{
    public class ScoreCalculater
    {
        public static IPathFinder pathFinder;
        public static float ScoreLocationForCharacter(Vector2Int location, IAIAgent agent)
        {
            float ret = 0;
            WeightSet w = agent.AIComponent.WeightSet;
            if (location.x == agent.GridComponent.GridPosition.X && location.y == agent.GridComponent.GridPosition.Y)
            {
                ret = w.StayOnTile;
            }
            ret += GetDistanceToGoalImproval(agent, new Vector2Int(location.x, location.y)) * w.GoalWeightSet.TargetDistanceKoefficient;
            return ret;
          
        }
        private static float GetDistanceToGoalImproval(IAIAgent agent, Vector2Int newLoc)
        {
            float oldDistance=0;
            float newDistance=0;
            //TODO Performance if it checks this for all possible movement options store similar movement option data(like check the options at max range) and score the others the same -1 per sqare
            //TODO for now just check manhatten distance
            foreach(Goal g in agent.AIComponent.AIGoals)
            {
                if (agent is IGridActor actor)
                {
                    // var path = pathFinder.FindPath(agent.GridComponent.GridPosition.X,
                    //     agent.GridComponent.GridPosition.Y, g.X, g.Y,
                    //     actor, false, actor.AttackRanges);
                    // if(path!=null)
                    //     oldDistance = path.GetLength();
                    // var pathNew = pathFinder.FindPath(newLoc.x, newLoc.y, g.X, g.Y,
                    //     actor, false, actor.AttackRanges);
                    // if(pathNew!=null)
                    //     newDistance = pathNew.GetLength();
                    oldDistance = Mathf.Abs(agent.GridComponent.GridPosition.X - g.X) +
                                  Mathf.Abs(agent.GridComponent.GridPosition.Y - g.Y);
                    newDistance = Mathf.Abs(newLoc.x - g.X) +
                                  Mathf.Abs(newLoc.y - g.Y);
                }
            }

            // return factor representing improvement in position, protect vs divide by 0
            return (oldDistance > 0) ? (1f - newDistance / oldDistance) : (1f - newDistance);
        }
        public static float ScoreAttackForUnit(IBattleActor attacker,WeightSet aiWeightSet, Vector2 location, IAttackableTarget defender)
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
    }
    
}