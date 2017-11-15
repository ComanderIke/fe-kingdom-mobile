using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.PathFinding;
using Assets.Scripts.Input;
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
public class MouseManager : MonoBehaviour {

    static MainScript mainScript;
    public static bool active = true;
    static UXRessources ressources;
    private static RaycastHit hit;
    static Transform gameWorld;
    static GameObject moveCursor;
    public static GridInput gridInput;

    //GameObject mouseCursor;
    //private static Ray ray;
    public static RaycastManager raycastManager;
	// Use this for initialization
	void Start () {
        mainScript = FindObjectOfType<MainScript>();
        // hit = new RaycastHit();
        gridInput = new GridInput();
        gameWorld = GameObject.FindGameObjectWithTag("World").transform;
        ressources = FindObjectOfType<UXRessources>();
        EventContainer.draggedOverUnit += DraggedOver;
        EventContainer.startDrag += StartDrag;
        EventContainer.unitDragged += CharacterDrag;
        EventContainer.unitClickedOnActiveTile += CalculateMousePathToPositon;
        EventContainer.monsterClickedOnActiveBigTile += CalculateMousePathToPositon;
        raycastManager = new RaycastManager();
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
            oldMousePath = new List<Vector2>(mousePath);
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
            oldMousePath = new List<Vector2>(mousePath);
            if (!gridInput.confirmClick)
            {
                ResetMousePath();
            }

        }

    }

    public static void StartDrag(int gridX, int gridY)
    {
        currentX = gridX;
        currentY = gridY;
        oldMousePath = new List<Vector2>(mousePath);
    }
    public static int currentX = -1;
    public static int currentY = -1;
    public static int oldX = -1;
    public static int oldY=-1;
    public static List<Vector2> mousePath = new List<Vector2>();
    public static List<CursorPosition> lastPositions = new List<CursorPosition>();
    public static List<Vector2> oldMousePath = new List<Vector2>();
    static List<GameObject> dots = new List<GameObject>();
    public static void ResetMousePath()
    {
        //Debug.Log("ResetMousePath");
        foreach (GameObject dot in dots)
        {
            GameObject.Destroy(dot);
        }
        dots.Clear();
        mousePath.Clear();
        oldX = -1;
        oldY= -1;
        ressources.attackPreview.SetActive(false);
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
        //FindObjectOfType<UXRessources>().movementFlag.SetActive(false);
    }
    static bool nonActive = false;
    public static void DraggedOver(LivingObject character)
    {
        //if (unitSelectionManager.SelectedCharacter != null && unitSelectionManager.SelectedCharacter != Unit)
        //{
            //TODO: Show Attack Icon or something
        //}
    }
    public static void DraggedExit()
    {
        //TODO: Hide Attack Icon or something
    }
    public static bool isOldDrag(int x, int y)
    {
        
        return x == oldX && y == oldY;
    }
    public static int GetDelta(Vector2 v, Vector2 v2)
    {
        
        int xDiff = (int)Mathf.Abs(v.x - v2.x);
        int zDiff = (int)Mathf.Abs(v.y - v2.y);
        return xDiff + zDiff;
    }
    public static Vector2 GetLastAttackPosition(LivingObject c, int xAttack, int zAttack)
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
    public static void CalculateMousePathToPositon(LivingObject character, int x, int y)
    {
        ResetMousePath();
        MovementPath p = mainScript.gridManager.GridLogic.getPath(character.GridPosition.x, character.GridPosition.y, x, y, character.Player.ID, false, character.Stats.AttackRanges);
        for (int i = 0; i < p.getLength(); i++)
        {
            Debug.Log(p.getStep(i));
        }
        if (p != null)
        {
            for (int i = p.getLength() - 2; i >= 0; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
        DrawMousePath();
    }
    public static void CalculateMousePathToEnemy(LivingObject character, Vector2 position)
    {
        ResetMousePath();

        Debug.Log("from" + character.GridPosition.x + " " + character.GridPosition.y + " to " + position.x + " " + position.y + " Player.number " + character.Player.ID + " " + character.Stats.AttackRanges[0]);
        MovementPath p = mainScript.gridManager.GridLogic.getPath(character.GridPosition.x, character.GridPosition.y, (int)position.x, (int)position.y, character.Player.ID, true, character.Stats.AttackRanges);
        for (int i = 0; i < p.getLength(); i++)
        {
            Debug.Log(p.getStep(i));
        }
        if (p != null)
        {
            for (int i = p.getLength() - 2; i >= mainScript.AttackRangeFromPath; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
    }
    public static void CalculateMousePathToÉnemy(LivingObject character, BigTile position)
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
    public static void CalculateMousePathToPositon(LivingObject character, BigTile position)
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
    public static void CharacterDrag(int x, int y, LivingObject character)
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
            nonActive = false;
        }
        Finish(character, field, x, y);
        
    }
    public static void ShowAttackPreview(Vector2 pos)
    {
        Debug.Log("attackPreview " +pos.x+" "+ pos.y);
        ressources.attackPreview.SetActive(true);
        Vector3 attackPreviewPos = Camera.main.WorldToScreenPoint(new Vector3(pos.x + GridManager.GRID_X_OFFSET+0.5f, pos.y + 1.5f, -0.05f));
        Debug.Log("attackPreview " + attackPreviewPos.x + " " + attackPreviewPos.y);
        attackPreviewPos.z = 0;
        ressources.attackPreview.transform.position = attackPreviewPos;
    }
    public static void DraggedOnEnemy(int x, int y, Tile field,LivingObject character)
    {

        Debug.Log("ENEMY: " + character.Name);
        LivingObject selectedCharacter = mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter;
        if (selectedCharacter is Monster)
        {
            raycastManager.GetMousePositionOnGrid();
            hit=raycastManager.GetLatestHit();
            Vector2 centerPos = gridInput.GetCenterPos(hit.point);
            BigTile clickedBigTile = gridInput.GetClickedBigTile((int)centerPos.x, (int)centerPos.y, x, y);
            BigTile nearestBigTile = mainScript.gridManager.GridLogic.GetNearestBigTileFromEnemy(character);
            Debug.Log(nearestBigTile);
            CalculateMousePathToPositon(selectedCharacter, nearestBigTile);
            DrawMousePath();
            if(character is Monster)
            {
                ShowAttackPreview(((BigTilePosition)character.GridPosition).Position.CenterPos());
            }
            else
            {
                ShowAttackPreview(new Vector2(x, y));
            }
            
            return;
        }
        if (FindObjectOfType<GridManager>().GridLogic.IsFieldAttackable(x, y))
        {
            bool reset = true;
            for (int i = selectedCharacter.Stats.AttackRanges.Count - 1; i >= 0; i--)
            {

                foreach (Vector2 v in mousePath)
                {
                    int xDiff = (int)Mathf.Abs(v.x - field.character.GridPosition.x);
                    int yDiff = (int)Mathf.Abs(v.y - field.character.GridPosition.y);
                    if ((xDiff + yDiff) == selectedCharacter.Stats.AttackRanges[i] && mainScript.gridManager.Tiles[(int)v.x, (int)v.y].isActive && mainScript.gridManager.Tiles[(int)v.x, (int)v.y].character == null)
                    {
                        if (mousePath.IndexOf(v) + 1 < mousePath.Count && mousePath.Count - (mousePath.IndexOf(v) + 1) > 0)
                        {
                            Vector2 lastAttackPosition = GetLastAttackPosition(character, (int)field.character.GridPosition.x, (int)field.character.GridPosition.y);
                            Debug.Log(lastAttackPosition);
                            CalculateMousePathToPositon(character, (int)lastAttackPosition.x, (int)lastAttackPosition.y);
                            Finish(character, field, x, y);
                            return;
                            Debug.Log("Was ist das?" + v + " " + mousePath.IndexOf(v) + " " + (mousePath.Count - (mousePath.IndexOf(v) + 1)) + " " + mousePath.Count + "  " + field.character.GridPosition.x + " " + field.character.GridPosition.y);
                            reset = false;
                            mousePath.RemoveRange(mousePath.IndexOf(v) + 1, mousePath.Count - (mousePath.IndexOf(v) + 1));
                            i = -1;
                            foreach (GameObject dot in dots)
                            {
                                GameObject.Destroy(dot);
                            }
                            dots.Clear();
                            DrawMousePath();
                            break;
                        }
                        else
                        {
                            mousePath.Clear();
                            foreach (GameObject dot in dots)
                            {
                                GameObject.Destroy(dot);
                            }
                            dots.Clear();
                            DrawMousePath();
                            i = -1;
                            break;
                        }
                    }
                }
                int xDif = (int)Mathf.Abs(character.GridPosition.x - field.character.GridPosition.x);
                int yDif = (int)Mathf.Abs(character.GridPosition.y - field.character.GridPosition.y);

                if (i != -1 && (xDif + yDif) == selectedCharacter.Stats.AttackRanges[i])
                {
                    Debug.Log("2---Vom Stand aus in Range und mit MousePath nicht in Range!");
                    mousePath.Clear();
                    foreach (GameObject dot in dots)
                    {
                        GameObject.Destroy(dot);
                    }
                    dots.Clear();
                    DrawMousePath();
                    i = -1;
                    reset = false;
                }
            }

            //Diagonal
            if ((Mathf.Abs(oldX - x) + Mathf.Abs(oldY - y) > 1))
            {
                reset = true;
            }
            if (reset)
            {
                Debug.Log("Reset");
                bool flag = false;
                for (int i = selectedCharacter.Stats.AttackRanges.Count - 1; i >= 0; i--)
                {
                    foreach (Vector2 pos in mousePath)
                    {
                        int xDiff = (int)Mathf.Abs(pos.x - field.character.GridPosition.x);
                        int yDiff = (int)Mathf.Abs(pos.y - field.character.GridPosition.y);
                        if ((xDiff + yDiff) == selectedCharacter.Stats.AttackRanges[i] && mainScript.gridManager.Tiles[(int)pos.x, (int)pos.y].isActive && mainScript.gridManager.Tiles[(int)pos.x, (int)pos.y].character == null)
                        {
                            Debug.Log("Reset" + pos + " " + mousePath.IndexOf(pos) + " " + (mousePath.Count - (mousePath.IndexOf(pos) + 1)) + " " + mousePath.Count + "  " + character.GridPosition.x + " " + character.GridPosition.y);
                            if (mousePath.IndexOf(pos) + 1 < mousePath.Count && mousePath.Count - (mousePath.IndexOf(pos) + 1) > 0)
                                mousePath.RemoveRange(mousePath.IndexOf(pos) + 1, mousePath.Count - (mousePath.IndexOf(pos) + 1));
                            i = -1;
                            flag = true;
                            break;

                        }
                    }

                }
                if (!flag)
                {
                    int xDiff = (int)Mathf.Abs(character.GridPosition.x - field.character.GridPosition.x);
                    int yDiff = (int)Mathf.Abs(character.GridPosition.y - field.character.GridPosition.y);
                    for (int i = selectedCharacter.Stats.AttackRanges.Count - 1; i >= 0; i--)
                    {
                        if ((xDiff + yDiff) == selectedCharacter.Stats.AttackRanges[i])
                        {
                            Debug.Log("Vom Stand aus in Range und mit MousePath nicht in Range!");
                            flag = true;
                            mousePath.Clear();
                            break;
                        }
                    }
                }
                if (flag)
                {
                    foreach (GameObject dot in dots)
                    {
                        GameObject.Destroy(dot);
                    }
                    dots.Clear();
                    DrawMousePath();
                }
                else
                {
                    ResetMousePath();
                   // Debug.Log("from " +character.x + " " + character.y + " " + x + " " + y + " ");
                    MovementPath p = mainScript.gridManager.GridLogic.getPath(selectedCharacter.GridPosition.x, selectedCharacter.GridPosition.y, x, y, character.Player.ID, false, character.Stats.AttackRanges);
                    int removeFromPath = 1;
                    foreach (int attackRange in selectedCharacter.Stats.AttackRanges)
                    {
                        int xAttackRange = (int)p.getStep(p.getLength() - 1 - attackRange).getX();
                        int yAttackRange = (int)p.getStep(p.getLength() - 1 - attackRange).getY();
                        if (mainScript.gridManager.Tiles[xAttackRange, yAttackRange].character == null && mainScript.gridManager.Tiles[xAttackRange, yAttackRange].isActive)
                        {
                            removeFromPath = attackRange;
                        }
                    }



                    if (p != null)
                    {
                        for (int i = p.getLength() - 2; i >= removeFromPath; i--)
                        {
                            mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
                        }
                    }

                    DrawMousePath();
                }
            }
            if (character is Monster)
            {
                ShowAttackPreview(((BigTilePosition)character.GridPosition).Position.CenterPos());
            }
            else
            {
                ShowAttackPreview(new Vector2(x, y));
            }
        }
    }
    public static void DraggedOnActiveField(int x, int y, LivingObject character)
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
    public static void DrawMousePath()
    {
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
        for (int i=0; i < mousePath.Count; i++) 
        {
            Vector2 v = mousePath[i];
            
            GameObject dot = GameObject.Instantiate(ressources.moveArrowDot,gameWorld);
            dot.transform.localPosition = new Vector3(v.x + 0.5f, v.y + 0.5f, -0.5f);
            dots.Add(dot);
            if (i == mousePath.Count - 1)
            {
                moveCursor = GameObject.Instantiate(ressources.moveCursor, gameWorld);
                moveCursor.transform.localPosition = new Vector3(v.x,v.y, -0.5f);
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
    public static void ArrowCurve(GameObject dot,Vector2 v, Vector2 vBefore, Vector2 vAfter)
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
    public static void Finish(LivingObject character, Tile field, int x, int y)
    {
        oldX = x;
        oldY = y;
        nonActive = false;
        if (!field.isActive && !(x == character.GridPosition.x && y == character.GridPosition.y))
        {
            nonActive = true;
        }
    }
    private static bool IsOutOfBounds(int x, int y)
    {
        return x < 0 || x >= mainScript.gridManager.grid.width || y < 0 || y >= mainScript.gridManager.grid.height;
    }
}

