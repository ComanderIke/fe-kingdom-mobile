using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.Grid;
using Game.Grid.PathFinding;
using Game.Map;
using Game.Mechanics;
using UnityEngine;

namespace Game.GameInput
{
    public class GameInputReceiver : IGameInputReceiver{
        
        private GameplayInput gameplayInput;
        private int selectedTileX = -1;
        private int selectedTileY = -1;
        private List<CursorPosition> LastPositions = new List<CursorPosition>(); 
        private readonly List<Vector2> dragPath = new List<Vector2>(); 
        private InputPathManager inputPathManager;
        private Unit confirmAttackTarget; /* stores the last attackPreview Target in order to 
                                                                                determine if the same target is clicked again, 
                                                                                which will result in an AttackAction */
        private IGridActor SelectedActor => gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter;
        public GameInputReceiver(GameplayInput gameplayInput)
        {
            this.gameplayInput = gameplayInput;
            UnitSelectionSystem.OnSelectedCharacter += OnSelectedCharacter;
            UnitSelectionSystem.OnSelectedInActiveCharacter += OnSelectedCharacter;
            inputPathManager = new InputPathManager();
        }
        private void OnSelectedCharacter(Unit u)
        {
            ResetInput();
        }
        public void DraggedOnGrid(int x, int y)
        {
            selectedTileX = x;
            selectedTileY = y;
            GridClicked(x, y);
        }
        private int GetDelta(Vector2 v, Vector2 v2)
        {
            var xDiff = (int) Mathf.Abs(v.x - v2.x);
            var zDiff = (int) Mathf.Abs(v.y - v2.y);
            return xDiff + zDiff;
        }
        private void StoreLatestValidPosition(int x, int y)
        {
            LastPositions.Add(new CursorPosition(new Vector2(x, y), null));

            if (LastPositions.Count > SelectedActor.MovementRage)
                LastPositions.RemoveAt(0);
        }

        public void ResetInput()
        {
            dragPath.Clear();
            selectedTileX = -1;
            selectedTileY = -1;
            inputPathManager.MovementPath.Clear();
        }

        public void DraggedOverGrid(int x, int y)
        {
            if (gridSystem.IsTileMoveableAndActive(x, y) && !IsActorOnTile(SelectedActor, x, y))
            {
                DraggedOnActiveField(x, y, SelectedActor);
            }
            if (gridSystem.IsTileMoveableAndActive(x, y) && tile.HasFreeSpace())
            {
                StoreLatestValidPosition(x, y);
            }
            inputPathManager.UpdatedMovementPath();
        }

        public void DraggedOnActor(IGridActor gridActor)
        {
            //Debug.Log("Dragged ended on unit: " + draggedOverUnit.name);
            if (actor.Faction.Id != gridGameManager.FactionManager.ActiveFaction.Id)//Enemy
            {
                if (gridGameManager.GetSystem<Map.GridSystem>().GridLogic.IsFieldAttackable(actor.GridPosition.X,
                    actor.GridPosition.Y))
                {
                    
                    AttackEnemy(SelectedCharacter, actor, MovementPath);
                }
                else
                {
                    //Debug.Log("enemy not in Range");
                    //Debug.Log("TODO Select Enemy! Without showing his range!");
                    gameplayInput.DeselectUnit();
                }
            }
            else
            {
                //Debug.Log("DraggedOnFriendlyUnit");
                gameplayInput.DeselectUnit();
            }
        }

        public void DraggedOverActor(IGridActor gridActor)
        {
            if (gridActor.IsEnemy(SelectedActor))
                DraggedOnEnemy(x, y, gridActor);
            inputPathManager.UpdatedMovementPath();

        }
        private bool IsActorOnTile(IGridActor actor, int x, int y)
        {
            return (x == actor.GridPosition.X && y == actor.GridPosition.Y);
        }
        public void ClickedOnUndefined()
        {
            gameplayInput.DeselectUnit();
        }

        public void StartDraggingActor(IGridActor actor)
        {
            //TODO
            Debug.Log("TODO: Add User Feedback!");
        }

        public void DoubleClickedActor(IGridActor unit)
        {
            if(IsActiveFaction(unit))
                gameplayInput.Wait(unit);
            else//If enemy proceed as if it was a normal click
            {
                UnitClicked(unit);
            }
        }

