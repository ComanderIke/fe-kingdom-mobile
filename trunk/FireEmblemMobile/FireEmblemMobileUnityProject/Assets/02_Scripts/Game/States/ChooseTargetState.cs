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
            UnitSelectionSystem.OnSkillDeselected += SkillDeselected;
            if (selectionSystem.SelectedSkill != null)
            {
                selectedSkill = selectionSystem.SelectedSkill;
                activeSkillMixin = selectedSkill.FirstActiveMixin;
                ShowSkillCastRange(selectionSystem.SelectedSkill.FirstActiveMixin);
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
                    .ShowCastRange(selectionSystem.SelectedCharacter, throwableItem.Range, 0);
            }
        }
        void ShowSkillCastRange(ActiveSkillMixin activeSkillMixin)
        {
            Debug.Log("ShowCastRange");
            if (activeSkillMixin is PositionTargetSkillMixin pts)
            {
                if (pts.Rooted)
                {
                    gridGameManager.GetSystem<GridSystem>().ShowRootedCastRange(selectionSystem.SelectedCharacter, selectionSystem.SelectedSkill.Level, pts);
                }
                else
                {
                    Debug.Log("ShowGridCastRange:");
                    gridGameManager.GetSystem<GridSystem>().ShowCastRange(selectionSystem.SelectedCharacter,
                        pts.GetRange(selectionSystem.SelectedSkill.Level),pts.GetMinRange(selectionSystem.SelectedSkill.Level)); //+ pts.GetCastRangeIncrease(((Unit)selectionSystem.SelectedCharacter).Stats
                          //  .BaseAttributes));
                }
            }
            else if (activeSkillMixin is SingleTargetMixin stm)
            {
                stm.ShowTargets((Unit)selectionSystem.SelectedCharacter);
               // + stm.GetCastRangeIncrease(((Unit)selectionSystem.SelectedCharacter).Stats
                       // .BaseAttributes));
            }
        }

        void SkillDeselected(Skill skill)
        {
            BackClicked();
        }

        public override void Exit()
        {
            if (UI != null)
            {
                UI.Hide();
                UI.OnBackClicked -= BackClicked;
            }
            new GameplayCommands().DeselectSkill();

            gridSystem.HideMoveRange();
            gridSystem.HideCast();
            gridInputSystem.ResetInput();
            gridInputSystem.SetActive(false);
            unitInputSystem.SetActive(false);
            gridGameManager.GetSystem<UiSystem>().ShowMainCanvas();
            gridInputSystem.inputReceiver = previousGridInputReceiver;
            unitInputSystem.InputReceiver = previousUnitInputReceiver;
            UnitSelectionSystem.OnSkillDeselected -= SkillDeselected;
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

        private ActiveSkillMixin activeSkillMixin;
        public void ClickedOnGrid(int x, int y)
        {
            if (activeSkillMixin is IPosTargeted psm)
            {
                PositionTargetClicked(psm , x, y);
               
            }
            else if (selectedItem is IPosTargeted throwableItem)
            {
                PositionTargetClicked(throwableItem, x,y);
            }
            else if (activeSkillMixin is SingleTargetMixin stm)
            {
                if (gridSystem.Tiles[x, y].GridObject != null &&gridSystem.Tiles[x, y].GridObject is Unit target)

                {
                    if (stm.CanTarget(selectedUnit, target)&& stm.IsInRange(selectedUnit, target))
                    {
                        Debug.Log("Activate SingleTargetMixin");
                        stm.Activate(selectedUnit,target);
                        if (selectionSystem.SelectedSkill.activeMixins.Count>1)
                        {
                            activeSkillMixin = selectionSystem.SelectedSkill.activeMixins[1];
                            ShowSkillCastRange(activeSkillMixin);
                            //selectionSystem.SelectedSkill.activeMixins[1].ShowTargets((Unit)selectionSystem.SelectedCharacter);
                            // + stm.GetCastRangeIncrease(((Unit)selectionSystem.SelectedCharacter).Stats
                            // .BaseAttributes));
                        }
                    }
                }
                
                
               
            }
        }

        private void PositionTargetClicked(IPosTargeted skill, int x, int y )
        {
             if (!skill.Rooted)
             {
                 if (gridSystem.IsTargetAble(x, y))
                 {

                     if (gridSystem.cursor.GetCurrentTile() == gridSystem.Tiles[x, y]|| !skill.ConfirmPosition())
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
                         Debug.Log("ShowSkillCastRange:");
                         if(skill is ActiveSkillMixin s)
                            ShowSkillCastRange(s);
                         else if(skill is Item)
                             ShowItemCastRange();
                         Debug.Log("ShowSkillCast");
                         gridSystem.ShowCast(skill.GetSize(), skill.TargetArea, skill.EffectType);
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
                         ShowSkillCastRange(((Skill)skill).FirstActiveMixin);
                         gridSystem.ShowRootedCast(selectedUnit.GridComponent.GridPosition.AsVector(),
                             skill.GetSize(), skill.TargetArea);
                     }
                 }
                    
             }
        }

        private bool IsInCastRange(int x, int y)
        {
            if (selectedSkill.FirstActiveMixin is PositionTargetSkillMixin pts)
            {
                int diff = Math.Abs(selectedUnit.GridComponent.GridPosition.X - x) +
                           Math.Abs(selectedUnit.GridComponent.GridPosition.Y - y);
                return diff <= pts.GetRange(selectedSkill.Level) + pts.GetCastRangeIncrease(selectedUnit.Stats.BaseAttributes);

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