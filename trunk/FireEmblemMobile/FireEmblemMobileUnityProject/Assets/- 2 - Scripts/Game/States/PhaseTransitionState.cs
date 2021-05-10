using Game.GUI.Text;
using Game.Manager;
using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;

namespace Game.States
{
    public class PhaseTransitionState : GameState<NextStateTrigger>
    {
        public IPhaseRenderer phaseRenderer;
        private FactionManager factionManager;
        private GameStateManager gameStateManager;
        public PhaseTransitionState(FactionManager factionManager, GameStateManager gameStateManager)
        {
            this.factionManager = factionManager;
        }
        public override void Enter()
        {
            Debug.Log("TransitionStart");
            phaseRenderer.Show(factionManager.ActiveFaction.Id, Finished);
        }

        void Finished()
        {
            if(factionManager.ActiveFaction.IsPlayerControlled)
                gameStateManager.Feed(NextStateTrigger.StartPlayerPhase);
            else
                gameStateManager.Feed(NextStateTrigger.StartEnemyPhase);
        }
        public override void Exit()
        {
            Debug.Log("TransitionEnd");
        }

        public override GameState<NextStateTrigger> Update()
        {

            return NextState;
        }
    }
}