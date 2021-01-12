using Game.GameActors.Units;
using UnityEngine;

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
        public readonly ITileRenderer TileRenderer;

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
        }

        public void SetMaterialAttack(int playerId)
        {
            TileRenderer.SetVisualStyle(playerId);
            TileRenderer.AttackVisual();
        }
        public void SetMaterial( int playerId)
        {
            TileRenderer.SetVisualStyle(playerId);
            if (Actor == null)
            {
                TileRenderer.MoveVisual();
            }
            else if (Actor.FactionId == playerId)
            {
                TileRenderer.AllyVisual();
            }
            else
            {
                SetMaterialAttack(playerId);
            }
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