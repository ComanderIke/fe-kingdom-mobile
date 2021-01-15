using Game.GameActors.Units;
using Game.Mechanics;

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
                if (unit.CanMoveOnTo(field))
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
            var tile = tiles[x, y];
            if (!invalid)
            {
                invalid = !unit.CanMoveOnTo(tile);
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

        public bool IsValidLocation(IGridActor unit, int sx, int sy, int x, int y, bool isAdjacent)
        {
            bool invalid = (x < 0) || (y < 0) || (x >= width) || (y >= height);
            if (invalid)
                return true;
            var tile = tiles[x, y];
            if ((sx != x) || (sy != y))
            {
                invalid = !unit.CanMoveOnTo(tile);
            }

            if (!invalid)
            {
                if (tile.Actor != null)
                {
                    if (tile.Actor.CanMoveThrough(unit))
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