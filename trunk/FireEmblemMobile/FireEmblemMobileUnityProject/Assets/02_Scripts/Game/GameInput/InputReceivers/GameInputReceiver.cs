using System;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
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
    public class GameInputReceiver : IGameInputReceiver
    {
        private GameplayCommands gameplayCommands;
        private InputPathManager inputPathManager;
        private ISelectionDataProvider selectionDataProvider;
        private LastInputPositionManager lastInputPositionManager;
        private GridSystem gridSystem;

        public GameInputReceiver(GridSystem gridSystem)
        {
            gameplayCommands = new GameplayCommands();
            selectionDataProvider = new SelectionManager();
            lastInputPositionManager = new LastInputPositionManager();

            this.gridSystem = gridSystem;
            inputPathManager = new InputPathManager(gridSystem.pathFinder, GameAssets.Instance.grid.moveArrowVisual);
            // UnitSelectionSystem.OnSelectedCharacter += OnSelectedCharacter;
            // UnitSelectionSystem.OnSelectedInActiveCharacter += OnSelectedCharacter;
        }


        public void DraggedOnGrid(int x, int y)
        {
            MyDebug.LogInput("DraggedOnGrid");
            selectionDataProvider.SetSelectedTile(x, y);
            ClickedOnGrid(x, y, true);
        }

        public void ResetInput(bool drag=false, bool move=false)
        {
            if(!drag)
                SetUnitToOriginPosition(false, move);
            inputPathManager.Reset();
            selectionDataProvider.ClearData();
           
            //gridSystem.cursor.Reset();
        }

        public void UndoClicked()
        {
            var unit = selectionDataProvider.GetUndoAbleActor();
            gameplayCommands.UndoUnit(unit);
        }


        public void DraggedOverGrid(int x, int y)
        {
            ResetDragSelectables();
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[x, y]);
            if (gridSystem.IsTileMoveableAndActive(x, y) && !IsActorOnTile(selectionDataProvider.SelectedActor, x, y))
            {
                DraggedOnActiveField(x, y, selectionDataProvider.SelectedActor);
                lastInputPositionManager.StoreLatestValidPosition(x, y,
                    selectionDataProvider.SelectedActor.MovementRange);
                SetUnitToLastValidPosition();
            }
            else if (gridSystem.IsTileMoveableAndActive(x, y) && gridSystem.Tiles[x, y].HasFreeSpace())
            {
                lastInputPositionManager.StoreLatestValidPosition(x, y,
                    selectionDataProvider.SelectedActor.MovementRange);
                SetUnitToLastValidPosition();
            }
            else
            {
           
                ResetInput();
                
                UpdateMovementPath(selectionDataProvider.SelectedActor, false);
            }

            

        }

        void SetUnitToLastInputPosition()
        {
            var pos=inputPathManager.MovementPath.Last();
            var tile = gridSystem.GetTile((int)pos.x,(int) pos.y);
            selectionDataProvider.SelectedActor.SetInternGridPosition(tile);
        }
        void SetUnitToOriginPosition(bool deleteOtherGridObject=true, bool moveGameObject=true)
        {
            //Debug.Log("SetUnitToOriginPosition" + selectionDataProvider.SelectedActor.GameTransformManager.GameObject);
            if (selectionDataProvider.SelectedActor != null)
            {
                // Debug.Log("SetUnitToOriginPosition" + selectionDataProvider.SelectedActor.GameTransformManager.GameObject+" "+  selectionDataProvider.SelectedActor.GridComponent.OriginTile);
                gridSystem.SetUnitPosition(selectionDataProvider.SelectedActor,
                    selectionDataProvider.SelectedActor.GridComponent.OriginTile, deleteOtherGridObject,
                    moveGameObject);
            }
            // selectionDataProvider.SelectedActor.SetToOriginPosition();
        }
        private void SetUnitToLastValidPosition()
        {
            var pos=lastInputPositionManager.GetLastValidPosition();
            var tile = gridSystem.GetTile((int)pos.x,(int) pos.y);
            selectionDataProvider.SelectedActor.SetInternGridPosition(tile);
        }

        public void DraggedOnObject(IGridObject actor)
        {
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[actor.GridComponent.GridPosition.X,
                actor.GridComponent.GridPosition.Y]);
            if (actor.IsEnemy(selectionDataProvider.SelectedActor) &&
                gridSystem.GridLogic.IsFieldAttackable(actor.GridComponent.GridPosition.X,
                    actor.GridComponent.GridPosition.Y))
            {
                AttackEnemy(selectionDataProvider.SelectedActor, actor, inputPathManager.MovementPath);
            }
            else
            {
                ResetInput();
                gameplayCommands.DeselectUnit(selectionDataProvider.SelectedActor);
            }
        }

        private void ResetDragSelectables()
        {
            selectionDataProvider.SetSelectedAttackTarget(null);
        }

        public void DraggedOverObject(IGridObject gridObject)
        {
            ResetDragSelectables();
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[gridObject.GridComponent.GridPosition.X,
                gridObject.GridComponent.GridPosition.Y]);
            MyDebug.LogInput("Dragged Over: "+gridObject.GridComponent.GridPosition.X+" "+gridObject.GridComponent.GridPosition.Y);
            if (!gridObject.IsEnemy(selectionDataProvider.SelectedActor))
            {
                if (gridObject == selectionDataProvider.SelectedActor)
                {
                    MyDebug.LogInput("Dragged over selected Actor");
               
                    ResetInput(true);
                    UpdateMovementPath(selectionDataProvider.SelectedActor);
                }
                else
                {
                    MyDebug.LogInput("Dragged over Ally! Show only Cursor on StartPos and ad as valid Position for passthrough");
                    if (gridSystem.IsTileMoveableAndActive(gridObject.GridComponent.GridPosition.X,
                            gridObject.GridComponent.GridPosition.Y))
                    {
                        inputPathManager.AddToPath(gridObject.GridComponent.GridPosition.X,
                            gridObject.GridComponent.GridPosition.Y, selectionDataProvider.SelectedActor);
                        SetUnitToLastInputPosition();
                    }
                    //ResetInput();
                }
            }

            if (gridObject.IsEnemy(selectionDataProvider.SelectedActor))
                DraggedOverEnemy(gridObject.GridComponent.GridPosition.X, gridObject.GridComponent.GridPosition.Y,
                    gridObject); //TODO should be dragged over enemy?
        }

        public void StartDraggingActor(IGridActor actor)
        {
            //TODO
        }

        public void DoubleClickedActor(IGridActor unit)
        {
            if (IsActiveFaction(unit))
            {
                gameplayCommands.Wait(unit);
                gameplayCommands.ExecuteInputActions(null);
                selectionDataProvider.SetUndoAbleActor(unit);
            }
            else
            {
                ClickedOnActor(unit);
            }
        }

        public void ClickedOnActor(IGridActor unit)
        {
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[unit.GridComponent.GridPosition.X,
                unit.GridComponent.GridPosition.Y]);
            if (IsActiveFaction(unit))
            {
                OwnedActorClicked(unit);
            }
            else
            {
                EnemyClicked(unit);
            }
        }

        public void ClickedDownOnGrid(int x, int y)
        {
            // Debug.Log("Show Movement Path if character is selected else still change cursor position");
            gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[x, y]);
            if (!gridSystem.GridLogic.IsTileFree(x, y))
            {
                var unit = ((Unit)(gridSystem.Tiles[x, y].GridObject));
                Debug.Log("Somehow clicked Down on non empty Tile"+unit.Name+ unit.GridComponent.GridPosition.AsVector());
                //ClickOnObject(x,y);
                return;
            }

            if (gridSystem.GridLogic.IsFieldFreeAndActive(x, y) && selectionDataProvider.SelectedActor != null &&
                !selectionDataProvider.SelectedActor.TurnStateManager.HasMoved)
            {
                if (!selectionDataProvider.IsSelectedTile(x, y))
                {
                    inputPathManager.CalculateMousePathToPosition(selectionDataProvider.SelectedActor, x, y);
                }
            }
        }

        public void LongClickOnCharacter(IGridActor unit)
        {
            gameplayCommands.ViewUnit((Unit)unit);
        }

        private void ClickOnObject(int x, int y)
        {
            if(selectionDataProvider.SelectedActor!=null)
                SetUnitToOriginPosition();
             if (gridSystem.Tiles[x, y].GridObject != null)
            {
                Debug.Log("Clicked On GridObject");
                if (selectionDataProvider.SelectedActor != null)
                {
                    if (gridSystem.Tiles[x, y].GridObject.Faction.Id !=
                        selectionDataProvider.SelectedActor.Faction.Id)
                    {
                        EnemyClicked(gridSystem.Tiles[x, y].GridObject);
                    }
                }
            }
        }

        public void ClickedOnGrid(int x, int y, bool resetPosition=false)
            {
                if(!resetPosition&&selectionDataProvider.SelectedActor!=null)
                    SetUnitToOriginPosition();
                gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[x, y]);
                if (!gridSystem.GridLogic.IsTileFree(x, y))
                {
                    Debug.Log("Somehow clicked on non empty Tile");
                    ClickOnObject(x,y);

                    return;
                }
                MyDebug.LogTest("Clicked on Grid: "+x+" "+y+" "+selectionDataProvider.SelectedActor +" "+resetPosition);
                if (gridSystem.GridLogic.IsFieldFreeAndActive(x, y) && selectionDataProvider.SelectedActor != null &&
                    (!selectionDataProvider.SelectedActor.TurnStateManager.HasMoved||(selectionDataProvider.SelectedActor.GridComponent.Canto>0&& !selectionDataProvider.SelectedActor.TurnStateManager.HasCantoed)))
                {
                    
                    if (selectionDataProvider.IsSelectedTile(x, y))
                    {
                        gameplayCommands.MoveUnit(selectionDataProvider.SelectedActor, new GridPosition(x, y),
                            GridPosition.GetFromVectorList(inputPathManager.MovementPath));
                        //gameplayCommands.Wait(selectionDataProvider.SelectedActor);

                        gameplayCommands.ExecuteInputActions(() =>
                        {
                            selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                        });
                        selectionDataProvider.ClearData();
                        // Debug.Log("SelectedTile");
                    }
                    else
                    {
                        // Debug.Log("NonSelectedTile");
                        inputPathManager.CalculateMousePathToPosition(selectionDataProvider.SelectedActor, x, y);
                        //Debug.Log("GameInput SetPosition");
                        //selectionDataProvider.SelectedActor.GameTransformManager.SetPosition(x, y);
                        gameplayCommands.MoveUnit(selectionDataProvider.SelectedActor, new GridPosition(x, y),
                            GridPosition.GetFromVectorList(inputPathManager.MovementPath));

                        gameplayCommands.ExecuteInputActions(() =>
                        {
                           // Debug.Log("attack targets from new position: " + x + " " + y);
                            if (gridSystem.GridLogic.GetAttackTargetsAtPosition(selectionDataProvider.SelectedActor, x, y).Count() > 0)
                            {
                                gridSystem.ShowAttackFromPosition((Unit)selectionDataProvider.SelectedActor, x, y);
                                selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                            }
                            else
                            {
                                gridSystem.ShowAttackRangeOnGrid(selectionDataProvider.SelectedActor,
                                    selectionDataProvider.SelectedActor.AttackRanges);
                                selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                                //gameplayCommands.Wait(selectionDataProvider.SelectedActor);
                                gameplayCommands.ExecuteInputActions(null);
                            }
                        });


                        selectionDataProvider.SetSelectedTile(x, y);
                        selectionDataProvider.ClearAttackTarget();
                    }
                }
                else if (selectionDataProvider.SelectedActor != null)
                {
                    // Debug.Log("SelectedActor Null");
                    if (selectionDataProvider.SelectedActor.GridComponent.GridPosition.X == x &&
                        y == selectionDataProvider.SelectedActor.GridComponent.GridPosition.Y)
                    {
                        //Debug.Log("ResetPos4");
                        selectionDataProvider.SelectedActor.GridComponent.ResetPosition();
                        gameplayCommands.DeselectUnit(selectionDataProvider.SelectedActor);
                    }
                    else
                    {
                        if (selectionDataProvider.GetSelectedAttackTarget() != null)
                            gameplayCommands.UndoUnit(selectionDataProvider.SelectedActor);
                        gameplayCommands.DeselectUnit(selectionDataProvider.SelectedActor);
                    }

                    ResetInput();
                }
                else
                {
                    // Debug.Log("None");
                    ResetInput();
                    gameplayCommands.DeselectUnit(selectionDataProvider.SelectedActor);
                }
            }

            private void OwnedActorClicked(IGridActor unit)
            {
                //Debug.Log("Set GridCursor Pos to: "+unit.GridComponent.GridPosition.X+" "+unit.GridComponent.GridPosition.Y);
                gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[unit.GridComponent.GridPosition.X,
                    unit.GridComponent.GridPosition.Y]);
                if ((int)unit.GameTransformManager.GetPosition().x == selectionDataProvider.GetSelectedTile().x &&
                    (int)unit.GameTransformManager.GetPosition().y ==
                    selectionDataProvider.GetSelectedTile().y) //Confirm Move
                {
                    ClickedOnGrid(selectionDataProvider.GetSelectedTile().x, selectionDataProvider.GetSelectedTile().y);
                }
                else
                {
                    gameplayCommands.SelectUnit(unit);
                }
            }

            private void EnemyClicked(IGridObject enemyActor)
            {
                gridSystem.cursor.SetCurrentTile(gridSystem.Tiles[enemyActor.GridComponent.GridPosition.X,
                    enemyActor.GridComponent.GridPosition.Y]);
               
                var selectedActor = selectionDataProvider.SelectedActor;
                if (selectedActor == null)
                {
                    if (enemyActor is IGridActor gridActor)
                        gameplayCommands.SelectUnit(gridActor);
                }
                else
                {
                    if (gridSystem.GridLogic.IsFieldAttackable(enemyActor.GridComponent.GridPosition.X,
                            enemyActor.GridComponent.GridPosition.Y))
                    {
                        gridSystem.GetTile(enemyActor.GridComponent.GridPosition.X,
                            enemyActor.GridComponent.GridPosition.Y).ShowAttackable(false);
                        if (selectionDataProvider.GetSelectedAttackTarget() != enemyActor)
                        {
                      
                            selectionDataProvider.ClearPositionData();
                            selectionDataProvider.ClearAttackData();
                            var gridPos = new GridPosition((int)selectedActor.GameTransformManager.GetPosition().x,
                                (int)selectedActor.GameTransformManager.GetPosition().y);
                            if (selectedActor.GetActorGridComponent()
                                .CanAttackFrom(gridPos, enemyActor.GridComponent.GridPosition))
                            {
                                // Debug.Log("Can Attack from Position");
                                if (selectedActor is IBattleActor battleActor &&
                                    enemyActor is IAttackableTarget enemyBattleActor)
                                    gameplayCommands.CheckAttackPreview(battleActor, enemyBattleActor, gridPos);
                                selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                            }
                            else if (selectedActor.GetActorGridComponent().CanAttack(
                                         enemyActor.GridComponent.GridPosition.X,
                                         enemyActor.GridComponent.GridPosition.Y))
                            {
                                //Debug.Log("Can Attack From Position2");
                                selectedActor.GridComponent.ResetPosition();
                                selectionDataProvider.ClearPositionData();
                                selectionDataProvider.ClearAttackData();
                                inputPathManager.Reset();
                                UpdateMovementPath(selectedActor);
                                if (selectedActor is IBattleActor battleActor &&
                                    enemyActor is IAttackableTarget enemyBattleActor)
                                    gameplayCommands.CheckAttackPreview(battleActor, enemyBattleActor,
                                        new GridPosition(selectedActor.GridComponent.GridPosition.X,
                                            selectedActor.GridComponent.GridPosition.Y));
                                selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                            }
                            else
                            {
                                //Debug.Log("Dont know");
                                selectedActor.GridComponent.ResetPosition();
                                inputPathManager.CalculateAttackPathToTarget(selectedActor, enemyActor);
                                if (!inputPathManager.IsMovementPathEmpty())
                                {
                                    var lastMovPathPos = inputPathManager.GetLastMovementPathPosition();

                                    //Debug.Log("GameInput2 SetPosition " + selectedActor);
                                    gameplayCommands.MoveUnit(selectionDataProvider.SelectedActor,
                                        new GridPosition(lastMovPathPos.x, lastMovPathPos.y),
                                        GridPosition.GetFromVectorList(inputPathManager.MovementPath));

                                    gameplayCommands.ExecuteInputActions(() =>
                                    {
                                        //Debug.Log(""+gridSystem.GridLogic.GetAttackTargets(selectionDataProvider.SelectedActor).Count);
                                        selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                                        gridSystem.ShowAttackFromPosition((Unit)selectionDataProvider.SelectedActor,
                                            lastMovPathPos.x, lastMovPathPos.y);
                                        if (selectedActor is IBattleActor battleActor &&
                                            enemyActor is IAttackableTarget enemyBattleActor)
                                            gameplayCommands.CheckAttackPreview(battleActor, enemyBattleActor,
                                                new GridPosition(lastMovPathPos.x, lastMovPathPos.y));
                                    });
                                    //gridSystem.SetUnitInternPosition(selectedActor,lastMovPathPos.x, lastMovPathPos.y);
                                    //Debug.Log("Test");
                                    // selectedActor.GameTransformManager.SetPosition(lastMovPathPos.x, lastMovPathPos.y);
                                }
                                else
                                {
                                    if (selectedActor is IBattleActor battleActor &&
                                        enemyActor is IAttackableTarget enemyBattleActor)
                                        gameplayCommands.CheckAttackPreview(battleActor, enemyBattleActor,
                                            new GridPosition(selectedActor.GridComponent.GridPosition.X,
                                                selectedActor.GridComponent.GridPosition.Y));
                                    selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
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
                        if (enemyActor is IGridActor gridActor)
                            gameplayCommands.SelectUnit(gridActor);
                        // if (enemyActor is Unit unit)
                        //     gameplayCommands.ViewUnit(unit);
                    }
                }
            }

            private bool IsActiveFaction(IGridActor actor)
            {
                return GridGameManager.Instance.FactionManager.IsActiveFaction(actor.Faction);
            }

            private void AttackEnemy(IGridActor character, IGridObject enemy, List<Vector2Int> movePath)
            {
                Debug.Log("ATTACK ENEMY: => RESET GRIDPOSITION => HIDE MOVE RANGE");
                ((GridActorComponent)character.GridComponent).ResetPosition(false);
                
                gridSystem.HideMoveRange();
                selectionDataProvider.ClearData();
                /* Enemy is in attackRange already */
                if ((movePath == null || movePath.Count == 0) && character.GetActorGridComponent()
                        .CanAttack(enemy.GridComponent.GridPosition.X, enemy.GridComponent.GridPosition.Y))
                {
                    if (character is IBattleActor battleActor)
                        gameplayCommands.AttackUnit(battleActor,(IAttackableTarget)enemy);
                    gameplayCommands.Wait(character);
                    gameplayCommands.ExecuteInputActions(null);
                }
                else if (movePath != null) //go to enemy cause not in range
                {
                    if (movePath.Count >= 1)
                    {
                        int xMov = (int)movePath[movePath.Count - 1].x;
                        int yMov = (int)movePath[movePath.Count - 1].y;
                        gameplayCommands.MoveUnit(character, new GridPosition(xMov, yMov),
                            GridPosition.GetFromVectorList(movePath));
                    }

                    if (character is IBattleActor battleActor)
                        gameplayCommands.AttackUnit(battleActor,(IAttackableTarget)enemy);
                    gameplayCommands.Wait(character);
                    gameplayCommands.ExecuteInputActions(null);
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

            void DraggedOverEnemyWithoutMovePath(IGridActor selectedActor, int x, int y, IGridObject enemy)
            {
                if (selectedActor.GetActorGridComponent().CanAttack(x, y))
                {
                    Debug.Log("Check Attack Preview from HEre: " + x + " " + y);

                    if (selectedActor is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                        gameplayCommands.CheckAttackPreview(battleActor, enemyBattleActor,
                            selectedActor.GridComponent.GridPosition);
                    selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                }
                else
                {
                    inputPathManager.CalculateAttackPathToTarget(selectedActor, enemy);
                }
            }
            private void DraggedOverEnemy(int x, int y, IGridObject enemy)
            {
                var selectedActor = selectionDataProvider.SelectedActor;
                // Debug.Log("Dragged on enemy: " + enemy +" at ["+x+"/"+y+"]");
                if (!IsTileAttackAble(x, y))
                    return;
                gridSystem.GetTile(x,y).ShowAttackable(false);
                selectionDataProvider.SetSelectedAttackTarget(enemy);
                if (inputPathManager.IsMovementPathEmpty())
                {
                  
                    if (inputPathManager.IsPreviousMovementPathEmpty())
                    {
                        // Debug.Log("PREVIOUS PATH EMPTY");
                        DraggedOverEnemyWithoutMovePath(selectedActor, x, y, enemy);
                    }
                    else
                    {
                        // Debug.Log("PREVIOUS PATH NOT EMPTY");
                        int attackPositionIndex = -1;
                        bool foundAttackPosition = false;
                        attackPositionIndex = SearchForSuitableAttackPositionFromPreviousDraggedOverTiles(x, y, selectedActor,
                            attackPositionIndex, ref foundAttackPosition);
                        if (foundAttackPosition)
                        {
                            // Debug.Log("PREVIOUS PATH FOUND ATTACK POS");
                            inputPathManager.SetMovementPathToPrevious();
                            SearchForSuitableAttackPosition(x, y, enemy, selectedActor);
                        }
                        else
                        {
                            // Debug.Log("PREVIOUS PATH FOUND NO ATTACK POS");
                            DraggedOverEnemyWithoutMovePath(selectedActor, x, y, enemy);
                        }
                    }
                }
                else // Search for suitable AttackPosition
                {
                    SearchForSuitableAttackPosition(x, y, enemy, selectedActor);
                }
                
                if (inputPathManager.HasValidMovementPath(selectedActor.MovementRange))
                { 
                  //  Debug.Log("PUT MOVEMENT STUFF HERE?");
                    UpdateMovementPath(selectedActor);
                    //Debug.Log("Check Attack Preview from HEre: "+x+" "+y+" "+inputPathManager.GetLastMovementPathPosition().x+" "+inputPathManager.GetLastMovementPathPosition().y);
                    if (selectedActor is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                    {
                        var lastMovementPathPosition = inputPathManager.GetLastMovementPathPosition();
                        if (lastMovementPathPosition.x == -1 || lastMovementPathPosition.y == -1)
                            return;
                        gameplayCommands.CheckAttackPreview(battleActor, enemyBattleActor,
                            new GridPosition(lastMovementPathPosition.x,
                                lastMovementPathPosition.y));
                        selectionDataProvider.SetUndoAbleActor(selectionDataProvider.SelectedActor);
                    }
                        
                }
            }

            void UpdateMovementPath(IGridActor selectedActor, bool resetPrevious=true)
            {
                inputPathManager.UpdatedMovementPath(selectedActor.GridComponent.OriginTile.X,
                    selectedActor.GridComponent.OriginTile.Y, resetPrevious);
            }
            private void SearchForSuitableAttackPosition(int x, int y, IGridObject enemy, IGridActor selectedActor)
            {
                var foundAttackPosition = false;
                int attackPositionIndex = -1;
                attackPositionIndex = SearchForSuitableAttackPositionFromAlreadyDraggedOverTiles(x, y, selectedActor,
                    attackPositionIndex, ref foundAttackPosition);

                if (foundAttackPosition) //Removes Parts of the drag Path to make it end at the correct attackPosition;
                {
                    inputPathManager.MovementPath.RemoveRange(attackPositionIndex + 1,
                        inputPathManager.MovementPath.Count - (attackPositionIndex + 1));
                    UpdateMovementPath(selectedActor);
                  
                }
                else
                {
                    int delta = Mathf.Abs(selectedActor.GridComponent.GridPosition.X - x) +
                                Mathf.Abs(selectedActor.GridComponent.GridPosition.Y - y);
                    if (selectedActor.AttackRanges.Contains(delta))
                    {
                        if (selectedActor is IBattleActor battleActor && enemy is IBattleActor enemyBattleActor)
                            gameplayCommands.CheckAttackPreview(battleActor, enemyBattleActor,
                                selectedActor.GridComponent.GridPosition);
                        inputPathManager.Reset();
                        UpdateMovementPath(selectedActor);
                    }
                    else
                    {
                        inputPathManager.CalculateAttackPathToTarget(selectedActor, enemy);
                    }
                }
            }

            private int SearchForSuitableAttackPositionFromAlreadyDraggedOverTiles(int x, int y,
                IGridActor selectedActor,
                int attackPositionIndex, ref bool foundAttackPosition)
            {
                for (int i = inputPathManager.MovementPath.Count - 1;
                     i >= 0;
                     i--) //Search for suitable Position from already dragged over tiles
                {
                    var lastMousePathPositionX = (int)inputPathManager.MovementPath[i].x;
                    var lastMousePathPositionY = (int)inputPathManager.MovementPath[i].y;
                    var lastMousePathField = gridSystem.GetTileFromVector2(inputPathManager.MovementPath[i]);
                    int delta = Mathf.Abs(lastMousePathPositionX - x) + Mathf.Abs(lastMousePathPositionY - y);
                    if (selectedActor.AttackRanges.Contains(delta))
                        if (lastMousePathField.GridObject == null)
                        {
                            attackPositionIndex = i;
                            foundAttackPosition = true;
                            break;
                        }
                }

                return attackPositionIndex;
            }
            private int SearchForSuitableAttackPositionFromPreviousDraggedOverTiles(int x, int y,
                IGridActor selectedActor,
                int attackPositionIndex, ref bool foundAttackPosition)
            {
                for (int i = inputPathManager.previousMovementPath.Count - 1;
                     i >= 0;
                     i--) //Search for suitable Position from already dragged over tiles
                {
                    var lastMousePathPositionX = (int)inputPathManager.previousMovementPath[i].x;
                    var lastMousePathPositionY = (int)inputPathManager.previousMovementPath[i].y;
                    var lastMousePathField = gridSystem.GetTileFromVector2(inputPathManager.previousMovementPath[i]);
                    int delta = Mathf.Abs(lastMousePathPositionX - x) + Mathf.Abs(lastMousePathPositionY - y);
                    if (selectedActor.AttackRanges.Contains(delta))
                        if (lastMousePathField.GridObject == null)
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
                gameplayCommands.DeselectUnit(selectionDataProvider.SelectedActor);
            }
        }
    }