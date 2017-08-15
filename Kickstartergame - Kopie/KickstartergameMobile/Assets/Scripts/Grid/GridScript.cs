using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[System.Serializable]
public class Grid{
	public int width;
	public int height;
}
[System.Serializable]
public class Step {
	private int x;
	private float y;
	private int z;

	public Step(int x, float y, int z){
		this.x = x;
		this.y  =y;
		this.z = z;
	}
	public int getX(){
		return x;
	}
	public float getY(){
		return y;
	}
	public int getZ(){
		return z;
	}
}
[System.Serializable]
public class MovementPath {
	private ArrayList steps = new ArrayList ();

	public MovementPath(){

	}
	public int getLength(){
		return steps.Count;
	}
	public Step getStep(int index){
		return (Step)steps [index];
	}
	public void prependStep(int x,float y, int z){
		steps.Add(new Step (x, y,z));
	}
	public void Reverse(){
		steps.Reverse ();
	}
}
[System.Serializable]
public class Node {
	
	public int x;
	public int y;
	public int c;
	public Node parent;
	public int cost;
	public int costfromStart;
	public bool check = false;
	public int depth;
	public Node (int x, int y, int c){
		this.x = x;
		this.y = y;
		this.c = c;
	}
	public int setParent(Node parent){
		depth = parent.depth + 1;
		this.parent = parent;
		return depth;
	}
}
[System.Serializable]
public enum FieldEffectType
{
    None,
    Heal
}
public class FieldEffect
{
    public FieldEffectType type;
    public int heal;

    public FieldEffect(FieldEffectType type, int heal)
    {
        this.type = type;
        this.heal = heal;
    }
    public void Effect(Character c, int playernumber)
    {
        if (type == FieldEffectType.Heal && c != null && playernumber == c.team)
        {
            c.Heal(heal);

        }
    }
}
[System.Serializable]
public class MapField{
	public GameObject gameObject;
	public bool isAccessible = true;
    public Character attackRangeCharacter;
	public Character character;
    public TreasureChest chest;
    public Door door;
	public FieldState fieldState = FieldState.Normal;
	public int movementCost = 1;
    public FieldEffect effect;
	public float height;
    public bool isJumpable = true;
	public bool isActive = false;
    public int x;
    public int y;
    public bool enemyAttackRange = false;
    public bool blockArrow = false;
	public MapField(int i, int j, GameObject gameObject,float height){
		this.gameObject = gameObject;
		this.height = height;
        x = i;
        y = j;
        effect = new FieldEffect(FieldEffectType.None, 0);
	}
    
}
[System.Serializable]
public enum FieldState
{
    Wall,
    Normal
}

public class GridScript : MonoBehaviour {
    
    #region const
    const string CELL_NAME = "Grid Cell";
    const string CELL_TAG = "Grid";
    const int CELL_LAYER = 0;
    const string BLOCK_FIELD_LAYER = "BlockField";
    const string BLOCK_ARROW_LAYER = "BlockArrows";
    #endregion

    #region fields
    public Grid grid;
	public float cellSize = 1;
	public float yOffset ;
	public float yOffsetStairs;
	public float yOffsetBridge;
	public Material cellMaterialValid;
	public Material cellMaterialInvalid;
	public GameObject field;
	public Texture MoveTexture;
	public Texture StandOnTexture;
	public Texture AttackTexture;
    public Texture EnemyAttackTexture;
    public Texture StandardTexture;
    public Texture healTexture;
    public Texture mouseHoverTexture;
    public Texture skillRangeTexture;
    public MapField[,] fields;
    public bool[,] stairs;
	public bool[,] bridge;
    private ArrayList closed;
    private ArrayList open;
    private  Node [,]nodes;
	private float[] _heights;
    #endregion

