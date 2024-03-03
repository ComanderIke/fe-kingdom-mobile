using System;
using Game.AI.DecisionMaking;
using Game.Audio;
using Game.GameActors.InteractableGridObjects;
using Game.GameActors.Units.Interfaces;
using Game.Manager;
using Game.Systems;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.States
{
    public class BattleState : GameState<NextStateTrigger>
    {
        public static Action OnEnter;
        public static Action OnExit;

        private IBattleActor attacker;
        private IAttackableTarget attackedTarget;
        
        public BattleSystem battleSystem; //Injected

        private string startMusic;
        public bool IsFinished;
        

        public void SetParticipants(IBattleActor attacker, IAttackableTarget attackedTarget)
        {
            this.attacker = attacker;
            this.attackedTarget = attackedTarget;
        }

       
        public override void Enter()
        {
            Debug.Log("ENTER BATTLE STATE");
           // battleSystem = new BattleSystem(attacker, defender);
          
           IsFinished = false;

           if(attackedTarget is IBattleActor actor)
            battleSystem.StartBattle(attacker, actor, true, false);
           else if(attackedTarget is IAttackableTargetThatCantFightBack target)
           {
               battleSystem.StartBattle(attacker, target);
           }
           
           BattleSystem.OnBattleFinished -= EndBattle;
           BattleSystem.OnBattleFinished += EndBattle;
           BattleSystem.OnBattleFinishedBeforeAfterBattleStuff -= ActualBattleEnded;
           BattleSystem.OnBattleFinishedBeforeAfterBattleStuff += ActualBattleEnded;
         
            //Debug.Log("ENTER FIGHTSTATE");

            AudioSystem.Instance.SwitchIntoBattle();
            OnEnter?.Invoke();
        }

        public override GameState<NextStateTrigger> Update()
        {
            if (IsFinished)
            {
                
            }
            return null;
        }

        private void ActualBattleEnded(AttackResult result)
        {
            AudioSystem.Instance.SwitchOutofBattle();
        }
        private void EndBattle(AttackResult result)
        {
            
            Debug.Log("Battle Ended");
            BattleSystem.OnBattleFinished -= EndBattle;
            if(GridGameManager.Instance.FactionManager.ActiveFaction.IsPlayerControlled)
                GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.PlayerPhaseState);
            // else
            //     GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.EnemyPhaseState);
            

        }
        public override void Exit()
        {
            // Debug.Log("BattleStateExit");
            // HideFightVisuals();
            attacker.TurnStateManager.HasAttacked = true;

            BattleSystem.OnBattleFinishedBeforeAfterBattleStuff -= ActualBattleEnded;
            battleSystem.CleanUp();
            attacker = null;
            attackedTarget = null;

            OnExit?.Invoke();
        }

        
        

        public static event Action<IBattleActor, IAttackableTarget> OnStartBattle;
        public void Start(IBattleActor battleActor, IAttackableTarget target)
        {
            SetParticipants(battleActor, target);
            OnStartBattle?.Invoke(battleActor, target);
            GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.BattleStarted);
        }
    }
}