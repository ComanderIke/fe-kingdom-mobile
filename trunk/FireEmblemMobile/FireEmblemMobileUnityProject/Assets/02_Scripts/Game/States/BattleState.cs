using System;
using Audio;
using Game.AI;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using Game.GameResources;
using Game.Graphics;
using Game.GUI;
using Game.Manager;
using Game.States;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Mechanics
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
         
            //Debug.Log("ENTER FIGHTSTATE");

            SetUpMusic();
            OnEnter?.Invoke();
        }

        public override GameState<NextStateTrigger> Update()
        {
            if (IsFinished)
            {
                
            }
            return null;
        }

        private void EndBattle(AttackResult result)
        {
            BattleSystem.OnBattleFinished -= EndBattle;
            if(GridGameManager.Instance.FactionManager.ActiveFaction.IsPlayerControlled)
                GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.PlayerPhaseState);
            else
                GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.EnemyPhaseState);
            

        }
        public override void Exit()
        {
            
            // HideFightVisuals();
            attacker.TurnStateManager.HasAttacked = true;

            battleSystem.CleanUp();
            attacker = null;
            attackedTarget = null;

            GridGameManager.Instance.GetSystem<AudioSystem>().ChangeMusic(startMusic, "BattleTheme", true);
            OnExit?.Invoke();
        }

        

        private void SetUpMusic()
        {
            startMusic = GridGameManager.Instance.GetSystem<AudioSystem>().GetCurrentlyPlayedMusicTracks()[0];
            GridGameManager.Instance.GetSystem<AudioSystem>().ChangeMusic("BattleTheme", startMusic);
        }

        public void Start(IBattleActor battleActor, IAttackableTarget target)
        {
            SetParticipants(battleActor, target);
            GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.BattleStarted);
        }
    }
}