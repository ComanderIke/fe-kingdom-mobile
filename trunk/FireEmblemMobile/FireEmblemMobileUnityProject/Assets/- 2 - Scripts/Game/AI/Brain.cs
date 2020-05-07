using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.AI
{
    public class Brain
    {
    //    private List<Goal> currentGoals;
    //    private bool finished;
    //    protected Faction Player;

    //    public Brain(Faction player)
    //    {
    //        Player = player;
    //    }

    //    public void Think()
    //    {
    //        if (IsStartOfTurn()) StartOfTurn();

    //        var units = Player.GetActiveUnits();
    //        if (units.Count > 0)
    //        {
    //            ChooseBestMove(units);
    //            UnitActionSystem.OnAllCommandsFinished += FinishedAction;
    //        }
    //        else
    //        {
    //            // we are finished
    //            finished = true;
    //        }
    //    }

    //    public bool IsFinished()
    //    {
    //        return finished;
    //    }

    //    private void ChooseBestMove(IEnumerable<Unit> units)
    //    {
    //        var bestScore = float.MinValue;
    //        Unit bestUnit = null;
    //        foreach (var u in units)
    //        {
    //            float unitScore = CalculateBestMove(u);
    //            if (unitScore > bestScore)
    //            {
    //                bestScore = unitScore;
    //                bestUnit = u;
    //            }
    //        }

    //        SubmitBestMoveForUnit(bestUnit);
    //    }

    //    private static float CalculateBestMove(Unit unit)
    //    {
    //        var currentBestScore = float.MinValue;
    //        Vector2 currentBestMoveLocation =
    //            new Vector3(unit.GridPosition.X, unit.GridPosition.Y); //By default stay on the same position

    //        //List<Vector2> moveLocs = GetMoveLocations(unit);
    //        Debug.Log("TODO Finish Calculating Best Move");
    //        return currentBestScore;
    //    }

    //    protected void SubmitBestMoveForUnit(Unit unit)
    //    {
    //        //float currentBestScore = float.MinValue; // always want to do something
    //        //                                 //Vector2 currentBestMoveLocation = new Vector3(unit.GridPosition.x, unit.GridPosition.y); // by default go nowhere
    //        //                                 //CombatAction currentBestCombatAction = new CombatAction(CharacterAction.Wait, null, new Vector2()); // and do nothing

    //        //// get all possible move locations for this unit
    //        //List<Vector2> moveLocs = mainScript.GetSystem<GridSystem>().GetMovement((int)c.GameTransform.GameObject.transform.position.x, (int)c.GameTransform.GameObject.transform.position.y, c.Stats.MoveRange, c.Player.ID);
    //        //Vector2 startLoc = new Vector2(unit.GridPosition.x, unit.GridPosition.y);
    //        //Vector2 currentBestMoveLocation = new Vector2();
    //        //Unit currentBestTarget = null;
    //        //foreach (Vector2 loc in moveLocs)
    //        //{
    //        //    SetCharacterPosition(unit, startLoc);
    //        //    // score this move location
    //        //    float locScore = ScoreLocationForCharacter(loc, unit);
    //        //    SetCharacterPosition(unit, loc);

    //        //    List<Unit> targets = mainScript.GetSystem<GridSystem>().GridLogic.GetAttackTargets((Unit)unit);

    //        //    foreach (Unit t in targets)
    //        //    {
    //        //        float attackscore = ScoreAttackForUnit(unit, loc, t);
    //        //        if ((locScore + attackscore) > currentBestScore)
    //        //        {
    //        //            currentBestMoveLocation = loc;
    //        //            currentBestScore = locScore + attackscore;

    //        //            currentBestTarget = t;
    //        //        }
    //        //    }

    //        //    if (locScore > currentBestScore)
    //        //    {
    //        //        currentBestMoveLocation = loc;
    //        //        currentBestScore = locScore;

    //        //    }

    //        //}
    //        //SetCharacterPosition(unit, startLoc);
    //        //SubmitMove(unit, currentBestMoveLocation);
    //        //unit.UnitTurnState.IsWaiting = true;
    //        //UnitActionSystem.onAllCommandsFinished += SwitchToAIState;
    //        //Debug.Log("TODO add AttackCommand!");
    //        //mainScript.GetSystem<UnitActionSystem>().AddCommand(currentPattern);
    //        ////will also execute all previous commands like Movement
    //        //mainScript.GetSystem<UnitActionSystem>().ExecuteActions();
    //    }

    //    private static void FinishedAction()
    //    {
    //        UnitActionSystem.OnAllCommandsFinished -= FinishedAction;
    //    }

    //    private bool IsStartOfTurn()
    //    {
    //        return Player.Units.All(u => u.IsActive());
    //    }

    //    private void StartOfTurn()
    //    {
    //        var units = Player.GetActiveUnits();

    //        Debug.Log("TODO:  Move Camera To Moving Character");

    //        ResetGoals(units);
    //        CreateGoalsForAllEnemyUnits();

    //        // add all our units as potential resources for every goal
    //        AssignUnitsAsGoalResources();
    //    }

    //    private void AssignUnitsAsGoalResources()
    //    {
    //        var cnt = 0;
    //        foreach (var g in currentGoals)
    //        {
    //            foreach (var u in Player.Units.Where(u => u.IsAlive() && u.Agent.AIGoals.Count <= 4))
    //                g.AssignUnitResourceSuitability(u, u.Agent.WeightSet);

    //            // Does goal have enough potential resources
    //            if (g.HasSufficientRessources() || cnt == currentGoals.Count - 1
    //                ) //Assign last goal even if not enough ressources
    //                // Assign resources to goal
    //                g.AssignGoalRessources();

    //            cnt++;
    //        }
    //    }

    //    private void CreateGoalsForAllEnemyUnits()
    //    {
    //        foreach (var unit in from faction in Player.GetOpponentFactions()
    //            from unit in faction.Units
    //            where unit.IsAlive()
    //            select unit)
    //            currentGoals.Add(new Goal(GoalType.Attack, unit.GridPosition.X, unit.GridPosition.Y));
    //    }

    //    private void ResetGoals(IEnumerable<Unit> units)
    //    {
    //        currentGoals.Clear();

    //        foreach (var u in units) u.Agent.AIGoals.Clear();
    //    }
    }
}