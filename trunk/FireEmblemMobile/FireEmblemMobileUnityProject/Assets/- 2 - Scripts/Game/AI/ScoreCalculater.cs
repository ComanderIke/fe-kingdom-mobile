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
            foreach(Goal g in agent.AIComponent.AIGoals)
            {
                if (agent is IGridActor actor)
                {
                    oldDistance = pathFinder.FindPath(agent.GridComponent.GridPosition.X,
                        agent.GridComponent.GridPosition.Y, g.X, g.Y,
                        actor, false, actor.AttackRanges).GetLength();
                    newDistance = pathFinder.FindPath(newLoc.x, newLoc.y, g.X, g.Y,
                        actor, false, actor.AttackRanges).GetLength();
                }
            }

            // return factor representing improvement in position, protect vs divide by 0
            return (oldDistance > 0) ? (1f - newDistance / oldDistance) : (1f - newDistance);
        }
        public static float ScoreAttackForUnit(IBattleActor attacker,WeightSet aiWeightSet, Vector2 location, IBattleActor defender)
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