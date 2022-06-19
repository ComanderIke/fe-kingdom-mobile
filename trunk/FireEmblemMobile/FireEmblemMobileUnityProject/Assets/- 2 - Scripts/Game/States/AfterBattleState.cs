using System.Collections.Generic;
using Game.GameActors.Players;
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
       
        private List<IAttackableTarget> defenders;
        public AfterBattleState(Unit attacker, IAttackableTarget defender)
        {
            this.attacker = attacker;
            defenders = new List<IAttackableTarget>();
            defenders.Add(defender);
        }
        public AfterBattleState(Unit attacker, List<IAttackableTarget> defenders)
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
            bool gettingexp = false;
          
            foreach (var defender in defenders)
            {

                if (defender is Unit unitDefender)
                {
                    if (!gettingexp)
                    {
                        gettingexp = true;
                        AnimationQueue.Add(GameObject.FindObjectOfType<ExpParticleSystem>().Play);
                    }

                    if (attacker.IsAlive())
                        GridGameManager.Instance.GetSystem<UnitProgressSystem>()
                            .DistributeExperience(attacker, unitDefender);
                }
                Debug.Log(defender+" Check IsAlive");
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