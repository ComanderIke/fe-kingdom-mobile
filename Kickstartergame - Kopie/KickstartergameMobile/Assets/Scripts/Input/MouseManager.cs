using Assets.Scripts.Characters;
using Assets.Scripts.Engine;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.PathFinding;
using Assets.Scripts.Input;
using Assets.Scripts.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
public class MouseManager : MonoBehaviour, EngineSystem {

    MainScript mainScript;
    public  bool active = true;
    UXRessources ressources;
    private  RaycastHit hit;
    Transform gameWorld;
    GameObject moveCursor;
    GameObject moveCursorStart;
    public  GridInput gridInput;
    public RaycastManager raycastManager;
    public PreferedMovementPath preferedPath;
    public int currentX = -1;
    public int currentY = -1;
    public int oldX = -1;
    public int oldY = -1;
    public List<Vector2> mousePath = new List<Vector2>();
    bool nonActive = false;
    public List<CursorPosition> lastPositions = new List<CursorPosition>();
    List<GameObject> dots = new List<GameObject>();
    GameObject crosshair;
    // Use this for initialization
    void Start () {
        mainScript = FindObjectOfType<MainScript>();
        // hit = new RaycastHit();
        gridInput = new GridInput();
        gameWorld = GameObject.FindGameObjectWithTag("World").transform;
        ressources = FindObjectOfType<UXRessources>();
        
        raycastManager = new RaycastManager();
        InitEvents();
        //mouseCursor = GameObject.Find("MouseCursor");
    }
    float updateFrequency = 0.1f;
    float updateTime = 0;
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
                    EventContainer.clickedOnGrid(x, y, hit.point);
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
        EventContainer.draggedOverUnit += DraggedOver;
        EventContainer.startDrag += StartDrag;
        EventContainer.unitDragged += CharacterDrag;
        EventContainer.unitClickedOnActiveTile += CalculateMousePathToPositon;
        EventContainer.monsterClickedOnActiveBigTile += CalculateMousePathToPositon;
        EventContainer.startMovingUnit += DeActivate;
        EventContainer.stopMovingUnit += Activate;
        EventContainer.unitMoveToEnemy += ResetMousePath;
        EventContainer.endDrag += EndDrag;
        EventContainer.deselectActiveCharacter += ResetMousePath;
        EventContainer.unitClicked += UnitClicked;
        EventContainer.enemyClicked += EnemyClicked;
        EventContainer.attackUIVisible += UIActive;
        EventContainer.reactUIVisible += UIActive;

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
           EventContainer.endDragOverGrid((int)gridPos.x, (int)gridPos.y);
        }
        else if (raycastManager.GetLatestHit().collider.gameObject.GetComponent<UnitController>() != null)
        {
            LivingObject draggedOverUnit = raycastManager.GetLatestHit().collider.gameObject.GetComponent<UnitController>().Unit;
            EventContainer.endDragOverUnit(draggedOverUnit);
        }
        else
        {
            EventContainer.endDragOverNothing();
        }
        ResetMousePath();
    }

    void UnitClicked(LivingObject unit)
    {
        if (!active)
            return;
        if (gridInput.confirmClick && gridInput.clickedField == new Vector2(currentX, currentY))
        {
            EventContainer.unitClickedConfirmed(unit,true);
        }
        else
        {
            gridInput.confirmClick = true;
            gridInput.clickedField = new Vector2(currentX, currentY);
           
            EventContainer.unitClickedConfirmed(unit, false);
        }

    }

    public void EnemyClicked(LivingObject unit)
    {
        if(mainScript.gridManager.GridLogic.IsFieldAttackable(currentX,currentY))
            CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, new Vector2(currentX, currentY));
        else
        {
            if(unit is Monster)
            {
                Vector2 bottomleft = ((BigTilePosition)unit.GridPosition).Position.BottomLeft();
                Vector2 bottomright = ((BigTilePosition)unit.GridPosition).Position.BottomRight();
                Vector2 topleft = ((BigTilePosition)unit.GridPosition).Position.TopLeft();
                Vector2 topright = ((BigTilePosition)unit.GridPosition).Position.TopRight();
                if (mainScript.gridManager.GridLogic.IsFieldAttackable((int)bottomleft.x, (int)bottomleft.y))
                    CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, bottomleft);
                else if (mainScript.gridManager.GridLogic.IsFieldAttackable((int)bottomright.x, (int)bottomright.y))
                    CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, bottomright);
                else if (mainScript.gridManager.GridLogic.IsFieldAttackable((int)topleft.x, (int)topleft.y))
                    CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, topleft);
                else if (mainScript.gridManager.GridLogic.IsFieldAttackable((int)topright.x, (int)topright.y))
                    CalculateMousePathToEnemy(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, topright);
            }
            
        }

        DrawMousePath();
        DrawCrossHair(unit);
        if (unit.GridPosition is BigTilePosition)
        {
            mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter,unit, ((BigTilePosition)unit.GridPosition).Position.CenterPos());
        }
        else
        {
            mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter,unit, new Vector2(unit.GridPosition.x, unit.GridPosition.y));
        }
    }

    public  void StartDrag(int gridX, int gridY)
    {
        currentX = gridX;
        currentY = gridY;
        preferedPath.path = new List<Vector2>(mousePath);
    }


    public  void ResetMousePath()
    {
        foreach (GameObject dot in dots)
        {
            GameObject.Destroy(dot);
        }
        LivingObject selectedCharacter = mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter;
        //if (selectedCharacter != null)
        //    selectedCharacter.ResetPosition();
        if (crosshair)
            Destroy(crosshair);
        dots.Clear();
        mousePath.Clear();
        oldX = -1;
        oldY= -1;
        mainScript.GetController<UIController>().HideAttackPreview();
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        if (moveCursorStart != null)
            GameObject.Destroy(moveCursorStart);
        FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
        //FindObjectOfType<UXRessources>().movementFlag.SetActive(false);
    }
    
    public  void DraggedOver(LivingObject character)
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
    public  Vector2 GetLastAttackPosition(LivingObject c, int xAttack, int zAttack)
    {
        for (int i = c.Stats.AttackRanges.Count - 1; i >= 0; i--)//Priotize Range Attacks
        {
            for (int j = lastPositions.Count - 1; j >= 0; j--)
            {
            
                if (GetDelta(lastPositions[j].position, new Vector2(xAttack, zAttack))== c.Stats.AttackRanges[i]&&mainScript.gridManager.Tiles[(int)lastPositions[j].position.x, (int)lastPositions[j].position.y].character==null)
                {
                    return lastPositions[j].position;
                }
            }
        }
        return new Vector2(-1,-1);
    }
    public  void CalculateMousePathToPositon(LivingObject character, int x, int y)
    {
        ResetMousePath();
        MovementPath p = mainScript.gridManager.GridLogic.getPath(character.GridPosition.x, character.GridPosition.y, x, y, character.Player.ID, false, character.Stats.AttackRanges);
        if (p != null)
        {
            for (int i = p.getLength() - 2; i >= 0; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
        DrawMousePath();
    }
    public  void CalculateMousePathToEnemy(LivingObject character, Vector2 position)
    {
        ResetMousePath();
        MovementPath p = mainScript.gridManager.GridLogic.getPath(character.GridPosition.x, character.GridPosition.y, (int)position.x, (int)position.y, character.Player.ID, true, character.Stats.AttackRanges);
        //for (int i = 0; i < p.getLength(); i++)
        //{
        //    Debug.Log(p.getStep(i));
        //}
        if (p != null)
        {
            for (int i = p.getLength() - 2; i >= mainScript.AttackRangeFromPath; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
    }
    public  void CalculateMousePathToÉnemy(LivingObject character, BigTile position)
    {
        ResetMousePath();
        Debug.Log("FUCKTESTfrom" + character.GridPosition.x + " " + character.GridPosition.y + " to " + position + " Player.number " + character.Player.ID + " " + character.Stats.AttackRanges[0]);
        MovementPath p = mainScript.gridManager.GridLogic.GetMonsterPath((Monster)character, position, true, character.Stats.AttackRanges);
        
        if (p != null)
        {
            for (int i = 0; i < p.getLength(); i++)
            {
                Debug.Log(p.getStep(i));
            }
            for (int i = p.getLength() - 2; i >= mainScript.AttackRangeFromPath; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
    }
    public  void CalculateMousePathToPositon(LivingObject character, BigTile position)
    {
        ResetMousePath();
        MovementPath p = mainScript.gridManager.GridLogic.GetMonsterPath((Monster)character, position);
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
    public  void CharacterDrag(int x, int y, LivingObject character)
    {
        
        if (!active)
            return;
        if (mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter == null)
        {
            ResetMousePath();
            return;
        }
        if (isOldDrag(x, y)|| IsOutOfBounds(x, y))
        {
            return;
        }
        currentX = x;
        currentY = y;
        
        Tile field = mainScript.gridManager.Tiles[x, y];
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
            FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().material.mainTexture = FindObjectOfType<TextureScript>().cursorTextures[0];
            if (field.isActive)
            {
                FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = true;
                FindObjectOfType<DragCursor>().transform.position = new Vector3(x + 0.5f, y + 0.5f,0);
            }
            else
            {
                FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
            }
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
    public void DraggedOnEnemy(int x, int y, Tile field,LivingObject character)
    {

        LivingObject selectedCharacter = mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter;
        if (selectedCharacter.GridPosition is BigTilePosition)
        {
            raycastManager.GetMousePositionOnGrid();
            hit=raycastManager.GetLatestHit();
            Vector2 centerPos = gridInput.GetCenterPos(hit.point);
            BigTile clickedBigTile = gridInput.GetClickedBigTile((int)centerPos.x, (int)centerPos.y, x, y);
            BigTile nearestBigTile = mainScript.gridManager.GridLogic.GetNearestBigTileFromEnemy(character);
            Debug.Log(nearestBigTile);
            CalculateMousePathToPositon(selectedCharacter, nearestBigTile);
            DrawMousePath();
            if(character.GridPosition is BigTilePosition)
            {
                mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter,character,((BigTilePosition)character.GridPosition).Position.CenterPos());
            }
            else
            {
                mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter,character,new Vector2(x, y));
            }
            
            return;
        }
        if (FindObjectOfType<GridManager>().GridLogic.IsFieldAttackable(x, y))
        {
            bool reset = true;
            for (int i = selectedCharacter.Stats.AttackRanges.Count - 1; i >= 0; i--)
            {
                //Debug.Log(i);
                int xDiff = (int)Mathf.Abs(selectedCharacter.GridPosition.x - field.character.GridPosition.x);
                int yDiff = (int)Mathf.Abs(selectedCharacter.GridPosition.y - field.character.GridPosition.y);
                if ((xDiff + yDiff) == selectedCharacter.Stats.AttackRanges[i])
                {
                    // CalculateMousePathToPositon(selectedCharacter, (int)v.x, (int)v.y);
                    ResetMousePath();
                    DrawCrossHair(character);
                    if (character.GridPosition is BigTilePosition)
                    {
                        mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, character, ((BigTilePosition)character.GridPosition).Position.CenterPos());
                    }
                    else
                    {
                        mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, character, new Vector2(x, y));
                    }
                    Finish(character, field, x, y);
                    return;
                }
                foreach (Vector2 v in mousePath)
                {
                    xDiff = (int)Mathf.Abs(v.x - field.character.GridPosition.x);
                    yDiff = (int)Mathf.Abs(v.y - field.character.GridPosition.y);
                    //Debug.Log(v + " "+xDiff + " "+ yDiff);
                    if ((xDiff + yDiff) == selectedCharacter.Stats.AttackRanges[i] && mainScript.gridManager.Tiles[(int)v.x, (int)v.y].isActive && mainScript.gridManager.Tiles[(int)v.x, (int)v.y].character == null)
                    {
                        CalculateMousePathToPositon(selectedCharacter, (int)v.x, (int)v.y);
                        DrawCrossHair(character);
                        if (character.GridPosition is BigTilePosition)
                        {
                            mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, character, ((BigTilePosition)character.GridPosition).Position.CenterPos());
                        }
                        else
                        {
                            mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter, character, new Vector2(x, y));
                        }
                        Finish(character, field, x, y);
                        return;
                    }
                }
                
            }
            if (character.GridPosition is BigTilePosition)
            {
                mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter,character,((BigTilePosition)character.GridPosition).Position.CenterPos());
            }
            else
            {
                mainScript.GetController<UIController>().ShowAttackPreview(mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter,character,new Vector2(x, y));
            }
        }
    }

    private void DrawCrossHair(LivingObject character)
    {
        
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

        crosshair.GetComponent<SpriteRenderer>().sprite = ressources.rangeAttackSprite;
    }

    public void DraggedOnActiveField(int x, int y, LivingObject character)
    {
        if (nonActive)
        {
            ResetMousePath();
        }
        LivingObject selectedCharacter = mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter;
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
            MovementPath p = mainScript.gridManager.GridLogic.getPath(character.GridPosition.x, character.GridPosition.y, x, y, character.Player.ID, false, character.Stats.AttackRanges);
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
        //Debug.Log("DrawMousePath");
        float startX = -1;
        float startY = -1;
        LivingObject selectedCharacter = mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter;
        if (selectedCharacter is Monster)
        {
            startX = ((BigTilePosition)selectedCharacter.GridPosition).Position.CenterPos().x;
            startY= ((BigTilePosition)selectedCharacter.GridPosition).Position.CenterPos().y;
        }
        else
        {
            startX= selectedCharacter.GridPosition.x;
            startY = selectedCharacter.GridPosition.y;
        }
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        if (moveCursorStart != null)
            GameObject.Destroy(moveCursorStart);
        if (mousePath.Count == 0)
        {
            moveCursor = GameObject.Instantiate(ressources.moveCursor, gameWorld);
            moveCursor.transform.localPosition = new Vector3(selectedCharacter.GridPosition.x, selectedCharacter.GridPosition.y, moveCursor.transform.localPosition.z);
        }
        else
        {
            moveCursorStart = GameObject.Instantiate(ressources.moveArrowDot, gameWorld);
            moveCursorStart.transform.localPosition = new Vector3(selectedCharacter.GridPosition.x + 0.5f, selectedCharacter.GridPosition.y + 0.5f, -0.03f);
            moveCursorStart.GetComponent<SpriteRenderer>().sprite = ressources.standOnArrowStart;
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
            
            GameObject dot = GameObject.Instantiate(ressources.moveArrowDot,gameWorld);
            dot.transform.localPosition = new Vector3(v.x + 0.5f, v.y + 0.5f, -0.03f);
            dots.Add(dot);
            if (i == mousePath.Count - 1)
            {
                moveCursor = GameObject.Instantiate(ressources.moveCursor, gameWorld);
                moveCursor.transform.localPosition = new Vector3(v.x,v.y, moveCursor.transform.localPosition.z);
                dot.GetComponent<SpriteRenderer>().sprite = ressources.moveArrowHead;
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
            dot.GetComponent<SpriteRenderer>().sprite = ressources.moveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (vBefore.y == vAfter.y)
        {
            dot.GetComponent<SpriteRenderer>().sprite = ressources.moveArrowStraight;
            dot.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            dot.GetComponent<SpriteRenderer>().sprite = ressources.moveArrowCurve;
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
    public void Finish(LivingObject character, Tile field, int x, int y)
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
        return x < 0 || x >= mainScript.gridManager.grid.width || y < 0 || y >= mainScript.gridManager.grid.height;
    }
}

