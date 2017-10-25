using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using Assets.Scripts.Grid;
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
    public static bool confirmClick = false;
    public static Vector2 clickedField;
    //GameObject mouseCursor;
    private static Ray ray;
	// Use this for initialization
	void Start () {
        mainScript = FindObjectOfType<MainScript>();
        hit = new RaycastHit();
        gameWorld = GameObject.FindGameObjectWithTag("World").transform;
        ressources = FindObjectOfType<UXRessources>();
        //mouseCursor = GameObject.Find("MouseCursor");
    }
    float updateFrequency = 0.1f;
    float updateTime = 0;
	// Update is called once per frame
    
	void Update () {
        
        if (!active)
            return;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            oldMousePath = new List<Vector2>(mousePath);
           
           
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            int x = (int)Mathf.Floor(hit.point.x-GridScript.GRID_X_OFFSET);
            int y = (int)Mathf.Floor(hit.point.y);
            MouseManager.currentX = x;
            MouseManager.currentY = y;
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Grid")
                {
                    ResetMousePath();
                    if (mainScript.activeCharacter != null)
                    {
                        if (confirmClick && clickedField == new Vector2(x,y))
                            MainScript.clickedOnField(x, y);
                        else
                        {
                            confirmClick = true;
                            clickedField = new Vector2(x, y);
                            if (mainScript.gridScript.fields[x, y].isActive)
                            {
                                if (mainScript.activeCharacter is Monster)
                                {
                                    
                                   // Debug.Log(hit.point.x - GridScript.GRID_X_OFFSET+" "+hit.point.y);
                                    Debug.Log(Mathf.Round(hit.point.x - GridScript.GRID_X_OFFSET) + " " + Mathf.Round(hit.point.y));
                                    int centerX = (int)Mathf.Round(hit.point.x - GridScript.GRID_X_OFFSET) - 1;
                                    int centerY = (int)Mathf.Round(hit.point.y) -1;
                                    BigTile clickedBigTile = GetClickedBigTile(centerX, centerY, x, y);
                                    MouseManager.CalculateMousePathToPositon(mainScript.activeCharacter, clickedBigTile);
                                    MouseManager.DrawMousePath(mainScript.activeCharacter.x, mainScript.activeCharacter.y);
                                }
                                else
                                {
                                    MouseManager.CalculateMousePathToPositon(mainScript.activeCharacter, new Vector2(x, y));
                                    MouseManager.DrawMousePath(mainScript.activeCharacter.x, mainScript.activeCharacter.y);
                                }
                            }
                        }
                    }
                    else
                        MainScript.clickedOnField(x, y);
                }
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            oldMousePath = new List<Vector2>(mousePath);
            if (!confirmClick)
            {
                
                ResetMousePath();
            }

        }

    }
    private static BigTile GetClickedBigTile(int centerX, int centerY, int x, int y)
    {
        BigTile clickedBigTile= new BigTile(new Vector2(centerX, centerY), new Vector2(centerX + 1, centerY), new Vector2(centerX, centerY + 1), new Vector2(centerX + 1, centerY + 1));

        if (!mainScript.gridScript.IsValidAndActive(clickedBigTile, mainScript.activeCharacter.team))
        {

            clickedBigTile = new BigTile(new Vector2(x, y), new Vector2(x + 1, y), new Vector2(x, y + 1), new Vector2(x + 1, y + 1));

            if (!mainScript.gridScript.IsValidAndActive(clickedBigTile, mainScript.activeCharacter.team))
            {


                clickedBigTile = new BigTile(new Vector2(x - 1, y), new Vector2(x, y), new Vector2(x - 1, y + 1), new Vector2(x, y + 1));

                if (!mainScript.gridScript.IsValidAndActive(clickedBigTile, mainScript.activeCharacter.team))
                {

                    clickedBigTile = new BigTile(new Vector2(x, y - 1), new Vector2(x + 1, y - 1), new Vector2(x, y), new Vector2(x + 1, y));

                    if (!mainScript.gridScript.IsValidAndActive(clickedBigTile, mainScript.activeCharacter.team))
                    {
                        clickedBigTile = new BigTile(new Vector2(x - 1, y - 1), new Vector2(x, y - 1), new Vector2(x - 1, y), new Vector2(x, y));
                    }
                }
            }
        }
        return clickedBigTile;
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
        if (moveCursor != null)
            GameObject.Destroy(moveCursor);
        FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
        //FindObjectOfType<UXRessources>().movementFlag.SetActive(false);
    }
    static bool nonActive = false;
    public static void DraggedOver(LivingObject character)
    {
        //TODO: Show Attack Icon or something
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
        for (int i = c.AttackRanges.Count - 1; i >= 0; i--)//Priotize Range Attacks
        {
            for (int j = lastPositions.Count - 1; j >= 0; j--)
            {
            
                if (GetDelta(lastPositions[j].position, new Vector2(xAttack, zAttack))== c.AttackRanges[i]&&mainScript.gridScript.fields[(int)lastPositions[j].position.x, (int)lastPositions[j].position.y].character==null)
                {
                    return lastPositions[j].position;
                }
            }
        }
        return new Vector2(-1,-1);
    }
    public static void CalculateMousePathToPositon(LivingObject character, Vector2 position)
    {
        ResetMousePath();
        MovementPath p = mainScript.gridScript.getPath(character.x, character.y, (int)position.x, (int)position.y, character.team, false, character.AttackRanges);
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
    }
    public static void CalculateMousePathToEnemy(LivingObject character, Vector2 position)
    {
        ResetMousePath();
        Debug.Log("from" + character.x + " " + character.y + " to " + position.x + " " + position.y + " team " + character.team + " " + character.AttackRanges[0]);
        MovementPath p = mainScript.gridScript.getPath(character.x, character.y, (int)position.x, (int)position.y, character.team, true, character.AttackRanges);
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
        Debug.Log("FUCKTESTfrom" + character.x + " " + character.y + " to " + position + " team " + character.team + " " + character.AttackRanges[0]);
        MovementPath p = mainScript.gridScript.GetMonsterPath((Monster)character, position, true, character.AttackRanges);
        
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
        MovementPath p = mainScript.gridScript.GetMonsterPath((Monster)character, position);
        if (p != null)
        {
            for (int i = p.getLength() - 2; i >= 0; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }

    }
    public static void CharacterDrag(int x, int y, LivingObject character)
    {
        
        if (mainScript.activeCharacter == null)
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
        
        MapField field = mainScript.gridScript.fields[x, y];
        if (field.isActive && field.character == null)
        {
            lastPositions.Add(new CursorPosition(new Vector2(x, y),null));
            
            if (lastPositions.Count > character.movRange)
            {
                
                lastPositions.RemoveAt(0);
               // foreach (Vector2 v in lastPositions)
                   // Debug.Log("In List:" +v);
            }
        }
        //Dragged on Enemy
        if (field.character != null && field.character.team != character.team)
        {
            DraggedOnEnemy(x, y, field, character);
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
        if (!(x == character.x && y == character.y)&&field.isActive)//&&field.character==null)
        {
            DraggedOnActiveField(x,y,character);
        }
        else if(x == character.x && y == character.y)
        {
            ResetMousePath();
            nonActive = false;
        }
        Finish(character, field, x, y);
        
    }
    public static void DraggedOnEnemy(int x, int y, MapField field,LivingObject character)
    {
        if(mainScript.activeCharacter is Monster)
        {
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            int centerX = (int)Mathf.Round(hit.point.x - GridScript.GRID_X_OFFSET) - 1;
            int centerY = (int)Mathf.Round(hit.point.y) - 1;
            BigTile clickedBigTile = GetClickedBigTile(centerX, centerY, x, y);
            CalculateMousePathToÉnemy(mainScript.activeCharacter, clickedBigTile);
            DrawMousePath(mainScript.activeCharacter.x, mainScript.activeCharacter.y);
            return;
        }
        if (FindObjectOfType<GridScript>().IsFieldAttackable(x, y))
        {
            bool reset = true;
            for (int i = mainScript.activeCharacter.AttackRanges.Count - 1; i >= 0; i--)
            {

                foreach (Vector2 v in mousePath)
                {
                    int xDiff = (int)Mathf.Abs(v.x - field.character.x);
                    int yDiff = (int)Mathf.Abs(v.y - field.character.y);
                    if ((xDiff + yDiff) == mainScript.activeCharacter.AttackRanges[i] && mainScript.gridScript.fields[(int)v.x, (int)v.y].isActive && mainScript.gridScript.fields[(int)v.x, (int)v.y].character == null)
                    {
                        if (mousePath.IndexOf(v) + 1 < mousePath.Count && mousePath.Count - (mousePath.IndexOf(v) + 1) > 0)
                        {
                            Debug.Log(GetLastAttackPosition(character, (int)field.character.x, (int)field.character.y));
                            CalculateMousePathToPositon(character, GetLastAttackPosition(character, (int)field.character.x, (int)field.character.y));
                            Finish(character, field, x, y);
                            return;
                            Debug.Log("Was ist das?" + v + " " + mousePath.IndexOf(v) + " " + (mousePath.Count - (mousePath.IndexOf(v) + 1)) + " " + mousePath.Count + "  " + field.character.x + " " + field.character.y);
                            reset = false;
                            mousePath.RemoveRange(mousePath.IndexOf(v) + 1, mousePath.Count - (mousePath.IndexOf(v) + 1));
                            i = -1;
                            foreach (GameObject dot in dots)
                            {
                                GameObject.Destroy(dot);
                            }
                            dots.Clear();
                            DrawMousePath(character.x, character.y);
                            break;
                        }
                        else
                        {
                            Debug.Log("WEL FUCK");
                            mousePath.Clear();
                            foreach (GameObject dot in dots)
                            {
                                GameObject.Destroy(dot);
                            }
                            dots.Clear();
                            DrawMousePath(character.x, character.y);
                            i = -1;
                            break;
                        }
                    }
                }
                int xDif = (int)Mathf.Abs(character.x - field.character.x);
                int yDif = (int)Mathf.Abs(character.y - field.character.y);

                if (i != -1 && (xDif + yDif) == mainScript.activeCharacter.AttackRanges[i])
                {
                    Debug.Log("2---Vom Stand aus in Range und mit MousePath nicht in Range!");
                    mousePath.Clear();
                    foreach (GameObject dot in dots)
                    {
                        GameObject.Destroy(dot);
                    }
                    dots.Clear();
                    DrawMousePath(character.x, character.y);
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
                for (int i = mainScript.activeCharacter.AttackRanges.Count - 1; i >= 0; i--)
                {
                    foreach (Vector2 pos in mousePath)
                    {
                        int xDiff = (int)Mathf.Abs(pos.x - field.character.x);
                        int yDiff = (int)Mathf.Abs(pos.y - field.character.y);
                        if ((xDiff + yDiff) == mainScript.activeCharacter.AttackRanges[i] && mainScript.gridScript.fields[(int)pos.x, (int)pos.y].isActive && mainScript.gridScript.fields[(int)pos.x, (int)pos.y].character == null)
                        {
                            Debug.Log("Reset" + pos + " " + mousePath.IndexOf(pos) + " " + (mousePath.Count - (mousePath.IndexOf(pos) + 1)) + " " + mousePath.Count + "  " + character.x + " " + character.y);
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
                    int xDiff = (int)Mathf.Abs(character.x - field.character.x);
                    int yDiff = (int)Mathf.Abs(character.y - field.character.y);
                    for (int i = mainScript.activeCharacter.AttackRanges.Count - 1; i >= 0; i--)
                    {
                        if ((xDiff + yDiff) == mainScript.activeCharacter.AttackRanges[i])
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
                    DrawMousePath(character.x, character.y);
                }
                else
                {
                    ResetMousePath();
                    MovementPath p = mainScript.gridScript.getPath(character.x, character.y, x, y, character.team, false, character.AttackRanges);
                    int removeFromPath = 1;
                    foreach (int attackRange in mainScript.activeCharacter.AttackRanges)
                    {
                        int xAttackRange = (int)p.getStep(p.getLength() - 1 - attackRange).getX();
                        int yAttackRange = (int)p.getStep(p.getLength() - 1 - attackRange).getY();
                        if (mainScript.gridScript.fields[xAttackRange, yAttackRange].character == null && mainScript.gridScript.fields[xAttackRange, yAttackRange].isActive)
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

                    DrawMousePath(character.x, character.y);
                }
            }
        }
    }
    public static void DraggedOnActiveField(int x, int y, LivingObject character)
    {
        if (nonActive)
        {
            ResetMousePath();
        }
        if (mainScript.activeCharacter is Monster)
        {
            Physics.Raycast(ray, out hit, Mathf.Infinity);
            int centerX = (int)Mathf.Round(hit.point.x - GridScript.GRID_X_OFFSET) - 1;
            int centerY = (int)Mathf.Round(hit.point.y) - 1;
            BigTile clickedBigTile = GetClickedBigTile(centerX, centerY, x, y);
            CalculateMousePathToPositon(mainScript.activeCharacter, clickedBigTile);
            DrawMousePath(mainScript.activeCharacter.x, mainScript.activeCharacter.y);
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
        if (mousePath.Count > character.movRange || contains || (Mathf.Abs(oldX - x) + Mathf.Abs(oldY - y) > 1))
        {
            mousePath.Clear();
            MovementPath p = mainScript.gridScript.getPath(character.x, character.y, x, y, character.team, false, character.AttackRanges);
            if (p != null)
            {
                for (int i = p.getLength() - 2; i >= 0; i--)
                {
                    mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
                }
            }
        }
        DrawMousePath(character.x, character.y);
    }
    public static void DrawMousePath(int startx, int starty)
    {
        //Debug.Log("DrawMousePath");
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
                    if (v.x - startx > 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 180);
                    else if (v.x -startx < 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 0);
                    else if (v.y - starty > 0)
                        dot.transform.rotation = Quaternion.Euler(0, 0, 270);
                    else if (v.y - starty < 0)
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
                    vBefore = new Vector2(startx, starty);
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
    public static void Finish(LivingObject character, MapField field, int x, int y)
    {
        oldX = x;
        oldY = y;
        nonActive = false;
        if (!field.isActive && !(x == character.x && y == character.y))
        {
            nonActive = true;
        }
    }
}

