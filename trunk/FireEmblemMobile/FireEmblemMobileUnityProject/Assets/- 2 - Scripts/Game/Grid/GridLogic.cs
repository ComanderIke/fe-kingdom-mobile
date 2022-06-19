using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using UnityEngine;

namespace Game.Grid
{
    public class GridLogic
    {
        private readonly GridGameManager gridGameManager;
        private Tile[,] Tiles { get; }
        private GridSystem GridManager { get; }
        public GridSessionData gridSessionData;
        public ITileChecker tileChecker { get; set; }

        public GridLogic(GridSystem gridManager)
        {
            gridGameManager = GridGameManager.Instance;
            GridManager = gridManager;
            gridSessionData = new GridSessionData();
            Tiles = gridManager.Tiles;

        }

        

        public List<IGridObject> GetAttackTargets(IGridActor unit)
        {
            int x = unit.GridComponent.GridPosition.X;
            int y = unit.GridComponent.GridPosition.Y;
            return GetAttackTargets(unit, x, y);
        }
        public List<IGridObject> GetAttackTargets(IGridActor unit, int x, int y)
        {
            var targets = new List<IGridObject>();
          
            foreach (int attackRange in unit.AttackRanges)
            {
                Debug.Log("AttackRanges: " +attackRange);
                for (int i = -attackRange; i <= +attackRange; i++)
                {
                    for (int j = -attackRange; j <= attackRange; j++)
                    {
                        if (Math.Abs(j) + Math.Abs(i) == attackRange)
                        {
                            Debug.Log("Check Position: "+(i + x) + " " + (j + y));
                            if (IsOutOfBounds(new Vector2(x + i, y + j)))
                                continue;
                            var unitOnTile = Tiles[i + x, j + y].GridObject;
                            Debug.Log("Unit On Tile: "+unitOnTile);
                            if (unitOnTile != null && unitOnTile.Faction.Id != unit.Faction.Id)
                            {
                                targets.Add(unitOnTile);
                            }
                        }
                    }
                }
            }

            return targets;
        }

        public List<IGridObject> GetAttackTargetsAtGameObjectPosition(Unit unit)
        {
            int x = (int) unit.GameTransformManager.GetPosition().x;
            int y = (int) unit.GameTransformManager.GetPosition().y;
            var targets = new List<IGridObject>();
            foreach (int attackRange in unit.Stats.AttackRanges)
            {
                for (int i = -attackRange; i <= +attackRange; i++)
                {
                    for (int j = -attackRange; j <= attackRange; j++)
                    {
                        if (Math.Abs(j + i) == attackRange || Math.Abs(j) + Math.Abs(i) == attackRange)
                        {
                            //Debug.Log("attackTargets at ["+ x+", "+y+"]: [" +(i + x) + ", " + (j + y)+"]");
                            if (IsOutOfBounds(new Vector2(x + i, y + j)))
                                continue;
                            var unitOnTile = Tiles[i + x, j + y].GridObject;
                            if (unitOnTile != null && unitOnTile.Faction.Id != unit.Faction.Id)
                            {
                                targets.Add(unitOnTile);
                            }
                        }
                    }
                }
            }

            //Debug.Log("Attackable Targets at Position: [" + x + " ," + y + "] and AttackRange of: ");
            //foreach (int attackRange in unit.Stats.AttackRanges)
            //{
            //    Debug.Log(attackRange +", ");
            //}
            //foreach (Unit target in targets)
            //{
            //    Debug.Log(target.Name + " [" + target.GridPosition.X + ", " + target.GridPosition.Y+"]");
            //}
            return targets;
        }

        public bool IsFieldAttackable(int x, int y)
        {
            return gridSessionData.IsAttackableAndActive(x, y);
        }

        public bool IsOutOfBounds(Vector2 pos)
        {
            return pos.x < 0 || pos.y < 0 || pos.x >= GridManager.width || pos.y >= GridManager.height;
        }

        public bool CheckField(int x, int y, IGridActor unit, int range)
        {
            if (x >= 0 && y >= 0 && x < GridManager.width && y < GridManager.height)
            {
                var field = Tiles[x, y];
                if (unit.GetActorGridComponent().CanMoveOnTo(field))
                {
                    if (field.GridObject == null)
                        return true;
                    if (!field.GridObject.IsEnemy(unit))
                        return true;
                }
                else
                {
                    return range < 0;
                }
            }

            return false;
        }

