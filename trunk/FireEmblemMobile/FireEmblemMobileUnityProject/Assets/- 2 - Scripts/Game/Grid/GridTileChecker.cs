﻿using Game.GameActors.Units;
using Game.Mechanics;
using UnityEngine;

namespace Game.Grid
{
    public class GridTileChecker : ITileChecker
    {
        private int width;
        private int height;
        private Tile[,] tiles;
        public GridTileChecker(Tile[,]tiles, int width, int height)
        {
            this.width = width;
            this.height = height;
            this.tiles = tiles;
        }
        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }
        public bool CheckField(int x, int y, IGridActor unit, int range)
        {
            if (x >= 0 && y >= 0 && x < width && y < height)
            {
                var field = tiles[x, y];
                if (unit.GridComponent.CanMoveOnTo(field))
                {
                    if (field.Actor == null)
                        return true;
                    if (!field.Actor.IsEnemy(unit))
                        return true;
                }
                else
                {
                    return range < 0;
                }
            }

            return false;
        }

        public bool IsTileAccessible(int x, int y, IGridActor unit)
        {
            bool invalid = (x < 0) || (y < 0) || (x >= width) || (y >= height);

            if (!invalid)
            {
                var tile = tiles[x, y];
                invalid = !unit.GridComponent.CanMoveOnTo(tile);
                if (tile.Actor != null)
                {
                    if (tile.Actor != unit)
                        invalid = true;
                }
            }

            return !invalid;
        }

        public bool IsTileFree(int x, int y)
        {
            return tiles[x, y].HasFreeSpace();
        }

        public int GetMovementCost(int x, int y, IGridActor unit)
        {
            return tiles[x, y].TileType.GetMovementCost(unit.MoveType);
        }

        public bool IsValidLocation(IGridActor unit, int sx, int sy, int x, int y, bool isAdjacent)
        {
            bool invalid = (x < 0) || (y < 0) || (x >= width) || (y >= height);
            if (invalid)
                return false;
            var tile = tiles[x, y];
            if ((sx != x) || (sy != y))
            {
                invalid = !unit.GridComponent.CanMoveOnTo(tile);
            }

            if (!invalid)
            {
                if (tile.Actor != null)
                {
                    if (tile.Actor.GridComponent.CanMoveThrough(unit))
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
    }
}