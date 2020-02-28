using Assets.Scripts.GameStates;
using UnityEngine;

public class WinState : GameState<NextStateTrigger>
{
    public override void Enter()
    {
        Debug.Log("Player Won");
    }

    public override void Exit()
    {
        
    }

    public override GameState<NextStateTrigger> Update()
    {
        return nextState;
    }
}