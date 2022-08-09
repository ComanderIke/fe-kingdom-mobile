﻿using System;
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
        private IAttackableTarget defender;
        
        public BattleSystem battleSystem; //Injected
        public IBattleAnimation BattleAnimation;
        public IBattleAnimation MapBattleAnimation;

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

        public void SetParticipants(IBattleActor attacker, IAttackableTarget defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }

        private BattleSimulation battleSimulation;
        public override void Enter()
        {
           // battleSystem = new BattleSystem(attacker, defender);
          
           IsFinished = false;

           if (defender is IBattleActor)
           {
               battleSimulation = battleSystem.GetBattleSimulation(attacker, (IBattleActor)defender);
               BattleAnimation.Show(battleSimulation, attacker, (IBattleActor)defender);
               BattleAnimation.OnFinished += EndBattle;
           }
           else{
              battleSimulation = battleSystem.GetBattleSimulation(attacker, defender);
             MapBattleAnimation.Show(battleSimulation, attacker, defender);
             MapBattleAnimation.OnFinished += EndBattle;
           }
         //  battleSystem.StartBattle(attacker, defender); TODO same RNG as battleAnimation
           
         
            //Debug.Log("ENTER FIGHTSTATE");

            SetUpMusic();
            OnEnter?.Invoke();
        }

        public override GameState<NextStateTrigger> Update()
        {
            if (IsFinished)
            {
                GridGameManager.Instance.GameStateManager.SwitchState(new AfterBattleState((Unit)attacker, defender));
 
            }
            return null;
        }

        private void EndBattle()
        {

            attacker.Hp = battleSimulation.Attacker.Hp;
            if (battleSimulation.AttackableTarget == null)
                defender.Hp = battleSimulation.Defender.Hp;
            else
                defender.Hp = battleSimulation.AttackableTarget.Hp;
            // if(battleSimulation.DefenderAttackCount!=0)
            //     defender.SpBars--; 
            // attacker.SpBars--;
          
       
            //battleStarted = false;
            //BattleRenderer.Hide();
            IsFinished = true;
            //GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.BattleEnded);

        }
        public override void Exit()
        {
            // HideFightVisuals();
            attacker.TurnStateManager.HasAttacked = true;
            Debug.Log("EXIT BATTLESTATE");
            
            
            attacker = null;
            defender = null;

            BattleAnimation.Hide();
            MapBattleAnimation.Hide();
            BattleAnimation.OnFinished -= EndBattle;
            MapBattleAnimation.OnFinished -= EndBattle;
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

    public interface IBattleAnimation
    {
        void Show(BattleSimulation battleSimulation, IBattleActor attacker, IAttackableTarget defender);
        void Hide();
        event Action OnFinished;
    }
}