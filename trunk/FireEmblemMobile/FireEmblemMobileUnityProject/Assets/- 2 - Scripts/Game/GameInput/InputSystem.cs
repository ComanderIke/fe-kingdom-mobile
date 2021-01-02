using Assets.Game.Manager;
using Assets.GameActors.Units;
using Assets.GameActors.Units.OnGameObject;
using Assets.GameEngine;
using Assets.Grid;
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
        #region Events
        public delegate void OnMovementPathUpdatedEvent(List<Vector2> mousePath, int startX, int startY);
        public static event OnMovementPathUpdatedEvent OnMovementPathUpdated;

        public static Action<bool, object> OnSetActive;
        public static event Action OnInputActivated;
        public static event Action OnInputDeactivated;

        public static event Action OnDragReset;
        public static event Action OnDragOverTile;
        public static event Action OnDraggedOnActiveField;
        #endregion

        public static bool Active { get; private set; }

        private readonly List<Vector2> dragPath = new List<Vector2>();          // tracks the path a unit is dragged along the grid.
        public List<CursorPosition> LastPositions = new List<CursorPosition>(); /* tracks the last positions the cursor was on,
                                                                                in order to determine the latest possible attack Position. */
        public List<Vector2> MovementPath = new List<Vector2>();                // stores the path a unit is moving along the grid.

        //[HideInInspector] public int AttackRangeFromPath;                       /* Very Complicated to Refactor! 
        //                                                                        This is used to store the Position from where a Unit will Attack.
        //                                                                        this changes based on AttackRange of the unit 
        //                                                                        and based on the prefered movementPath. */

        private GridGameManager gridGameManager;
        public RaycastManager RaycastManager;
        private GameplayInput gameplayInput;

        private List<object> inputActivationRequests = new List<object>();
        private object lastInputActivationCaller = null;
        private int lastDragPosX = -1;
        private int lastDragPosY = -1;
        private int selectedTileX = -1;
        private int selectedTileY = -1;
        private Unit confirmAttackTarget = null;                                /* stores the last attackPreview Target in order to 
                                                                                determine if the same target is clicked again, 
                                                                                which will result in an AttackAction */
        private void Awake()
        {
            
        }
        private Unit SelectedCharacter { get => gridGameManager.GetSystem<UnitSelectionSystem>().SelectedCharacter; }
        private void Start()
        {

            Active = true;
            gridGameManager = GridGameManager.Instance;
            gameplayInput = new GameplayInput();
            RaycastManager = new RaycastManager();
            InitEvents();
        }
        private void InitEvents()
        {
            OnSetActive += SetInputActive;

            UnitInputController.OnUnitClicked += UnitClicked;
            UnitInputController.OnUnitDoubleClicked += UnitDoubleClicked;
            UnitInputController.OnStartDrag += StartDrag;
            UnitInputController.OnDraggedOverUnit += DraggedOver;
            UnitInputController.OnEndDrag += DragEnded;
            UnitInputController.OnUnitDragged += CharacterDrag;
            UnitSelectionSystem.OnSelectedCharacter += OnSelectedCharacter;
            UnitSelectionSystem.OnSelectedInActiveCharacter += OnSelectedCharacter;

        }
        private void OnSelectedCharacter(Unit u)
        {
            ResetDrag();
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
            RaycastHit2D hit = RaycastManager.GetLatestHit();
            var x = (int)gridPos.x;
            var y = (int)gridPos.y;
            if( hit.collider != null && hit.collider.tag == TagManager.GridTag)
            {
                //ResetDrag();
                Debug.Log("Player Input: Grid clicked: " + x + " " + y);
                GridClicked(x, y);
            }
        }
        
        public void SetInputActive(bool active, object caller)
        {
            bool oldActive = Active;
            bool contains = false;
            foreach(object obj in inputActivationRequests)
            {
                if(obj == caller)
                {
                    contains = true;
                    break;
                }
            }
            if(active && contains)
                inputActivationRequests.Remove(caller);
            if (!contains&&!active)
                inputActivationRequests.Add(caller);

            Active = inputActivationRequests.Count == 0;
            //Debug.Log("Input: "+Active+ " "+inputActivationRequests.Count+ " "+caller.ToString());
            if (Active != oldActive)
            {
                if (Active)
                {
                    OnInputActivated?.Invoke();
                    //Debug.Log("Input Activated!");
                }
                else
                {
                    OnInputDeactivated?.Invoke();
                    //Debug.Log("Input Deactivated!");
                }
            }
        }

        private void DragEnded()
        {
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            if (RaycastManager.ConnectedLatestHit())
            {
                var latestHit = RaycastManager.GetLatestHit();
                if (latestHit.collider.gameObject.CompareTag(TagManager.GridTag)) //Dragged on Grid
                {
                    UnitEndDragOnGrid((int)gridPos.x, (int)gridPos.y);
                }
                else if (latestHit.collider.gameObject.CompareTag(TagManager.UnitTag))//Dragged on Unit
                {
                    var draggedOverUnit = latestHit.collider.gameObject.GetComponent<UnitInputController>().Unit;
                    UnitEndDragOnUnit(draggedOverUnit);
                }
                else
                {
                    gameplayInput.DeselectUnit();
                }
            }
            else
            {
                gameplayInput.DeselectUnit();
            }
            ResetDrag();
        }

        private void UnitDoubleClicked(Unit unit)
        {
            //Debug.Log("Player Input: Unit double clicked: " + unit.name);
            if (!Active)
                return;
            if(unit.Faction.Id== gridGameManager.FactionManager.ActivePlayerNumber)
                gameplayInput.Wait(unit);
            else//If enemy proceed as if it was a normal click
            {
                UnitClicked(unit);
            }
        }
        private void UnitClicked(Unit unit)
        {
            //Debug.Log("Player Input: Unit clicked: " +unit.name );
            if (!Active)
                return;
            if (unit.Faction.Id == gridGameManager.FactionManager.ActiveFaction.Id) // Player Unit Clicked
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
        private void EnemyClicked(Unit unit)
        {
            //Debug.Log("Enemy clicked!");
            if (SelectedCharacter == null)
            {
                gameplayInput.SelectUnit(unit);
            }
            else
            {
                if (gridGameManager.GetSystem<MapSystem>().GridLogic.IsFieldAttackable(unit.GridPosition.X, unit.GridPosition.Y))
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
                        else if (SelectedCharacter.GridPosition.CanAttack(SelectedCharacter.Stats.AttackRanges, unit.GridPosition))
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
                            CalculateMousePathToEnemy(SelectedCharacter, new Vector2(unit.GridPosition.X, unit.GridPosition.Y));
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
        public void GridClicked(int x, int y)
        {
            if (gridGameManager.GetSystem<MapSystem>().GridLogic.IsFieldFreeAndActive(x, y))
            {
                
                if(selectedTileX == x && selectedTileY == y)
                {
 
                    selectedTileX = -1;
                    selectedTileY = -1;
                    gameplayInput.MoveUnit(SelectedCharacter, new GridPosition(x, y), GridPosition.GetFromVectorList(MovementPath));
                    gameplayInput.ExecuteInputActions(() => gridGameManager.GameStateManager.SwitchState(GameStateManager.GameplayState));
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
                    ResetDrag();
                    gameplayInput.DeselectUnit();
                    
                }
                else
                {
                    ResetDrag();
                    gameplayInput.DeselectUnit();
                }
            }
            else
            {
                ResetDrag();
                gameplayInput.DeselectUnit();
            }
        }

        private void UnitEndDragOnGrid(int x, int y)
        {
            selectedTileX = x;
            selectedTileY = y;
            GridClicked(x, y);
        }

        private void UnitEndDragOnUnit(Unit draggedOverUnit)
        {
            //Debug.Log("Dragged ended on unit: " + draggedOverUnit.name);
            if (draggedOverUnit.Faction.Id != gridGameManager.FactionManager.ActiveFaction.Id)//Enemy
            {
                if (gridGameManager.GetSystem<Map.MapSystem>().GridLogic.IsFieldAttackable(draggedOverUnit.GridPosition.X,
                    draggedOverUnit.GridPosition.Y))
                {
                    
                    AttackEnemy(SelectedCharacter, draggedOverUnit, MovementPath);
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
        private void AttackEnemy(Unit character, Unit enemy, List<Vector2> movePath)
        {
            character.ResetPosition();
            //ResetDrag();
            gridGameManager.GetSystem<Map.MapSystem>().HideMovementRangeOnGrid();
            /* Enemy is in attackRange already */
            if ((movePath == null || movePath.Count == 0) &&
                character.GridPosition.CanAttack(character.Stats.AttackRanges, enemy.GridPosition))
            {

                //Debug.Log("Enemy is in Range:");
                gameplayInput.AttackUnit(character, enemy);
                gameplayInput.ExecuteInputActions(() => gridGameManager.GameStateManager.SwitchState(GameStateManager.GameplayState));
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
                gameplayInput.ExecuteInputActions(() => gridGameManager.GameStateManager.SwitchState(GameStateManager.GameplayState));
                
                //AttackRangeFromPath = 0;//No Idea why this is here!
            }
        }
      
        public void ResetDrag()
        {
            dragPath.Clear();
            MovementPath.Clear();
            lastDragPosX = -1;
            lastDragPosY = -1;
            selectedTileX = -1;
            selectedTileY = -1;
            confirmAttackTarget = null;
            OnDragReset?.Invoke();
        }

        public void StartDrag(int gridX, int gridY)
        {
            //TODO UserFeedback?
        }
        public void DraggedOver(Unit character)
        {
           //TODO: Show Attack Icon or something
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
            UpdatedMovementPath();
        }
        private void UpdatedMovementPath()
        {
            int startX = SelectedCharacter.GridPosition.X;
            int startY = SelectedCharacter.GridPosition.Y;
            OnMovementPathUpdated?.Invoke(MovementPath, startX, startY);
        }
        /* Calculates a Path from one character to another including Attack Range */
        public void CalculateMousePathToEnemy(Unit character, Vector2 position)
        {
            ResetDrag();
            var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                character.GridPosition.Y, (int)position.x, (int)position.y, character.Faction.Id, true,
                character.Stats.AttackRanges);
            MovementPath = new List<Vector2>();
            p.Reverse();
            for (int i = 1; i < p.GetLength(); i++)
            {
                //Debug.Log(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
                MovementPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            }
            //Debug.Log(AttackRangeFromPath);
            //if (p != null)
            //    for (int i = p.GetLength() - 2; i >= AttackRangeFromPath; i--)
            //        dragPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            //MovementPath = new List<Vector2>(p);
            //PrintMovementPath();
            UpdatedMovementPath();
        }
        private void PrintMovementPath()
        {
            Debug.Log("Movement Path: ");
            for(int i= 0; i< MovementPath.Count; i++)
            {
                Debug.Log(MovementPath[i]);
            }
        }
        /* Calculates a Path from one character to a position that is in attackRange from x and y*/
        /* Not sure if better then CalculateMousePathToEnemy. Probably not needed */
        /*private void CalculateMousePathToAttackField(Unit unit, int x, int y)
        {
            var moveLocations = new List<Vector2>();

            GridGameManager.Instance.GetSystem<MapSystem>().GridLogic.GetMoveLocations(unit.GridPosition.X,
                unit.GridPosition.Y, moveLocations, unit.Stats.Mov, 0,
                unit.Faction.Id);
            moveLocations.Insert(0, new Vector2(unit.GridPosition.X, unit.GridPosition.Y));
            foreach (var loc in moveLocations)
            {
                var locX = (int)loc.x;
                var locY = (int)loc.y;
                var currentField = gridGameManager.GetSystem<MapSystem>().GetTileFromVector2(loc);
                Debug.Log(loc);
                foreach (int unused in unit.Stats.AttackRanges)
                {
                    int delta = Mathf.Abs(locX - x) + Mathf.Abs(locY - y);
                    if (unit.Stats.AttackRanges.Contains(delta))
                        if (currentField.Unit == null || currentField.Unit == unit)
                        {
                            Debug.Log("AttackPosition found: " + locX + " " + locY);
                            CalculateMousePathToPosition(unit, locX, locY);
                            return;
                        }
                }
            }
            Debug.Log("No AttackPosition Found!");
        }*/
        public void CharacterDrag(int x, int y, Unit character)
        {

            if (!Active || IsOldDrag(x, y))
                return;
            lastDragPosX = x;
            lastDragPosY = y;
            if (SelectedCharacter == null|| IsOutOfBounds(x, y))
            {
                ResetDrag();
                return;
            }

            OnDragOverTile?.Invoke();
           
            /* store latest correct Positions */
            var field = gridGameManager.GetSystem<MapSystem>().Tiles[x, y];
            if (field.IsActive && field.Unit == null)
            {
                LastPositions.Add(new CursorPosition(new Vector2(x, y), null));

                if (LastPositions.Count > character.Ap)
                    LastPositions.RemoveAt(0);
            }
            
            
            //Dragged on Enemy
            if (field.Unit != null && field.Unit.Faction.Id != character.Faction.Id)
                DraggedOnEnemy(x, y, field.Unit);

            
            //If Field is Active and not the field currently standing on
            if (!(x == character.GridPosition.X && y == character.GridPosition.Y) && field.IsActive)
            {
                DraggedOnActiveField(x, y, character);
            }
            else if (x == character.GridPosition.X && y == character.GridPosition.Y)
            {
                /*Dragged On StartPosition*/
                ResetDrag();
            }
            UpdatedMovementPath();
            lastDragPosX = x;
            lastDragPosY = y;
        }

        public void DraggedOnEnemy(int x, int y, Unit enemy)
        {
            
            //Debug.Log("Dragged on enemy: " + enemy.name +" at ["+x+"/"+y+"]");
            if (!gridGameManager.GetSystem<MapSystem>().GridLogic.IsFieldAttackable(x, y))
                return;
            //CalculateMousePathToEnemy(selectedCharacter, new Vector2(x, y));
            if (MovementPath == null || MovementPath.Count == 0)
            {
                //Debug.Log("Mousepath empty");
                if (SelectedCharacter.GridPosition.CanAttack(SelectedCharacter.Stats.AttackRanges, enemy.GridPosition))
                {
                    gameplayInput.CheckAttackPreview(SelectedCharacter, enemy, SelectedCharacter.GridPosition);
                }
                else
                {
                    CalculateMousePathToEnemy(SelectedCharacter, new Vector2(x, y));
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
                    var lastMousePathField = gridGameManager.GetSystem<MapSystem>().GetTileFromVector2(MovementPath[i]);
                    int delta = Mathf.Abs(lastMousePathPositionX - x) + Mathf.Abs(lastMousePathPositionY - y);
                    if (SelectedCharacter.Stats.AttackRanges.Contains(delta))
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

                    int delta = Mathf.Abs(SelectedCharacter.GridPosition.X - x) + Mathf.Abs(SelectedCharacter.GridPosition.Y - y);
                    if (SelectedCharacter.Stats.AttackRanges.Contains(delta))
                    {
                        //Debug.Log("HERe" + delta);
                        gameplayInput.CheckAttackPreview(SelectedCharacter, enemy, SelectedCharacter.GridPosition);
                        MovementPath.Clear();
                        UpdatedMovementPath();
                    }
                    else
                    {
                        //Debug.Log("No suitable AttackPosition found in dragPath!");
                        CalculateMousePathToEnemy(SelectedCharacter, new Vector2(x, y));
                    }
                }
            }

            if (MovementPath != null && MovementPath.Count <= SelectedCharacter.Ap)
            {
                UpdatedMovementPath();
                gameplayInput.CheckAttackPreview(SelectedCharacter, enemy, new GridPosition(x, y));
            }
            else
            {
                //Debug.Log("Not enough Movement!");
            }
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
        public void DraggedOnActiveField(int x, int y, Unit character)
        {
            bool contains = dragPath.Contains(new Vector2(x, y));

            dragPath.Add(new Vector2(x, y));
            
            //Create new MovementPath if dragPath is to long or other conditions apply
            if (dragPath.Count > character.Ap || contains || IsLastActiveFieldAdjacent(x,y,character))
            {
                dragPath.Clear();
                var p = gridGameManager.GetSystem<MoveSystem>().GetPath(character.GridPosition.X,
                    character.GridPosition.Y, x, y, character.Faction.Id, false, character.Stats.AttackRanges);
                if (p != null)
                    for (int i = p.GetLength() - 2; i >= 0; i--)
                        dragPath.Add(new Vector2(p.GetStep(i).GetX(), p.GetStep(i).GetY()));
            }
            MovementPath = new List<Vector2>(dragPath);
            OnDraggedOnActiveField?.Invoke();
          
        }

        public Vector2 GetCenterPos(Vector2 clickedPos)
        {
            int centerX = (int)Mathf.Round(clickedPos.x - MapSystem.GRID_X_OFFSET) - 1;
            int centerY = (int)Mathf.Round(clickedPos.y) - 1;
            return new Vector2(centerX, centerY);
        }

        private bool IsOutOfBounds(int x, int y)
        {
            return x < 0 || x >= gridGameManager.GetSystem<MapSystem>().GridData.Width || y < 0 ||
                   y >= gridGameManager.GetSystem<MapSystem>().GridData.Height;
        }

        private void OnDestroy()
        {
            OnMovementPathUpdated = null;
            OnSetActive = null;
            OnDragReset = null;
        }
    }
}