using Assets.Scripts.AI;
using Assets.Scripts.AI.AttackPatterns;
using Assets.Scripts.Characters;
using Assets.Scripts.Commands;
using Assets.Scripts.GameStates;
using Assets.Scripts.Players;
using System.Collections.Generic;
using UnityEngine;

public class Brain
{
    protected Army player;
    public bool finished = false;
    bool startTurn = true;
    List<Goal> CurrentGoals;
    public Brain(Army player)
    {
        this.player = player;
    }

    public void Think()
    {
        if (IsStartOfTurn())
        {
           StartOfTurn();
        }
        List<Unit> units = player.GetActiveUnits();
        if (units.Count > 0)
        {
            ChooseBestMove(units);
            UnitActionSystem.onAllCommandsFinished += FinishedAction;
        }
        else
        {
            // we are finished
            finished = true;
        }
    }
    private void ChooseBestMove(List<Unit> units)
    {
        float bestScore = float.MinValue;
        Unit bestUnit = null;
        foreach(Unit u in units)
        {
            float unitScore = CalculateBestMove(u);
            if (unitScore > bestScore)
            {
                bestScore = unitScore;
                bestUnit = u;
            }
        }
        SubmitBestMoveForUnit(bestUnit);
    }
    private float CalculateBestMove(Unit unit)
    {
        float currentBestScore = float.MinValue;
        Vector2 currentBestMoveLocation = new Vector3(unit.GridPosition.x, unit.GridPosition.y); //By default stay on the same position
        AttackPattern currentBestAction = null; // By default do nothing

        //List<Vector2> moveLocs = GetMoveLocations(unit);
        Debug.Log("TODO Finish Calculating Best Move");
        return currentBestScore;
    }

    protected void SubmitBestMoveForUnit(Unit unit)
    {

        //float currentBestScore = float.MinValue; // always want to do something
        //                                 //Vector2 currentBestMoveLocation = new Vector3(unit.GridPosition.x, unit.GridPosition.y); // by default go nowhere
        //                                 //CombatAction currentBestCombatAction = new CombatAction(CharacterAction.Wait, null, new Vector2()); // and do nothing

        //// get all possible move locations for this unit
        //List<Vector2> moveLocs = mainScript.GetSystem<GridSystem>().GetMovement((int)c.GameTransform.GameObject.transform.position.x, (int)c.GameTransform.GameObject.transform.position.y, c.Stats.MoveRange, c.Player.ID);
        //Vector2 startLoc = new Vector2(unit.GridPosition.x, unit.GridPosition.y);
        //Vector2 currentBestMoveLocation = new Vector2();
        //Unit currentBestTarget = null;
        //foreach (Vector2 loc in moveLocs)
        //{
        //    SetCharacterPosition(unit, startLoc);
        //    // score this move location
        //    float locScore = ScoreLocationForCharacter(loc, unit);
        //    SetCharacterPosition(unit, loc);

        //    List<Unit> targets = mainScript.GetSystem<GridSystem>().GridLogic.GetAttackTargets((Unit)unit);

        //    foreach (Unit t in targets)
        //    {
        //        float attackscore = ScoreAttackForUnit(unit, loc, t);
        //        if ((locScore + attackscore) > currentBestScore)
        //        {
        //            currentBestMoveLocation = loc;
        //            currentBestScore = locScore + attackscore;

        //            currentBestTarget = t;
        //        }
        //    }

        //    if (locScore > currentBestScore)
        //    {
        //        currentBestMoveLocation = loc;
        //        currentBestScore = locScore;

        //    }

        //}
        //SetCharacterPosition(unit, startLoc);
        //SubmitMove(unit, currentBestMoveLocation);
        //unit.UnitTurnState.IsWaiting = true;
        //UnitActionSystem.onAllCommandsFinished += SwitchToAIState;
        //Debug.Log("TODO add AttackCommand!");
        //mainScript.GetSystem<UnitActionSystem>().AddCommand(currentPattern);
        ////will also execute all previous commands like Movement
        //mainScript.GetSystem<UnitActionSystem>().ExecuteActions();


    }

    void FinishedAction()
    {
        UnitActionSystem.onAllCommandsFinished -= FinishedAction;
    }

    private bool IsStartOfTurn()
    {
        foreach (Unit u in player.Units)
        {
            if (!u.IsActive())
                return false;
        }
        return true;
        
    }

    private void StartOfTurn()
    {
        List<Unit> units = player.GetActiveUnits();

        Debug.Log("TODO:  Move Camera To Moving Character");

        ResetGoals(units);
        CreateGoalsForAllEnemyUnits();

        // add all our units as potential resources for every goal
        AssignUnitsAsGoalRessources();
    }

    private void AssignUnitsAsGoalRessources()
    {
        int cnt = 0;
        foreach (Goal g in CurrentGoals)
        {
            foreach (Unit u in player.Units)
            {
                // if unit is not yet assigned 4 goals, add it as a potential resource
                if (u.IsAlive() && u.Agent.AIGoals.Count <= 4)
                {
                    g.AssignUnitResourceSuitability(u, u.Agent.WeightSet);
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
    }

    private void CreateGoalsForAllEnemyUnits()
    {
        foreach (Army p in player.GetOpponentArmys())
        {
            foreach (Unit u in p.Units)
            {
                if (u.IsAlive())
                {
                    Goal g = new Goal(GoalType.ATTACK, u.GridPosition.x, u.GridPosition.y);
                    CurrentGoals.Add(g);
                }
            }
        }
    }

    private void ResetGoals(List<Unit> units)
    {
        CurrentGoals.Clear();

        foreach (Unit u in units)
        {
            u.Agent.AIGoals.Clear();
        }
    }
}