    void Start() {
		stairs = new bool[grid.width, grid.height];
		for (int i = 0; i < grid.width; i++) {
			for (int j = 0; j < grid.height; j++) {

				stairs [i, j] = false;
			}
		}
		bridge = new bool[grid.width, grid.height];
		for (int i = 0; i < grid.width; i++) {
			for (int j = 0; j < grid.height; j++) {

				bridge [i, j] = false;
			}
		}
		stairs [16, grid.height-1] = true;
		stairs [16, grid.height-4] = true;
		stairs [16, grid.height-5] = true;
		stairs [16, 0] = true;
		stairs [16, 1] = true;
		stairs [18, 6] = true;
		stairs [10, 6] = true;
		stairs [9, 6] = true;
		bridge [8, 14] = true;
		bridge [9, 14] = true;
		_heights = new float[(grid.height + 1) * (grid.width + 1)];
		UpdateHeights();
		fields = new MapField[grid.width, grid.height];
		nodes = new Node[grid.width, grid.height];
		for (int i = 0; i < grid.width; i++) {
			for (int j = 0; j < grid.height; j++) {
				if(stairs[i,j])
					fields[i,j] = new MapField(i,j,CreateChild(i,_heights[j * (grid.width + 1) + i] + yOffsetStairs,j), _heights[j * (grid.width+1) + i]);
				else if(bridge[i,j])
					fields[i,j] = new MapField(i,j,CreateChild(i,_heights[j * (grid.width + 1) + i] + yOffsetBridge,j), _heights[j * (grid.width+1) + i]);
				else
					fields[i,j] = new MapField(i,j,CreateChild(i,_heights[j * (grid.width + 1) + i] + yOffset,j), _heights[j * (grid.width+1) + i]);
                nodes[i,j] = new Node(i, j, 1000);
			}
		}
		UpdateCells();
	}

    private void addToClosed(Node node)
    {
        closed.Add(node);
    }

