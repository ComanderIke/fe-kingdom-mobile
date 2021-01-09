using Game.GameActors.Units;

namespace Game.Grid
{
    public class Tile
    {
        public IGridActor Actor;
        public readonly int X;
        public readonly int Y;
      //  public bool IsActive;
     //   public bool IsAttackable;
        
        public TileType TileType;
        public ITileRenderer TileRenderer;

        public Tile(int i, int j, TileType tileType, ITileRenderer tileRenderer)
        {
            X = i;
            Y = j;
            TileType = tileType;
            TileRenderer = tileRenderer;
            
        }

        public void Reset()
        {
            TileRenderer.Reset();
          //  IsActive = false;
          //  IsAttackable = false;
        }

        public void SetMaterialAttack()
        {
            // if (IsAttackable || IsActive)
            //     return;
            SetAttackFieldMaterial();
        }
        public void SetMaterial( int playerId)
        {

            // if (IsAttackable || IsActive)
            //     return;
            if (Actor != null && Actor.FactionId == playerId)
            {
                TileRenderer.AllyVisual();
                //IsActive = true;
            }
            else
            {
                TileRenderer.MoveVisual();
                //    IsActive = true;
            }
            if (Actor != null &&
                Actor.FactionId != playerId)
            {
                SetAttackFieldMaterial();
            }
        }
        private void SetAttackFieldMaterial()
        {
            TileRenderer.AttackVisual();
            //OnRenderEnemyTile?.Invoke(X, Y, Unit, playerId);
           //IsAttackable = true;

        }

        public bool HasFreeSpace()
        {
            return Actor == null;
        }

        public bool ContainsEnemyTo(IGridActor actor)
        {
            if (Actor == null)
                return false;
            return Actor.FactionId != actor.FactionId;
        }
    }
}