        public void ClickedOnActor(IGridActor unit)
        {
            if(IsActiveFaction(unit))
            {
                if (unit.GetGameTransformPosition().x == selectedTileX && unit.GetGameTransformPosition().y == selectedTileY)//Confirm Move
                {
                    GridClicked(selectedTileX, selectedTileY);
                }
                else
                {
                    gameplayInput.SelectUnit(unit);
                }
            }
            else {
                EnemyClicked(unit);
            }
        }

        public void ClickedOnGrid(int x, int y)
        {
            if (gridGameManager.GetSystem<GridSystem>().GridLogic.IsFieldFreeAndActive(x, y))
            {
                
                if(selectedTileX == x && selectedTileY == y)
                {
 
                    selectedTileX = -1;
                    selectedTileY = -1;
                    gameplayInput.MoveUnit(SelectedCharacter, new GridPosition(x, y), GridPosition.GetFromVectorList(MovementPath));
                    //gameplayInput.ExecuteInputActions(() => gridGameManager.GameStateManager.SwitchState(GameStateManager.GameplayState));
                }
                else
                {
                    CalculateMousePathToPosition(SelectedCharacter, x, y);
                    SelectedCharacter.SetGameTransformPosition(x, y);
                    selectedTileX = x;
                    selectedTileY = y;
                    
                }

            }
            else if(SelectedCharacter != null)
            {
                if (SelectedCharacter.GridPosition.X == x && y == SelectedCharacter.GridPosition.Y)
                {
                    selectedTileX = -1;
                    selectedTileY = -1;
                    SelectedCharacter.ResetPosition();
                    ResetInput();
                    gameplayInput.DeselectUnit();
                    
                }
                else
                {
                    ResetInput();
                    gameplayInput.DeselectUnit();
                }
            }
            else
            {
                ResetInput();
                gameplayInput.DeselectUnit();
            }
        }

       

