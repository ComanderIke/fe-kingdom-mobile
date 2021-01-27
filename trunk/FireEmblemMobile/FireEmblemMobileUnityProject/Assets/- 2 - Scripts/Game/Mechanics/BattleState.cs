using System;
using Audio;
using Game.GameActors.Units;
using Game.GUI;
using Game.Manager;
using GameEngine;
using GameEngine.GameStates;

namespace Game.Mechanics
{
    public class BattleState : GameState<NextStateTrigger>
    {
        public static Action OnEnter;
        public static Action OnExit;

        private Unit attacker;
        private Unit defender;
        public BattleSystem BattleSystem;

        private string startMusic;
        //private readonly UiSystem uiController;//TODO bad Dependency?
        private readonly UnitsSystem unitController;//TODO bad Dependency?
       // private readonly BattleRenderer battleRenderer;

        public BattleState()
        {
           // uiController = GridGameManager.Instance.GetSystem<UiSystem>();
            unitController = GridGameManager.Instance.GetSystem<UnitsSystem>();
            //battleRenderer = uiController.BattleRenderer;//TODO bad dependency
        }
//test
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
           // ShowFightVisuals(BattleSystem.GetAttackSequence());
           
            SetUpMusic();
            OnEnter?.Invoke();
        }

        public override GameState<NextStateTrigger> Update()
        {
            return NextState;
        }

        public override void Exit()
        {
            // HideFightVisuals();
            attacker.TurnStateManager.HasAttacked = true;
            attacker = null;
            defender = null;

            BattleRenderer.OnAttackConnected -= BattleSystem.ContinueBattle;
            BattleRenderer.OnFinished -= BattleSystem.EndBattle;
            GridGameManager.Instance.GetSystem<AudioSystem>().ChangeMusic(startMusic, "BattleTheme", true);
            OnExit?.Invoke();
        }

        // private void ShowFightVisuals(bool [] attackSequence)
        // {
        //    uiController.ShowFightUi(attacker, defender);
        //    battleRenderer.Show(attacker, defender, attackSequence);
        // }
        // private void HideFightVisuals()
        // {
        //     uiController.HideFightUi();
        //     battleRenderer.Hide();
        // }

        private void SetUpMusic()
        {
            startMusic = GridGameManager.Instance.GetSystem<AudioSystem>().GetCurrentlyPlayedMusicTracks()[0];
            GridGameManager.Instance.GetSystem<AudioSystem>().ChangeMusic("BattleTheme", startMusic);
        }
    }
}