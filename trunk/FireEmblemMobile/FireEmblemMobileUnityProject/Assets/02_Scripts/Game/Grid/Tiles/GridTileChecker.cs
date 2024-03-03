using Game.GameActors.Units.Interfaces;
using Game.States.Mechanics;

namespace Game.Grid.Tiles
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

        public bool IsTileAccessible(int x, int y, IGridActor unit)
        {
            bool invalid = (x < 0) || (y < 0) || (x >= width) || (y >= height);

            if (!invalid)
            {
                var tile = tiles[x, y];
                invalid = !unit.GetActorGridComponent().CanMoveOnTo(tile);
                if (tile.GridObject != null)
                {
                    if (tile.GridObject != unit)
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
            return unit.GetActorGridComponent().GetMovementCosts(tiles[x, y].TileData.TerrainType);
           // return tiles[x, y].TileData.GetMovementCost(unit.MoveType);
        }

        public bool IsValidLocation(IGridActor unit, int sx, int sy, int x, int y)
        {
            bool invalid = (x < 0) || (y < 0) || (x >= width) || (y >= height);
            if (invalid)
                return false;
            var tile = tiles[x, y];
            if ((sx != x) || (sy != y))
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
                }
            }

            return !invalid;
        }
    }
}