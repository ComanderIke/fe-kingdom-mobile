using System.Collections.Generic;
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

        private Unit attacker;
       
        private List<Unit> defenders;
        public AfterBattleState(Unit attacker, Unit defender)
        {
            this.attacker = attacker;
            defenders = new List<Unit>();
            defenders.Add(defender);
        }
        public AfterBattleState(Unit attacker, List<Unit> defenders)
        {
            this.attacker = attacker;
            this.defenders = defenders;
        }
        public override void Enter()
        {

            if (!attacker.IsAlive())
            {
                attacker.Die();
            }
            AnimationQueue.Add(GameObject.FindObjectOfType<ExpParticleSystem>().Play);
            foreach (var defender in defenders)
            {
                
                
                if(attacker.IsAlive())
                    GridGameManager.Instance.GetSystem<UnitProgressSystem>().DistributeExperience(attacker, defender);
                if (!defender.IsAlive())
                {
                    defender.Die();
                }
            }
    
            
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