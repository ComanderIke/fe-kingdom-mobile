using System;
using Game.GameActors.Units;
using Game.GameActors.Units.Skills;
using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.Map;
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
        private readonly GridSystem gridSystem;
        private IGameInputReceiver previousGridInputReceiver;
        private IUnitInputReceiver previousUnitInputReceiver;
        private Skill selectedSkill;
        private Unit selectedUnit;
        public ChooseTargetState(GridGameManager gridGameManager, GridInputSystem gridInputSystem, UnitInputSystem unitInputSystem)
        {
            this.gridGameManager = gridGameManager;
            factionManager = this.gridGameManager.FactionManager;
            this.gridInputSystem = gridInputSystem;
            this.unitInputSystem = unitInputSystem;
            gridSystem = gridGameManager.GetSystem<GridSystem>();
        }
        public override void Enter()
        {
            gridGameManager.GetSystem<UiSystem>().HideMainCanvas();
            UnitSelectionSystem selectionSystem = gridGameManager.GetSystem<UnitSelectionSystem>();
            gridInputSystem.SetActive(true);
            unitInputSystem.SetActive(true);
            previousGridInputReceiver = gridInputSystem.inputReceiver;
            previousUnitInputReceiver = unitInputSystem.InputReceiver;
            gridInputSystem.inputReceiver = this;
            unitInputSystem.InputReceiver = this;
            selectedUnit = (Unit)selectionSystem.SelectedCharacter;
            if (selectionSystem.SelectedSkill != null)
            {
                selectedSkill = selectionSystem.SelectedSkill;
                Debug.Log("Remove Magic NUmber!");
                if(selectionSystem.SelectedSkill is PositionTargetSkill pts)
                    gridGameManager.GetSystem<GridSystem>().ShowCastRange(selectionSystem.SelectedCharacter,pts.range+pts.GetCastRangeIncrease(((Unit) selectionSystem.SelectedCharacter).Stats.Attributes));
                UI.Show((Unit)selectionSystem.SelectedCharacter, selectionSystem.SelectedSkill);
            }
            else if (selectionSystem.SelectedItem != null)
            {
                gridGameManager.GetSystem<GridSystem>().ShowCastRange(selectionSystem.SelectedCharacter,1);
                UI.Show((Unit)selectionSystem.SelectedCharacter, selectionSystem.SelectedItem);
            }
            else
            {
                Debug.LogError("No Item or Skill Selected BUGGGG!");
            }
        }

        public override void Exit()
        {
            UI.Hide();
            gridInputSystem.SetActive(false);
            unitInputSystem.SetActive(false);
            gridGameManager.GetSystem<UiSystem>().ShowMainCanvas();
            gridInputSystem.inputReceiver = previousGridInputReceiver;
            unitInputSystem.InputReceiver = previousUnitInputReceiver;
        }

        public override GameState<PPStateTrigger> Update()
        {
            unitInputSystem.Update();
            gridInputSystem.Update();
            return NextState;
        }

        public void ClickedOnGrid(int x, int y)
        {
            if (IsInCastRange(x,y))
            {
                gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[x, y]);
                if (selectedSkill is PositionTargetSkill pts)
                {
                    gridSystem.cursor.ShowCast(pts.radius, pts.horizontal, pts.vertical, pts.diagonal, pts.fullBox);
                }
            }
            else
            {
                
            }
        }

        private bool IsInCastRange(int x, int y)
        {
            if (selectedSkill is PositionTargetSkill pts)
            {
                int diff = Math.Abs(selectedUnit.GridComponent.GridPosition.X - x) +
                           Math.Abs(selectedUnit.GridComponent.GridPosition.Y - y);
                return diff <= pts.range + pts.GetCastRangeIncrease(selectedUnit.Stats.Attributes);

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

        public void DraggedOnActor(IGridActor gridActor)
        {
            //
        }

        public void DraggedOverActor(IGridActor gridActor)
        {
            //
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
    }
}