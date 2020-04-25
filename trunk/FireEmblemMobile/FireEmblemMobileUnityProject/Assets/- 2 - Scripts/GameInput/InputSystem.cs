using Assets.Core;
using Assets.GameActors.Players;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Monsters;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameResources;
using Assets.Grid;
using Assets.GUI;
using Assets.Manager;
using Assets.Map;
using Assets.Mechanics;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.GameInput
{
    public class InputSystem : MonoBehaviour, IEngineSystem
    {
        public bool Active;

        private readonly List<Vector2> dragPath = new List<Vector2>();          // tracks the path a unit is dragged along the grid.
        public List<CursorPosition> LastPositions = new List<CursorPosition>(); /* tracks the last positions the cursor was on,
                                                                                in order to determine the latest possible attack Position. */
        public List<Vector2> MovementPath = new List<Vector2>();                // stores the path a unit is moving along the grid.

        [HideInInspector] public int AttackRangeFromPath;                       /* Very Complicated to Refactor! 
                                                                                This is used to store the Position from where a Unit will Attack.
                                                                                this changes based on AttackRange of the unit 
                                                                                and based on the prefered movementPath. */

        private GridGameManager gridGameManager;
        public RaycastManager RaycastManager;
        private PlayerInputFeedback playerInputFeedback;
        private GameplayInput gameplayInput;

        private int lastDragPosX = -1;
        private int lastDragPosY = -1;

        private void Start()
        {
            gridGameManager = GridGameManager.Instance;
            playerInputFeedback = new PlayerInputFeedback();
            gameplayInput = new GameplayInput();
            RaycastManager = new RaycastManager();
            InitEvents();
        }
        private void InitEvents()
        {
            OnSetActive += SetInputActive;

            OnUnitClicked += UnitClicked;
            OnUnitDoubleClicked += UnitDoubleClicked;

            OnStartDrag += StartDrag;
            OnUnitDragged += CharacterDrag;
            OnDraggedOverUnit += DraggedOver;
            OnEndDrag += DragEnded;
        }
        private void Update()
        {
            if (!Active)
                return;
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())// player clicked on a non UI Object 
            {
                CheckClickOnGrid(); /*checks if player clicked on an empty grid tile
                                    Clicks on Characters will be managed by the UnitController */
            }
        }
        private void CheckClickOnGrid()
        {
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            RaycastHit hit = RaycastManager.GetLatestHit();
            var x = (int)gridPos.x;
            var y = (int)gridPos.y;
            if( hit.collider != null && hit.collider.tag == TagManager.GridTag)
            {
                ResetDrag();
                Debug.Log("Grid clicked:" + x + " " + y + " " + hit.point + " " + gridPos);
                GridClicked(x, y);
            }
        }

        public void SetInputActive(bool active)
        {
            Active = !active;
        }

        private void DragEnded()
        {
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            if (RaycastManager.ConnectedLatestHit())
            {
                var latestHit = RaycastManager.GetLatestHit();
                if (latestHit.collider.gameObject.CompareTag(TagManager.GridTag)) //Dragged on Grid
                {
                    UnitDraggedOnGrid((int)gridPos.x, (int)gridPos.y);
                }
                else if (latestHit.collider.gameObject.CompareTag(TagManager.UnitTag))//Dragged on Unit
                {
                    var draggedOverUnit = latestHit.collider.gameObject.GetComponent<UnitController>().Unit;
                    UnitDraggedOnUnit(draggedOverUnit);
                }
                else
                {
                    OnDragCanceled();
                }
            }
            else
            {
                OnDragCanceled();
            }
            ResetDrag();
        }

        private void UnitDoubleClicked(Unit unit)
        {
            Debug.Log("Unit Double Clicked!");
            if (!Active)
                return;
            gameplayInput.Wait(unit);
        }
        private void UnitClicked(Unit unit)
        {
            Debug.Log("Unit Clicked!");
            if (!Active)
                return;

            if (unit.Faction.Id == gridGameManager.FactionManager.ActiveFaction.Id) // Player Unit Clicked
            {
                gameplayInput.SelectUnit(unit);
            }
            else {
                EnemyClicked(unit);
            }
        }
        private void EnemyClicked(Unit unit)
        {
            Debug.Log("Enemy clicked!");
            var selectedCharacter = gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            if (selectedCharacter == null)
            {
                Debug.Log("TODO Select Enemy!");
                Debug.Log("TODO Show Enemy Range!");
            }
            else
            {
                if (gridGameManager.GetSystem<MapSystem>().GridLogic.IsFieldAttackable(unit.GridPosition.X, unit.GridPosition.Y))
                {
                    CalculateMousePathToEnemy(selectedCharacter, new Vector2(unit.GridPosition.X, unit.GridPosition.Y));
                    DrawMousePath();
                    if (dragPath.Count >= 1)
                    {
                        gameplayInput.CheckAttackPreview(selectedCharacter, unit, 
                            new GridPosition((int)dragPath[dragPath.Count - 1].x, (int)dragPath[dragPath.Count - 1].y));
                    }
                }
                else
                {
                    Debug.Log("Enemy not Attackable!");
                    Debug.Log("TODO Select Enemy! Without showing his range!");
                }
            }
        }
        public void GoToEnemy(Unit character, Unit enemy, bool drag, List<Vector2> movePath)
        {
            character.ResetPosition();
            ResetDrag();
            if ((movePath == null || movePath.Count == 0) &&
                character.GridPosition.CanAttack(character.Stats.AttackRanges, enemy.GridPosition))
            {
                gridGameManager.GetSystem<Map.MapSystem>().HideMovement();
                Debug.Log("Enemy is in Range:");
                gameplayInput.AttackUnit(character, enemy);

                return;
            }
            else //go to enemy cause not in range
            {
                if (movePath == null)
                {
                    Debug.LogError("MovePath was null");
                    return;
                }
                Debug.Log("Got to Enemy!");
                if (gridGameManager.GetSystem<Map.MapSystem>().GridLogic
                    .IsFieldAttackable(enemy.GridPosition.X, enemy.GridPosition.Y))
                {
                    gridGameManager.GetSystem<Map.MapSystem>().HideMovement();

                    int xMov = 0;
                    int yMov = 0;
                    if (movePath.Count >= 1)
                    {
                        xMov = (int)movePath[movePath.Count - 1].x;
                        yMov = (int)movePath[movePath.Count - 1].y;
                    }
                    gameplayInput.MoveUnit(character, new GridPosition(xMov, yMov), GridPosition.GetFromVectorList(movePath));
                    gameplayInput.AttackUnit(character, enemy);
                    Debug.Log("TODO? Works only for melee?");

                    gridGameManager.GetSystem<InputSystem>().AttackRangeFromPath = 0;

                    return;
                }
                else
                {
                    Debug.Log("Enemy not in Range!");
                    return;
                }
            }
        }
      

        public void GridClicked(int x, int y)
        {
            var selectedCharacter = gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            if (gridGameManager.GetSystem<MapSystem>().GridLogic.IsFieldFreeAndActive(x, y))
            {
                var movePath = gridGameManager.GetSystem<InputSystem>().MovementPath;
                gridGameManager.GetSystem<MapSystem>().HideMovement();
                gameplayInput.MoveUnit(selectedCharacter, new GridPosition(x, y), GridPosition.GetFromVectorList(movePath));
                gameplayInput.ExecuteInputActions(() => gridGameManager.GameStateManager.SwitchState(GameStateManager.GameplayState));
            }
        }
        private void UnitDraggedOnGrid(int x, int y)
        {
            var unitSelectionManager = gridGameManager.GetSystem<UnitSelectionSystem>();
            var selectedUnit = unitSelectionManager.SelectedCharacter;
            //Debug.Log("TEST: "+mainScript.GetSystem<Map.MapSystem>().Tiles[x, y]);
            if (gridGameManager.GetSystem<Map.MapSystem>().Tiles[x, y].IsActive &&
                !(x == selectedUnit.GridPosition.X && y == selectedUnit.GridPosition.Y))
            {

                selectedUnit.GridPosition.SetPosition(selectedUnit.GridPosition.X, selectedUnit.GridPosition.Y);

                gameplayInput.MoveUnit(selectedUnit, new GridPosition(x, y), GridPosition.GetFromVectorList(MovementPath));
                gameplayInput.ExecuteInputActions(()=>gridGameManager.GameStateManager.SwitchState(GameStateManager.GameplayState));

            }
            else if (gridGameManager.GetSystem<Map.MapSystem>().Tiles[x, y].Unit != null &&
                     gridGameManager.GetSystem<Map.MapSystem>().Tiles[x, y].Unit.Faction.Id != selectedUnit.Faction.Id)
            {
                GoToEnemy(selectedUnit, gridGameManager.GetSystem<Map.MapSystem>().Tiles[x, y].Unit, true, MovementPath);
            }
            else
            {
                unitSelectionManager.DeselectActiveCharacter();
            }
        }
        private void UnitDraggedOnUnit(Unit draggedOverUnit)
        {
            Debug.Log("MoveToOtherUnit");
            var unitSelectionManager = gridGameManager.GetSystem<UnitSelectionSystem>();
            if (draggedOverUnit.Faction.Id != unitSelectionManager.SelectedCharacter.Faction.Id)
            {
                if (gridGameManager.GetSystem<Map.MapSystem>().GridLogic.IsFieldAttackable(draggedOverUnit.GridPosition.X,
                    draggedOverUnit.GridPosition.Y))
                    GoToEnemy(unitSelectionManager.SelectedCharacter, draggedOverUnit, true, MovementPath);
                else
                {
                    Debug.Log("enemy not in Range");
                    gameplayInput.DeselectUnit();
                }
            }
            else
            {
                gameplayInput.DeselectUnit();
            }
        }
        public void StartDrag(int gridX, int gridY)
        {
        }

        public void ResetDrag()
        {
            dragPath.Clear();
            lastDragPosX = -1;
            lastDragPosY = -1;
            OnDragReset?.Invoke();
        }

        public void DraggedOver(Unit character)
        {
            //if (unitSelectionManager.SelectedCharacter != null && unitSelectionManager.SelectedCharacter != Unit)
            //{
            //TODO: Show Attack Icon or something
            //}
        }

        public void DraggedExit()
        {
            //TODO: Hide Attack Icon or something
        }

        public bool IsOldDrag(int x, int y)
        {
            return x == lastDragPosX && y == lastDragPosY;
        }

        public int GetDelta(Vector2 v, Vector2 v2)
        {
            var xDiff = (int)Mathf.Abs(v.x - v2.x);
            var zDiff = (int)Mathf.Abs(v.y - v2.y);
            return xDiff + zDiff;
        }

        public Vector2 GetLastAttackPosition(Unit c, int xAttack, int zAttack)
        {
            for (int i = c.Stats.AttackRanges.Count - 1; i >= 0; i--) //Prioritize Range Attacks
                for (int j = LastPositions.Count - 1; j >= 0; j--)
                    if (GetDelta(LastPositions[j].Position, new Vector2(xAttack, zAttack)) == c.Stats.AttackRanges[i] &&
                        gridGameManager.GetSystem<MapSystem>().Tiles[(int)LastPositions[j].Position.x,
                            (int)LastPositions[j].Position.y].Unit == null)
                        return LastPositions[j].Position;

            return new Vector2(-1, -1);
        }

        public void CalculateMousePathToPosition(Unit character, int x, int y)
        {
            ResetDrag();
            var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                character.GridPosition.Y, x, y, character.Faction.Id, false, character.Stats.AttackRanges);
            if (p != null)
                for (int i = p.GetLength() - 2; i >= 0; i--)
                    dragPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            MovementPath = new List<Vector2>(dragPath);
            DrawMousePath();
        }
        private void DrawMousePath()
        {
            int startX = gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter.GridPosition.X;
            int startY = gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter.GridPosition.Y;
            playerInputFeedback.DrawMovementPath(dragPath, startX, startY);
        }
        
        public void CalculateMousePathToEnemy(Unit character, Vector2 position)
        {
            ResetDrag();
            var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                character.GridPosition.Y, (int)position.x, (int)position.y, character.Faction.Id, true,
                character.Stats.AttackRanges);
            if (p != null)
                for (int i = p.GetLength() - 2; i >= AttackRangeFromPath; i--)
                    dragPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            MovementPath = new List<Vector2>(dragPath);
        }


        public void CalculateMousePathToPosition(Unit character, BigTile position)
        {
            ResetDrag();
            var movementPath = gridGameManager.GetSystem<MoveSystem>().GetMonsterPath((Monster)character, position);
            if (movementPath != null)
            {
                Debug.Log(movementPath.GetLength());
                Debug.Log(movementPath.GetStep(0).GetX() + " " + movementPath.GetStep(0).GetY());
                for (int i = movementPath.GetLength() - 2; i >= 0; i--)
                    dragPath.Add(new Vector2(movementPath.GetStep(i).GetX(), movementPath.GetStep(i).GetY()));
            }

            Debug.Log("======");
            MovementPath = new List<Vector2>(dragPath);
            DrawMousePath();
        }

        public void CharacterDrag(int x, int y, Unit character)
        {
            if (!Active)
                return;
            if (gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter == null)
            {
                ResetDrag();
                return;
            }

            if (IsOutOfBounds(x, y)) //TODO: could be redundant since unitcontroller checks this already?
            {
                ResetDrag();
                return;
            }

            if (IsOldDrag(x, y)) return;



            var field = gridGameManager.GetSystem<MapSystem>().Tiles[x, y];
            if (field.IsActive && field.Unit == null)
            {
                LastPositions.Add(new CursorPosition(new Vector2(x, y), null));

                if (LastPositions.Count > character.Stats.Mov)
                    LastPositions.RemoveAt(0);
            }

            //Dragged on Enemy
            if (field.Unit != null && field.Unit.Faction.Id != character.Faction.Id)
                DraggedOnEnemy(x, y, field, field.Unit);
            //No enemy

            //If Field is Active and not the filed currently standing on
            if (!(x == character.GridPosition.X && y == character.GridPosition.Y) && field.IsActive
            ) //&&field.character==null)
            {
                DraggedOnActiveField(x, y, character);
            }
            else if (x == character.GridPosition.X && y == character.GridPosition.Y)
            {
                ResetDrag();
                DrawMousePath();
            }

            Finish(character, field, x, y);
        }

        private void CalculateMousePathToAttackField(Unit selectedCharacter, int x, int y)
        {
            var moveLocations = new List<Vector2>();

            GridGameManager.Instance.GetSystem<MapSystem>().GridLogic.GetMoveLocations(selectedCharacter.GridPosition.X,
                selectedCharacter.GridPosition.Y, moveLocations, selectedCharacter.Stats.Mov, 0,
                selectedCharacter.Faction.Id);
            moveLocations.Insert(0, new Vector2(selectedCharacter.GridPosition.X, selectedCharacter.GridPosition.Y));
            foreach (var loc in moveLocations)
            {
                var locX = (int)loc.x;
                var locY = (int)loc.y;
                var currentField = gridGameManager.GetSystem<MapSystem>().GetTileFromVector2(loc);
                Debug.Log(loc);
                foreach (int unused in selectedCharacter.Stats.AttackRanges)
                {
                    int delta = Mathf.Abs(locX - x) + Mathf.Abs(locY - y);
                    if (selectedCharacter.Stats.AttackRanges.Contains(delta))
                        if (currentField.Unit == null || currentField.Unit == selectedCharacter)
                        {
                            Debug.Log("AttackPosition found: " + locX + " " + locY);
                            CalculateMousePathToPosition(selectedCharacter, locX, locY);
                            return;
                        }
                }
            }

            Debug.Log("No AttackPosition Found!");
        }

        public void DraggedOnEnemy(int x, int y, Tile field, Unit enemy)
        {
            var selectedCharacter = gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            Debug.Log("draggedOnEnemy");
            if (!(selectedCharacter.GridPosition is BigTilePosition))
            {
                if (!FindObjectOfType<MapSystem>().GridLogic.IsFieldAttackable(x, y))
                    return;
                //CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
                if (dragPath == null || dragPath.Count == 0)
                {
                    Debug.Log("Mousepath empty");
                    //CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
                    CalculateMousePathToAttackField(selectedCharacter, x, y);
                }
                else
                {
                    var foundAttackPosition = false;
                    int attackPositionIndex = -1;
                    for (int i = dragPath.Count - 1; i >= 0; i--)
                    {
                        var lastMousePathPositionX = (int)dragPath[i].x;
                        var lastMousePathPositionY = (int)dragPath[i].y;
                        var lastMousePathField = gridGameManager.GetSystem<MapSystem>().GetTileFromVector2(dragPath[i]);
                        int delta = Mathf.Abs(lastMousePathPositionX - x) + Mathf.Abs(lastMousePathPositionY - y);
                        if (selectedCharacter.Stats.AttackRanges.Contains(delta))
                            if (lastMousePathField.Unit == null)
                            {
                                Debug.Log("Valid AttackPosition!");
                                attackPositionIndex = i;
                                foundAttackPosition = true;
                                break;
                            }

                    }

                    if (foundAttackPosition)
                    {
                        dragPath.RemoveRange(attackPositionIndex + 1, dragPath.Count - (attackPositionIndex + 1));
                    }
                    else
                    {
                        Debug.Log("No AttackPosition found!");
                        CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
                    }
                }

                if (dragPath != null && dragPath.Count <= selectedCharacter.Stats.Mov)
                {
                    DrawMousePath();
                    gridGameManager.GetSystem<UiSystem>()
                        .ShowAttackPreview(gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter, enemy);
                    gridGameManager.GetSystem<UiSystem>().ShowAttackableEnemy(x, y);
                }
                else
                {
                    Debug.Log("Not enough Movement!");
                }

                // DraggedOnAttackableField(selectedCharacter, x, y, field, enemy);
            }
        }
        public Vector2 GetCenterPos(Vector2 clickedPos)
        {
            int centerX = (int)Mathf.Round(clickedPos.x - MapSystem.GRID_X_OFFSET) - 1;
            int centerY = (int)Mathf.Round(clickedPos.y) - 1;
            return new Vector2(centerX, centerY);
        }
        public void DraggedOnActiveField(int x, int y, Unit character)
        {

            var selectedCharacter = gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            bool contains = dragPath.Contains(new Vector2(x, y));

            dragPath.Add(new Vector2(x, y));

            if (dragPath.Count > character.Stats.Mov || contains || Mathf.Abs(lastDragPosX - x) + Mathf.Abs(lastDragPosY - y) > 1)
            {
                dragPath.Clear();
                var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                    character.GridPosition.Y, x, y, character.Faction.Id, false, character.Stats.AttackRanges);
                if (p != null)
                    for (int i = p.GetLength() - 2; i >= 0; i--)
                        dragPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            }
            MovementPath = new List<Vector2>(dragPath);
            DrawMousePath();
        }

        public void Finish(Unit character, Tile field, int x, int y)
        {
            lastDragPosX = x;
            lastDragPosY = y;
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || x >= gridGameManager.GetSystem<MapSystem>().GridData.Width || y < 0 ||
                   y >= gridGameManager.GetSystem<MapSystem>().GridData.Height;
        }

        private void OnDestroy()
        {
            OnUnitClicked = null;
            OnDraggedOverUnit = null;
            OnStartDrag = null;
            OnUnitDragged = null;
            OnEndDrag = null;
            OnDragCanceled = null;
            OnDragReset = null;
            OnUnitDoubleClicked = null;
        }

        #region ClickEvents

        public delegate void OnUnitClickedEvent(Unit character);

        public static OnUnitClickedEvent OnUnitClicked;
        public static OnUnitClickedEvent OnUnitDoubleClicked;

        #endregion

        #region DragEvents

        public delegate void OnStartDragEvent(int gridX, int gridY);
        public static OnStartDragEvent OnStartDrag;

        public delegate void OnUnitDraggedEvent(int x, int y, Unit character);
        public static OnUnitDraggedEvent OnUnitDragged;

        public delegate void OnDraggedOverUnitEvent(Unit unit);
        public static OnDraggedOverUnitEvent OnDraggedOverUnit;

        public static Action OnEndDrag;

        public static Action OnDragCanceled;

        public static Action OnDragReset;

        public static Action<bool> OnSetActive;

        #endregion
    }
}