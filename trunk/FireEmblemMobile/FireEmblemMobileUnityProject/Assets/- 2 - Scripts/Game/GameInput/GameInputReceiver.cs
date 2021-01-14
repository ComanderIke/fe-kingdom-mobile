using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.Grid;
using Game.Grid.PathFinding;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameInput
{
    public class GameInputReceiver : IGameInputReceiver{
        
        private GameplayInput gameplayInput;
        private InputPathManager inputPathManager;
        private ISelectionDataProvider selectionDataProvider;
        private LastInputPositionManager lastInputPositionManager;
        private GridSystem gridSystem;
        private FactionManager factionManager;
        public GameInputReceiver()
        {
            gameplayInput = new GameplayInput();
            selectionDataProvider = new SelectionManager();
            lastInputPositionManager = new LastInputPositionManager();
            
            gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            
            inputPathManager = new InputPathManager(gridSystem.pathFinder);
            factionManager = GridGameManager.Instance.GetSystem<FactionManager>();
            UnitSelectionSystem.OnSelectedCharacter += OnSelectedCharacter;
            UnitSelectionSystem.OnSelectedInActiveCharacter += OnSelectedCharacter;
        }
       
        public void DraggedOnGrid(int x, int y)
        {
            selectionDataProvider.SetSelectedTile(x, y);
            ClickedOnGrid(x, y);
        }
        public void ResetInput()
        {
            inputPathManager.Reset();
            selectionDataProvider.ClearSelectedTile();
        }
        public void DraggedOverGrid(int x, int y)
        {
            if (gridSystem.IsTileMoveableAndActive(x, y) && !IsActorOnTile(selectionDataProvider.SelectedActor, x, y))
            {
                DraggedOnActiveField(x, y, selectionDataProvider.SelectedActor);
            }
            if (gridSystem.IsTileMoveableAndActive(x, y) && gridSystem.Tiles[x,y].HasFreeSpace())
            {
                lastInputPositionManager.StoreLatestValidPosition(x, y, selectionDataProvider.SelectedActor.MovementRage);
            }
            inputPathManager.UpdatedMovementPath(selectionDataProvider.SelectedActor);
        }
        
        public void DraggedOnActor(IGridActor actor)
        {
            if (actor.IsEnemy(selectionDataProvider.SelectedActor)&&gridSystem.GridLogic.IsFieldAttackable(actor.GridPosition.X, actor.GridPosition.Y))
            {
                AttackEnemy(selectionDataProvider.SelectedActor, actor, inputPathManager.MovementPath);
            }
            else
            {
                gameplayInput.DeselectUnit();
            }
        }

        public void DraggedOverActor(IGridActor gridActor)
        {
            if (gridActor.IsEnemy(selectionDataProvider.SelectedActor))
                DraggedOnEnemy(gridActor.GridPosition.X, gridActor.GridPosition.Y, gridActor);
            inputPathManager.UpdatedMovementPath(selectionDataProvider.SelectedActor);
        }
        
        public void StartDraggingActor(IGridActor actor)
        {
            //TODO
        }

        public void DoubleClickedActor(IGridActor unit)
        {
            if (IsActiveFaction(unit))
            {
                if(unit is ISelectableActor selectAbleUnit)
                    gameplayInput.Wait(selectAbleUnit);
            }
            else
            {
                ClickedOnActor(unit);
            }
        }
        
        public void ClickedOnActor(IGridActor unit)
        {
            if(IsActiveFaction(unit))
            {
                OwnedActorClicked(unit);
            }
            else { 
                EnemyClicked(unit);
            }
        }

        public void ClickedOnGrid(int x, int y)
        {
            if (gridSystem.GridLogic.IsFieldFreeAndActive(x, y))
            {
                if(selectionDataProvider.IsSelectedTile(x,y))
                {
                    gameplayInput.MoveUnit(selectionDataProvider.SelectedActor, new GridPosition(x, y), GridPosition.GetFromVectorList(inputPathManager.MovementPath));
                }
                else
                {
                    inputPathManager.CalculateMousePathToPosition(selectionDataProvider.SelectedActor, x, y);
                    selectionDataProvider.SelectedActor.SetGameTransformPosition(x, y);
                }
                selectionDataProvider.ClearSelectedTile();
            }
            else if(selectionDataProvider.SelectedActor != null)
            {
                if (selectionDataProvider.SelectedActor.GridPosition.X == x && y == selectionDataProvider.SelectedActor.GridPosition.Y)
                {
                    selectionDataProvider.SelectedActor.ResetPosition();
                    gameplayInput.DeselectUnit();
                }
                else
                {
                    gameplayInput.DeselectUnit();
                }
                ResetInput();
            }
            else
            {
                ResetInput();
                gameplayInput.DeselectUnit();
            }
        }
        
        private void OwnedActorClicked(IGridActor unit)
        {
            if ((int)unit.GetGameTransformPosition().x == selectionDataProvider.GetSelectedTile().x && (int)unit.GetGameTransformPosition().y == selectionDataProvider.GetSelectedTile().y)//Confirm Move
            {
                ClickedOnGrid(selectionDataProvider.GetSelectedTile().x, selectionDataProvider.GetSelectedTile().y);
            }
            else
            {
                gameplayInput.SelectUnit(unit);
            }
        }
        private void EnemyClicked(IGridActor enemyActor)
        {
            var selectedActor = selectionDataProvider.SelectedActor;
            if (selectedActor == null)
            {
                gameplayInput.SelectUnit(enemyActor);
            }
            else
            {
                if (gridSystem.GridLogic.IsFieldAttackable(enemyActor.GridPosition.X, enemyActor.GridPosition.Y))
                {
                    if (selectionDataProvider.selectedAttackTarget!=enemyActor)
                    {
                        selectionDataProvider.ClearSelectedTile();
                        var gridPos = new GridPosition((int)selectedActor.GetGameTransformPosition().x, (int)selectedActor.GetGameTransformPosition().y);
                        if (selectedActor.CanAttackFrom(gridPos, enemyActor.GridPosition))
                        {
                            if(selectedActor is IBattleActor battleActor&& enemyActor is IBattleActor enemyBattleActor)
                                gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, gridPos);
                        }
                        else if (selectedActor.CanAttack(enemyActor.GridPosition.X, enemyActor.GridPosition.Y))
                        {
                            selectedActor.ResetPosition();
                            selectionDataProvider.ClearSelectedTile();
                            inputPathManager.Reset();
                            inputPathManager.UpdatedMovementPath(selectedActor);
                            if(selectedActor is IBattleActor battleActor&& enemyActor is IBattleActor enemyBattleActor)
                                gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, new GridPosition(selectedActor.GridPosition.X, selectedActor.GridPosition.Y));
                        }
                        else
                        {
                            selectedActor.ResetPosition();
                            inputPathManager.CalculatePathToPosition(selectedActor, new Vector2(enemyActor.GridPosition.X, enemyActor.GridPosition.Y));
                            if (!inputPathManager.IsMovementPathEmpty())
                            {
                                var lastMovPathPos = inputPathManager.GetLastMovementPathPosition();
                                selectedActor.SetGameTransformPosition(lastMovPathPos.x, lastMovPathPos.y);
                                if(selectedActor is IBattleActor battleActor&& enemyActor is IBattleActor enemyBattleActor)
                                    gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, new GridPosition(lastMovPathPos.x, lastMovPathPos.y));
                            }
                            else
                            {
                                if(selectedActor is IBattleActor battleActor&& enemyActor is IBattleActor enemyBattleActor)
                                    gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, new GridPosition(selectedActor.GridPosition.X, selectedActor.GridPosition.Y));
                            }
                        }
                        if(enemyActor is ISelectableActor selectableEnemyActor)
                            selectionDataProvider.selectedAttackTarget = selectableEnemyActor;
                    }
                    else
                    {
                        AttackEnemy(selectedActor, enemyActor, inputPathManager.MovementPath);
                    }
                }
                else
                {
                    if(enemyActor is Unit unit)
                        gameplayInput.ViewUnit(unit);
                }
            }
        }
        private bool IsActiveFaction(IGridActor actor)
        {
            return GridGameManager.Instance.FactionManager.IsActiveFaction(actor.FactionId);
        }
        private void AttackEnemy(IGridActor character, IGridActor enemy, List<Vector2> movePath)
        {
            character.ResetPosition();
            gridSystem.HideMoveRange();
            
            /* Enemy is in attackRange already */
            if ((movePath == null || movePath.Count == 0) && character.CanAttack( enemy.GridPosition.X, enemy.GridPosition.Y))
            {
                if(character is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                    gameplayInput.AttackUnit(battleActor, enemyBattleActor);
            }
            else if(movePath!=null) //go to enemy cause not in range
            {
                if (movePath.Count >= 1)
                {
                    int xMov = (int)movePath[movePath.Count - 1].x;
                    int yMov = (int)movePath[movePath.Count - 1].y;
                    gameplayInput.MoveUnit(character, new GridPosition(xMov, yMov), GridPosition.GetFromVectorList(movePath));
                }
                if(character is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                    gameplayInput.AttackUnit(battleActor, enemyBattleActor);
            }
        }
        private bool IsActorOnTile(IGridActor actor, int x, int y)
        {
            return (x == actor.GridPosition.X && y == actor.GridPosition.Y);
        }
        private bool IsTileAttackAble(int x, int y)
        {
            return gridSystem.GridLogic.IsFieldAttackable(x, y);
        }
        

        
        private void DraggedOnActiveField(int x, int y, IGridActor gridActor)
        {
            inputPathManager.AddToPath(x, y, gridActor);
        }

        private void DraggedOnEnemy(int x, int y, IGridActor enemy)
        {
            var selectedActor = selectionDataProvider.SelectedActor;
            Debug.Log("Dragged on enemy: " + enemy +" at ["+x+"/"+y+"]");
            if (!IsTileAttackAble(x,y))
                return;
            if (inputPathManager.IsMovementPathEmpty())
            {
                if(selectedActor.CanAttack(x, y))
                {
                    if(selectedActor is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                        gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, selectedActor.GridPosition);
                }
                else
                {
                    inputPathManager.CalculatePathToPosition(selectedActor, new Vector2(x, y));
                }
            }
            else  // Search for suitable AttackPosition
            {
                SearchForSuitableAttackPosition(x, y, enemy, selectedActor);
            }
            if (inputPathManager.HasValidMovementPath(selectedActor.MovementRage))
            {
                inputPathManager.UpdatedMovementPath(selectedActor);
                if(selectedActor is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                    gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, new GridPosition(x, y));
            }
        }

        private void SearchForSuitableAttackPosition(int x, int y, IGridActor enemy, IGridActor selectedActor)
        {
            var foundAttackPosition = false;
            int attackPositionIndex = -1;
            attackPositionIndex = SearchForSuitableAttackPositionFromAlreadyDraggedOverTiles(x, y, selectedActor, attackPositionIndex, ref foundAttackPosition);

            if (foundAttackPosition) //Removes Parts of the drag Path to make it end at the correct attackPosition;
            {
           
                inputPathManager.MovementPath.RemoveRange(attackPositionIndex + 1, inputPathManager.MovementPath.Count - (attackPositionIndex + 1));
                inputPathManager.UpdatedMovementPath(selectedActor);
            }
            else
            {
                int delta = Mathf.Abs(selectedActor.GridPosition.X - x) + Mathf.Abs(selectedActor.GridPosition.Y - y);
                if (selectedActor.AttackRanges.Contains(delta))
                {
                    if(selectedActor is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                        gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, selectedActor.GridPosition);
                    inputPathManager.Reset();
                    inputPathManager.UpdatedMovementPath(selectedActor);
                }
                else
                {
                    inputPathManager.CalculatePathToPosition(selectedActor, new Vector2(x, y));
                }
            }
        }

        private int SearchForSuitableAttackPositionFromAlreadyDraggedOverTiles(int x, int y, IGridActor selectedActor,
            int attackPositionIndex, ref bool foundAttackPosition)
        {
            for (int i = inputPathManager.MovementPath.Count - 1; i >= 0; i--) //Search for suitable Position from already dragged over tiles
            {
                var lastMousePathPositionX = (int) inputPathManager.MovementPath[i].x;
                var lastMousePathPositionY = (int) inputPathManager.MovementPath[i].y;
                var lastMousePathField = gridSystem.GetTileFromVector2(inputPathManager.MovementPath[i]);
                int delta = Mathf.Abs(lastMousePathPositionX - x) + Mathf.Abs(lastMousePathPositionY - y);
                if (selectedActor.AttackRanges.Contains(delta))
                    if (lastMousePathField.Actor == null)
                    {
                        attackPositionIndex = i;
                        foundAttackPosition = true;
                        break;
                    }
            }
            return attackPositionIndex;
        }

        
        private void OnSelectedCharacter(ISelectableActor u)
        {
            ResetInput();
        }
        private Vector2 GetCenterPos(Vector2 clickedPos)
        {
            int centerX = (int)Mathf.Round(clickedPos.x - GridSystem.GRID_X_OFFSET) - 1;
            int centerY = (int)Mathf.Round(clickedPos.y) - 1;
            return new Vector2(centerX, centerY);
        }
        private void ClickedOnUndefined()
        {
            gameplayInput.DeselectUnit();
        }
          
        
    }
}