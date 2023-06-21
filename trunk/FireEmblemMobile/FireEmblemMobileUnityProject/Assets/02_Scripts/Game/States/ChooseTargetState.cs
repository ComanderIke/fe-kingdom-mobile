using System;
using _02_Scripts.Game.GameActors.Items.Consumables;
using Game.GameActors.Items;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.Map;
using Game.States;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.Mechanics
{
    public class ChooseTargetState: GameState<PPStateTrigger>, IGameInputReceiver, IUnitInputReceiver
    {
        private readonly GridGameManager gridGameManager;
        public IChooseTargetUI UI;//injected
        private FactionManager factionManager;
        private readonly GridInputSystem gridInputSystem;
        private readonly UnitInputSystem unitInputSystem;
        private  GridSystem gridSystem;
        private IGameInputReceiver previousGridInputReceiver;
        private IUnitInputReceiver previousUnitInputReceiver;
        private UnitSelectionSystem selectionSystem;
        private Skill selectedSkill;
        private Item selectedItem;
        private Unit selectedUnit;
        private PlayerPhaseState playerPhaseState;
        public ChooseTargetState( GridInputSystem gridInputSystem, UnitInputSystem unitInputSystem, PlayerPhaseState playerPhaseState)
        {
            gridGameManager=GridGameManager.Instance;
            factionManager =GridGameManager.Instance.FactionManager;
            this.gridInputSystem = gridInputSystem;
            this.unitInputSystem = unitInputSystem;
            
            this.playerPhaseState = playerPhaseState;
        }
        public override void Enter()
        {
            gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            gridGameManager.GetSystem<UiSystem>().HideMainCanvas();
            selectionSystem = gridGameManager.GetSystem<UnitSelectionSystem>();
            gridInputSystem.SetActive(true);
            unitInputSystem.SetActive(true);
            previousGridInputReceiver = gridInputSystem.inputReceiver;
            previousUnitInputReceiver = unitInputSystem.InputReceiver;
            gridInputSystem.inputReceiver = this;
            unitInputSystem.InputReceiver = this;
            selectedUnit = (Unit)selectionSystem.SelectedCharacter;
            UI.OnBackClicked += BackClicked;
            if (selectionSystem.SelectedSkill != null)
            {
                selectedSkill = selectionSystem.SelectedSkill;
                ShowSkillCastRange();
                UI.Show((Unit)selectionSystem.SelectedCharacter, selectionSystem.SelectedSkill);
            }
            else if (selectionSystem.SelectedItem != null)
            {
                selectedItem = selectionSystem.SelectedItem;
                ShowItemCastRange();
              
                UI.Show((Unit)selectionSystem.SelectedCharacter, selectionSystem.SelectedItem);
            }
            else
            {
                Debug.LogError("No Item or Skill Selected BUGGGG!");
            }
        }

        void ShowItemCastRange()
        {
            if (selectionSystem.SelectedItem is IThrowableItem throwableItem)
            {
                gridGameManager.GetSystem<GridSystem>()
                    .ShowCastRange(selectionSystem.SelectedCharacter, throwableItem.Range);
            }
        }
        void ShowSkillCastRange()
        {
            if (selectionSystem.SelectedSkill is PositionTargetSkill pts)
            {
                if (pts.Rooted)
                {
                    gridGameManager.GetSystem<GridSystem>().ShowRootedCastRange(selectionSystem.SelectedCharacter, pts);
                }
                else
                {
                    gridGameManager.GetSystem<GridSystem>().ShowCastRange(selectionSystem.SelectedCharacter,
                        pts.Range + pts.GetCastRangeIncrease(((Unit)selectionSystem.SelectedCharacter).Stats
                            .BaseAttributes));
                }
            }
        }

        public override void Exit()
        {
            if (UI != null)
            {
                UI.Hide();
                UI.OnBackClicked += BackClicked;
            }

            gridSystem.HideMoveRange();
            gridSystem.HideCast();
            gridInputSystem.ResetInput();
            gridInputSystem.SetActive(false);
            unitInputSystem.SetActive(false);
            gridGameManager.GetSystem<UiSystem>().ShowMainCanvas();
            gridInputSystem.inputReceiver = previousGridInputReceiver;
            unitInputSystem.InputReceiver = previousUnitInputReceiver;
        }

        private void BackClicked()
        {
            playerPhaseState.Feed(PPStateTrigger.Cancel);
        }

        public override GameState<PPStateTrigger> Update()
        {
            unitInputSystem.Update();
            gridInputSystem.Update();
           
            return NextState;
        }

        public void ClickedOnGrid(int x, int y)
        {
            if (selectedSkill is IPosTargeted pts)
            {
                PositionTargetClicked(pts , x, y);
               
            }
            else if (selectedItem is IPosTargeted throwableItem)
            {
                PositionTargetClicked(throwableItem, x,y);
            }
        }

        private void PositionTargetClicked(IPosTargeted skill, int x, int y )
        {
             if (!skill.Rooted)
             {
                 if (gridSystem.IsTargetAble(x, y))
                 {

                     if (gridSystem.cursor.GetCurrentTile() == gridSystem.Tiles[x, y])
                     {
                         var targets = skill.GetAllTargets(selectedUnit, gridSystem.Tiles, x,y);
                         skill.Activate(selectedUnit, gridSystem.Tiles, x,y);
                         new GameplayCommands().Wait(selectedUnit);
                         new GameplayCommands().ExecuteInputActions(()=>
                         {
                             playerPhaseState.Feed(PPStateTrigger.Cancel);
                             var task = new AfterBattleTasks(ServiceProvider.Instance.GetSystem<UnitProgressSystem>(),(Unit)selectedUnit, targets);
                             task.StartTask();
                             task.OnFinished += () =>
                             {
                                 if(GridGameManager.Instance.FactionManager.ActiveFaction.IsPlayerControlled)
                                     GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.PlayerPhaseState);
                                 else
                                     GridGameManager.Instance.GameStateManager.SwitchState( GridGameManager.Instance.GameStateManager.EnemyPhaseState);
                             };
                         });
                         //Selected same Tile again
                     }
                     else
                     {
                         gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[x, y]);
                         gridSystem.HideCast();
                         if(skill is Skill)
                            ShowSkillCastRange();
                         else if(skill is Item)
                             ShowItemCastRange();
                         gridSystem.ShowCast(skill.Size, skill.TargetArea, skill.EffectType);
                     }



                 }
             }
             else
             {
                 if (gridSystem.IsTargetAble(x, y))
                 {
                     if (gridSystem.cursor.GetCurrentTile() == gridSystem.Tiles[x, y])
                     {
                         skill.Activate(selectedUnit, gridSystem.Tiles, x,y);
                         new GameplayCommands().Wait(selectedUnit);
                         new GameplayCommands().ExecuteInputActions(()=>
                         {
                             playerPhaseState.Feed(PPStateTrigger.Cancel);
                         });
                         //Selected same Tile again
                     }
                     else
                     {
                         gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[x, y]);
                         gridSystem.HideCast();
                         ShowSkillCastRange();
                         gridSystem.ShowRootedCast(selectedUnit.GridComponent.GridPosition.AsVector(),
                             skill.Size, skill.TargetArea);
                     }
                 }
                    
             }
        }

        private bool IsInCastRange(int x, int y)
        {
            if (selectedSkill is PositionTargetSkill pts)
            {
                int diff = Math.Abs(selectedUnit.GridComponent.GridPosition.X - x) +
                           Math.Abs(selectedUnit.GridComponent.GridPosition.Y - y);
                return diff <= pts.Range + pts.GetCastRangeIncrease(selectedUnit.Stats.BaseAttributes);

            }
            else
            {
                return true;
            }

        }

        public void DraggedOnGrid(int gridPosX, int gridPosY)
        {
            //
        }

        public void DraggedOverGrid(int gridPosX, int gridPosY)
        {
            //
        }

        public void ClickedOnActor(IGridActor unit)//KACKFACE
        {
           
        }

        public void DoubleClickedActor(IGridActor unit)
        {
            //
        }

        public void DraggedOnObject(IGridObject gridActor)
        {
            //
        }

        public void DraggedOverObject(IGridObject gridActor)
        {
            //
        }

        public void ActorLongHold(IGridActor unit)
        {
            Debug.Log("LongHoldinCHooseTarget");
        }

        public void ActorDragEnded(IGridActor gridActor, int x, int y)
        {
            //
        }

        public void ActorDragged(IGridActor actor, int x, int y)
        {
            //
        }

        public void ActorClicked(IGridActor unit)
        {
            ClickedOnGrid(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
        }

        public void ActorDoubleClicked(IGridActor unit)
        {
           //
        }

        public void StartDraggingActor(IGridActor actor)
        {
            //
        }

        public void ResetInput()
        {
            //
        }

        public void UndoClicked()
        {
            //
        }

        public void ClickedDownOnGrid(int x, int y)
        {
            //
        }

        public void LongClickOnCharacter(IGridActor unit)
        {
            //
        }
    }
}