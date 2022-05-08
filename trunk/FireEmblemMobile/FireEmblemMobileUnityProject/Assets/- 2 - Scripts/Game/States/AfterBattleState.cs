using Game.GameActors.Units;
using Game.GameInput;
using Game.GUI.Text;
using Game.Manager;
using Game.Mechanics;
using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;
using Utility;

namespace Game.States
{
    public class AfterBattleState : GameState<NextStateTrigger>
    {

        private IBattleActor attacker;
        private IBattleActor defender;
        public AfterBattleState(IBattleActor attacker, IBattleActor defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }
        public override void Enter()
        {
            
         
    
            GridGameManager.Instance.GetSystem<UnitProgressSystem>().DistributeExperience(attacker, defender);
            if (!attacker.IsAlive())
            {
                attacker.Die();
            }
            if (!defender.IsAlive())
            {
                defender.Die();
            }

            Debug.Log("TODO IS ANIMATION FINISHED THEN CALL FINISHED");
            AnimationQueue.OnAllAnimationsEnded += Finished;
        }

        void Finished()
        {
            if(GridGameManager.Instance.FactionManager.ActiveFaction.IsPlayerControlled)
                GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.PlayerPhaseState);
            else
                GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.EnemyPhaseState);
        }
        public override void Exit()
        {
            AnimationQueue.OnAllAnimationsEnded -= Finished;
        }

        public override GameState<NextStateTrigger> Update()
        {
            if (AnimationQueue.IsNoAnimationRunning())
            {
                Debug.Log("AFTER BATTLE FINISHED!");
                Finished();
            }

            return NextState;
        }
    }
}