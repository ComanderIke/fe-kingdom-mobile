﻿using Assets.__2___Scripts.Mechanics;
using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.GameStates;
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.PathFinding;
using Assets.Scripts.Input;
using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorPosition
{
    public CursorPosition before;
    public Vector2 position;

    public CursorPosition(Vector2 position, CursorPosition before)
    {
        this.position = position;
        this.before = before;
    }
}
public class InputSystem : MonoBehaviour, EngineSystem {


    #region ClickEvents
    public delegate void OnEnemyClicked(Unit unit);
    public static OnEnemyClicked onEnemyClicked;

    public delegate void OnUnitClickedConfirmed(Unit unit, bool confirm);
    public static OnUnitClickedConfirmed onUnitClickedConfirmed;

    public delegate void OnClickedGrid(int x, int y, Vector2 clickedPos);
    public static OnClickedGrid onClickedGrid;

    public delegate void OnClickedField(int x, int y);
    public static OnClickedField onClickedField;

    public delegate void OnClickedMovableTile(Unit unit, int x, int y);
    public static OnClickedMovableTile onClickedMovableTile;

    public delegate void OnClickedMovableBigTile(Unit unit, BigTile position);
    public static OnClickedMovableBigTile onClickedMovableBigTile;

    public delegate void OnUnitClicked(Unit character);
    public static OnUnitClicked onUnitClicked;
    #endregion

    #region DragEvents
    public delegate void OnDraggedOverUnit(Unit unit);
    public static OnDraggedOverUnit onDraggedOverUnit;

    public delegate void OnStartDrag(int gridX, int gridY);
    public static OnStartDrag onStartDrag;

    public delegate void OnUnitDragged(int x, int y, Unit character);
    public static OnUnitDragged onUnitDragged;

    public delegate void OnEndDrag();
    public static OnEndDrag onEndDrag;

    public delegate void OnEndDragOverNothing();
    public static OnEndDragOverNothing onEndDragOverNothing;

    public delegate void OnEndDragOverUnit(Unit character);
    public static OnEndDragOverUnit onEndDragOverUnit;

    public delegate void OnEndDragOverGrid(int x, int y);
    public static OnEndDragOverGrid onEndDragOverGrid;
    #endregion

    MainScript mainScript;
    [HideInInspector]
    public bool active = false;
    RessourceScript ressources;
    private  RaycastHit hit;
    Transform gameWorld;
    GameObject moveCursor;
    GameObject moveCursorStart;
    public  GridInput gridInput;
    public RaycastManager raycastManager;
    public PreferedMovementPath preferedPath;
    int currentX = -1;
    int currentY = -1;
    int oldX = -1;
    int oldY = -1;
    List<Vector2> mousePath = new List<Vector2>();
    bool nonActive = false;
    public List<CursorPosition> lastPositions = new List<CursorPosition>();
    List<GameObject> dots = new List<GameObject>();
    [HideInInspector]
    public int AttackRangeFromPath;
    // Use this for initialization
    void Start () {
        currentX = -1;
        currentY = -1;
        oldX = -1;
        oldY = -1;
        mainScript = FindObjectOfType<MainScript>();
        // hit = new RaycastHit();
        gridInput = new GridInput();
        gameWorld = GameObject.FindGameObjectWithTag("World").transform;
        ressources = FindObjectOfType<RessourceScript>();
        
        raycastManager = new RaycastManager();
        InitEvents();
        //mouseCursor = GameObject.Find("MouseCursor");
    }
  
    // Update is called once per frame

    void Update () {
        if (!active)
            return;
        //ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            preferedPath.path = new List<Vector2>(mousePath);
            Vector2 gridPos = raycastManager.GetMousePositionOnGrid();
            hit = raycastManager.GetLatestHit();
            int x = (int)gridPos.x;
            int y = (int)gridPos.y;
            currentX = x;
            currentY = y;
            
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Grid")
                {
                    ResetMousePath();
                    onClickedGrid(x, y, hit.point);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            preferedPath.path = new List<Vector2>(mousePath);
            if (!gridInput.confirmClick)
            {
                ResetMousePath();
            }

        }

    }