        private void EnemyClicked(Unit unit)
        {
            //Debug.Log("Enemy clicked!");
            if (SelectedCharacter == null)
            {
                gameplayInput.SelectUnit(unit);
            }
            else
            {
                if (gridGameManager.GetSystem<GridSystem>().GridLogic.IsFieldAttackable(unit.GridPosition.X, unit.GridPosition.Y))
                {
                    if (confirmAttackTarget!=unit)
                    {
                        selectedTileX = -1;
                        selectedTileY = -1;
                        GridPosition gridPos = new GridPosition((int)SelectedCharacter.GetGameTransformPosition().x, (int)SelectedCharacter.GetGameTransformPosition().y);
                        if (gridPos.CanAttack(SelectedCharacter.Stats.AttackRanges, unit.GridPosition))
                        {
                            //MovementPath.Clear();
                            //UpdatedMovementPath();
                           
                            gameplayInput.CheckAttackPreview(SelectedCharacter, unit,
                                   gridPos);
                        }
                        else if (SelectedCharacter.CanAttack(unit.GridPosition.X, unit.GridPosition.Y))
                        {
                            SelectedCharacter.ResetPosition();
                            selectedTileX = -1;
                            selectedTileY = -1;
                            MovementPath.Clear();
                            UpdatedMovementPath();
                            gameplayInput.CheckAttackPreview(SelectedCharacter, unit,
                                   new GridPosition(SelectedCharacter.GridPosition.X, SelectedCharacter.GridPosition.Y));
                        }
                        else
                        {
                            SelectedCharacter.ResetPosition();
                            CalculatePathToPosition(SelectedCharacter, new Vector2(unit.GridPosition.X, unit.GridPosition.Y));
                            if (MovementPath.Count >= 1)
                            {
                                //Debug.Log("Here!");
                                //Set the Sprite to the AttackPosition
                                //SelectedCharacter.GameTransform.SetPosition((int)dragPath[dragPath.Count - 1].x, (int)dragPath[dragPath.Count - 1].y);
                                SelectedCharacter.SetGameTransformPosition((int)MovementPath[MovementPath.Count - 1].x, (int)MovementPath[MovementPath.Count - 1].y);
                                gameplayInput.CheckAttackPreview(SelectedCharacter, unit,
                                    new GridPosition((int)MovementPath[MovementPath.Count - 1].x, (int)MovementPath[MovementPath.Count - 1].y));
                            }
                            else
                            {
                                //Debug.Log("HERE");
                                //Debug.Log("Here!"+ SelectedCharacter.GridPosition.X+","+SelectedCharacter.GridPosition.Y);
                                gameplayInput.CheckAttackPreview(SelectedCharacter, unit,
                                   new GridPosition(SelectedCharacter.GridPosition.X, SelectedCharacter.GridPosition.Y));
                            }
                        }
                        confirmAttackTarget = unit;
                    }
                    else
                    {
                        AttackEnemy(SelectedCharacter, unit, MovementPath);
                    }
                }
                else
                {
                    //gameplayInput.DeselectUnit();
                    gameplayInput.ViewUnit(unit);
                    //Debug.Log("Enemy not Attackable!");
                    //Debug.Log("TODO Select Enemy! Without showing his range!");
                }
            }
        }
        private bool IsActiveFaction(IGridActor actor)
        {
            return actor.FactionId == gridGameManager.FactionManager.ActivePlayerNumber;
        }
        private void AttackEnemy(Unit character, Unit enemy, List<Vector2> movePath)
        {
            character.ResetPosition();
            //ResetDrag();
            gridGameManager.GetSystem<Map.GridSystem>().HideMoveRange();
            /* Enemy is in attackRange already */
            if ((movePath == null || movePath.Count == 0) &&
                character.CanAttack( enemy.GridPosition.X, enemy.GridPosition.Y))
            {

                //Debug.Log("Enemy is in Range:");
                gameplayInput.AttackUnit(character, enemy);
                // gameplayInput.ExecuteInputActions(() => gridGameManager.GameStateManager.SwitchState(GameStateManager.GameplayState));
            }
            else if(movePath!=null) //go to enemy cause not in range
            {
                //Debug.Log("Got to Enemy!" + movePath.Count);

            
                if (movePath.Count >= 1)
                {
                    int xMov = (int)movePath[movePath.Count - 1].x;
                    int yMov = (int)movePath[movePath.Count - 1].y;
                    gameplayInput.MoveUnit(character, new GridPosition(xMov, yMov), GridPosition.GetFromVectorList(movePath));
                }
                gameplayInput.AttackUnit(character, enemy);
                //gameplayInput.ExecuteInputActions(() => gridGameManager.GameStateManager.SwitchState(GameStateManager.GameplayState));
                
                //AttackRangeFromPath = 0;//No Idea why this is here!
            }
        }
        public Vector2 GetCenterPos(Vector2 clickedPos)
        {
            int centerX = (int)Mathf.Round(clickedPos.x - GridSystem.GRID_X_OFFSET) - 1;
            int centerY = (int)Mathf.Round(clickedPos.y) - 1;
            return new Vector2(centerX, centerY);
        }
        private bool IsTileAttackAble(int x, int y)
        {
            return gridGameManager.GetSystem<GridSystem>().GridLogic.IsFieldAttackable(x, y);
        }
        public Vector2 GetLastAttackPosition(Unit c, int xAttack, int zAttack)
        {
            for (int i = c.Stats.AttackRanges.Count - 1; i >= 0; i--) //Prioritize Range Attacks
            for (int j = LastPositions.Count - 1; j >= 0; j--)
                if (GetDelta(LastPositions[j].Position, new Vector2(xAttack, zAttack)) == c.Stats.AttackRanges[i] &&
                    gridGameManager.GetSystem<GridSystem>().Tiles[(int)LastPositions[j].Position.x,
                        (int)LastPositions[j].Position.y].Unit == null)
                    return LastPositions[j].Position;

            return new Vector2(-1, -1);
        }
        private bool IsFieldAdjacent(float x, float y, float x2, float y2)
        {
            return Mathf.Abs(x - x2) + Mathf.Abs(y - y2) > 1;
        }
        private bool IsLastActiveFieldAdjacent(float x, float y, Unit character)
        {
            if (dragPath.Count >= 2 && IsFieldAdjacent(dragPath[dragPath.Count - 2].x, dragPath[dragPath.Count - 2].y, x, y))
                return true;
            if (dragPath.Count == 1 && IsFieldAdjacent(character.GridPosition.X, character.GridPosition.Y, x, y))
                return true;
            if (IsFieldAdjacent(lastDragPosX, lastDragPosY, x, y))
                return true;
            return false;
        }
        public void DraggedOnActiveField(int x, int y, IGridActor character)
        {
            bool contains = dragPath.Contains(new Vector2(x, y));

            dragPath.Add(new Vector2(x, y));
            
            //Create new MovementPath if dragPath is to long or other conditions apply
            if (dragPath.Count > character.MovementRage || contains || IsLastActiveFieldAdjacent(x,y,character))
            {
                dragPath.Clear();
                var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                    character.GridPosition.Y, x, y, character, false, character.AttackRanges);
                if (p != null)
                    for (int i = p.GetLength() - 2; i >= 0; i--)
                        dragPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            }
            MovementPath = new List<Vector2>(dragPath);
            OnDraggedOnActiveField?.Invoke();
          
        }
          public void DraggedOnEnemy(int x, int y, IGridActor enemy)
        {
            
            Debug.Log("Dragged on enemy: " + enemy +" at ["+x+"/"+y+"]");
            if (!IsTileAttackAble(x,y))
                return;
            //CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
            if (!HasMovementPath())
            {
                //Debug.Log("Mousepath empty");
                if(SelectedActor.CanAttack(x, y))
                {
                    if(enemy is IBattleActor battleActor)
                        gameplayInput.CheckAttackPreview(SelectedActor, battleActor, SelectedActor.GridPosition);
                }
                else
                {
                    CalculatePathToPosition(SelectedActor, new Vector2(x, y));
                }
                //CalculateMousePathToAttackField(SelectedCharacter, x, y);
            }
            else  // Search for suitable AttackPosition
            {
                var foundAttackPosition = false;
                int attackPositionIndex = -1;
                for (int i = MovementPath.Count - 1; i >= 0; i--)//Search for suitable Position from already dragged over tiles
                {
                    var lastMousePathPositionX = (int)MovementPath[i].x;
                    var lastMousePathPositionY = (int)MovementPath[i].y;
                    var lastMousePathField = gridGameManager.GetSystem<GridSystem>().GetTileFromVector2(MovementPath[i]);
                    int delta = Mathf.Abs(lastMousePathPositionX - x) + Mathf.Abs(lastMousePathPositionY - y);
                    if (SelectedActor.Stats.AttackRanges.Contains(delta))
                        if (lastMousePathField.Unit == null)
                        {
                            //Debug.Log("Valid AttackPosition!");
                            attackPositionIndex = i;
                            foundAttackPosition = true;
                            break;
                        }
                }

                if (foundAttackPosition)//Removes Parts of the drag Path to make it end at the correct attackPosition;
                {
                    MovementPath.RemoveRange(attackPositionIndex + 1, MovementPath.Count - (attackPositionIndex + 1));
                    UpdatedMovementPath();
                }
                else
                {

                    int delta = Mathf.Abs(SelectedActor.GridPosition.X - x) + Mathf.Abs(SelectedActor.GridPosition.Y - y);
                    if (SelectedActor.Stats.AttackRanges.Contains(delta))
                    {
                        //Debug.Log("HERe" + delta);
                        if(enemy is IBattleActor battleActor)
                            gameplayInput.CheckAttackPreview(SelectedActor, battleActor, SelectedActor.GridPosition);
                        MovementPath.Clear();
                        UpdatedMovementPath();
                    }
                    else
                    {
                        //Debug.Log("No suitable AttackPosition found in dragPath!");
                        CalculatePathToPosition(SelectedActor, new Vector2(x, y));
                    }
                }
            }

            if (MovementPath != null && MovementPath.Count <= SelectedActor.Stats.Mov)
            {
                UpdatedMovementPath();
                if(enemy is IBattleActor battleActor)
                    gameplayInput.CheckAttackPreview(SelectedActor, battleActor, new GridPosition(x, y));
            }
            else
            {
                //Debug.Log("Not enough Movement!");
            }
        }
        
    }
}