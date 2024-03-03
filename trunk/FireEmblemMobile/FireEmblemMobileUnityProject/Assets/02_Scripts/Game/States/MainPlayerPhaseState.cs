using System.Linq;
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
using GameEngine.GameStates;
using UnityEngine;

namespace Game.States
{
    public class MainPlayerPhaseState: GameState<PPStateTrigger>{
        
        public IPlayerPhaseUI playerPhaseUI;//Inject
        private readonly GridGameManager gridGameManager;
        private FactionManager factionManager;
        private readonly GridInputSystem gridInputSystem;
        private readonly UnitInputSystem unitInputSystem;
        private PlayerPhaseState playerPhaseState;

        public MainPlayerPhaseState(GridGameManager gridGameManager, FactionManager factionManager, GridInputSystem gridInputSystem, UnitInputSystem unitInputSystem, PlayerPhaseState playerPhaseState)
        {
            this.gridGameManager = gridGameManager;
            this.factionManager = factionManager;
            this.gridInputSystem = gridInputSystem;
            this.unitInputSystem = unitInputSystem;
            this.playerPhaseState = playerPhaseState;
        }

        public void ToggleZoom()
        {
            playerPhaseState.ToggleZoom();
        }
        public override void Enter()
        {
            gridGameManager.GetSystem<GridSystem>().cursor.OnCursorPositionChanged += CursorPosChanged;
            gridGameManager.GetSystem<GridSystem>().cursor.Hide();
            gridInputSystem.SetActive(true);
            unitInputSystem.SetActive(true);
            playerPhaseUI.Show(gridGameManager.GetSystem<TurnSystem>().TurnCount);
            playerPhaseUI.SubscribeOnToggleZoomClicked(ToggleZoom);
            playerPhaseUI.SubscribeOnBackClicked(Undo);
            playerPhaseUI.SubscribeOnCharacterCircleClicked(OnCharacterCircleClicked);
            foreach (var member in factionManager.EnemyFaction.Units)
            {
                if(member.IsBoss)
                    playerPhaseUI.ShowBossUI(member);
            }
            
            SetUpInputForUnits();
            UnitSelectionSystem.OnSelectedInActiveCharacter -=OnSelectedCharacter;
            UnitSelectionSystem.OnDeselectCharacter -= OnDeselectedCharacter;
            UnitSelectionSystem.OnSelectedCharacter -= OnSelectedCharacter;
            UnitSelectionSystem.OnSkillSelected -= SkillSelected;
            UnitSelectionSystem.OnItemSelected -= ItemSelected;
            GameplayCommands.OnViewUnit -= ViewUnit;
            UnitSelectionSystem.OnSelectedInActiveCharacter += OnSelectedCharacter;
            UnitSelectionSystem.OnSelectedCharacter += OnSelectedCharacter;
            UnitSelectionSystem.OnDeselectCharacter +=OnDeselectedCharacter;
            UnitSelectionSystem.OnSkillSelected += SkillSelected;
            UnitSelectionSystem.OnItemSelected += ItemSelected;
            GameplayCommands.OnViewUnit += ViewUnit;
        }

        public override void Exit()
        {
            playerPhaseUI.UnsubscribeOnBackClicked(Undo);
            playerPhaseUI.UnsubscribeOnToggleZoomClicked(ToggleZoom);
            playerPhaseUI.UnsubscribeOnCharacterCircleClicked(OnCharacterCircleClicked);
            UnitSelectionSystem.OnSelectedInActiveCharacter -=OnSelectedCharacter;
            UnitSelectionSystem.OnDeselectCharacter -= OnDeselectedCharacter;
            UnitSelectionSystem.OnSelectedCharacter -= OnSelectedCharacter;
            UnitSelectionSystem.OnSkillSelected -= SkillSelected;
            UnitSelectionSystem.OnItemSelected -= ItemSelected;
            
            GameplayCommands.OnViewUnit -= ViewUnit;
            gridGameManager.GetSystem<GridSystem>().cursor.OnCursorPositionChanged -= CursorPosChanged;
            gridInputSystem.ResetInput(true);
            gridInputSystem.SetActive(false);
            unitInputSystem.SetActive(false);
            foreach (var unit in factionManager.Factions[1].Units)
            {
                unit.visuals.unitRenderer.HideTemporaryVisuals();
            }
            // Debug.Log("Exit MAIN PLAYER PHASE STATE");
            playerPhaseUI.Hide();
            playerPhaseUI.HideTileInfo();
        }

        void ViewUnit(IGridActor unit)
        {
            playerPhaseUI.ViewUnit((Unit)unit);
        }
        public void Undo()
        {

            gridInputSystem.inputReceiver.UndoClicked();
            gridGameManager.GetSystem<GridSystem>().HideMoveRange();
        
        }
        private void CursorPosChanged(Vector2Int tilePos)
        {
            playerPhaseUI.ShowTileInfo(gridGameManager.GetSystem<GridSystem>().cursor.GetCurrentTile());
        }

       
       
        private void OnCharacterCircleClicked(Unit unit)
        {
            gridInputSystem.inputReceiver.ClickedOnActor(unit);
        }
        private void SkillSelected(Skill skill)
        {
            // Debug.Log("Skill Selected");
            playerPhaseState.Feed(PPStateTrigger.ChooseTarget);
        }
        
        private void ItemSelected(Item item)
        {
            if (item is ConsumableItem consumableItem)
            {
                Debug.Log("ConsumeableItemSelected!");
                if (consumableItem.target == ItemTarget.Self)
                {
                    var selectedCharacter = gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter;
                        //Debug.Log("SelectedCharacter: " + selectedCharacter);
                        consumableItem.Use((Unit)selectedCharacter, Player.Instance.Party);
                       // new GameplayCommands().Wait(selectedCharacter);
                        new GameplayCommands().ExecuteInputActions(null);
                }
                else
                {
                    playerPhaseState.Feed(PPStateTrigger.ChooseTarget);
                }
            }
        }
        private void OnSelectedCharacter(IGridActor character)
        {
            gridInputSystem.inputReceiver.ResetInput();
            // foreach (var unit in factionManager.Factions[1].Units)
            // {
            //     unit.visuals.unitRenderer.ShowAttackDamage((Unit)character);
            // }
            foreach (var unit in factionManager.Factions[1].Units)
            {
         
                unit.visuals.unitRenderer.ShowEffectiveness((Unit)character);
            }
        }
        private void OnDeselectedCharacter(IGridActor character)
        {
            gridInputSystem.inputReceiver.ResetInput(false, true);
            foreach (var unit in factionManager.Factions[1].Units)
            {
                unit.visuals.unitRenderer.HideTemporaryVisuals();
            }
        }
        public override GameState<PPStateTrigger> Update()
        {
            unitInputSystem.Update();
            gridInputSystem.Update();
            
            
            

            return NextState;
        }
        private void SetUpInputForUnits()
        {
            foreach (var unit in factionManager.Factions.SelectMany(faction => faction.Units))
            {
                if (unit.GameTransformManager.GameObject != null)
                {
                    unit.GameTransformManager.UnitController.touchInputReceiver = unitInputSystem;
                    
                }
            }
        }
    }
}