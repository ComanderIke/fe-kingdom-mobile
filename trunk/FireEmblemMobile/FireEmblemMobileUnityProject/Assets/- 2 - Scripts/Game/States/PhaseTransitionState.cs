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
        public PhaseTransitionState(FactionManager factionManager)
        {
            this.factionManager = this.factionManager;
        }
        public override void Enter()
        {
            Debug.Log("TransitionStart");
            phaseRenderer.Show(factionManager.ActiveFaction.Id, Finished);
        }

        void Finished()
        {
            if(GridGameManager.Instance.FactionManager.ActiveFaction.IsPlayerControlled)
                GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.StartPlayerPhase);
            else
                GridGameManager.Instance.GameStateManager.Feed(NextStateTrigger.StartEnemyPhase);
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