    private void InitEvents()
    {
        onDraggedOverUnit += DraggedOver;
        onStartDrag += StartDrag;
        onUnitDragged += CharacterDrag;
        onClickedMovableTile += CalculateMousePathToPositon;
        onClickedMovableBigTile += CalculateMousePathToPositon;
        UnitActionSystem.onStartMovingUnit += DeActivate;
        UnitActionSystem.onStopMovingUnit += Activate;
        UnitActionSystem.onUnitMoveToEnemy += ResetMousePath;
        onEndDrag += EndDrag;
        UnitActionSystem.onDeselectCharacter += ResetMousePath;
        onUnitClicked += UnitClicked;
        onEnemyClicked += EnemyClicked;
        UISystem.onAttackUIVisible += UIActive;
        UISystem.onReactUIVisible += UIActive;

    }

    void Activate()
    {
        UIActive(false);
    }

    void DeActivate()
    {
        UIActive(true);
    }

    void UIActive(bool active)
    {
        this.active = !active;
    }

    void EndDrag()
    {
        Vector2 gridPos = raycastManager.GetMousePositionOnGrid();
        if (raycastManager.GetLatestHit().collider.gameObject.tag == "Grid")
        {
           onEndDragOverGrid((int)gridPos.x, (int)gridPos.y);
        }
        else if (raycastManager.GetLatestHit().collider.gameObject.GetComponent<UnitController>() != null)
        {
            Unit draggedOverUnit = raycastManager.GetLatestHit().collider.gameObject.GetComponent<UnitController>().Unit;
            onEndDragOverUnit(draggedOverUnit);
        }
        else
        {
            onEndDragOverNothing();
        }
        ResetMousePath();
    }

    void UnitClicked(Unit unit)
    {
        Debug.Log("Unit Clicked!");
        if (!active)
            return;
       
        if (gridInput.confirmClick && gridInput.clickedField == new Vector2(currentX, currentY))
        {
            Debug.Log("Unit Clicked Confirmed!");
            onUnitClickedConfirmed(unit,true);
        }
        else
        {
            Debug.Log("Unit Clicked not Confirmed!");
            if (mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter!=null && unit.Player.ID != mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter.Player.ID)
            {
                Debug.Log("Unit Clicked not allied!");
                gridInput.confirmClick = true;
                gridInput.clickedField = new Vector2(currentX, currentY);
            }
           
            onUnitClickedConfirmed(unit, false);
        }

    }

