using Assets.Scripts.AI;
using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using Assets.Scripts.Players;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMonsterAI : AIInterface {

    bool startTurn = true;
    bool doingAction = false;
    Dictionary<LivingObject, List<AttackPattern>> attackPatterns;
    List<Goal> CurrentGoals;
    AttackPattern currentPattern;
    List<LivingObject> currentBestTargets;

    public SimpleMonsterAI(Player p):base (p){
        attackPatterns = new Dictionary<LivingObject, List<AttackPattern>>();
        CurrentGoals = new List<Goal>();
    }

    public override void Think()
    {
        if (doingAction)
            return;
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
            EventContainer.allCommandsFinished += FinishedAction;
            doingAction = true;
        }
        else
        {
            endturn = true;
        }
    }
    void FinishedAction()
    {
        EventContainer.allCommandsFinished -= FinishedAction;
        doingAction = false;
    }

    private void StartOfTurn()
    {
        //move Camera to first character
        List<LivingObject> units = GetUnitsLeftToMove();
        foreach(LivingObject unit in units)
        {
            if (unit.GridPosition is BigTilePosition)
            {
                List<AttackPattern> attackPattern = new List<AttackPattern>();
                attackPattern.Add(new Stampede(unit,((BigTilePosition)unit.GridPosition).Position, new Vector2(-1, 0), 3));
                attackPattern.Add(new Stampede(unit, ((BigTilePosition)unit.GridPosition).Position, new Vector2(1, 0), 3));
                attackPattern.Add(new Stampede(unit, ((BigTilePosition)unit.GridPosition).Position, new Vector2(0, 1), 3));
                attackPattern.Add(new Stampede(unit, ((BigTilePosition)unit.GridPosition).Position, new Vector2(0, -1), 3));
                attackPatterns.Add(unit, attackPattern);

            }
            else if(((Monster)unit).Type == MonsterType.Sabertooth)
            {
                List<AttackPattern> attackPattern = new List<AttackPattern>();
                //attackPattern.Add(new Howl(unit, unit.GridPosition.GetPos()));
                
                attackPattern.Add(new Flee(unit));
                attackPattern.Add(new LickWounds(unit));
                attackPattern.Add(new JumpAttack(unit));
                attackPatterns.Add(unit, attackPattern);
            }
        }
        
        //mainScript.MoveCameraTo(characters[0].x, characters[0].z);
        // reset all goals
        CurrentGoals.Clear();

        // reset our own unit goals from last turn, and create attack goal for enemy units
        foreach (LivingObject u in player.Units)
        {
            u.AIGoals.Clear();
        }
        foreach (Player p in MainScript.GetInstance().GetSystem<TurnSystem>().Players)
        {
            if (p != player)
            {
                foreach (LivingObject u in p.Units)
                {
                    // enemy unit, add as goal assuming is alive
                    if (u.IsAlive())
                    {
                        Goal g = new Goal(GoalType.ATTACK, u.GridPosition.x, u.GridPosition.y, new WeightSet());
                        CurrentGoals.Add(g);
                    }
                }
            }
        }

        // add all our units as potential resources for every goal
        int cnt = 0;
        foreach (Goal g in CurrentGoals)
        {
            foreach (LivingObject u in player.Units)
            {
                // if unit is living, friendly, and not yet assigned 4 goals, add it as a potential resource
                if (u.IsAlive() && u.AIGoals.Count <= 4)
                {
                    g.AssignUnitResourceSuitability(u, new WeightSet());
                }
            }
            // Does goal have enough potential resources
            if (g.HasSufficientRessources() || cnt == CurrentGoals.Count - 1)//Assign last goal even if not enough ressources
            {
                // Assign resources to goal
                g.AssignGoalRessources();
            }
            cnt++;
        }
        startTurn = false;
    }

    private float GetDistanceToGoalImproval(LivingObject u, Vector2 newLoc)
    {
        float oldDistance=0;
        float newDistance=0;
        
        foreach(Goal g in u.AIGoals)
        {
            oldDistance += mainScript.GetSystem<GridSystem>().GetDistance(u.GridPosition.x, u.GridPosition.y, g.x, g.y, u.Player.ID);
            newDistance += mainScript.GetSystem<GridSystem>().GetDistance((int)newLoc.x, (int)newLoc.y, g.x, g.y, u.Player.ID);
        }
        
        // return factor representing improvement in position, protect vs divide by 0
        return (oldDistance > 0) ? (1f - newDistance / oldDistance) : (1f - newDistance);
    }

    protected float ScoreLocationForCharacter(Vector2 location, LivingObject c)
    {
        float ret = 0;
        WeightSet w = new WeightSet();
        if (location.x == c.GridPosition.x && location.y == c.GridPosition.y)
        {
            ret = w.STAY;
            if (currentPattern.Type == AttackPatternType.Passive)
                ret = 1000000;
        }
        if(currentPattern.Type == AttackPatternType.Aggressive)
            ret += GetDistanceToGoalImproval(c, new Vector2(location.x, location.y)) * w.ATTACK_GOAL.TARGET_DISTANCE_FAKTOR;
        else
            ret -= GetDistanceToGoalImproval(c, new Vector2(location.x, location.y)) * w.ATTACK_GOAL.TARGET_DISTANCE_FAKTOR;
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
        if (attacker.BattleStats.CanKillTarget(defender,1.0f))
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
        List<AttackPattern> aps = attackPatterns[unit];
        currentPattern = null;
        int cnt = 0;
        foreach (AttackPattern ap in aps)
        {
            if (true)
            {
                currentPattern = ap;
                cnt = ap.TargetCount;
            }
        }

        float currentBestScore = -10000; // always want to do something
                                         //Vector2 currentBestMoveLocation = new Vector3(unit.GridPosition.x, unit.GridPosition.y); // by default go nowhere
                                         //CombatAction currentBestCombatAction = new CombatAction(CharacterAction.Wait, null, new Vector2()); // and do nothing

        // get all possible move locations for this unit
        List<Vector2> moveLocs = GetMoveLocations(unit);
        Vector2 startLoc = new Vector2(unit.GridPosition.x, unit.GridPosition.y);
        Vector2 currentBestMoveLocation = new Vector2();
        currentBestTargets = new List<LivingObject>();
        foreach (Vector2 loc in moveLocs)
        {
            SetCharacterPosition(unit, startLoc);
            // score this move location
            float locScore = ScoreLocationForCharacter(loc, unit);
            SetCharacterPosition(unit, loc);
           
            if(currentPattern.TargetType == AttackTargetType.SingleEnemy)
            {
                List<LivingObject> targets = mainScript.GetSystem<GridSystem>().GridLogic.GetAttackTargets((LivingObject)unit);

                foreach (LivingObject t in targets)
                {
                    float attackscore = ScoreAttackForUnit(unit, loc, t);
                    if ((locScore + attackscore) > currentBestScore)
                    {
                        currentBestMoveLocation = loc;
                        currentBestScore = locScore + attackscore;
                        
                        currentBestTargets.Add(t);
                    }
                }
                
                if(currentBestTargets.Count!=0)
                    currentPattern.TargetPositions.Add(currentBestTargets[0].GridPosition.GetPos());
            }
            else
            {
                if (locScore > currentBestScore)
                {
                    currentBestMoveLocation = loc;
                    currentBestScore = locScore;

                }
            }

        }
        SetCharacterPosition(unit, startLoc);
        SubmitMove(unit, currentBestMoveLocation);
        unit.UnitTurnState.IsWaiting = true;
        EventContainer.allCommandsFinished += SwitchToAIState;
        Debug.Log(currentPattern);
        mainScript.GetSystem<UnitActionSystem>().AddCommand(currentPattern);
        //will also execute all previous commands like Movement
        mainScript.GetSystem<UnitActionSystem>().ExecuteActions();
        
        
    }

    private void SwitchToAIState()
    {
        Debug.Log("SwitchToAIState");
        EventContainer.allCommandsFinished -= SwitchToAIState;
        mainScript.SwitchState(new AIState(this.player));
    }
}
