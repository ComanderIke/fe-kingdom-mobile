using Assets.Scripts.GameStates;
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
    RaycastHit hit;
    GameObject mouseCursor;
    Ray ray;
	// Use this for initialization
	void Start () {
        mainScript = FindObjectOfType<MainScript>();
        hit = new RaycastHit();
        mouseCursor = GameObject.Find("MouseCursor");
    }
    float updateFrequency = 0.1f;
    float updateTime = 0;
	// Update is called once per frame
    
	void Update () {
        /*
        if (!active)
            return;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)&&!EventSystem.current.IsPointerOverGameObject())
        {
            oldMousePath = new List<Vector2>(mousePath);
            ResetMoveArrow();
            GameObject.Find("AttackIcon").GetComponent<Image>().enabled = false;

            Physics.Raycast(ray, out hit, Mathf.Infinity);
            int x = (int)Mathf.Floor(hit.point.x);
            int z = (int)Mathf.Floor(hit.point.z);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Grid")
                {

                    MainScript.clickedOnField(x, z);;
                }
            }

        }
        else if (Input.GetMouseButtonDown(1))
        {
            ResetMoveArrow();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            oldMousePath = new List<Vector2>(mousePath);
            GameObject.Find("AttackIcon").GetComponent<Image>().enabled = false;
            ResetMoveArrow();

        }
        else if(CharacterScript.drag==false){
            updateTime += Time.deltaTime;
            if (updateTime > updateFrequency) {
                updateTime = 0;
                Physics.Raycast(ray, out hit, Mathf.Infinity);
                if (hit.collider != null)
                {
                    CharacterScript cs = hit.collider.gameObject.GetComponent<CharacterScript>();
                    if (hit.collider.tag == "Grid")
                    {
                        int x = (int)Mathf.Floor(hit.point.x);
                        int z = (int)Mathf.Floor(hit.point.z);
                        mouseCursor.transform.position = new Vector3(x + 0.5f, hit.point.y, z + 0.5f);
                        if (mainScript.activeCharacter != null)
                        {
                            CharacterDrag(x, z, mainScript.activeCharacter);
                        }
                    }
                    else if (cs != null)
                    {
                        if (cs.character == mainScript.activeCharacter)
                        {
                            Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain"));
                            int x = (int)Mathf.Floor(hit.point.x);
                            int z = (int)Mathf.Floor(hit.point.z);
                            if (x == FindObjectOfType<MainScript>().activeCharacter.x && z == mainScript.activeCharacter.y)
                                ResetMoveArrow();
                        }
                        else if (mainScript.activeCharacter != null && cs.character.team != mainScript.activeCharacter.team)
                        {
                            int x = (int)Mathf.Floor(cs.character.x);
                            int z = (int)Mathf.Floor(cs.character.y);
                            mouseCursor.transform.position = new Vector3(x + 0.5f, hit.point.y, z + 0.5f);
                            CharacterDrag(x, z, mainScript.activeCharacter);
                        }
                    }
                }
            }

        }
        */

    }
    static int oldX = -1;
    static int oldY=-1;
    public static List<Vector2> mousePath = new List<Vector2>();
    public static List<CursorPosition> lastPositions = new List<CursorPosition>();
    public static List<Vector2> oldMousePath = new List<Vector2>();
    static List<GameObject> dots = new List<GameObject>();
    public static void ResetMoveArrow()
    {
      
        foreach (GameObject dot in dots)
        {
            GameObject.Destroy(dot);
        }
        dots.Clear();
        mousePath.Clear();
        oldX = -1;
        oldY= -1;
        FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = false;
        FindObjectOfType<UXRessources>().movementFlag.SetActive(false);
    }
    static bool nonActive = false;
    public static void DraggedOver(Character character)
    {
        if (mainScript.activeCharacter.team != character.team&&mainScript.gridScript.IsFieldAttackable(character.x, character.y))
        {
            FindObjectOfType<AttackPreview>().Show(mainScript.activeCharacter, character);
        }
    }
    public static void DraggedExit()
    {
        FindObjectOfType<AttackPreview>().Hide();
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
    public static Vector2 GetLastAttackPosition(Character c, int xAttack, int zAttack)
    {
        for (int i = c.charclass.AttackRanges.Count - 1; i >= 0; i--)//Priotize Range Attacks
        {
            for (int j = lastPositions.Count - 1; j >= 0; j--)
            {
            
                if (GetDelta(lastPositions[j].position, new Vector2(xAttack, zAttack))== c.charclass.AttackRanges[i]&&mainScript.gridScript.fields[(int)lastPositions[j].position.x, (int)lastPositions[j].position.y].character==null)
                {
                    return lastPositions[j].position;
                }
            }
        }
        return new Vector2(-1,-1);
    }
    public static void CalculateMousePathToPositon(Character character, Vector2 position)
    {
        ResetMoveArrow();
        MovementPath p = mainScript.gridScript.getPath(character.x, character.y, (int)position.x,(int) position.y, character.team, false, character.charclass.AttackRanges);
        if (p != null)
        {
            for (int i = p.getLength() - 2; i >= 0; i--)
            {
                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
            }
        }
        foreach (Vector2 v in mousePath)
        {
            GameObject dot = GameObject.Instantiate(FindObjectOfType<UXRessources>().moveArrowDot);
            dot.transform.position = new Vector3(v.x + 0.5f, v.y,0);
            dots.Add(dot);
        }
    }
    public static void CharacterDrag(int x, int y, Character character)
    {
        
        if (mainScript.activeCharacter == null)
        {
            ResetMoveArrow();
            return;
        }
        if (isOldDrag(x, y))
        {
            return;
        }
       
        MapField field = mainScript.gridScript.fields[x, y];
        if (field.isActive && field.character == null)
        {
            lastPositions.Add(new CursorPosition(new Vector2(x, y),null));
            
            if (lastPositions.Count > character.charclass.movRange)
            {
                
                lastPositions.RemoveAt(0);
               // foreach (Vector2 v in lastPositions)
                   // Debug.Log("In List:" +v);
            }
        }
        //Dragged on Enemy
        if (field.character != null && field.character.team != character.team)
        {
            if (FindObjectOfType<GridScript>().IsFieldAttackable(x, y))
            {
                FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().material.mainTexture = FindObjectOfType<TextureScript>().cursorTextures[1];
                FindObjectOfType<DragCursor>().GetComponentInChildren<MeshRenderer>().enabled = true;
                //FindObjectOfType<DragCursor>().transform.position = new Vector3(x + 0.5f, FindObjectOfType<GridScript>().fields[x, z].height, z + 0.5f);
                
                GameObject.Find("AttackIcon").GetComponent<Image>().sprite = FindObjectOfType<IconScript>().AttackSprite;
                GameObject.Find("AttackIcon").GetComponent<Image>().enabled = true;
                //GameObject.Find("AttackIconCanvas").transform.position = new Vector3(x + 0.5f, FindObjectOfType<GridScript>().fields[x, z].height + 1.5f, z + 0.5f);
                FindObjectOfType<UXRessources>().movementFlag.SetActive(false);

                bool reset = true;
                for (int i = mainScript.activeCharacter.charclass.AttackRanges.Count - 1; i >= 0; i--)
                {
                    //if (mainScript.activeCharacter.charclass.AttackRanges[i] == 1)
                     //   continue;
                    foreach (Vector2 v in mousePath)
                    {
                        int xDiff = (int)Mathf.Abs(v.x - field.character.x);
                        int yDiff = (int)Mathf.Abs(v.y - field.character.y);
                        if ((xDiff + yDiff) == mainScript.activeCharacter.charclass.AttackRanges[i] && mainScript.gridScript.fields[(int)v.x, (int)v.y].isActive && mainScript.gridScript.fields[(int)v.x, (int)v.y].character == null)
                        {
                            //Debug.Log("WHY?");

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
                                foreach (Vector2 ve in mousePath)
                                {
                                    GameObject dot = GameObject.Instantiate(FindObjectOfType<UXRessources>().moveArrowDot);
                                    dot.transform.position = new Vector3(ve.x + 0.5f,  ve.y + 0.5f,0);
                                    dots.Add(dot);
                                }
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
                                foreach (Vector2 ve in mousePath)
                                {
                                    GameObject dot = GameObject.Instantiate(FindObjectOfType<UXRessources>().moveArrowDot);
                                    dot.transform.position = new Vector3(ve.x + 0.5f, ve.y + 0.5f,0);
                                    dots.Add(dot);
                                }
                                i = -1;
                                break;
                            }
                        }
                    }
                    int xDif = (int)Mathf.Abs(character.x - field.character.x);
                    int yDif = (int)Mathf.Abs(character.y - field.character.y);

                    if (i != -1 && (xDif + yDif) == mainScript.activeCharacter.charclass.AttackRanges[i])
                    {
                        Debug.Log("2---Vom Stand aus in Range und mit MousePath nicht in Range!");
                        mousePath.Clear();
                        foreach (GameObject dot in dots)
                        {
                            GameObject.Destroy(dot);
                        }
                        dots.Clear();
                        foreach (Vector2 ve in mousePath)
                        {
                            GameObject dot = GameObject.Instantiate(FindObjectOfType<UXRessources>().moveArrowDot);
                            dot.transform.position = new Vector3(ve.x + 0.5f, ve.y + 0.5f,0);
                            dots.Add(dot);
                        }
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
                    for (int i = mainScript.activeCharacter.charclass.AttackRanges.Count - 1; i >= 0; i--)
                    {
                        foreach (Vector2 pos in mousePath)
                        {
                            int xDiff = (int)Mathf.Abs(pos.x - field.character.x);
                            int yDiff = (int)Mathf.Abs(pos.y - field.character.y);
                            if ((xDiff + yDiff) == mainScript.activeCharacter.charclass.AttackRanges[i] && mainScript.gridScript.fields[(int)pos.x, (int)pos.y].isActive && mainScript.gridScript.fields[(int)pos.x, (int)pos.y].character == null)
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
                        for (int i = mainScript.activeCharacter.charclass.AttackRanges.Count - 1; i >= 0; i--)
                        {
                            if ((xDiff + yDiff) == mainScript.activeCharacter.charclass.AttackRanges[i])
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
                        foreach (Vector2 v in mousePath)
                        {
                            GameObject dot = GameObject.Instantiate(FindObjectOfType<UXRessources>().moveArrowDot);
                            dot.transform.position = new Vector3(v.x + 0.5f, v.y + 0.5f,0);
                            dots.Add(dot);
                        }
                    }
                    else
                    {
                        ResetMoveArrow();
                        MovementPath p = mainScript.gridScript.getPath(character.x, character.y, x, y, character.team, false, character.charclass.AttackRanges);
                        int removeFromPath = 1;
                        foreach (int attackRange in mainScript.activeCharacter.charclass.AttackRanges)
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
                            //  for (int i = removeFromPath; i < p.getLength()-1 ; i++)
                            {
                                mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
                                // Debug.Log(mousePath[i]);
                            }
                        }

                        foreach (Vector2 v in mousePath)
                        {
                            GameObject dot = GameObject.Instantiate(FindObjectOfType<UXRessources>().moveArrowDot);
                            dot.transform.position = new Vector3(v.x + 0.5f, v.y + 0.5f,0);
                            dots.Add(dot);
                        }
                    }
                }
            }
        }
        //No enemy
        else
        {
            GameObject.Find("AttackIcon").GetComponent<Image>().enabled = false;
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
            if (field.isActive && !(field.x == character.x && field.y == character.y))
            {

                FindObjectOfType<UXRessources>().movementFlag.SetActive(true);
                FindObjectOfType<UXRessources>().movementFlag.transform.position = new Vector3(x + 0.5f, y + 0.5f,0);
            }
            else
            {
                FindObjectOfType<UXRessources>().movementFlag.SetActive(false);
            }
        }
        //If Field is Orange and not the filed currently standing on
        if (!(x == character.x && y == character.y)&&field.isActive)//&&field.character==null)
        {
            if (nonActive)
            {
                ResetMoveArrow();
            }
            nonActive = false;
            bool contains = false;
            if(mousePath.Contains(new Vector2(x, y)))
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
            if (mousePath.Count> character.charclass.movRange||contains||(Mathf.Abs(oldX - x) + Mathf.Abs(oldY - y) > 1))
            {
                mousePath.Clear();
                MovementPath p = mainScript.gridScript.getPath(character.x, character.y, x, y, character.team, false, character.charclass.AttackRanges);
                if (p != null)
                {
                    for (int i = p.getLength() - 2; i >= 0; i--)
                    {
                        mousePath.Add(new Vector2(p.getStep(i).getX(), p.getStep(i).getY()));
                    }
                }
            }
            foreach(Vector2 v in mousePath)
            {
                GameObject dot = GameObject.Instantiate(FindObjectOfType<UXRessources>().moveArrowDot);
                dot.transform.position = new Vector3(v.x + 0.5f, v.y + 0.5f,0);
                dots.Add(dot);
            }
        }
        else if(x == character.x && y == character.y)
        {
            ResetMoveArrow();
            nonActive = false;
        }
        Finish(character, field, x, y);
        
    }
    public static void Finish(Character character, MapField field, int x, int y)
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