    public void EnemyClicked(Unit unit)
    {

        Debug.Log("Enemy clicked!");

        if (mainScript.GetSystem<MapSystem>().GridLogic.IsFieldAttackable(unit.GridPosition.x,unit.GridPosition.y))
            CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter, new Vector2(unit.GridPosition.x, unit.GridPosition.y));
        else
        {
            if(unit.GridPosition is BigTilePosition)
            {
                Vector2 bottomleft = ((BigTilePosition)unit.GridPosition).Position.BottomLeft();
                Vector2 bottomright = ((BigTilePosition)unit.GridPosition).Position.BottomRight();
                Vector2 topleft = ((BigTilePosition)unit.GridPosition).Position.TopLeft();
                Vector2 topright = ((BigTilePosition)unit.GridPosition).Position.TopRight();
                if (mainScript.GetSystem<MapSystem>().GridLogic.IsFieldAttackable((int)bottomleft.x, (int)bottomleft.y))
                    CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter, bottomleft);
                else if (mainScript.GetSystem<MapSystem>().GridLogic.IsFieldAttackable((int)bottomright.x, (int)bottomright.y))
                    CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter, bottomright);
                else if (mainScript.GetSystem<MapSystem>().GridLogic.IsFieldAttackable((int)topleft.x, (int)topleft.y))
                    CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter, topleft);
                else if (mainScript.GetSystem<MapSystem>().GridLogic.IsFieldAttackable((int)topright.x, (int)topright.y))
                    CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter, topright);
            }
            else
            {
                Debug.Log("Enemy not Attackable!");
                mainScript.GetSystem<UnitSelectionSystem>().DeselectActiveCharacter();
            }
        }

        DrawMousePath();
        DrawCrossHair(unit);
        if (mousePath.Count >= 1)
        {
            mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter.GameTransform.SetPosition((int)mousePath[mousePath.Count - 1].x, (int)mousePath[mousePath.Count - 1].y);
            mainScript.GetSystem<UISystem>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter, unit);
        }

    }

    public  void StartDrag(int gridX, int gridY)
    {
        currentX = gridX;
        currentY = gridY;
        preferedPath.path = new List<Vector2>(mousePath);
    }

    public void ResetAll()
    {
        gridInput.Reset();
        ResetMousePath();
    }
    public void ResetMousePath()
    {
        foreach (GameObject dot in dots)
        {
            GameObject.Destroy(dot);
        }
        //if (selectedCharacter != null)
        //    selectedCharacter.ResetPosition();
        dots.Clear();
        mousePath.Clear();
        oldX = -1;
        oldY= -1;
        mainScript.GetSystem<UISystem>().HideAttackPreview();
        
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        if (moveCursorStart != null)
            GameObject.Destroy(moveCursorStart);
        //FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
        //FindObjectOfType<UXRessources>().movementFlag.SetActive(false);
    }
    
    public  void DraggedOver(Unit character)
    {
        //if (unitSelectionManager.SelectedCharacter != null && unitSelectionManager.SelectedCharacter != Unit)
        //{
            //TODO: Show Attack Icon or something
        //}
    }
    public  void DraggedExit()
    {
        //TODO: Hide Attack Icon or something
    }
    public  bool isOldDrag(int x, int y)
    {
        
        return x == oldX && y == oldY;
    }
    public  int GetDelta(Vector2 v, Vector2 v2)
    {
        
        int xDiff = (int)Mathf.Abs(v.x - v2.x);
        int zDiff = (int)Mathf.Abs(v.y - v2.y);
        return xDiff + zDiff;
    }
    public  Vector2 GetLastAttackPosition(Unit c, int xAttack, int zAttack)
    {
        for (int i = c.Stats.AttackRanges.Count - 1; i >= 0; i--)//Priotize Range Attacks
        {
            for (int j = lastPositions.Count - 1; j >= 0; j--)
            {
            
                if (GetDelta(lastPositions[j].position, new Vector2(xAttack, zAttack))== c.Stats.AttackRanges[i]&&mainScript.GetSystem<MapSystem>().Tiles[(int)lastPositions[j].position.x, (int)lastPositions[j].position.y].character==null)
                {
                    return lastPositions[j].position;
                }
            }
        }
        return new Vector2(-1,-1);
    }
    public  void CalculateMousePathToPositon(Unit character, int x, int y)
    {
        ResetMousePath();
        MovementPath p = mainScript.GetSystem<MoveSystem>().getPath(character.GridPosition.x, character.GridPosition.y, x, y, character.Player.ID, false, character.Stats.AttackRanges);
        if (p != null)
        {
            for (int i = p.getLength() - 2; i >= 0; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
        DrawMousePath();
    }
    public  void CalculateMousePathToEnemy(Unit character, Vector2 position)
    {
        ResetMousePath();
        MovementPath p = mainScript.GetSystem<MoveSystem>().getPath(character.GridPosition.x, character.GridPosition.y, (int)position.x, (int)position.y, character.Player.ID, true, character.Stats.AttackRanges);
        if (p != null)
        {
            for (int i = p.getLength() - 2; i >= AttackRangeFromPath; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
    }
    public  void CalculateMousePathToÉnemy(Unit character, BigTile position)
    {
        ResetMousePath();
        Debug.Log("FUCKTESTfrom" + character.GridPosition.x + " " + character.GridPosition.y + " to " + position + " Player.number " + character.Player.ID + " " + character.Stats.AttackRanges[0]);
        MovementPath p = mainScript.GetSystem<MapSystem>().GridLogic.GetMonsterPath((Monster)character, position, true, character.Stats.AttackRanges);
        
        if (p != null)
        {
            for (int i = 0; i < p.getLength(); i++)
            {
                Debug.Log(p.getStep(i));
            }
            for (int i = p.getLength() - 2; i >= AttackRangeFromPath; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
    }
    public  void CalculateMousePathToPositon(Unit character, BigTile position)
    {
        ResetMousePath();
        MovementPath p = mainScript.GetSystem<MoveSystem>().GetMonsterPath((Monster)character, position);
        if (p != null)
        {
            Debug.Log(p.getLength());
            Debug.Log(p.getStep(0).getX()+" "+p.getStep(0).getY());
            for (int i = p.getLength() - 2; i >= 0; i--)
            {

                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
        Debug.Log("======");
        for (int i = 0; i < mousePath.Count; i++)
        {
            Debug.Log(mousePath[i]);
        }
        DrawMousePath();

    }
    public  void CharacterDrag(int x, int y, Unit character)
    {
        
        if (!active)
            return;
        if (mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter == null)
        {
            ResetMousePath();
            return;
        }
        if(IsOutOfBounds(x, y))
        {
            ResetMousePath();
            return;
        }
        if (isOldDrag(x, y))
        {
            return;
        }
        currentX = x;
        currentY = y;
        
        Tile field = mainScript.GetSystem<MapSystem>().Tiles[x, y];
        if (field.isActive && field.character == null)
        {
            lastPositions.Add(new CursorPosition(new Vector2(x, y),null));
            
            if (lastPositions.Count > character.Stats.MoveRange)
            {
                
                lastPositions.RemoveAt(0);
               // foreach (Vector2 v in lastPositions)
                   // Debug.Log("In List:" +v);
            }
        }
        //Dragged on Enemy
        if (field.character != null && field.character.Player.ID != character.Player.ID)
        {
            DraggedOnEnemy(x, y, field, field.character);
        }
        //No enemy
        else
        {

        }
        //If Field is Active and not the filed currently standing on
        if (!(x == character.GridPosition.x && y == character.GridPosition.y) &&field.isActive)//&&field.character==null)
        {
                DraggedOnActiveField(x,y,character);
        }
        else if(x == character.GridPosition.x && y == character.GridPosition.y)
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
       
        List<Vector2> moveLocations = new List<Vector2>();
        
        MainScript.instance.GetSystem<MapSystem>().GridLogic.GetMoveLocations(selectedCharacter.GridPosition.x,selectedCharacter.GridPosition.y, moveLocations, selectedCharacter.Stats.MoveRange,0,selectedCharacter.Player.ID);
        moveLocations.Insert(0,new Vector2(selectedCharacter.GridPosition.x, selectedCharacter.GridPosition.y));
        foreach (Vector2 loc in moveLocations)
        {
            int currentX = (int)loc.x;
            int currentY = (int)loc.y;
            Tile currentField = mainScript.GetSystem<MapSystem>().GetTileFromVector2(loc);
            Debug.Log(loc);
            foreach (int ar in selectedCharacter.Stats.AttackRanges)
            {
                int delta = Mathf.Abs(currentX - x) + Mathf.Abs(currentY - y);
                Debug.Log("Delta: " + delta);
                if (selectedCharacter.Stats.AttackRanges.Contains(delta))
                {
                    if (currentField.character == null|| currentField.character==selectedCharacter)
                    {
                        Debug.Log("Delta: " + delta );
                        Debug.Log("AttackPosition found: " +currentX +" "+ currentY);
                        CalculateMousePathToPositon(selectedCharacter, currentX, currentY);
                        return;
                    }
                }
            }
        }
        Debug.Log("No AttackPosition Found!");
    }
    public void DraggedOnEnemy(int x, int y, Tile field,Unit enemy)
    {
        
        Unit selectedCharacter = mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter;
        Debug.Log("draggedOnEnemy");
        if (selectedCharacter.GridPosition is BigTilePosition)
        {
            BigCharacterDraggedOnEnemy(selectedCharacter, x,y,enemy);
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
                bool foundAttackPosition = false;
                int attackPositionIndex = -1;
                for (int i = mousePath.Count - 1; i >= 0; i--)
                {
                    int lastMousePathPositionX = (int)mousePath[i].x;
                    int lastMousePathPositionY = (int)mousePath[i].y;
                    Tile lastMousePathField = mainScript.GetSystem<MapSystem>().GetTileFromVector2(mousePath[i]);
                    int delta = Mathf.Abs(lastMousePathPositionX - x) + Mathf.Abs(lastMousePathPositionY - y);
                    if (selectedCharacter.Stats.AttackRanges.Contains(delta))
                    {
                        if (lastMousePathField.character == null)
                        {
                            Debug.Log("Valid AttackPosition!");
                            attackPositionIndex = i;
                            foundAttackPosition = true;
                            break;
                        }
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
            if (mousePath.Count <= selectedCharacter.Stats.MoveRange)
            {
                DrawMousePath();
                mainScript.GetSystem<UISystem>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter, enemy);
            }
            else
            {
                Debug.Log("Not enough Movement!");
                return;
            }
            // DraggedOnAttackableField(selectedCharacter, x, y, field, enemy);
        }
        
    }
    private void DraggedOnAttackableField(Unit selectedCharacter, int x, int y, Tile field,Unit enemy)
    {
        throw new System.Exception("FUCK");
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
    private void DrawCrossHair(Unit character)
    {
        /*
        crosshair = GameObject.Instantiate(ressources.rangeAttackGO, gameWorld);
        crosshair.transform.localPosition = new Vector3(character.GameTransform.GameObject.transform.localPosition.x+0.5f, character.GameTransform.GameObject.transform.localPosition.y+0.5f, -1f);
        if (character.GridPosition is BigTilePosition)
        {
            Vector2 centerPos = ((BigTilePosition)character.GridPosition).Position.CenterPos();
            crosshair.transform.localPosition = new Vector3(centerPos.x, centerPos.y, -1f);
            //crosshair.transform.localScale = new Vector3(.3f, .3f, .3f);
        }
        else
            //crosshair.transform.localScale = new Vector3(.3f, .3f, .3f);

        crosshair.GetComponent<SpriteRenderer>().sprite = ressources.rangeAttackSprite;*/
    }

    public void DraggedOnActiveField(int x, int y, Unit character)
    {
        if (nonActive)
        {
            ResetMousePath();
        }
        Unit selectedCharacter = mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter;
        if (selectedCharacter is Monster)
        {
            raycastManager.GetMousePositionOnGrid();//just to shoot Ray
            hit=raycastManager.GetLatestHit();
            Vector2 centerPos= gridInput.GetCenterPos(hit.point);
            BigTile clickedBigTile = gridInput.GetClickedBigTile((int)centerPos.x, (int)centerPos.y, x, y);
            CalculateMousePathToPositon(selectedCharacter, clickedBigTile);
            DrawMousePath();
            return;
        }
        
        nonActive = false;
        bool contains = false;
        if (mousePath.Contains(new Vector2(x, y)))
        {
            contains = true;
        }
        // Debug.Log("ADD");
        mousePath.Add(new Vector2(x, y));
        //Debug.Log("WAHT");
        foreach (GameObject dot in dots)
        {
            GameObject.Destroy(dot);
        }
        dots.Clear();
        if (mousePath.Count > character.Stats.MoveRange || contains || (Mathf.Abs(oldX - x) + Mathf.Abs(oldY - y) > 1))
        {
            mousePath.Clear();
            MovementPath p = mainScript.GetSystem<MoveSystem>().getPath(character.GridPosition.x, character.GridPosition.y, x, y, character.Player.ID, false, character.Stats.AttackRanges);
            if (p != null)
            {
                for (int i = p.getLength() - 2; i >= 0; i--)
                {
                    mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
                }
            }
        }
        DrawMousePath();
    }
    public void DrawMousePath()
    {
        preferedPath.path = mousePath;
        foreach(GameObject dot in dots)
        {
            GameObject.Destroy(dot);
        }
        //Debug.Log("DrawMousePath");
        float startX = -1;
        float startY = -1;
        Unit selectedCharacter = mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter;
        if (selectedCharacter is Monster)
        {
            startX = ((BigTilePosition)selectedCharacter.GridPosition).Position.CenterPos().x;
            startY= ((BigTilePosition)selectedCharacter.GridPosition).Position.CenterPos().y;
        }
        else
        {
            startX = selectedCharacter.GridPosition.x;
            startY = selectedCharacter.GridPosition.y;
        }
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        if (moveCursorStart != null)
            GameObject.Destroy(moveCursorStart);
        if (mousePath.Count == 0)
        {
            moveCursor = GameObject.Instantiate(ressources.prefabs.moveCursor, gameWorld);
            moveCursor.transform.localPosition = new Vector3(selectedCharacter.GridPosition.x, selectedCharacter.GridPosition.y, moveCursor.transform.localPosition.z);
        }
        else
        {
            moveCursorStart = GameObject.Instantiate(ressources.prefabs.moveArrowDot, gameWorld);
            moveCursorStart.transform.localPosition = new Vector3(selectedCharacter.GridPosition.x + 0.5f, selectedCharacter.GridPosition.y + 0.5f, -0.03f);
            moveCursorStart.GetComponent<SpriteRenderer>().sprite = ressources.sprites.standOnArrowStart;
            Vector2 v = new Vector2(selectedCharacter.GridPosition.x, selectedCharacter.GridPosition.y);
            if (v.x - mousePath[0].x > 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 180);
            else if (v.x - mousePath[0].x < 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 0);
            else if (v.y - mousePath[0].y > 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 270);
            else if (v.y - mousePath[0].y < 0)
                moveCursorStart.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        for (int i=0; i < mousePath.Count; i++) 
        {
            Vector2 v = mousePath[i];
            
            GameObject dot = GameObject.Instantiate(ressources.prefabs.moveArrowDot,gameWorld);
            dot.transform.localPosition = new Vector3(v.x + 0.5f, v.y + 0.5f, -0.03f);
            dots.Add(dot);
            if (i == mousePath.Count - 1)
            {
                moveCursor = GameObject.Instantiate(ressources.prefabs.moveCursor, gameWorld);
                moveCursor.transform.localPosition = new Vector3(v.x,v.y, moveCursor.transform.localPosition.z);
                dot.GetComponent<SpriteRenderer>().sprite = ressources.sprites.moveArrowHead;
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
                    else if (v.x -startX < 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                    else if (v.y - startY > 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                    else if (v.y - startY < 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 90);
                }
            }
            else 
            {
                Vector2 vAfter = mousePath[i + 1];
                Vector2 vBefore;
                if (i != 0)
                {
                    vBefore = mousePath[i - 1];
                    ArrowCurve(dot,v, vBefore, vAfter);
                    
                }
                else
                {
                    vBefore = new Vector2(startX, startY);
                    ArrowCurve(dot, v, vBefore, vAfter);
                }
            }
        }
    }
    public void ArrowCurve(GameObject dot,Vector2 v, Vector2 vBefore, Vector2 vAfter)
    {
        if (vBefore.x == vAfter.x)
        {
            dot.GetComponent<SpriteRenderer>().sprite = ressources.sprites.moveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (vBefore.y == vAfter.y)
        {
            dot.GetComponent<SpriteRenderer>().sprite = ressources.sprites.moveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            dot.GetComponent<SpriteRenderer>().sprite = ressources.sprites.moveArrowCurve;
            if (vBefore.x - vAfter.x > 0)
            {
                if ((vBefore.y - vAfter.y > 0))
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
                if ((vBefore.y - vAfter.y > 0))
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
        nonActive = false;
        if (!field.isActive && !(x == character.GridPosition.x && y == character.GridPosition.y))
        {
            nonActive = true;
        }
    }
    private bool IsOutOfBounds(int x, int y)
    {
        return x < 0 || x >= mainScript.GetSystem<MapSystem>().grid.width || y < 0 || y >= mainScript.GetSystem<MapSystem>().grid.height;
    }
    void OnDestroy()
    {
        onEnemyClicked = null;
        onUnitClickedConfirmed = null;
        onClickedGrid = null;
        onClickedField = null;
        onClickedMovableTile = null;
        onClickedMovableBigTile = null;
        onUnitClicked = null;

        onDraggedOverUnit = null;
        onStartDrag = null;
        onUnitDragged = null;
        onEndDrag = null;
        onEndDragOverNothing = null;
        onEndDragOverUnit = null;
        onEndDragOverGrid = null;
    }
}

