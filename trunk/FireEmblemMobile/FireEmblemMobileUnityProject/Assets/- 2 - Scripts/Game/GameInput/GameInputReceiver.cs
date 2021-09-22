using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Units;
using Game.GameResources;
using Game.Grid;
using Game.Grid.GridPathFinding;
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
        public GameInputReceiver(GridSystem gridSystem)
        {
            gameplayInput = new GameplayInput();
            selectionDataProvider = new SelectionManager();
            lastInputPositionManager = new LastInputPositionManager();

            this.gridSystem = gridSystem;
            inputPathManager = new InputPathManager(gridSystem.pathFinder, GameAssets.Instance.grid.moveArrowVisual);
            // UnitSelectionSystem.OnSelectedCharacter += OnSelectedCharacter;
            // UnitSelectionSystem.OnSelectedInActiveCharacter += OnSelectedCharacter;
        }
        
       
        public void DraggedOnGrid(int x, int y)
        {
            selectionDataProvider.SetSelectedTile(x, y);
            ClickedOnGrid(x, y);
        }
        public void ResetInput()
        {
            inputPathManager.Reset();
            selectionDataProvider.ClearData();
            gridSystem.cursor.Reset();
        }
        public void DraggedOverGrid(int x, int y)
        {
            ResetDragSelectables();
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[x, y]);
            if (gridSystem.IsTileMoveableAndActive(x, y) && !IsActorOnTile(selectionDataProvider.SelectedActor, x, y))
            {
                DraggedOnActiveField(x, y, selectionDataProvider.SelectedActor);
                lastInputPositionManager.StoreLatestValidPosition(x, y, selectionDataProvider.SelectedActor.MovementRange);
            }
            else if (gridSystem.IsTileMoveableAndActive(x, y) && gridSystem.Tiles[x,y].HasFreeSpace())
            {
                lastInputPositionManager.StoreLatestValidPosition(x, y, selectionDataProvider.SelectedActor.MovementRange);
            }
            else
            {
                ResetInput();
                inputPathManager.UpdatedMovementPath(selectionDataProvider.SelectedActor.GridComponent.GridPosition.X,selectionDataProvider.SelectedActor.GridComponent.GridPosition.Y);
            }
        }
        
        public void DraggedOnActor(IGridActor actor)
        {
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[actor.GridComponent.GridPosition.X, actor.GridComponent.GridPosition.Y]);
            if (actor.IsEnemy(selectionDataProvider.SelectedActor)&&gridSystem.GridLogic.IsFieldAttackable(actor.GridComponent.GridPosition.X, actor.GridComponent.GridPosition.Y))
            {
                AttackEnemy(selectionDataProvider.SelectedActor, actor, inputPathManager.MovementPath);
            }
            else
            {
                gameplayInput.DeselectUnit();
            }
        }

        private void ResetDragSelectables()
        {
            selectionDataProvider.SetSelectedAttackTarget(null);
        }
        public void DraggedOverActor(IGridActor gridActor)
        {
            ResetDragSelectables();
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[gridActor.GridComponent.GridPosition.X, gridActor.GridComponent.GridPosition.Y]);
            if (!gridActor.IsEnemy(selectionDataProvider.SelectedActor))
            {
                if (gridActor == selectionDataProvider.SelectedActor)
                {
                    //Debug.Log("Dragged over selected Actor");
                    ResetInput();
                    inputPathManager.UpdatedMovementPath(selectionDataProvider.SelectedActor.GridComponent.GridPosition.X,selectionDataProvider.SelectedActor.GridComponent.GridPosition.Y);
                }
                else
                {
                    //Debug.Log("Dragged over Ally! Show only Cursor on StartPos and ad as valid Position for passthrough");
                    if(gridSystem.IsTileMoveableAndActive(gridActor.GridComponent.GridPosition.X, gridActor.GridComponent.GridPosition.Y))
                        inputPathManager.AddToPath(gridActor.GridComponent.GridPosition.X, gridActor.GridComponent.GridPosition.Y, selectionDataProvider.SelectedActor);
                    //ResetInput();
                }
            }
            if (gridActor.IsEnemy(selectionDataProvider.SelectedActor))
                DraggedOverEnemy(gridActor.GridComponent.GridPosition.X, gridActor.GridComponent.GridPosition.Y, gridActor);//TODO should be dragged over enemy?
  
           

        }
        
        public void StartDraggingActor(IGridActor actor)
        {
            //TODO
        }

        public void DoubleClickedActor(IGridActor unit)
        {
            if (IsActiveFaction(unit))
            {
                gameplayInput.Wait(unit);
                gameplayInput.ExecuteInputActions(null);
            }
            else
            {
                ClickedOnActor(unit);
            }
        }
        
        public void ClickedOnActor(IGridActor unit)
        {
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y]);
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
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[x, y]);
            if(!gridSystem.GridLogic.IsTileFree(x,y))
            {
                //Debug.Log("Somehow clicked on non empty Tile");
                
                return;
            }
            if (gridSystem.GridLogic.IsFieldFreeAndActive(x, y)&&selectionDataProvider.SelectedActor!=null &&!selectionDataProvider.SelectedActor.TurnStateManager.HasMoved)
            {
                
                if(selectionDataProvider.IsSelectedTile(x,y))
                {
                    gameplayInput.MoveUnit(selectionDataProvider.SelectedActor, new GridPosition(x, y), GridPosition.GetFromVectorList(inputPathManager.MovementPath));
                    gameplayInput.Wait(selectionDataProvider.SelectedActor);
                    gameplayInput.ExecuteInputActions(null);
                    selectionDataProvider.ClearData();
                    
                }
                else
                {
                    inputPathManager.CalculateMousePathToPosition(selectionDataProvider.SelectedActor, x, y);
                    Debug.Log("GameInput SetPosition");
                    //selectionDataProvider.SelectedActor.GameTransformManager.SetPosition(x, y);
                    gameplayInput.MoveUnit(selectionDataProvider.SelectedActor, new GridPosition(x, y), GridPosition.GetFromVectorList(inputPathManager.MovementPath));
                   
                    gameplayInput.ExecuteInputActions( ()=>
                    {
                        if (gridSystem.GridLogic.GetAttackTargets(selectionDataProvider.SelectedActor).Count > 0)
                        {
                            Debug.Log(""+gridSystem.GridLogic.GetAttackTargets(selectionDataProvider.SelectedActor).Count);
                            gridSystem.ShowAttackFromPosition((Unit) selectionDataProvider.SelectedActor,x,y);
                            selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                            
                        }
                        else
                        {
                            gridSystem.ShowAttackRangeOnGrid(selectionDataProvider.SelectedActor,
                                selectionDataProvider.SelectedActor.AttackRanges);
                            selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                            gameplayInput.Wait(selectionDataProvider.SelectedActor);
                            gameplayInput.ExecuteInputActions(null);
                        }
                    });
                    
                    
                    selectionDataProvider.SetSelectedTile(x, y);
                    selectionDataProvider.ClearAttackTarget();
                }
                
            }
            else if(selectionDataProvider.SelectedActor != null)
            {
                if (selectionDataProvider.SelectedActor.GridComponent.GridPosition.X == x && y == selectionDataProvider.SelectedActor.GridComponent.GridPosition.Y)
                {  Debug.Log("ResetPos4");
                    selectionDataProvider.SelectedActor.GridComponent.ResetPosition();
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
            if ((int)unit.GameTransformManager.GetPosition().x == selectionDataProvider.GetSelectedTile().x && (int)unit.GameTransformManager.GetPosition().y == selectionDataProvider.GetSelectedTile().y)//Confirm Move
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
            Debug.Log("Enemy Clikced!");
            var selectedActor = selectionDataProvider.SelectedActor;
            if (selectedActor == null)
            {
                gameplayInput.SelectUnit(enemyActor);
            }
            else
            {
                if (gridSystem.GridLogic.IsFieldAttackable(enemyActor.GridComponent.GridPosition.X, enemyActor.GridComponent.GridPosition.Y))
                {
                    if (selectionDataProvider.GetSelectedAttackTarget()!=enemyActor)
                    {
                        selectionDataProvider.ClearPositionData();
                        selectionDataProvider.ClearAttackData();
                        var gridPos = new GridPosition((int)selectedActor.GameTransformManager.GetPosition().x, (int)selectedActor.GameTransformManager.GetPosition().y);
                        if (selectedActor.GridComponent.CanAttackFrom(gridPos, enemyActor.GridComponent.GridPosition))
                        {
                            Debug.Log("Can Attack from Position");
                            if(selectedActor is IBattleActor battleActor&& enemyActor is IBattleActor enemyBattleActor)
                                gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, gridPos);
                        }
                        else if (selectedActor.GridComponent.CanAttack(enemyActor.GridComponent.GridPosition.X, enemyActor.GridComponent.GridPosition.Y))
                        {
                            Debug.Log("Can Attack From Position2");
                            selectedActor.GridComponent.ResetPosition();
                            selectionDataProvider.ClearPositionData();
                            selectionDataProvider.ClearAttackData();
                            inputPathManager.Reset();
                            inputPathManager.UpdatedMovementPath(selectedActor.GridComponent.GridPosition.X, selectedActor.GridComponent.GridPosition.Y);
                            if(selectedActor is IBattleActor battleActor&& enemyActor is IBattleActor enemyBattleActor)
                                gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, new GridPosition(selectedActor.GridComponent.GridPosition.X, selectedActor.GridComponent.GridPosition.Y));
                        }
                        else
                        {
                            Debug.Log("Dont know");
                            selectedActor.GridComponent.ResetPosition();
                            inputPathManager.CalculatePathToPosition(selectedActor, new Vector2(enemyActor.GridComponent.GridPosition.X, enemyActor.GridComponent.GridPosition.Y));
                            if (!inputPathManager.IsMovementPathEmpty())
                            {
                                var lastMovPathPos = inputPathManager.GetLastMovementPathPosition();
                              
                                Debug.Log("GameInput2 SetPosition " + selectedActor);
                                gameplayInput.MoveUnit(selectionDataProvider.SelectedActor, new GridPosition(lastMovPathPos.x, lastMovPathPos.y), GridPosition.GetFromVectorList(inputPathManager.MovementPath));
                   
                                gameplayInput.ExecuteInputActions( ()=>
                                {
                                    Debug.Log(""+gridSystem.GridLogic.GetAttackTargets(selectionDataProvider.SelectedActor).Count);
                                    gridSystem.ShowAttackFromPosition((Unit) selectionDataProvider.SelectedActor,lastMovPathPos.x,lastMovPathPos.y);
                                    if(selectedActor is IBattleActor battleActor&& enemyActor is IBattleActor enemyBattleActor)
                                        gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, new GridPosition(lastMovPathPos.x, lastMovPathPos.y));

                                });
                                //gridSystem.SetUnitInternPosition(selectedActor,lastMovPathPos.x, lastMovPathPos.y);
                                Debug.Log("Test");
                               // selectedActor.GameTransformManager.SetPosition(lastMovPathPos.x, lastMovPathPos.y);
                                
                            }
                            else
                            {
                                if(selectedActor is IBattleActor battleActor&& enemyActor is IBattleActor enemyBattleActor)
                                    gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, new GridPosition(selectedActor.GridComponent.GridPosition.X, selectedActor.GridComponent.GridPosition.Y));
                            }
                        }
                        selectionDataProvider.SetSelectedAttackTarget(enemyActor);
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
            return GridGameManager.Instance.FactionManager.IsActiveFaction(actor.Faction);
        }
        private void AttackEnemy(IGridActor character, IGridActor enemy, List<Vector2Int> movePath)
        {
            character.GridComponent.ResetPosition();
            gridSystem.HideMoveRange();
            selectionDataProvider.ClearData();
            /* Enemy is in attackRange already */
            if ((movePath == null || movePath.Count == 0) && character.GridComponent.CanAttack( enemy.GridComponent.GridPosition.X, enemy.GridComponent.GridPosition.Y))
            {
                if(character is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                    gameplayInput.AttackUnit(battleActor, enemyBattleActor);
                gameplayInput.Wait(character);
                gameplayInput.ExecuteInputActions(null);
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
                gameplayInput.Wait(character);
                gameplayInput.ExecuteInputActions(null);
            }
        }
        private bool IsActorOnTile(IGridActor actor, int x, int y)
        {
            return (x == actor.GridComponent.GridPosition.X && y == actor.GridComponent.GridPosition.Y);
        }
        private bool IsTileAttackAble(int x, int y)
        {
            return gridSystem.GridLogic.IsFieldAttackable(x, y);
        }
        

        
        private void DraggedOnActiveField(int x, int y, IGridActor gridActor)
        {
            inputPathManager.AddToPath(x, y, gridActor);
        }

        private void DraggedOverEnemy(int x, int y, IGridActor enemy)
        {
            var selectedActor = selectionDataProvider.SelectedActor;
            Debug.Log("Dragged on enemy: " + enemy +" at ["+x+"/"+y+"]");
            if (!IsTileAttackAble(x,y))
                return;
            selectionDataProvider.SetSelectedAttackTarget(enemy);
            if (inputPathManager.IsMovementPathEmpty())
            {
                if(selectedActor.GridComponent.CanAttack(x, y))
                {
                    
                    if(selectedActor is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                        gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, selectedActor.GridComponent.GridPosition);
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
            if (inputPathManager.HasValidMovementPath(selectedActor.MovementRange))
            {
                inputPathManager.UpdatedMovementPath(selectedActor.GridComponent.GridPosition.X, selectedActor.GridComponent.GridPosition.Y);
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
                inputPathManager.UpdatedMovementPath(selectedActor.GridComponent.GridPosition.X, selectedActor.GridComponent.GridPosition.Y);
            }
            else
            {
                int delta = Mathf.Abs(selectedActor.GridComponent.GridPosition.X - x) + Mathf.Abs(selectedActor.GridComponent.GridPosition.Y - y);
                if (selectedActor.AttackRanges.Contains(delta))
                {
                    if(selectedActor is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                        gameplayInput.CheckAttackPreview(battleActor, enemyBattleActor, selectedActor.GridComponent.GridPosition);
                    inputPathManager.Reset();
                    inputPathManager.UpdatedMovementPath(selectedActor.GridComponent.GridPosition.X, selectedActor.GridComponent.GridPosition.Y);
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

        
        // private void OnSelectedCharacter(IGridActor u)
        // {
        //     ResetInput();
        // }
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