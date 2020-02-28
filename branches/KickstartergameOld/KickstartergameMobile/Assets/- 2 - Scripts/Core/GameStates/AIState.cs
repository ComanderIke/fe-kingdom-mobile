using Assets.Scripts;
using Assets.Scripts.GameStates;
using Assets.Scripts.Players;
using UnityEngine;


public class AIState : GameState<NextStateTrigger>
{

    public const float PAUSE_BETWEEN_ACTIONS = 0.5f;



    public float pausetime = 0;
    private Brain brain;

    public AIState()
    {
    }


    public override void Enter()
    {
        brain = new Brain(MainScript.instance.PlayerManager.ActivePlayer);
    }

    public override void Exit()
    {
    }

    public override GameState<NextStateTrigger> Update()
    {
        pausetime += Time.deltaTime;
        //wait so the player can follow what the AI is doing
        if (pausetime >= PAUSE_BETWEEN_ACTIONS)
        {
            pausetime = 0;
            
            if (!brain.finished)
            {
                brain.Think();
            }
            else
            {
                TurnSystem.onEndTurn();
                MainScript.instance.GameStateManager.Feed(NextStateTrigger.AISystemFinished);
            }
        }
        return nextState;

    }
}
