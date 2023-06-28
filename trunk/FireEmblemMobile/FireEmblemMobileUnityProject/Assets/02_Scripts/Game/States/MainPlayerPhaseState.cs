using System.Linq;
using Game.GameActors.Items;
using Game.GameActors.Items.Weapons;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Humans;
using Game.GameActors.Units.Skills;
using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.Map;
using Game.WorldMapStuff.Model;
using GameEngine.GameStates;
using LostGrace;
using UnityEngine;

namespace Game.Mechanics
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

        public override void Enter()
        {
            gridGameManager.GetSystem<GridSystem>().cursor.OnCursorPositionChanged += CursorPosChanged;
            
            gridInputSystem.SetActive(true);
            unitInputSystem.SetActive(true);
            playerPhaseUI.Show(gridGameManager.GetSystem<TurnSystem>().TurnCount);
            playerPhaseUI.SubscribeOnBackClicked(Undo);
            playerPhaseUI.SubscribeOnCharacterCircleClicked(OnCharacterCircleClicked);
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
            playerPhaseUI.UnsubscribeOnCharacterCircleClicked(OnCharacterCircleClicked);
            UnitSelectionSystem.OnSelectedInActiveCharacter -=OnSelectedCharacter;
            UnitSelectionSystem.OnDeselectCharacter -= OnDeselectedCharacter;
            UnitSelectionSystem.OnSelectedCharacter -= OnSelectedCharacter;
            UnitSelectionSystem.OnSkillSelected -= SkillSelected;
            UnitSelectionSystem.OnItemSelected -= ItemSelected;
            
            GameplayCommands.OnViewUnit -= ViewUnit;
            gridGameManager.GetSystem<GridSystem>().cursor.OnCursorPositionChanged -= CursorPosChanged;
            gridInputSystem.ResetInput();
            gridInputSystem.SetActive(false);
            unitInputSystem.SetActive(false);
            foreach (var unit in factionManager.Factions[1].Units)
            {
                unit.visuals.unitRenderer.HideTemporaryVisuals();
            }
               Debug.Log("Exit MAIN PLAYER PHASE STATE");
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
            Debug.Log("Skill Selected");
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
                        consumableItem.Use((Unit)selectedCharacter, Player.Instance.Party.Convoy);
                        new GameplayCommands().Wait(selectedCharacter);
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
            gridInputSystem.inputReceiver.ResetInput();
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