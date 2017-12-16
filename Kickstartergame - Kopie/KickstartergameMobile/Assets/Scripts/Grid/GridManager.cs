using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Characters;
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.PathFinding;

public class GridManager : MonoBehaviour {
    

    public const float GRID_X_OFFSET = 0.25f;

    [SerializeField]
    public GridData grid;
    public GridBuilder GridBuilder { get; set; }

    [SerializeField]
    public GridRessources gridRessources;

    public GridRenderer GridRenderer { get; set; }
    public GridLogic GridLogic { get; set; }
    
    public Tile[,] Tiles { get; set; }
    public AStar PathFindingManager { get; set; }

    void Start() {
        Tiles = new Tile[grid.width, grid.height];
        GridBuilder = new GridBuilder(this);
         GridRenderer = new GridRenderer(this);
        GridLogic = new GridLogic(this);
        PathFindingManager = new AStar(this,grid.width, grid.height);
    }



    public void ShowMovement(LivingObject c)
    {
       
        if (c is Monster)
            ShowMonsterMovement(c.GridPosition.x, c.GridPosition.y, c.Stats.MoveRange, new List<int>(c.Stats.AttackRanges), 0, c.Player.ID);
        else
            ShowMovement(c.GridPosition.x, c.GridPosition.y, c.Stats.MoveRange, c.Stats.MoveRange, new List<int>(c.Stats.AttackRanges), 0, c.Player.ID, false);

    }
    private void ShowMonsterMovement(int x, int y, int range, List<int> attack, int c, int playerId)
    {
        if (range < 0)
        {
            return;
        }
        GridRenderer.SetBigTileActive(new BigTile(new Vector2(x, y), new Vector2(x + 1, y), new Vector2(x, y + 1), new Vector2(x + 1, y + 1)), playerId,false);
        if (GridLogic.checkMonsterField(new BigTile(new Vector2(x - 1, y), new Vector2(x, y), new Vector2(x - 1, y + 1), new Vector2(x, y + 1)), playerId, range))
            ShowMonsterMovement(x - 1, y, range - 1, new List<int>(attack), c, playerId);
        if (GridLogic.checkMonsterField(new BigTile(new Vector2(x + 1, y), new Vector2(x + 2, y), new Vector2(x + 1, y + 1), new Vector2(x + 2, y + 1)), playerId, range))
            ShowMonsterMovement(x + 1, y, range - 1, new List<int>(attack), c, playerId);
        if (GridLogic.checkMonsterField(new BigTile(new Vector2(x, y - 1), new Vector2(x + 1, y - 1), new Vector2(x, y), new Vector2(x + 1, y)), playerId, range))
            ShowMonsterMovement(x, y - 1, range - 1, new List<int>(attack), c, playerId);
        if (GridLogic.checkMonsterField(new BigTile(new Vector2(x, y + 1), new Vector2(x + 1, y + 1), new Vector2(x, y + 2), new Vector2(x + 1, y + 2)), playerId, range))
            ShowMonsterMovement(x, y + 1, range - 1, new List<int>(attack), c, playerId);
    }
    public void ShowAttack(LivingObject character, List<int> attack)
    {
        List<Tile> TilesFromWhereUCanAttack = new List<Tile>();
        foreach (Tile f in Tiles)
        {
            if (f.isActive)
            {
                TilesFromWhereUCanAttack.Add(f);
            }
        }
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                PathFindingManager.nodes[i, j].c = 1000;
            }
        }
        foreach (Tile f in TilesFromWhereUCanAttack)
        {

            int x = f.x;
            int y = f.y;
            foreach (int range in attack)
            {
                ShowAttackRecursive(character, x, y, range, new List<int>());

            }
        }
        GridRenderer.ShowStandOnTexture(character);
        StartCoroutine(GridRenderer.FieldAnimation());

    }
   
    public void HideMovement()
    {
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                MeshRenderer m = Tiles[i, j].gameObject.GetComponent<MeshRenderer>();
                if (Tiles[i, j].isAccessible)
                    m.material.mainTexture = gridRessources.StandardTexture;
                else
                {
                    m.material = gridRessources.cellMaterialInvalid;
                }
                PathFindingManager.nodes[i, j].c = 1000;
                Tiles[i, j].isActive = false;
            }
        }
    }
    public void ShowAttackRecursive(LivingObject character, int x, int y, int range, List<int> direction)
    {
        MeshRenderer m = Tiles[x, y].gameObject.GetComponent<MeshRenderer>();


        if (range <= 0)
        {
            GridRenderer.SetFieldTexture(new Vector2(x, y), character.Player.ID, true);
            
            return;
        }
        if (!direction.Contains(2))
        {
            if (GridLogic.CheckAttackField(x + 1, y))
            { //&& AttackNodeFaster(x+1, y, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                ShowAttackRecursive(character, x + 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(1))
        {
            if (GridLogic.CheckAttackField(x - 1, y))
            { //&& AttackNodeFaster(x - 1, y, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                ShowAttackRecursive(character, x - 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(4))
        {
            if (GridLogic.CheckAttackField(x, y + 1))
            {// && AttackNodeFaster(x , y+1, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                ShowAttackRecursive(character, x, y + 1, range - 1, newdirection);
            }
        }
        if (!direction.Contains(3))
        {
            if (GridLogic.CheckAttackField(x, y - 1))
            { //&& AttackNodeFaster(x , y-1, c))
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                ShowAttackRecursive(character, x, y - 1, range - 1, newdirection);
            }
        }

    }
    public void ShowAttackRanges(int x, int y, int range, List<int> direction)
    {
        if (range <= 0)
        {
            LivingObject c = Tiles[x, y].character;
            MeshRenderer m = Tiles[x, y].gameObject.GetComponent<MeshRenderer>();
            m.material.mainTexture = gridRessources.AttackTexture;
            return;
        }
        if (!direction.Contains(2))
        {
            if (GridLogic.CheckAttackField(x + 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(1);
                ShowAttackRanges(x + 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(1))
        {
            if (GridLogic.CheckAttackField(x - 1, y))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(2);
                ShowAttackRanges(x - 1, y, range - 1, newdirection);
            }
        }
        if (!direction.Contains(4))
        {
            if (GridLogic.CheckAttackField(x, y + 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(3);
                ShowAttackRanges(x, y + 1, range - 1, newdirection);
            }
        }
        if (!direction.Contains(3))
        {
            if (GridLogic.CheckAttackField(x, y - 1))
            {
                List<int> newdirection = new List<int>(direction);
                newdirection.Add(4);
                ShowAttackRanges(x, y - 1, range - 1, newdirection);
            }
        }

    }

    public void ShowAttackRange(LivingObject c)
    {
        List<LivingObject> characters = new List<LivingObject>();
        int x = c.GridPosition.x;
        int z = c.GridPosition.y;
        foreach (int range in c.Stats.AttackRanges)
        {
            ShowAttackRanges(x, z, range, new List<int>());
        }
    }
    private void ShowMovement(int x, int y, int range, int attackIndex, List<int> attack, int c, int playerId, bool enemy)
    {
        if (range < 0)
        {
            return;
        }
        if (!enemy)
        {
            GridRenderer.SetFieldTexture(new Vector2(x, y), playerId,false);
        }
        PathFindingManager.nodes[x, y].c = c;
        c++;
        if (GridLogic.checkField(x - 1, y, playerId, range) && PathFindingManager.nodeFaster(x - 1, y, c))
            ShowMovement(x - 1, y, range - 1, attackIndex, new List<int>(attack), c, playerId, enemy);
        if (GridLogic.checkField(x + 1, y, playerId, range) && PathFindingManager.nodeFaster(x + 1, y, c))
            ShowMovement(x + 1, y, range - 1, attackIndex, new List<int>(attack), c, playerId, enemy);
        if (GridLogic.checkField(x, y - 1, playerId, range) && PathFindingManager.nodeFaster(x, y - 1, c))
            ShowMovement(x, y - 1, range - 1, attackIndex, new List<int>(attack), c, playerId, enemy);
        if (GridLogic.checkField(x, y + 1, playerId, range) && PathFindingManager.nodeFaster(x, y + 1, c))
            ShowMovement(x, y + 1, range - 1, attackIndex, new List<int>(attack), c, playerId, enemy);
    }
}
