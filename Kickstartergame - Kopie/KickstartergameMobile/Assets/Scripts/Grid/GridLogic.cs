using Assets.Scripts.Characters;
using Assets.Scripts.Events;
using Assets.Scripts.GameStates;
using Assets.Scripts.Grid.PathFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class GridLogic
    {
        private MainScript mainScript;
        public Tile[,] Tiles { get; set; }
        public GridManager GridManager { get; set; }
        private GridData grid;
        public GridLogic(GridManager gridManager)
        {
            mainScript = MainScript.GetInstance();
            GridManager = gridManager;
            Tiles = gridManager.Tiles;
            grid = gridManager.grid;
            EventContainer.clickedOnField += FieldClicked;
        }
        public void FieldClicked(int x, int y)
        {
            LivingObject selectedCharacter = mainScript.GetSystem<UnitSelectionManager>().SelectedCharacter;
            if (Tiles[x, y].character == null)
            {
                if (selectedCharacter != null)
                {
                    if (Tiles[x, y].isActive)
                    {
                        int sx = (int)selectedCharacter.GameTransform.GameObject.transform.position.x;
                        int sy = (int)selectedCharacter.GameTransform.GameObject.transform.position.y;
                        int tx = (int)x;
                        int ty = (int)y;
                        List<Vector2> movePath = new List<Vector2>();
                        for (int i = 0; i < MouseManager.oldMousePath.Count; i++)
                        {
                            movePath.Add(new Vector2(MouseManager.oldMousePath[i].x, MouseManager.oldMousePath[i].y));
                        }
                        mainScript.GetSystem<UnitActionManager>().MoveCharacter(selectedCharacter, x, y, movePath, false, new GameplayState());
                        mainScript.gridManager.HideMovement();
                        return;
                    }
                    else
                    { // clicked on Field where he cant walk to
                      //Do nothing
                    }
                }
            }

        }
        public bool IsFieldAttackable(int x, int z)
        {
            return Tiles[x, z].gameObject.GetComponent<MeshRenderer>().material.mainTexture == GridManager.gridRessources.AttackTexture;
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

        public BigTile GetMoveableBigTileFromPosition(Vector2 position, int team, LivingObject character)
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
        public BigTile GetNearestBigTileFromEnemy(LivingObject character)
        {
            Vector2 leftPosition = new Vector2(character.GridPosition.x - 1, character.GridPosition.y);
            Vector2 rightPosition = new Vector2(character.GridPosition.x + 1, character.GridPosition.y);
            Vector2 topPosition = new Vector2(character.GridPosition.x, character.GridPosition.y + 1);
            Vector2 bottomPosition = new Vector2(character.GridPosition.x, character.GridPosition.y - 1);
            if (isValidLocation(leftPosition, character.Player.ID, character))
            {
                BigTile moveOption = GetMoveableBigTileFromPosition(leftPosition, character.Player.ID, character);
                if (moveOption != null)
                    return moveOption;
            }
            if (isValidLocation(rightPosition, character.Player.ID, character))
            {
                BigTile moveOption = GetMoveableBigTileFromPosition(rightPosition, character.Player.ID, character);
                if (moveOption != null)
                    return moveOption;
            }
            if (isValidLocation(topPosition, character.Player.ID, character))
            {
                BigTile moveOption = GetMoveableBigTileFromPosition(topPosition, character.Player.ID, character);
                if (moveOption != null)
                    return moveOption;
            }
            if (isValidLocation(bottomPosition, character.Player.ID, character))
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
            GridManager.PathFindingManager.Reset();
            for (int i = 0; i < grid.width; i++)
            {
                for (int j = 0; j < grid.height; j++)
                {
                    Tiles[i, j].isActive = false;
                }
            }
        }
        public MovementPath getPath(int x, int y, int x2, int y2, int team, bool toadjacentPos, List<int> range)
        {
            MainScript.GetInstance().AttackRangeFromPath = 0;

            GridManager.PathFindingManager.Reset();
            
            return GridManager.PathFindingManager.getPath(x, y, x2, y2, team, toadjacentPos, range);
        }
        public MovementPath GetMonsterPath(Monster monster, BigTile position)
        {
            MainScript.GetInstance().AttackRangeFromPath = 0;
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
            MovementPath p = aStar.GetPath(((BigTilePosition)monster.GridPosition).Position, position);
            return p;
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
            GridManager.PathFindingManager.nodes[x, y].c = c;
            c++;
            if (checkField(x - 1, y, team, range) && GridManager.PathFindingManager.nodeFaster(x - 1, y, c))
                GetMoveLocations(x - 1, y, locations, range - 1, c, team);
            if (checkField(x + 1, y, team, range) && GridManager.PathFindingManager.nodeFaster(x + 1, y, c))
                GetMoveLocations(x + 1, y, locations, range - 1, c, team);
            if (checkField(x, y - 1, team, range) && GridManager.PathFindingManager.nodeFaster(x, y - 1, c))
                GetMoveLocations(x, y - 1, locations, range - 1, c, team);
            if (checkField(x, y + 1, team, range) && GridManager.PathFindingManager.nodeFaster(x, y + 1, c))
                GetMoveLocations(x, y + 1, locations, range - 1, c, team);
        }
        #endregion
        public MovementPath GetMonsterPath(Monster monster, BigTile position, bool adjacent, List<int> attackRanges)
        {
            MainScript.GetInstance().AttackRangeFromPath = 0;
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
        private bool isValidLocation(Vector2 pos, int team, LivingObject character)
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
        private bool isMovableLocation(BigTile position, int team, LivingObject character)
        {

            return isValidLocation(position.BottomLeft(), team, character) && isValidLocation(position.BottomRight(), team, character) && isValidLocation(position.TopLeft(), team, character) && isValidLocation(position.TopRight(), team, character);
        }
    }
}
