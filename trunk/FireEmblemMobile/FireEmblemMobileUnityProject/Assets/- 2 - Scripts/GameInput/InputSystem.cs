using Assets.Core;
using Assets.GameActors.Units;
using Assets.GameActors.Units.Monsters;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameResources;
using Assets.Grid;
using Assets.GUI;
using Assets.Map;
using Assets.Mechanics;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.GameInput
{
    public class InputSystem : MonoBehaviour, IEngineSystem
    {
        private readonly List<GameObject> dots = new List<GameObject>();
        private readonly List<Vector2> mousePath = new List<Vector2>();
        [HideInInspector] public bool Active;

        [HideInInspector] public int AttackRangeFromPath;
        private int currentX = -1;
        private int currentY = -1;
        private Transform gameWorld;
        public GridInput GridInput;
        private RaycastHit hit;
        public List<CursorPosition> LastPositions = new List<CursorPosition>();

        private GridGameManager gridGameManager;
        private GameObject moveCursor;
        private GameObject moveCursorStart;
        private bool nonActive;
        private int oldX = -1;
        private int oldY = -1;
        public PreferredMovementPath PreferredPath;
        public RaycastManager RaycastManager;
        private ResourceScript resources;

        private void Start()
        {
            currentX = -1;
            currentY = -1;
            oldX = -1;
            oldY = -1;
            gridGameManager = FindObjectOfType<GridGameManager>();

            GridInput = new GridInput();
            gameWorld = GameObject.FindGameObjectWithTag("World").transform;
            resources = FindObjectOfType<ResourceScript>();

            RaycastManager = new RaycastManager();
            InitEvents();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("CLICKED!");
            }
            if (!Active)
                return;
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                PreferredPath.Path = new List<Vector2>(mousePath);
                var gridPos = RaycastManager.GetMousePositionOnGrid();
                hit = RaycastManager.GetLatestHit();
                var x = (int)gridPos.x;
                var y = (int)gridPos.y;
                currentX = x;
                currentY = y;
                //Debug.Log("CLICKED ON: "+hit.collider.tag+" "+hit.collider.gameObject.transform.parent.name);
                if (hit.collider != null)
                    if (hit.collider.tag == "Grid")
                    {
                        ResetMousePath();
                        Debug.Log("Grid clicked:" + x + " " + y + " " + hit.point + " " + gridPos);
                        OnClickedGrid(x, y, hit.point);
                    }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                PreferredPath.Path = new List<Vector2>(mousePath);
                Debug.Log(mousePath.Count);
                if (!GridInput.ConfirmClick) ResetMousePath();
            }
        }

        private void InitEvents()
        {
            OnDraggedOverUnit += DraggedOver;
            OnStartDrag += StartDrag;
            OnUnitDragged += CharacterDrag;
            OnClickedMovableTile += CalculateMousePathToPosition;
            OnClickedMovableBigTile += CalculateMousePathToPosition;
            UnitActionSystem.OnStartMovingUnit += DeActivate;
            UnitActionSystem.OnStopMovingUnit += Activate;
            UnitActionSystem.OnUnitMoveToEnemy += ResetMousePath;
            OnEndDrag += EndDrag;
            UnitActionSystem.OnDeselectCharacter += ResetMousePath;
            OnUnitClicked += UnitClicked;
            OnEnemyClicked += EnemyClicked;
            UiSystem.OnAttackAnimationActive += SetInputActive;
        }

        private void Activate()
        {
            SetInputActive(false);
        }

        private void DeActivate()
        {
            SetInputActive(true);
        }

        private void SetInputActive(bool active)
        {
            Active = !active;
        }

        private void EndDrag()
        {
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            if (RaycastManager.ConnectedLatestHit())
            {
                var latestHit = RaycastManager.GetLatestHit();
                if (latestHit.collider.gameObject.CompareTag("Grid"))
                {
                    OnEndDragOverGrid((int)gridPos.x, (int)gridPos.y);
                }
                else if (latestHit.collider.gameObject.GetComponent<UnitController>() != null)
                {
                    var draggedOverUnit = latestHit.collider.gameObject.GetComponent<UnitController>().Unit;
                    OnEndDragOverUnit(draggedOverUnit);
                }
                else
                {
                    OnEndDragOverNothing();
                }
            }
            else
            {
                OnEndDragOverNothing();
            }

            ResetMousePath();
        }

        private void UnitClicked(Unit unit, bool doubleClicked = false)
        {
            Debug.Log(doubleClicked ? "Unit DoubleClicked!" : "Unit Clicked!");
            if (!Active)
                return;
            if (doubleClicked)
            {
                gridGameManager.GetSystem<UnitActionSystem>().ActiveCharWait();
                return;
            }
            if (GridInput.ConfirmClick && GridInput.ClickedField == new Vector2(currentX, currentY))
            {
                Debug.Log("Unit Clicked Confirmed!");
                OnUnitClickedConfirmed(unit, true);
            }
            else
            {
                Debug.Log("Unit Clicked not Confirmed!");
                if (gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter != null && unit.Faction.Id !=
                    gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter.Faction.Id)
                {
                    Debug.Log("Unit Clicked not allied!");
                    GridInput.ConfirmClick = true;
                    GridInput.ClickedField = new Vector2(currentX, currentY);
                }

                OnUnitClickedConfirmed(unit, false);
            }
        }

        public void EnemyClicked(Unit unit)
        {
            Debug.Log("Enemy clicked!");

            if (gridGameManager.GetSystem<MapSystem>().GridLogic.IsFieldAttackable(unit.GridPosition.X, unit.GridPosition.Y))
            {
                CalculateMousePathToEnemy(gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter,
                    new Vector2(unit.GridPosition.X, unit.GridPosition.Y));
            }
            else
            {
                if (unit.GridPosition is BigTilePosition)
                {
                    var bottomLeft = ((BigTilePosition)unit.GridPosition).Position.BottomLeft();
                    var bottomRight = ((BigTilePosition)unit.GridPosition).Position.BottomRight();
                    var topLeft = ((BigTilePosition)unit.GridPosition).Position.TopLeft();
                    var topRight = ((BigTilePosition)unit.GridPosition).Position.TopRight();
                    if (gridGameManager.GetSystem<MapSystem>().GridLogic
                        .IsFieldAttackable((int)bottomLeft.x, (int)bottomLeft.y))
                        CalculateMousePathToEnemy(gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter,
                            bottomLeft);
                    else if (gridGameManager.GetSystem<MapSystem>().GridLogic
                        .IsFieldAttackable((int)bottomRight.x, (int)bottomRight.y))
                        CalculateMousePathToEnemy(gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter,
                            bottomRight);
                    else if (gridGameManager.GetSystem<MapSystem>().GridLogic
                        .IsFieldAttackable((int)topLeft.x, (int)topLeft.y))
                        CalculateMousePathToEnemy(gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter,
                            topLeft);
                    else if (gridGameManager.GetSystem<MapSystem>().GridLogic
                        .IsFieldAttackable((int)topRight.x, (int)topRight.y))
                        CalculateMousePathToEnemy(gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter,
                            topRight);
                }
                else
                {
                    Debug.Log("Enemy not Attackable!");
                    gridGameManager.GetSystem<UnitSelectionSystem>().DeselectActiveCharacter();
                    return;
                }
            }

            DrawMousePath();
            if (mousePath.Count >= 1)
            {
                gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter.GameTransform
                    .SetPosition((int)mousePath[mousePath.Count - 1].x, (int)mousePath[mousePath.Count - 1].y);
                gridGameManager.GetSystem<UiSystem>()
                    .ShowAttackPreview(gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter, unit);
                gridGameManager.GetSystem<UiSystem>().ShowAttackableEnemy(unit.GridPosition.X, unit.GridPosition.Y);
            }
        }

        public void StartDrag(int gridX, int gridY)
        {
            currentX = gridX;
            currentY = gridY;
            PreferredPath.Path = new List<Vector2>(mousePath);
        }

        public void ResetAll()
        {
            GridInput.Reset();
            ResetMousePath();
        }

        public void ResetMousePath()
        {
            foreach (var dot in dots) Destroy(dot);

            dots.Clear();
            mousePath.Clear();
            oldX = -1;
            oldY = -1;
            gridGameManager.GetSystem<UiSystem>().HideAttackPreview();

            if (moveCursor != null)
                Destroy(moveCursor);
            if (moveCursorStart != null)
                Destroy(moveCursorStart);
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
            return x == oldX && y == oldY;
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
            ResetMousePath();
            var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                character.GridPosition.Y, x, y, character.Faction.Id, false, character.Stats.AttackRanges);
            if (p != null)
                for (int i = p.GetLength() - 2; i >= 0; i--)
                    mousePath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));

            DrawMousePath();
        }

        public void CalculateMousePathToEnemy(Unit character, Vector2 position)
        {
            ResetMousePath();
            var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                character.GridPosition.Y, (int)position.x, (int)position.y, character.Faction.Id, true,
                character.Stats.AttackRanges);
            if (p != null)
                for (int i = p.GetLength() - 2; i >= AttackRangeFromPath; i--)
                    mousePath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
        }

        public void CalculateMousePathToEnemy(Unit character, BigTile position)
        {
            ResetMousePath();
            var p = gridGameManager.GetSystem<MapSystem>().GridLogic
                .GetMonsterPath((Monster)character, position, true, character.Stats.AttackRanges);

            if (p != null)
            {
                for (var i = 0; i < p.GetLength(); i++) Debug.Log(p.GetStep(i));

                for (int i = p.GetLength() - 2; i >= AttackRangeFromPath; i--)
                    mousePath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            }
        }

        public void CalculateMousePathToPosition(Unit character, BigTile position)
        {
            ResetMousePath();
            var movementPath = gridGameManager.GetSystem<MoveSystem>().GetMonsterPath((Monster)character, position);
            if (movementPath != null)
            {
                Debug.Log(movementPath.GetLength());
                Debug.Log(movementPath.GetStep(0).GetX() + " " + movementPath.GetStep(0).GetY());
                for (int i = movementPath.GetLength() - 2; i >= 0; i--)
                    mousePath.Add(new Vector2(movementPath.GetStep(i).GetX(), movementPath.GetStep(i).GetY()));
            }

            Debug.Log("======");

            DrawMousePath();
        }

        public void CharacterDrag(int x, int y, Unit character)
        {
            if (!Active)
                return;
            if (gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter == null)
            {
                ResetMousePath();
                return;
            }

            if (IsOutOfBounds(x, y)) //TODO: could be redundant since unitcontroller checks this already?
            {
                ResetMousePath();
                return;
            }

            if (IsOldDrag(x, y)) return;

            currentX = x;
            currentY = y;

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
                ResetMousePath();
                DrawMousePath();

                nonActive = false;
            }

            Finish(character, field, x, y);
        }

        //TODO not used because no playable BigCharacters yet
        private void BigCharacterDraggedOnEnemy(Unit selectedCharacter, int x, int y, Unit enemy)
        {
            //raycastManager.GetMousePositionOnGrid();
            //hit = raycastManager.GetLatestHit();
            //Vector2 centerPos = gridInput.GetCenterPos(hit.point);
            //BigTile clickedBigTile = gridInput.GetClickedBigTile((int)centerPos.x, (int)centerPos.y, x, y);
            //BigTile nearestBigTile = mainScript.gridManager.GridLogic.GetNearestBigTileFromEnemy(enemy);
            //Debug.Log(nearestBigTile);
            //CalculateMousePathToPositon(selectedCharacter, nearestBigTile);
            //DrawMousePath();
            //mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, enemy);
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
                    Debug.Log("Delta: " + delta);
                    if (selectedCharacter.Stats.AttackRanges.Contains(delta))
                        if (currentField.Unit == null || currentField.Unit == selectedCharacter)
                        {
                            Debug.Log("Delta: " + delta);
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
            if (selectedCharacter.GridPosition is BigTilePosition)
            {
                BigCharacterDraggedOnEnemy(selectedCharacter, x, y, enemy);
            }
            else
            {
                if (!FindObjectOfType<MapSystem>().GridLogic.IsFieldAttackable(x, y))
                    return;
                //CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
                if (mousePath == null || mousePath.Count == 0)
                {
                    Debug.Log("Mousepath empty");
                    //CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
                    CalculateMousePathToAttackField(selectedCharacter, x, y);
                }
                else
                {
                    var foundAttackPosition = false;
                    int attackPositionIndex = -1;
                    for (int i = mousePath.Count - 1; i >= 0; i--)
                    {
                        var lastMousePathPositionX = (int)mousePath[i].x;
                        var lastMousePathPositionY = (int)mousePath[i].y;
                        var lastMousePathField = gridGameManager.GetSystem<MapSystem>().GetTileFromVector2(mousePath[i]);
                        int delta = Mathf.Abs(lastMousePathPositionX - x) + Mathf.Abs(lastMousePathPositionY - y);
                        if (selectedCharacter.Stats.AttackRanges.Contains(delta))
                            if (lastMousePathField.Unit == null)
                            {
                                Debug.Log("Valid AttackPosition!");
                                attackPositionIndex = i;
                                foundAttackPosition = true;
                                break;
                            }

                        //if (lastMousePathField.character != null)
                        //{
                        //    Debug.Log("last Mousepath not empty");
                        //    CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
                        //}
                        //else
                        //{
                        //    int delta = Mathf.Abs(lastMousePathPositionX - x) + Mathf.Abs(lastMousePathPositionY - y);
                        //    Debug.Log("Delta: " + delta + " " + x + " " + y + " " + lastMousePathPositionX + " " + lastMousePathPositionY);
                        //    if (!selectedCharacter.Stats.AttackRanges.Contains(delta))
                        //    {
                        //        Debug.Log("last Mousepath not in attackRange");
                        //        CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
                        //    }
                        //}
                    }

                    if (foundAttackPosition)
                    {
                        mousePath.RemoveRange(attackPositionIndex + 1, mousePath.Count - (attackPositionIndex + 1));
                    }
                    else
                    {
                        Debug.Log("No AttackPosition found!");
                        CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
                    }
                }

                if (mousePath != null && mousePath.Count <= selectedCharacter.Stats.Mov)
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

        private void DraggedOnAttackableField(Unit selectedCharacter, int x, int y, Tile field, Unit enemy)
        {
            throw new Exception("FUCK");
            //bool reset = true;
            //for (int i = selectedCharacter.Stats.AttackRanges.Count - 1; i >= 0; i--)
            //{
            //    //Debug.Log(i);
            //    int xDiff = (int)Mathf.Abs(selectedCharacter.GridPosition.x - field.character.GridPosition.x);
            //    int yDiff = (int)Mathf.Abs(selectedCharacter.GridPosition.y - field.character.GridPosition.y);
            //    if ((xDiff + yDiff) == selectedCharacter.Stats.AttackRanges[i])
            //    {
            //        // CalculateMousePathToPositon(selectedCharacter, (int)v.x, (int)v.y);
            //        ResetMousePath();

            //        mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, enemy);

            //        Finish(enemy, field, x, y);
            //        return;
            //    }
            //    foreach (Vector2 v in mousePath)
            //    {
            //        xDiff = (int)Mathf.Abs(v.x - field.character.GridPosition.x);
            //        yDiff = (int)Mathf.Abs(v.y - field.character.GridPosition.y);
            //        //Debug.Log(v + " "+xDiff + " "+ yDiff);
            //        if ((xDiff + yDiff) == selectedCharacter.Stats.AttackRanges[i] && mainScript.gridManager.Tiles[(int)v.x, (int)v.y].isActive && mainScript.gridManager.Tiles[(int)v.x, (int)v.y].character == null)
            //        {
            //            CalculateMousePathToPositon(selectedCharacter, (int)v.x, (int)v.y);
            //            mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, enemy);
            //            Finish(enemy, field, x, y);
            //            return;
            //        }
            //    }

            //}
            //mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, enemy);
        }

        public void DraggedOnActiveField(int x, int y, Unit character)
        {
            if (nonActive) ResetMousePath();

            var selectedCharacter = gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            if (selectedCharacter is Monster)
            {
                RaycastManager.GetMousePositionOnGrid(); //just to shoot Ray
                hit = RaycastManager.GetLatestHit();
                var centerPos = GridInput.GetCenterPos(hit.point);
                var clickedBigTile = GridInput.GetClickedBigTile((int)centerPos.x, (int)centerPos.y, x, y);
                CalculateMousePathToPosition(selectedCharacter, clickedBigTile);
                DrawMousePath();
                return;
            }

            nonActive = false;
            bool contains = mousePath.Contains(new Vector2(x, y));

            mousePath.Add(new Vector2(x, y));
            foreach (var dot in dots) Destroy(dot);

            dots.Clear();
            if (mousePath.Count > character.Stats.Mov || contains || Mathf.Abs(oldX - x) + Mathf.Abs(oldY - y) > 1)
            {
                mousePath.Clear();
                var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                    character.GridPosition.Y, x, y, character.Faction.Id, false, character.Stats.AttackRanges);
                if (p != null)
                    for (int i = p.GetLength() - 2; i >= 0; i--)
                        mousePath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            }

            DrawMousePath();
        }

        public void DrawMousePath()
        {
            PreferredPath.Path = new List<Vector2>(mousePath);
            foreach (var dot in dots) Destroy(dot);

            //Debug.Log("DrawMousePath");
            float startX = -1;
            float startY = -1;
            var selectedCharacter = gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter;
            if (selectedCharacter is Monster)
            {
                startX = ((BigTilePosition)selectedCharacter.GridPosition).Position.CenterPos().x;
                startY = ((BigTilePosition)selectedCharacter.GridPosition).Position.CenterPos().y;
            }
            else
            {
                startX = selectedCharacter.GridPosition.X;
                startY = selectedCharacter.GridPosition.Y;
            }

            if (moveCursor != null)
                Destroy(moveCursor);
            if (moveCursorStart != null)
                Destroy(moveCursorStart);
            if (mousePath.Count == 0)
            {
                moveCursor = Instantiate(resources.Prefabs.MoveCursor, gameWorld);
                moveCursor.transform.localPosition = new Vector3(selectedCharacter.GridPosition.X,
                    selectedCharacter.GridPosition.Y, moveCursor.transform.localPosition.z);
            }
            else
            {
                moveCursorStart = Instantiate(resources.Prefabs.MoveArrowDot, gameWorld);
                moveCursorStart.transform.localPosition = new Vector3(selectedCharacter.GridPosition.X + 0.5f,
                    selectedCharacter.GridPosition.Y + 0.5f, -0.03f);
                moveCursorStart.GetComponent<SpriteRenderer>().sprite = resources.Sprites.StandOnArrowStart;
                var v = new Vector2(selectedCharacter.GridPosition.X, selectedCharacter.GridPosition.Y);
                if (v.x - mousePath[0].x > 0)
                    moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 180);
                else if (v.x - mousePath[0].x < 0)
                    moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 0);
                else if (v.y - mousePath[0].y > 0)
                    moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 270);
                else if (v.y - mousePath[0].y < 0)
                    moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 90);
            }

            for (var i = 0; i < mousePath.Count; i++)
            {
                var v = mousePath[i];

                var dot = Instantiate(resources.Prefabs.MoveArrowDot, gameWorld);
                dot.transform.localPosition = new Vector3(v.x + 0.5f, v.y + 0.5f, -0.03f);
                dots.Add(dot);
                if (i == mousePath.Count - 1)
                {
                    moveCursor = Instantiate(resources.Prefabs.MoveCursor, gameWorld);
                    moveCursor.transform.localPosition = new Vector3(v.x, v.y, moveCursor.transform.localPosition.z);
                    dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowHead;
                    if (i != 0)
                    {
                        if (v.x - mousePath[i - 1].x > 0)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                        else if (v.x - mousePath[i - 1].x < 0)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                        else if (v.y - mousePath[i - 1].y > 0)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                        else if (v.y - mousePath[i - 1].y < 0)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                    else
                    {
                        if (v.x - startX > 0)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                        else if (v.x - startX < 0)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                        else if (v.y - startY > 0)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                        else if (v.y - startY < 0)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                }
                else
                {
                    var vAfter = mousePath[i + 1];
                    Vector2 vBefore;
                    if (i != 0)
                    {
                        vBefore = mousePath[i - 1];
                        ArrowCurve(dot, v, vBefore, vAfter);
                    }
                    else
                    {
                        vBefore = new Vector2(startX, startY);
                        ArrowCurve(dot, v, vBefore, vAfter);
                    }
                }
            }
        }

        public void ArrowCurve(GameObject dot, Vector2 v, Vector2 vBefore, Vector2 vAfter)
        {
            if (vBefore.x == vAfter.x)
            {
                dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowStraight;
                dot.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (vBefore.y == vAfter.y)
            {
                dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowStraight;
                dot.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                dot.GetComponent<SpriteRenderer>().sprite = resources.Sprites.MoveArrowCurve;
                if (vBefore.x - vAfter.x > 0)
                {
                    if (vBefore.y - vAfter.y > 0)
                    {
                        if (vBefore.x == v.x)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                        else
                            dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                    }

                    else
                    {
                        if (vBefore.x == v.x)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                        else
                            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                }
                else if (vBefore.x - vAfter.x < 0)
                {
                    if (vBefore.y - vAfter.y > 0)
                    {
                        if (vBefore.x == v.x)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                        else
                            dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                    }
                    else
                    {
                        if (vBefore.x == v.x)
                            dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                        else
                            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                    }
                }
            }
        }

        public void Finish(Unit character, Tile field, int x, int y)
        {
            oldX = x;
            oldY = y;
            nonActive = !field.IsActive && !(x == character.GridPosition.X && y == character.GridPosition.Y);
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || x >= gridGameManager.GetSystem<MapSystem>().GridData.Width || y < 0 ||
                   y >= gridGameManager.GetSystem<MapSystem>().GridData.Height;
        }

        private void OnDestroy()
        {
            OnEnemyClicked = null;
            OnUnitClickedConfirmed = null;
            OnClickedGrid = null;
            OnClickedField = null;
            OnClickedMovableTile = null;
            OnClickedMovableBigTile = null;
            OnUnitClicked = null;

            OnDraggedOverUnit = null;
            OnStartDrag = null;
            OnUnitDragged = null;
            OnEndDrag = null;
            OnEndDragOverNothing = null;
            OnEndDragOverUnit = null;
            OnEndDragOverGrid = null;
        }

        #region ClickEvents

        public delegate void OnEnemyClickedEvent(Unit unit);

        public static OnEnemyClickedEvent OnEnemyClicked;

        public delegate void OnUnitClickedConfirmedEvent(Unit unit, bool confirm);

        public static OnUnitClickedConfirmedEvent OnUnitClickedConfirmed;

        public delegate void OnClickedGridEvent(int x, int y, Vector2 clickedPos);

        public static OnClickedGridEvent OnClickedGrid;

        public delegate void OnClickedFieldEvent(int x, int y);

        public static OnClickedFieldEvent OnClickedField;

        public delegate void OnClickedMovableTileEvent(Unit unit, int x, int y);

        public static OnClickedMovableTileEvent OnClickedMovableTile;

        public delegate void OnClickedMovableBigTileEvent(Unit unit, BigTile position);

        public static OnClickedMovableBigTileEvent OnClickedMovableBigTile;

        public delegate void OnUnitClickedEvent(Unit character, bool doubleClick = false);

        public static OnUnitClickedEvent OnUnitClicked;

        #endregion

        #region DragEvents

        public delegate void OnDraggedOverUnitEvent(Unit unit);

        public static OnDraggedOverUnitEvent OnDraggedOverUnit;

        public delegate void OnStartDragEvent(int gridX, int gridY);

        public static OnStartDragEvent OnStartDrag;

        public delegate void OnUnitDraggedEvent(int x, int y, Unit character);

        public static OnUnitDraggedEvent OnUnitDragged;

        public delegate void OnEndDragEvent();

        public static OnEndDragEvent OnEndDrag;

        public delegate void OnEndDragOverNothingEvent();

        public static OnEndDragOverNothingEvent OnEndDragOverNothing;

        public delegate void OnEndDragOverUnitEvent(Unit character);

        public static OnEndDragOverUnitEvent OnEndDragOverUnit;

        public delegate void OnEndDragOverGridEvent(int x, int y);

        public static OnEndDragOverGridEvent OnEndDragOverGrid;

        #endregion
    }
}