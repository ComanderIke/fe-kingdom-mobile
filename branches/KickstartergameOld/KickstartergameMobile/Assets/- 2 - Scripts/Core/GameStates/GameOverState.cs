using Assets.Scripts.GameStates;
using Assets.Scripts.Players;
using System;
using UnityEngine;
using UnityEngine.UI;


public class GameOverState : GameState<NextStateTrigger>
{

    public GameOverState()
    {
       
    }
    public override void Enter()
    {
        Debug.Log("Game Over");
    }

    public override void Exit()
    {
    }

    public override GameState<NextStateTrigger> Update()
    {
        return nextState;
    }
}
