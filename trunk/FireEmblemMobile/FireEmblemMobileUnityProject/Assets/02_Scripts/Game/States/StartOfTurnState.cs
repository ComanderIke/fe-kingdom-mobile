using System;
using System.Linq;
using Codice.Client.GameUI.Explorer;
using Game.GameActors.Items;
using Game.GameActors.Items.Consumables;
using Game.GameActors.Player;
using Game.GameActors.Units;
using Game.GameActors.Units.Interfaces;
using Game.GameActors.Units.Skills.Base;
using Game.GameInput.GameplayCommands;
using Game.GameInput.GridInput;
using Game.Grid;
using Game.Interfaces;
using Game.Manager;
using Game.Systems;
using Game.Utility;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;
using Utility;

namespace Game.States
{
    public class StartOfTurnState: GameState<PPStateTrigger>{
        
        //public IPlayerPhaseUI playerPhaseUI;//Inject
        private readonly GridGameManager gridGameManager;
        private FactionManager factionManager;
        //private readonly GridInputSystem gridInputSystem;
        //private readonly UnitInputSystem unitInputSystem;
        private IMainPhaseState  parentState;
        public static event Action OnStartOfTurnEffects;
     

        public StartOfTurnState(GridGameManager gridGameManager, FactionManager factionManager, IMainPhaseState parentState)
        {
            this.gridGameManager = gridGameManager;
            this.factionManager = factionManager;
            // this.gridInputSystem = gridInputSystem;
            // this.unitInputSystem = unitInputSystem;
            this.parentState = parentState;
        }
        
        public override void Enter()
        {
            AnimationQueue.OnAllAnimationsEnded += Finished;
            gridGameManager.GetSystem<GridSystem>().cursor.Hide();
            foreach (var faction in factionManager.Factions)
            {
                var cursedUnits = factionManager.ActiveFaction.Units.Where(u => u.IsCursed());
                foreach (var cursedUnit in cursedUnits)
                {
                    cursedUnit.SpreadCurse();
                }
            }
            
            for (int i=factionManager.ActiveFaction.Units.Count-1; i >=0; i--)//Collection might be modified(tempted)
            {
                factionManager.ActiveFaction.Units[i].StatusEffectManager.UpdateTurn();
                
            }
            OnStartOfTurnEffects?.Invoke();

            if (AnimationQueue.IsNoAnimationRunning())
                Finished();
            // TODO Wait for all effects to be over. AnimationQueue?
        }

        public override void Exit()
        {
            // playerPhaseUI.UnsubscribeOnBackClicked(Undo);
            // playerPhaseUI.UnsubscribeOnToggleZoomClicked(ToggleZoom);
            // playerPhaseUI.UnsubscribeOnCharacterCircleClicked(OnCharacterCircleClicked);
            // UnitSelectionSystem.OnSelectedInActiveCharacter -=OnSelectedCharacter;
            // UnitSelectionSystem.OnDeselectCharacter -= OnDeselectedCharacter;
            // UnitSelectionSystem.OnSelectedCharacter -= OnSelectedCharacter;
            // UnitSelectionSystem.OnSkillSelected -= SkillSelected;
            // UnitSelectionSystem.OnItemSelected -= ItemSelected;
            //
            // GameplayCommands.OnViewUnit -= ViewUnit;
            // gridGameManager.GetSystem<GridSystem>().cursor.OnCursorPositionChanged -= CursorPosChanged;
            // gridInputSystem.ResetInput(true);
            // gridInputSystem.SetActive(false);
            // unitInputSystem.SetActive(false);
            // foreach (var unit in factionManager.Factions[1].Units)
            // {
            //     unit.visuals.unitRenderer.HideTemporaryVisuals();
            // }
            // // Debug.Log("Exit MAIN PLAYER PHASE STATE");
            // playerPhaseUI.Hide();
            // playerPhaseUI.HideTileInfo();
        }


        void Finished()
        {
            AnimationQueue.OnAllAnimationsEnded -= Finished;
            parentState.SetStartTurnFinished();
            parentState.Feed(PPStateTrigger.StartTurnFinished);
        }
       
        
        public override GameState<PPStateTrigger> Update()
        {
            // unitInputSystem.Update();
            // gridInputSystem.Update();
            //
            
            

            return NextState;
        }
    }
}