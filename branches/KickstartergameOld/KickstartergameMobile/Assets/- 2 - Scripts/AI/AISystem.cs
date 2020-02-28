using Assets.Scripts.AI;
using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.GameStates;
using Assets.Scripts.Players;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AISystem : EngineSystem {

   


  

    

    //private float GetDistanceToGoalImproval(Unit u, Vector2 newLoc)
    //{
    //    float oldDistance=0;
    //    float newDistance=0;
        
    //    foreach(Goal g in u.AIGoals)
    //    {
    //        oldDistance += mainScript.GetSystem<GridSystem>().GetDistance(u.GridPosition.x, u.GridPosition.y, g.X, g.Y, u.Player.ID);
    //        newDistance += mainScript.GetSystem<GridSystem>().GetDistance((int)newLoc.x, (int)newLoc.y, g.X, g.Y, u.Player.ID);
    //    }
        
    //    // return factor representing improvement in position, protect vs divide by 0
    //    return (oldDistance > 0) ? (1f - newDistance / oldDistance) : (1f - newDistance);
    //}

    //protected float ScoreLocationForCharacter(Vector2 location, Unit c)
    //{
    //    float ret = 0;
    //    WeightSet w = new WeightSet();
    //    if (location.x == c.GridPosition.x && location.y == c.GridPosition.y)
    //    {
    //        ret = w.STAY;
    //    }
    //    ret += GetDistanceToGoalImproval(c, new Vector2(location.x, location.y)) * w.GOAL.TARGET_DISTANCE_FAKTOR;
    //    return ret;
    //}

    //private int GetAttackRange(Vector2 location, Vector2 target)
    //{
    //    return (int)(Mathf.Abs(location.x - target.x) + Mathf.Abs(location.y - target.y));
    //}

    //protected float ScoreAttackForUnit(Unit attacker, Vector2 location, Unit defender)
    //{
    //    WeightSet w = new WeightSet();
    //    float ret = w.ATTACK_START_WEIGHT;//start with 10 so enemies will also attack if they get more dmg themselves(Most players like Enemies attacks even if the attack doesn't do much)
    //    /*if (attacker.goalID == Goal.GoalType.ATTACK && attacker.goalTarget == new Vector2(location.x, location.z))
    //    {
    //        ret += w.ATTACK_GOAL.ATTACK_TARGET_BONUS;
    //    }*/
    //    // will this kill the target?
    //    if (attacker.BattleStats.CanKillTarget(defender,1.0f))
    //    {
    //        // bonus is 10, + current HP (do not want to reward too much overkill)
    //        ret += w.ATTACK_KILL_BONUS_WEIGHT + defender.HP * w.ATTACK_KILL_HP_MULT;
    //    }
    //    else
    //    {
    //        ret += attacker.BattleStats.GetTotalDamageAgainstTarget(defender) * w.DEALT_DAMAGE_MULT;//we value dealt damage higher than received damage
    //    }

    //    return ret;
    //}

    

    //private void SwitchToAIState()
    //{
    //    Debug.Log("SwitchToAIState");
    //    UnitActionSystem.onAllCommandsFinished -= SwitchToAIState;
    //    MainScript.instance.SwitchState(MainScript.instance.AIState);
    //}
}
