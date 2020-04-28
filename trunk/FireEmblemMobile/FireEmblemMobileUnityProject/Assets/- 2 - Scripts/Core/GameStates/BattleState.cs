using Assets.Audio;
using Assets.GameActors.Units;
using Assets.GUI;
using Assets.Mechanics;
using System;
using UnityEngine;

namespace Assets.Core.GameStates
{
    public class BattleState : GameState<NextStateTrigger>
    {
        public static Action OnEnter;
        public static Action OnExit;

        private Unit attacker;
        private Unit defender;
        public BattleSystem BattleSystem;

        private string startMusic;
        private readonly UiSystem uiController;
        private readonly UnitsSystem unitController;
        private readonly BattleRenderer battleRenderer;

        public BattleState()
        {
            uiController = GridGameManager.Instance.GetSystem<UiSystem>();
            unitController = GridGameManager.Instance.GetSystem<UnitsSystem>();
            battleRenderer = uiController.BattleRenderer;
        }

        public void SetParticipants(Unit attacker, Unit defender)
        {
            this.attacker = attacker;
            this.defender = defender;
        }

        public override void Enter()
        {
            BattleSystem = new BattleSystem(attacker, defender);
            BattleSystem.StartBattle();
            BattleRenderer.OnAttackConnected += BattleSystem.ContinueBattle;
            BattleRenderer.OnFinished += BattleSystem.EndBattle;
            
            //Debug.Log("ENTER FIGHTSTATE");
            ShowFightVisuals(BattleSystem.GetAttackSequence());
           
            SetUpMusic();
            OnEnter?.Invoke();
        }

        public override GameState<NextStateTrigger> Update()
        {
            return NextState;
        }

        public override void Exit()
        {
            HideFightVisuals();
            attacker.UnitTurnState.HasAttacked = true;
            attacker = null;
            defender = null;

            BattleRenderer.OnAttackConnected -= BattleSystem.ContinueBattle;
            BattleRenderer.OnFinished -= BattleSystem.EndBattle;
            GridGameManager.Instance.GetSystem<AudioSystem>().ChangeMusic(startMusic, "BattleTheme", true);
            OnExit?.Invoke();
        }

        private void ShowFightVisuals(bool [] attackSequence)
        {
            uiController.ShowFightUi(attacker, defender);
            battleRenderer.Show(attacker, defender, attackSequence);
        }
        private void HideFightVisuals()
        {
            uiController.HideFightUi();
            battleRenderer.Hide();
        }

        private void SetUpMusic()
        {
            startMusic = GridGameManager.Instance.GetSystem<AudioSystem>().GetCurrentlyPlayedMusicTracks()[0];
            GridGameManager.Instance.GetSystem<AudioSystem>().ChangeMusic("BattleTheme", startMusic);
        }
    }
}