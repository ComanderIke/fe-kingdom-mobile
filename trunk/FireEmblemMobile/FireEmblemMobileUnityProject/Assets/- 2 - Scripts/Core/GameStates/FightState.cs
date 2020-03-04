using Assets.Audio;
using Assets.GameActors.Units;
using Assets.GUI;
using Assets.Mechanics;
using UnityEngine;

namespace Assets.Core.GameStates
{
    public class BattleState : GameState<NextStateTrigger>
    {
        private readonly Unit attacker;
        private readonly Unit defender;
        public BattleSystem FightSystem;

        private string startMusic;
        private readonly UiSystem uiController;
        private readonly UnitsSystem unitController;

        public BattleState(Unit attacker, Unit defender)
        {
            this.attacker = attacker;
            this.defender = defender;

            Debug.Log("FightState " + attacker.Name + " " + defender.Name);
            uiController = MainScript.Instance.GetSystem<UiSystem>();
            unitController = MainScript.Instance.GetSystem<UnitsSystem>();
        }

        public override void Enter()
        {
            FightSystem = new BattleSystem(attacker, defender);
            ShowFightUi();
            unitController.HideUnits();
            BattleSystem.OnStartAttack += FightSystem.DoAttack;
            SetUpMusic();
        }

        public override GameState<NextStateTrigger> Update()
        {
            return NextState;
        }

        public override void Exit()
        {
            uiController.HideFightUi();
            unitController.ShowUnits();

            BattleSystem.OnStartAttack -= FightSystem.DoAttack;
            MainScript.Instance.GetSystem<AudioSystem>().ChangeMusic(startMusic, "BattleTheme", true);
        }

        private void ShowFightUi()
        {
            uiController.ShowFightUi(attacker, defender);
        }

        private void SetUpMusic()
        {
            startMusic = MainScript.Instance.GetSystem<AudioSystem>().GetCurrentlyPlayedMusicTracks()[0];
            MainScript.Instance.GetSystem<AudioSystem>().ChangeMusic("BattleTheme", startMusic);
        }
    }
}