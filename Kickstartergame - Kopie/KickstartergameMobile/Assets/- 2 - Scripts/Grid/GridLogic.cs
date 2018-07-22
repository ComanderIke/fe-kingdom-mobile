using Assets.Scripts.Characters;
using Assets.Scripts.GameStates;
using Assets.Scripts.Grid;
using Assets.Scripts.Grid.PathFinding;
using System;
using System.Collections.Generic;
using UnityEngine;


public class GridLogic
{
    private MainScript mainScript;
    public Tile[,] Tiles { get; set; }
    public MapSystem GridManager { get; set; }
    private Grid grid;

    public GridLogic(global::MapSystem gridManager)
    {
        mainScript = MainScript.instance;
        GridManager = gridManager;
        Tiles = gridManager.Tiles;
        grid = gridManager.grid;
        InputSystem.onClickedField += FieldClicked;
    }
    public void FieldClicked(int x, int y)
    {
        Unit selectedCharacter = mainScript.GetSystem<UnitSelectionSystem>().SelectedCharacter;
        if (Tiles[x, y].character == null)
        {
            if (selectedCharacter != null)
            {
                if (Tiles[x, y].isActive)
                {
                    List<Vector2> movePath = new List<Vector2>();
                    for (int i = 0; i < GridManager.gridRessources.preferedPath.path.Count; i++)
                    {
                        movePath.Add(new Vector2(GridManager.gridRessources.preferedPath.path[i].x, GridManager.gridRessources.preferedPath.path[i].y));
                    }
                    mainScript.GetSystem<UnitActionSystem>().MoveCharacter(selectedCharacter, x, y, movePath);
                    UnitActionSystem.onAllCommandsFinished += SwitchToGamePlayState;
                    Debug.Log("All Commands Setup");
                    mainScript.GetSystem<UnitActionSystem>().ExecuteActions();

                    mainScript.GetSystem<MapSystem>().HideMovement();
                    return;
                }
                else
                { // clicked on Field where he cant walk to
                  //Do nothing
                }
            }
        }

    }
    private void SwitchToGamePlayState()
    {
        Debug.Log("Switch State Commands Delete");
        UnitActionSystem.onAllCommandsFinished -= SwitchToGamePlayState;
        mainScript.GameStateManager.SwitchState(GameStateManager.GameplayState);
    }
    public List<Unit> GetAttackTargets(Unit unit)
    {
        int x = unit.GridPosition.x;
        int y = unit.GridPosition.y;
        List<Unit> targets = new List<Unit>();
        foreach (int attackRange in unit.Stats.AttackRanges)
        {
            for (int i = -attackRange; i <= +attackRange; i++)
            {
                for (int j = -attackRange; j <= attackRange; j++)
                {
                    if (Math.Abs(j + i) == attackRange)
                    {
                        // Debug.Log("attackTargets at "+ unit.GridPosition.GetPos()+": " +(i + x) + " " + (j + y));
                        if (IsOutOfBounds(new Vector2(x + i, y + j)))
                            continue;
                        Unit l = Tiles[i + x, j + y].character;
                        if (l != null && l.Player.ID != unit.Player.ID)
                        {

                            targets.Add(l);
                        }
                    }
                }
            }
        }
        return targets;
    }
    public List<Unit> GetAttackTargetsAtGameObjectPosition(Unit unit)
    {
        int x = (int)unit.GameTransform.GetPosition().x;
        int y = (int)unit.GameTransform.GetPosition().y;
        List<Unit> targets = new List<Unit>();
        foreach (int attackRange in unit.Stats.AttackRanges)
        {
            for (int i = -attackRange; i <= +attackRange; i++)
            {
                for (int j = -attackRange; j <= attackRange; j++)
                {
                    if (Math.Abs(j + i) == attackRange)
                    {
                        // Debug.Log("attackTargets at "+ unit.GridPosition.GetPos()+": " +(i + x) + " " + (j + y));
                        if (IsOutOfBounds(new Vector2(x + i, y + j)))
                            continue;
                        Unit l = Tiles[i + x, j + y].character;
                        if (l != null && l.Player.ID != unit.Player.ID)
                        {

                            targets.Add(l);
                        }
                    }
                }
            }
        }
        return targets;
    }
    public bool IsFieldAttackable(int x, int z)
    {
        Debug.Log(x + " " + z + " " + Tiles[x, z].gameObject.GetComponent<MeshRenderer>().sharedMaterial.name + " " + GridManager.gridRessources.cellMaterialAttack.name);
        return Tiles[x, z].gameObject.GetComponent<MeshRenderer>().sharedMaterial == GridManager.gridRessources.cellMaterialAttack;
    }
    public bool IsOutOfBounds(Vector2 pos)
    {
        return pos.x < 0 || pos.y < 0 || pos.x >= grid.width || pos.y >= grid.height;
    }
    private bool IsValidAndActive(Vector2 pos, int team)
    {
        bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= grid.width) || (pos.y >= grid.height);
        if (!invalid)
        {
            invalid = !Tiles[(int)pos.x, (int)pos.y].isAccessible;
            if (Tiles[(int)pos.x, (int)pos.y].character != null)
                if (Tiles[(int)pos.x, (int)pos.y].character.Player.ID == team)
                    invalid = false;
            if (!Tiles[(int)pos.x, (int)pos.y].isActive)
                invalid = true;
        }
        return !invalid;
    }
    public bool IsValidAndActive(BigTile position, int team)
    {
        return IsValidAndActive(position.BottomLeft(), team) && IsValidAndActive(position.BottomRight(), team) && IsValidAndActive(position.TopLeft(), team) && IsValidAndActive(position.TopRight(), team);
    }

    public BigTile GetMoveableBigTileFromPosition(Vector2 position, int team, Unit character)
    {
        BigTile leftTop = new BigTile(new Vector2(position.x - 1, position.y), new Vector2(position.x, position.y), new Vector2(position.x - 1, position.y + 1), new Vector2(position.x, position.y + 1));
        BigTile leftBottom = new BigTile(new Vector2(position.x - 1, position.y - 1), new Vector2(position.x, position.y - 1), new Vector2(position.x - 1, position.y), new Vector2(position.x, position.y));
        BigTile rightTop = new BigTile(new Vector2(position.x, position.y), new Vector2(position.x + 1, position.y), new Vector2(position.x, position.y + 1), new Vector2(position.x + 1, position.y + 1));
        BigTile rightBottom = new BigTile(new Vector2(position.x, position.y - 1), new Vector2(position.x + 1, position.y - 1), new Vector2(position.x, position.y), new Vector2(position.x + 1, position.y));
        if (isMovableLocation(leftTop, team, character))
            return leftTop;
        if (isMovableLocation(leftBottom, team, character))
            return leftBottom;
        if (isMovableLocation(rightTop, team, character))
            return rightTop;
        if (isMovableLocation(rightBottom, team, character))
            return rightBottom;
        return null;
    }
    public BigTile GetNearestBigTileFromEnemy(Unit character)
    {
        Vector2 leftPosition = new Vector2(character.GridPosition.x - 1, character.GridPosition.y);
        Vector2 rightPosition = new Vector2(character.GridPosition.x + 1, character.GridPosition.y);
        Vector2 topPosition = new Vector2(character.GridPosition.x, character.GridPosition.y + 1);
        Vector2 bottomPosition = new Vector2(character.GridPosition.x, character.GridPosition.y - 1);
        if (IsValidLocation(leftPosition, character))
        {
            BigTile moveOption = GetMoveableBigTileFromPosition(leftPosition, character.Player.ID, character);
            if (moveOption != null)
                return moveOption;
        }
        if (IsValidLocation(rightPosition, character))
        {
            BigTile moveOption = GetMoveableBigTileFromPosition(rightPosition, character.Player.ID, character);
            if (moveOption != null)
                return moveOption;
        }
        if (IsValidLocation(topPosition, character))
        {
            BigTile moveOption = GetMoveableBigTileFromPosition(topPosition, character.Player.ID, character);
            if (moveOption != null)
                return moveOption;
        }
        if (IsValidLocation(bottomPosition, character))
        {
            BigTile moveOption = GetMoveableBigTileFromPosition(bottomPosition, character.Player.ID, character);
            if (moveOption != null)
                return moveOption;
        }

        return null;
    }
    public bool checkField(int x, int y, int team, int range)
    {

        if (x >= 0 && y >= 0 && x < grid.width && y < grid.height)
        {
            Tile field = Tiles[x, y];
            if (field.isAccessible)
            {

                if (field.character == null)
                    return true;
                if (field.character.Player.ID == team)
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
    public bool CheckAttackField(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < grid.width && y < grid.height)
        {
            return true;
        }
        return false;
    }
    public bool checkMonsterField(BigTile bigTile, int team, int range)
    {
        return checkField((int)bigTile.BottomLeft().x, (int)bigTile.BottomLeft().y, team, range) && checkField((int)bigTile.BottomRight().x, (int)bigTile.BottomRight().y, team, range) && checkField((int)bigTile.TopLeft().x, (int)bigTile.TopLeft().y, team, range) && checkField((int)bigTile.TopRight().x, (int)bigTile.TopRight().y, team, range);

    }
    bool IsCellValid(int x, int z)
    {
        return true;
    }
    public bool IsValidLocation(int team, int sx, int sy, int x, int y, bool isAdjacent)
    {
        bool invalid = (x < 0) || (y < 0) || (x >= grid.width) || (y >= grid.height);
        if ((!invalid) && ((sx != x) || (sy != y)))
        {
            invalid = !Tiles[x, y].isAccessible;
        }

        if (!invalid)
        {
            //Material m = fields[x, y].gameObject.GetComponent<MeshRenderer>().material;
            //if (m.mainTexture != MoveTexture)
            //    invalid = true;
            if (Tiles[x, y].character != null)
            {
                if (Tiles[x, y].character.Player.ID != team)
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
    public void ResetActiveFields()
    {
        GridManager.nodeHelper.Reset();
        for (int i = 0; i < grid.width; i++)
        {
            for (int j = 0; j < grid.height; j++)
            {
                Tiles[i, j].isActive = false;
            }
        }
    }
    
    #region AIHELP
    public void GetMoveLocations(int x, int y, List<Vector2> locations, int range, int c, int team)
    {
        if (range < 0)
        {
            return;
        }
        if (!locations.Contains(new Vector2(x, y)) && Tiles[x, y].character == null)
            locations.Add(new Vector3(x, y));//TODO Height?!
        GridManager.nodeHelper.nodes[x, y].c = c;
        c++;
        if (checkField(x - 1, y, team, range) && GridManager.nodeHelper.nodeFaster(x - 1, y, c))
            GetMoveLocations(x - 1, y, locations, range - 1, c, team);
        if (checkField(x + 1, y, team, range) && GridManager.nodeHelper.nodeFaster(x + 1, y, c))
            GetMoveLocations(x + 1, y, locations, range - 1, c, team);
        if (checkField(x, y - 1, team, range) && GridManager.nodeHelper.nodeFaster(x, y - 1, c))
            GetMoveLocations(x, y - 1, locations, range - 1, c, team);
        if (checkField(x, y + 1, team, range) && GridManager.nodeHelper.nodeFaster(x, y + 1, c))
            GetMoveLocations(x, y + 1, locations, range - 1, c, team);
    }
    #endregion
    public MovementPath GetMonsterPath(Monster monster, BigTile position, bool adjacent, List<int> attackRanges)
    {
        MainScript.instance.GetSystem<InputSystem>().AttackRangeFromPath = 0;
        PathFindingNode[,] nodes = new PathFindingNode[grid.width, grid.height];
        for (int x = 0; x < grid.width; x++)
        {
            for (int y = 0; y < grid.height; y++)
            {
                bool isAccesible = Tiles[x, y].isAccessible;
                if (Tiles[x, y].character != null && Tiles[x, y].character.Player.ID != monster.Player.ID)
                    isAccesible = false;
                nodes[x, y] = new PathFindingNode(x, y, isAccesible);
            }
        }
        AStar2x2 aStar = new AStar2x2(nodes);
        MovementPath p = aStar.GetPath(((BigTilePosition)monster.GridPosition).Position, position, monster.Player.ID, adjacent, attackRanges);
        return p;
    }
    public bool IsValidLocation(Vector2 pos, Unit character)
    {
        bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= grid.width) || (pos.y >= grid.height);

        if (!invalid)
        {
            if (!Tiles[(int)pos.x, (int)pos.y].isActive)
                return false;
            invalid = !Tiles[(int)pos.x, (int)pos.y].isAccessible;
            if (Tiles[(int)pos.x, (int)pos.y].character != null)
            {
                if (Tiles[(int)pos.x, (int)pos.y].character != character)
                    invalid = true;
            }
        }

        return !invalid;
    }
    public bool IsTileAccessible(Vector2 pos, Unit character)
    {
        bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= grid.width) || (pos.y >= grid.height);

        if (!invalid)
        {
            invalid = !Tiles[(int)pos.x, (int)pos.y].isAccessible;
            if (Tiles[(int)pos.x, (int)pos.y].character != null)
            {
                if (Tiles[(int)pos.x, (int)pos.y].character != character)
                    invalid = true;
            }
        }

        return !invalid;
    }
    public bool IsTileAccessible(Vector2 pos)
    {
        bool invalid = (pos.x < 0) || (pos.y < 0) || (pos.x >= grid.width) || (pos.y >= grid.height);

        if (!invalid)
        {
            invalid = !Tiles[(int)pos.x, (int)pos.y].isAccessible;
        }

        return !invalid;
    }
    private bool isMovableLocation(BigTile position, int team, Unit character)
    {

        return IsValidLocation(position.BottomLeft(), character) && IsValidLocation(position.BottomRight(), character) && IsValidLocation(position.TopLeft(), character) && IsValidLocation(position.TopRight(), character);
    }
    public bool IsBigTileAccessible(BigTile position, Unit character)
    {

        return IsTileAccessible(position.BottomLeft(), character) && IsTileAccessible(position.BottomRight(), character) && IsTileAccessible(position.TopLeft(), character) && IsTileAccessible(position.TopRight(), character);
    }
    public bool IsBigTileAccessible(BigTile position)
    {

        return IsTileAccessible(position.BottomLeft()) && IsTileAccessible(position.BottomRight()) && IsTileAccessible(position.TopLeft()) && IsTileAccessible(position.TopRight());
    }


}
