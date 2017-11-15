using Assets.Scripts.AI;
using Assets.Scripts.Characters;
using Assets.Scripts.Players;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMonsterAI : AIInterface {

    bool startTurn = true;

    public SimpleMonsterAI(Player p):base (p){

    }

    public override void Think()
    {
        startTurn = true;
        foreach (LivingObject u in player.Units)
        {
            if (u.UnitTurnState.IsWaiting)
                startTurn = false;
        }
        if (startTurn)
        {
            StartOfTurn();
        }

        List<LivingObject> units = GetUnitsLeftToMove();
        if (units.Count > 0)
        {
            SubmitBestMoveForUnit(units[0]);
            units[0].UnitTurnState.IsWaiting = true;
        }
        else
        {
            endturn = true;
        }
    }

    private void StartOfTurn()
    {
        //move Camera to first character
        List<LivingObject> units = GetUnitsLeftToMove();
        //mainScript.MoveCameraTo(characters[0].x, characters[0].z);
        // reset all goals
        //CurrentGoals.Clear();

        // reset our own unit goals from last turn, and create attack goal for enemy units
        //foreach (Character u in player.getCharacters())
        //{
        //    u.ResetAiGoal();
        //}
        /*foreach (Character u in MainScript.players[0].getCharacters())
        {
            // enemy unit, add as goal assuming is alive
            if (u.isAlive)
            {
                // Goal g = new Goal();
                // g.CreateGoal(Goal.GoalType.ATTACK, u.x, u.z, new WeightSet(u.behaviour), influenceMap);
                // CurrentGoals.Add(g);
            }
        }*/

        // Sort goals in descending order by priority
        /*var goalsByPriority = from ug in CurrentGoals
                              orderby ug.Priority descending
                              select ug;
        // add all our units as potential resources for every goal
        int cnt = 0;
        foreach (Goal g in goalsByPriority)
        {
            foreach (Character u in player.getCharacters())
            {
                // if unit is living, friendly, and not yet assigned a goal, add it as a potential resource
                if (u.isAlive && !u.HasAiGoal())
                {
                    g.AssignUnitResourceSuitability(u, new WeightSet(u.behaviour));
                }
                //else
                //{
                //    Debug.Log(u.name + " " + u.goalID);
                //}
            }
            // Does goal have enough potential resources
            if (g.HasSufficientResources || cnt == goalsByPriority.Count() - 1)//Assign last goal even if not enough ressources
            {
                // Assign resources to goal
                g.AssignGoalResources();
            }
            cnt++;
        }*/
        //Debug.Log("GOALS:::-------------");
        //foreach (Character u in player.getCharacters())
        //{
        //    Debug.Log(u.name + " " + u.goalID);
        //}
        startTurn = false;
    }

    /*private float GetDistanceToGoalImproval(Character u, Vector2 newLoc)
    {
        //Debug.Log(u.x + " " + u.z + " " + u.goalTarget.x + " "+u.goalTarget.y);
        float oldDistance = mainScript.GetDistance(u.x, u.z, (int)u.goalTarget.x, (int)u.goalTarget.y);
        float newDistance = mainScript.GetDistance((int)newLoc.x, (int)newLoc.y, (int)u.goalTarget.x, (int)u.goalTarget.y);
        //Debug.Log("old" + oldDistance);
        //Debug.Log("newloc" + newLoc);
        //Debug.Log("new" + newDistance);
        // return factor representing improvement in position, protect vs divide by 0
        return (oldDistance > 0) ? (1f - newDistance / oldDistance) : (1f - newDistance);
    }*/

    protected float ScoreLocationForCharacter(Vector2 location, LivingObject c)
    {
        float ret = 0;
        WeightSet w = new WeightSet();
        if (location.x == c.GridPosition.x && location.y == c.GridPosition.y)
        {
            ret = w.STAY;
        }
       /* switch (c.goalID)
        {
            case Goal.GoalType.ATTACK: ret += GetDistanceToGoalImproval(c, new Vector2(location.x, location.z)) * w.ATTACK_GOAL.TARGET_DISTANCE_FAKTOR; break;
        }*/
        

        return ret;

    }

    private int GetAttackRange(Vector2 location, Vector2 target)
    {
        return (int)(Mathf.Abs(location.x - target.x) + Mathf.Abs(location.y - target.y));
    }

    protected float ScoreAttackForUnit(LivingObject attacker, Vector2 location, LivingObject defender)
    {
        WeightSet w = new WeightSet();
        float ret = w.ATTACK_START_WEIGHT;//start with 10 so enemies will also attack if they get more dmg themselves(Most players like Enemies attacks even if the attack doesn't do much)
        /*if (attacker.goalID == Goal.GoalType.ATTACK && attacker.goalTarget == new Vector2(location.x, location.z))
        {
            ret += w.ATTACK_GOAL.ATTACK_TARGET_BONUS;
        }*/
        // will this kill the target?
        if (attacker.BattleStats.CanKillTarget(defender))
        {
            // bonus is 10, + current HP (do not want to reward too much overkill)
            ret += w.ATTACK_KILL_BONUS_WEIGHT + defender.Stats.HP * w.ATTACK_KILL_HP_MULT;
        }
        else
        {
            ret += attacker.BattleStats.GetTotalDamageAgainstTarget(defender) * w.DEALT_DAMAGE_MULT;//we value dealt damage higher than received damage
        }

        return ret;
    }

    protected void SubmitBestMoveForUnit(LivingObject unit)
    {
        float currentBestScore = -10000; // always want to do something
        Vector2 currentBestMoveLocation = new Vector3(unit.GridPosition.x, unit.GridPosition.y); // by default go nowhere
        CombatAction currentBestCombatAction = new CombatAction(CharacterAction.Wait, null, new Vector2()); // and do nothing

        // get all possible move locations for this unit
        List<Vector2> moveLocs = GetMoveLocations(unit);
        Vector2 startLoc = new Vector2(unit.GridPosition.x, unit.GridPosition.y);
        foreach (Vector2 loc in moveLocs)
        {
            SetCharacterPosition(unit, startLoc);
            // score this move location
            float locScore = ScoreLocationForCharacter(loc, unit);
            SetCharacterPosition(unit, loc);

            // get all possible actions at this location

            List<CharacterAction> acts = GetActionsForUnit(unit);
            /*if (acts.Contains(CharacterAction.Attack))
            {
                List<AttackTarget> targets = mainScript.GetAttackTargets((Character)unit);
                foreach (AttackTarget t in targets)
                {
                    if (t.character != null)
                    {
                        float attackscore = ScoreAttackForUnit((unit, loc, t.character);
                        if ((locScore + attackscore) > currentBestScore)
                        {
                            currentBestMoveLocation = loc;
                            currentBestScore = locScore + attackscore;
                            currentBestCombatAction = new CombatAction(CharacterAction.Attack, t.character, new Vector2());
                        }
                    }
                }
            }*/
            if (acts.Contains(CharacterAction.Wait) && locScore > currentBestScore)
            {
                currentBestMoveLocation = loc;
                currentBestScore = locScore;
                currentBestCombatAction = new CombatAction(CharacterAction.Wait, null, new Vector2());
            }

        }
        SetCharacterPosition(unit, startLoc);
        SubmitMove(unit, currentBestMoveLocation, currentBestCombatAction);
    }
}
