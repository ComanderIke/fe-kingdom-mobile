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
            this.gameStateManager = gameStateManager;
        }
        public override void Enter()
        {
            phaseRenderer.Show(factionManager.ActiveFaction.Id, Finished);
        }

        void Finished()
        {
           // Debug.Log("Phase Trans Finished: "+factionManager.ActiveFaction.IsPlayerControlled);

            if(factionManager.ActiveFaction.IsPlayerControlled)
                gameStateManager.Feed(NextStateTrigger.StartPlayerPhase);
            else
                gameStateManager.Feed(NextStateTrigger.StartEnemyPhase);
        }
        public override void Exit()
        {

        }

        public override GameState<NextStateTrigger> Update()
        {

            return NextState;
        }
    }
}