    private void addToOpen(Node node)
    {
        open.Add(node);
    }
    public bool IsFieldAttackable(int x, int z)
    {
        return fields[x, z].gameObject.GetComponent<MeshRenderer>().material.mainTexture == AttackTexture;
    }
    public bool checkField(int x, int y, int team, int range)
    {
        if (x >= 0 && y >= 0 && x < grid.width && y < grid.height)
        {
            MapField field = fields[x, y];
            if ((field.isAccessible || field.door != null))
            {
                if (field.chest != null)
                    return false;
                if (field.door != null)
                {
                    if (field.door.opened)
                        return true;
                    return false;
                }
                if (field.character == null)
                    return true;
                else if (field.character.team == team)
                    return true;
            }
            else
            {
                //MeshRenderer m = fields[x, y].gameObject.GetComponent<MeshRenderer>();
                //m.material.mainTexture = AttackTexture;
                if (range < 0)
                    return true;
                return false;
            }
        }
        return false;
    }
    public bool checkAttackField(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.width && y < grid.height)
        {
            if (!fields[x,y].blockArrow)
                return true;
            else
            {
                //Debug.Log("BLOCKARROWS"+x + " " + y);
            }
        }
        return false;
    }

    GameObject CreateChild(int xvalue, float yvalue, int zvalue)
    {
        GameObject go = new GameObject();
        go.layer = CELL_LAYER;
        go.tag = CELL_TAG;
		go.name = CELL_NAME +" "+ xvalue+" "+zvalue;
        go.transform.parent = transform;
        go.transform.localPosition = new Vector3(0, yvalue, 0);
        go.transform.localRotation = Quaternion.identity;
        go.AddComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        Mesh meshobj = CreateMesh();
        go.AddComponent<MeshFilter>().mesh = meshobj;
        go.GetComponent<MeshFilter>().sharedMesh.bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(500, 500, 500));//this is important!
        go.AddComponent<BoxCollider>().center = new Vector3(xvalue + 0.5f, 0, zvalue + 0.5f);
        go.GetComponent<BoxCollider>().size = new Vector3(1, 0.4f, 1);
        //go.AddComponent<FieldClicked>().position = new Vector3(xvalue, yvalue, zvalue);
        return go;
    }

    Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.name = CELL_NAME;
        mesh.vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
        mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
        mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
        mesh.uv = new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };
        return mesh;
    }
	public void ShowStandOnTexture(Character c){
		fields [c.x, c.z].gameObject.GetComponent<MeshRenderer> ().material.mainTexture = StandOnTexture;
	}
	public void HideStandOnTexture(Character c){
		//Do Nothing?
	}
    public MovementPath findPath(int sx, int sz, int tx, int tz, int team, bool toadjacentPos, List<int> range)
    {
        nodes[sx, sz].costfromStart = 0;
        nodes[sx, sz].depth = 0;
        closed.Clear();
        open.Clear();
        open.Add(nodes[sx, sz]);
        nodes[tx, tz].parent = null;
        int maxDepth = 0;
        int maxSearchDistance = 100;
        while ((maxDepth < maxSearchDistance) && (open.Count != 0))
        {
            Node current = getFirstInOpen();
            if (current == nodes[tx, tz])
            {
                break;
            }
            
           
            removeFromOpen(current);
            addToClosed(current);
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                        continue;
                    if (x != 0 && y != 0)   //no diagonal movement
                        continue;
                    int xp = x + current.x;
                    int yp = y + current.y;
                    bool isAdjacent = false;
                    if (toadjacentPos)
                    {
                        int delta = Mathf.Abs(xp- nodes[tx, tz].x) + Mathf.Abs(yp - nodes[tx, tz].y);
                        range.Reverse();
                       foreach(int r in range)
                        {
                            if (delta == r)
                            {
                                
                                isAdjacent = true;
                               
                                MainScript m = GameObject.Find(MainScript.MAIN_GAME_OBJ).GetComponent<MainScript>();
                                if (m.AttackRangeFromPath < r)
                                {
                                    m.AttackRangeFromPath = r;
                                   // break;
                                }
                               
                                
                            }
                        }
                        range.Reverse();
                    }
                    if (isValidLocation(team, sx, sz, xp, yp, isAdjacent)|| (xp == tx && yp== tz))
                    {
                        int nextStepCost = current.costfromStart + fields[xp, yp].movementCost;
                        Node neighbour = nodes[xp, yp];
                        if (nextStepCost < neighbour.costfromStart)
                        {
                            if (InOpenList(neighbour))
                            {
                                removeFromOpen(neighbour);
                            }
                            if (InClosedList(neighbour))
                            {
                                removeFromClosed(neighbour);
                            }
                        }
                        if (!InOpenList(neighbour) && !InClosedList(neighbour))
                        {
                            neighbour.costfromStart = nextStepCost;
                            maxDepth = Mathf.Max(maxDepth, neighbour.setParent(current));
                            addToOpen(neighbour);
                        }
                    }
                }
            }
        }
        if (nodes[tx, tz].parent == null)
        {
            return null;
        }
        MovementPath path = new MovementPath();
        Node target = nodes[tx, tz];
        while (target != nodes[sx, sz])
        {
            path.prependStep(target.x, fields[target.x, target.y].height, target.y);
            target = target.parent;
        }
        path.prependStep(sx, fields[sx, sz].height, sz);
        return path;

    }

    public MapField GetField(Vector2 pos)
    {
        return fields[(int)pos.x, (int)pos.y];
    }
    private Node getFirstInOpen()
    {
        return (Node)open[0];
    }

    public MovementPath getPath(int x, int z, int x2, int z2, int team, bool toadjacentPos, List<int>range)
    {
        nodes = new Node[grid.width, grid.height];
        closed = new ArrayList();
        open = new ArrayList();
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                nodes[i, j] = new Node(i, j, 1000);
            }
        }

        return findPath(x, z, x2, z2, team, toadjacentPos, range);
    }

    public void HideAllyMovement()
    {
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                MeshRenderer m = fields[i, j].gameObject.GetComponent<MeshRenderer>();
                if (fields[i, j].isAccessible)
                {
                    if (fields[i, j].enemyAttackRange == false)
                        m.material.mainTexture = StandardTexture;
                }
                else
                {
                    if (fields[i, j].enemyAttackRange == false)
                        m.material = cellMaterialInvalid;
                }
                nodes[i, j].c = 1000;
                fields[i, j].isActive = false;
            }
        }
    }

    public void HideCharacterMovement(Character character)
    {
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                MeshRenderer m = fields[i, j].gameObject.GetComponent<MeshRenderer>();
                if (fields[i, j].attackRangeCharacter == character)
                {
                    if (fields[i, j].isAccessible)
                        m.material.mainTexture = StandardTexture;
                    else
                    {
                        m.material = cellMaterialInvalid;
                    }
                    nodes[i, j].c = 1000;
                    fields[i, j].isActive = false;
                }
            }
        }
    }

    public void HideMovement()
    {
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                MeshRenderer m = fields[i, j].gameObject.GetComponent<MeshRenderer>();
                fields[i,j].enemyAttackRange=false;
                if (fields[i, j].isAccessible)
                    m.material.mainTexture = StandardTexture;
                else
                {
                    m.material = cellMaterialInvalid;
                }
                nodes[i, j].c = 1000;
                fields[i, j].isActive = false;
            }
        }
    }

    private bool InClosedList(Node node){
		return closed.Contains (node);
	}

    private bool InOpenList(Node node){
		return open.Contains (node);
	}

    bool IsCellValid(int x, int z)
    {

        return true;
        float avg = (_heights[z * (grid.width + 1) + x] +
            _heights[(z + 1) * (grid.width + 1) + x] +
            _heights[z * (grid.width + 1) + x + 1] +
            _heights[(z + 1) * (grid.width + 1) + x + 1]) / 4;
        float offset = 0.2f;
        if (avg > (_heights[z * (grid.width + 1) + x] + offset) || avg > (_heights[z * (grid.width + 1) + x + 1] + offset) || avg > _heights[(z + 1) * (grid.width + 1) + x] + offset || avg > _heights[(z + 1) * (grid.width + 1) + x + 1] + offset)
            return false;

        return true;
    }

    private bool isValidLocation(int team, int sx, int sy, int x, int y, bool isAdjacent)
    {
        bool invalid = (x < 0) || (y < 0) || (x >= grid.width) || (y >= grid.height);
        if (!invalid && fields[x, y].door != null)
        {
            return fields[x, y].door.opened;
        }
        if ((!invalid) && ((sx != x) || (sy != y)))
        {
            invalid = !fields[x, y].isAccessible;
        }
        
        if (!invalid)
        {
            //Material m = fields[x, y].gameObject.GetComponent<MeshRenderer>().material;
            //if (m.mainTexture != MoveTexture)
            //    invalid = true;
            if (fields[x, y].character != null)
            {
                if (fields[x, y].character.team != team)
                {
                        invalid = true;
                }
                if (isAdjacent)//TODO passthrouhgh should be ok but not stopping on it
                {
                    invalid = true;
                }
            }
        }
        return !invalid;
    }

	Vector3 MeshVertex(int x, int z, float minus)
    {
		return new Vector3(x * cellSize, _heights[z * (grid.width + 1) + x]-minus, z * cellSize);
    }

    private bool nodeFaster(int x, int y, int c)
    {
        if (nodes[x, y].c < c)
            return false;
        return true;
    }

    private void removeFromClosed(Node node)
    {
        closed.Remove(node);
    }

    private void removeFromOpen(Node node){
		open.Remove (node);
	}

    public void ResetActiveFields()
    {
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                //MeshRenderer m = fields[i, j].gameObject.GetComponent<MeshRenderer>();
                //if (fields[i, j].isAccessible)
                //    m.material.mainTexture = StandardTexture;
                //else
                //{
                //    m.material = cellMaterialInvalid;
                //}
                nodes[i, j].c = 1000;
                fields[i, j].isActive = false;
            }
        }
    }

    public void ShowAttack(Character character, List<int> attack, bool enemy)
    {
        List<MapField> fieldsFromWhereUCanAttack = new List<MapField>();
        foreach (MapField f in fields)
        {
            if (f.isActive)
            {
                fieldsFromWhereUCanAttack.Add(f);
            }
        }
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                nodes[i, j].c = 1000;
            }
        }
        foreach (MapField f in fieldsFromWhereUCanAttack)
        {

            int x = f.x;
            int y = f.y;
            foreach (int range in attack)
            {
                ShowAttackRecursive(character, x, y, range, new List<int>(), enemy);

            }
        }
		ShowStandOnTexture (character);

    }

    public void ShowHealRange(Character character, List<int> healrange)
    {
        List<MapField> fieldsFromWhereUCanAttack = new List<MapField>();
        foreach (MapField f in fields)
        {
            if (f.isActive)
            {
                fieldsFromWhereUCanAttack.Add(f);
            }
        }
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                nodes[i, j].c = 1000;
            }
        }
        foreach (MapField f in fieldsFromWhereUCanAttack)
        {

            int x = f.x;
            int y = f.y;
            foreach (int range in healrange)
            {
                ShowHealRecursive(character, x, y, range, new List<int>());

            }
        }
		ShowStandOnTexture (character);
    }

    public void ShowHealRecursive(Character character, int x, int y, int range, List<int> direction)
    {
        MeshRenderer m = fields[x, y].gameObject.GetComponent<MeshRenderer>();


        if (range <= 0)
        {
            m = fields[x, y].gameObject.GetComponent<MeshRenderer>();
            if (m.material.mainTexture == MoveTexture)
                return;
            m.material.mainTexture = healTexture;
            return;
        }
        if (!direction.Contains(2))
        {
            if (checkAttackField(x + 1, y))
            { //&& AttackNodeFaster(x+1, y, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                ShowHealRecursive(character, x + 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(1))
        {
            if (checkAttackField(x - 1, y))
            { //&& AttackNodeFaster(x - 1, y, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                ShowHealRecursive(character, x - 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(4))
        {
            if (checkAttackField(x, y + 1))
            {// && AttackNodeFaster(x , y+1, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                ShowHealRecursive(character, x, y + 1, range - 1, newdirection);
            }
        }
        if (!direction.Contains(3))
        {
            if (checkAttackField(x, y - 1))
            { //&& AttackNodeFaster(x , y-1, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                ShowHealRecursive(character, x, y - 1, range - 1, newdirection);
            }
        }

    }

    public void ShowAttackRecursive(Character character, int x, int y, int range, List<int> direction, bool enemy)
    {
        MeshRenderer m = fields[x, y].gameObject.GetComponent<MeshRenderer>();


        if (range <= 0)
        {
            m = fields[x, y].gameObject.GetComponent<MeshRenderer>();
            if (m.material.mainTexture == MoveTexture)
                return;
            m.material.mainTexture = AttackTexture;
            if (enemy)
            {
                fields[x, y].attackRangeCharacter = character;
                fields[x, y].enemyAttackRange = true;
                m.material.mainTexture = EnemyAttackTexture;
            }
            return;
        }
        if (!direction.Contains(2))
        {
            if (checkAttackField(x + 1, y))
            { //&& AttackNodeFaster(x+1, y, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                ShowAttackRecursive(character, x + 1, y, range - 1, newdirection, enemy);
            }
        }
        if (!direction.Contains(1))
        {
            if (checkAttackField(x - 1, y))
            { //&& AttackNodeFaster(x - 1, y, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                ShowAttackRecursive(character, x - 1, y, range - 1, newdirection, enemy);
            }
        }
        if (!direction.Contains(4))
        {
            if (checkAttackField(x, y + 1))
            {// && AttackNodeFaster(x , y+1, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                ShowAttackRecursive(character, x, y + 1, range - 1, newdirection, enemy);
            }
        }
        if (!direction.Contains(3))
        {
            if (checkAttackField(x, y - 1))
            { //&& AttackNodeFaster(x , y-1, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                ShowAttackRecursive(character, x, y - 1, range - 1, newdirection, enemy);
            }
        }

    }

    public float GetHeight(int x, int z)
    {
        return fields[x, z].height;
    }

    public void ShowMovement(int x, int z, int range, int attackIndex, List<int> attack, int c, int team, bool enemy)
    {
        if (range < 0)
        {
            return;

        }

        if (!enemy)
        {
            MeshRenderer m = fields[x, z].gameObject.GetComponent<MeshRenderer>();
            m.material.mainTexture = MoveTexture;
            if(fields[x,z].character!=null&& fields[x,z].character.team!= team)
            {
                Debug.Log("test4");
                m.material.mainTexture = AttackTexture;
            }
        }
        fields[x, z].isActive = true;
        nodes[x, z].c = c;
        c++;
        if (checkField(x - 1, z, team, range) && nodeFaster(x - 1, z, c))
            ShowMovement(x - 1, z, range - 1, attackIndex, new List<int>(attack), c, team, enemy);
        if (checkField(x + 1, z, team, range) && nodeFaster(x + 1, z, c))
            ShowMovement(x + 1, z, range - 1, attackIndex, new List<int>(attack), c, team, enemy);
        if (checkField(x, z - 1, team, range) && nodeFaster(x, z - 1, c))
            ShowMovement(x, z - 1, range - 1, attackIndex, new List<int>(attack), c, team, enemy);
        if (checkField(x, z + 1, team, range) && nodeFaster(x, z + 1, c))
            ShowMovement(x, z + 1, range - 1, attackIndex, new List<int>(attack), c, team, enemy);
    }
    public void GetMovement(int x, int z, List<Vector3> locations, int range, int c, int team)
    {
        if (range < 0)
        {
            return;
        }
        if(!locations.Contains(new Vector3(x, 0, z))&&fields[x,z].character==null)
            locations.Add(new Vector3(x, 0, z));//TODO Height?!
        nodes[x, z].c = c;
        c++;
        if (checkField(x - 1, z, team, range) && nodeFaster(x - 1, z, c))
            GetMovement(x - 1, z, locations, range - 1, c, team);
        if (checkField(x + 1, z, team, range) && nodeFaster(x + 1, z, c))
            GetMovement(x + 1, z, locations, range - 1, c, team);
        if (checkField(x, z - 1, team, range) && nodeFaster(x, z - 1, c))
            GetMovement(x, z - 1, locations, range - 1, c, team);
        if (checkField(x, z + 1, team, range) && nodeFaster(x, z + 1, c))
            GetMovement(x, z + 1, locations, range - 1, c, team);
    }
    void UpdateCells()
    {
        GetBlockedFields();
        bool[,] blockedArrows = GetBlockedArrows();
        for (int z = 0; z < grid.height; z++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                GameObject cell = fields[x, z].gameObject;
                MeshRenderer meshRenderer = cell.GetComponent<MeshRenderer>();
                MeshFilter meshFilter = cell.GetComponent<MeshFilter>();
               
                //meshRenderer.material = IsCellValid(x, z) ? cellMaterialValid : cellMaterialInvalid;
                //if (!IsCellValid(x, z))
                //    fields[x, z].isAccessible = false;
                meshRenderer.material = blockedFields[x, z] ? cellMaterialInvalid : cellMaterialValid;
                if (blockedFields[x, z])
                {
                    fields[x, z].isAccessible = false;
                    fields[x, z].gameObject.GetComponent<MeshRenderer>().enabled = false;
                    fields[x, z].gameObject.GetComponent<BoxCollider>().enabled = false;
                }
                if (blockedArrows[x, z])
                {
                    fields[x, z].blockArrow = true;
                }
                UpdateMesh(meshFilter.mesh, x, z);
            }
        }
    }

    void UpdateMesh(Mesh mesh, int x, int z)
    {
        mesh.vertices = new Vector3[] {
			MeshVertex(x, z, _heights[z * (grid.width + 1) + x]),
			MeshVertex(x, z + 1,_heights[z * (grid.width + 1) + x]),
			MeshVertex(x + 1, z,_heights[z * (grid.width + 1) + x]),
			MeshVertex(x + 1, z + 1,_heights[z * (grid.width + 1) + x]),
        };
    }

    public bool[,] blockedFields;
    bool[,] GetBlockedArrows()
    {
        RaycastHit hitInfo;
        Vector3 origin;
        bool[,] blockedArrows = new bool[grid.width, grid.height];
        for (int z = 0; z < grid.height; z++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                //Debug.Log(x + " " + z);
                origin = new Vector3(x * cellSize + (cellSize / 2), 200, z * cellSize + (cellSize / 2));
                if (Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask(BLOCK_ARROW_LAYER)))
                {
                    blockedArrows[x, z] = true;
                }
                else
                {
                    //Debug.Log(hitInfo.point);
                    blockedArrows[x, z] = false;
                }

                //Debug.Log("X: " + x + "Y: "+
            }
        }
        return blockedArrows;
    }
    void GetBlockedFields()
    {
        RaycastHit hitInfo;
        Vector3 origin;
        blockedFields = new bool[grid.width, grid.height];
        for (int z = 0; z < grid.height; z++)
        {
            for (int x = 0; x < grid.width; x++)
            {
                //Debug.Log(x + " " + z);
                origin = new Vector3(x * cellSize+(cellSize/2), 200, z * cellSize + (cellSize / 2));
                if (Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask(BLOCK_FIELD_LAYER)))
                {
                    blockedFields[x, z] = true;
                }
                else if (!Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain")))
                {
                    //Debug.Log("FALSE"+x + " " + z);
                    blockedFields[x, z] = true;
                }
                else
                {
                    //Debug.Log(hitInfo.point);
                    blockedFields[x, z] = false;
                }

                //Debug.Log("X: " + x + "Y: "+
            }
        }
    }
    void UpdateHeights()
    {
        RaycastHit hitInfo;
        Vector3 origin;

        for (int z = 0; z < grid.height + 1; z++)
        {
            for (int x = 0; x < grid.width + 1; x++)
            {
                origin = new Vector3(x * cellSize, 200, z * cellSize);
                Physics.Raycast(transform.TransformPoint(origin), Vector3.down, out hitInfo, Mathf.Infinity, LayerMask.GetMask("Terrain"));
                //Debug.Log("X: " + x + "Z: " + z + " " + hitInfo.point);
                _heights[z * (grid.width + 1) + x] = hitInfo.point.y;
                //Debug.Log("X: " + x + "Y: "+
            }
        }
    }

    public void ShowGreenFields(List<Vector3> jumpPositionTargets)
    {
        HideMovement();
        foreach (Vector3 pos in jumpPositionTargets)
        {
           fields[(int)pos.x, (int)pos.z].gameObject.GetComponent<MeshRenderer>().material.mainTexture = healTexture;
        }
    }
}