        public bool CheckAttackField(int x, int y)
        {
            return x >= 0 && y >= 0 && x < GridManager.width && y < GridManager.height;
        }

        public bool IsFieldFreeAndActive(int x, int y)
        {
            return Tiles[x, y].GridObject == null && gridSessionData.IsMoveableAndActive(x, y);
        }

        public void ResetActiveFields()
        {
            GridManager.NodeHelper.Reset();
            gridSessionData.Clear();
        }

        #region AIHELP

        public IEnumerable<Vector2Int> GetMoveLocations(IGridActor unit)
        {
            var locations = new List<Vector2Int>();
            GetMoveLocations(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y, locations,
                unit.MovementRange, 0, unit);
            return locations;
        }
        private void GetMoveLocations(int x, int y, List<Vector2Int> locations, int range, int c, IGridActor unit)
        {
            if (range < 0)
            {
                return;
            }

            if (!locations.Contains(new Vector2Int(x, y)) && Tiles[x, y].GridObject == null)
                locations.Add(new Vector2Int(x, y)); //TODO Height?!
            GridManager.NodeHelper.Nodes[x, y].C = c;
            c++;
            if (CheckField(x - 1, y, unit, range) && GridManager.NodeHelper.NodeFaster(x - 1, y, c))
                GetMoveLocations(x - 1, y, locations, range - 1, c, unit);
            if (CheckField(x + 1, y, unit, range) && GridManager.NodeHelper.NodeFaster(x + 1, y, c))
                GetMoveLocations(x + 1, y, locations, range - 1, c, unit);
            if (CheckField(x, y - 1, unit, range) && GridManager.NodeHelper.NodeFaster(x, y - 1, c))
                GetMoveLocations(x, y - 1, locations, range - 1, c, unit);
            if (CheckField(x, y + 1, unit, range) && GridManager.NodeHelper.NodeFaster(x, y + 1, c))
                GetMoveLocations(x, y + 1, locations, range - 1, c, unit);
        }

        #endregion

        public bool IsValidLocation(IGridActor unit, int sx, int sy, int x, int y, bool isAdjacent)
        {
            bool invalid = (x < 0) || (y < 0) || (x >= GridManager.width) || (y >= GridManager.height);
            var tile = Tiles[x, y];
            if ((!invalid) && ((sx != x) || (sy != y)))
            {
                invalid = !unit.GetActorGridComponent().CanMoveOnTo(tile);
            }

            if (!invalid)
            {
                if (tile.GridObject != null)
                {
                    if (tile.GridObject.IsEnemy(unit))
                    {
                        invalid = true;
                    }

                    if (isAdjacent)
                    {
                        //Do nothing will be checked with attackRange Later Passthrough is ok but not stopping on it
                    }
                }
            }

            return !invalid;
        }

        public bool IsTileAccessible(int x, int y, IGridActor character)
        {
            bool invalid = (x < 0) || (y < 0) || (x >= GridManager.width) || (y >= GridManager.height);
            var tile = Tiles[x, y];
            if (!invalid)
            {
                invalid = !character.GetActorGridComponent().CanMoveOnTo(tile);
                if (tile.GridObject != null)
                {
                    if (tile.GridObject != character)
                        invalid = true;
                }
            }

            return !invalid;
        }

        public bool IsTileFree(int x, int y)
        {
            return Tiles[x, y].HasFreeSpace();
        }

        public IEnumerable<Tile> TilesFromWhereYouCanAttack(IGridActor character)
        {
            return (from Tile f in Tiles
                where (f.X == character.GridComponent.GridPosition.X && f.Y == character.GridComponent.GridPosition.Y) ||
                      (gridSessionData.IsMoveableAndActive(f.X, f.Y) && (f.GridObject == null || f.GridObject == character))
                select f);
            //Todo fix on soft select tiles are not active so attack range from enemies not visible
        }

        public bool IsMoveableAndActive(int x, int y)
        {
            return gridSessionData.IsMoveableAndActive(x, y);
        }

        public bool IsAttackableAndActive(int x, int y)
        {
            return gridSessionData.IsAttackableAndActive(x, y);
        }

        public bool IsFieldTargetable(int x, int y)
        {
            return gridSessionData.IsTargetable(x,y);
        }
    }
}