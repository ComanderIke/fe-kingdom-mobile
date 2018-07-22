using Assets.__2___Scripts.Mechanics;
using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using Assets.Scripts.Grid.PathFinding;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : GameState<NextStateTrigger>
{
    private readonly int x;
    private readonly int y;
    public int pathCounter = 0;
    public MovementPath path;
    private MainScript mainScript;
    private Unit unit;
    List<Vector2> mousePath;
    bool active;

    public MovementState(Unit c, int x, int y, List<Vector2> path = null)
    {
        mousePath = path;
        this.x = x;
        this.y = y;
        mainScript = MainScript.instance;
        unit = c;
        pathCounter = 0;
    }

    public override void Enter()
    {
        if (unit.GridPosition.x == x && unit.GridPosition.y == y)//already on Destination
        {
            FinishMovement();
            return;
        }
        active = true;
        UnitActionSystem.onStartMovingUnit();
        if (mousePath == null || mousePath.Count == 0)
        {
            path = mainScript.GetSystem<MoveSystem>().getPath(unit.GridPosition.x, unit.GridPosition.y, x, y, unit.Player.ID, false, new List<int>());
            if (path != null)
                path.Reverse();
        }
        else
        {
            path = new MovementPath();
            for (int i = 0; i < mousePath.Count; i++)
            {
                path.prependStep(mousePath[i].x, mousePath[i].y);
            }
        }
    }

    public override GameState<NextStateTrigger> Update()
    {
        if (!active)
            return null;
        MoveUnit();
        return nextState;
    }

    void MoveUnit()
    {
        float x = unit.GameTransform.GameObject.transform.localPosition.x;
        float y = unit.GameTransform.GameObject.transform.localPosition.y;
        float z = unit.GameTransform.GameObject.transform.localPosition.z;
        float tx = path.getStep(pathCounter).getX();
        float ty = path.getStep(pathCounter).getY();
        float walkspeed = 5f;
        float value = walkspeed * Time.deltaTime;
        float offset = 0.05f;
        x = (x + offset > tx && x - offset < tx) || (x == tx) ? tx : x + (x < tx ? value : -value);
        y = (y + offset > ty && y - offset < ty) || (y == ty) ? ty : y + (y < ty ? value : -value);
        unit.GameTransform.GameObject.transform.localPosition = new Vector3(x, y, z);

        if (x == tx && y == ty)
        {
            pathCounter++;
        }

        if (pathCounter >= path.getLength())
        {
            active = false;
            FinishMovement();
        }
    }
    public override void Exit()
    {
        unit.SetPosition(x, y);
        UnitActionSystem.onStopMovingUnit();
    }
    void FinishMovement()
    {
        unit.UnitTurnState.HasMoved = true;
        if (unit.Player.IsPlayerControlled)
            mainScript.GetSystem<UnitActionSystem>().ActiveCharWait();
        UnitActionSystem.onCommandFinished();
    }
}

