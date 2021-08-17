﻿using System;
using Audio;
using Game.AI;
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
    public class BattleState : GameState<NextStateTrigger>, IDependecyInjection
    {
        public static Action OnEnter;
        public static Action OnExit;

        private IBattleActor attacker;
        private IBattleActor defender;
        
        public BattleSystem battleSystem; //Injected
        public IBattleAnimation BattleAnimation;

        private string startMusic;
        public bool IsFinished;

        public BattleState()
        {
            //battleRenderer = GameObject.FindObjectOfType<IBattleRenderer>();
            //battleRenderer = DataScript.Instance.battleRenderer;
            //battleRenderer = Resources.Load(BattlerendererPrefab);
            // Inject from Constructor
            // Inject with Setter
            // No BattleRenderer dependency at all and use Events

        }

        public void SetParticipants(IBattleActor attacker, IBattleActor defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }

        public override void Enter()
        {
           // battleSystem = new BattleSystem(attacker, defender);
          
           IsFinished = false;
           BattleAnimation.Show(battleSystem.GetBattleSimulation(attacker,defender));
           battleSystem.StartBattle(attacker, defender);
           BattleAnimation.OnFinished += battleSystem.EndBattle;
         
            //Debug.Log("ENTER FIGHTSTATE");

            SetUpMusic();
            OnEnter?.Invoke();
        }

        public override GameState<NextStateTrigger> Update()
        {
            if (battleSystem.IsFinished)
            {
                IsFinished = true;
                GridGameManager.Instance.GameStateManager.SwitchState(new AfterBattleState(attacker, defender));
 
            }
            return null;
        }

        public override void Exit()
        {
            // HideFightVisuals();
            attacker.TurnStateManager.HasAttacked = true;
           // Debug.Log("EXIT BATTLESTATE");
            
            
            attacker = null;
            defender = null;

            BattleAnimation.Hide();
            BattleAnimation.OnFinished -= battleSystem.EndBattle;
            GridGameManager.Instance.GetSystem<AudioSystem>().ChangeMusic(startMusic, "BattleTheme", true);
            OnExit?.Invoke();
        }

        

        private void SetUpMusic()
        {
            startMusic = GridGameManager.Instance.GetSystem<AudioSystem>().GetCurrentlyPlayedMusicTracks()[0];
            GridGameManager.Instance.GetSystem<AudioSystem>().ChangeMusic("BattleTheme", startMusic);
        }

        public void Start(IBattleActor battleActor, IBattleActor target)
        {
            SetParticipants(battleActor, target);
            GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.BattleStarted);
        }
    }

    public interface IBattleAnimation
    {
        void Show(BattleSimulation battleSimulation);
        void Hide();
        event Action OnFinished;
    }
}