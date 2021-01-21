using Game.GameActors.Units;
using Game.Graphics;
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
        private TileEffectVisual TileVfx;

        public Tile(int i, int j, TileType tileType, ITileRenderer tileRenderer, TileEffectVisual tileVfx)
        {
            X = i;
            Y = j;
            TileType = tileType;
            TileRenderer = tileRenderer;
            TileVfx = tileVfx;

        }

        public void Reset()
        {
            TileRenderer.Reset();
        }

        public void SetAttackMaterial(int playerId, bool activeUnit)
        {
            TileRenderer.SetVisualStyle(playerId);
            if (Actor!=null && playerId != Actor.FactionId)
            {
                TileVfx.ShowAttackableField(X,Y);
            }
            if(activeUnit)
                TileRenderer.ActiveAttackVisual();
            else
            {
                TileRenderer.AttackVisual();
            }
        }
        public void SetMaterial( int playerId, bool activeUnit)
        {
            TileRenderer.SetVisualStyle(playerId);
            if (Actor == null)
            {
                if(activeUnit)
                    TileRenderer.ActiveMoveVisual();
                else
                    TileRenderer.MoveVisual();
            }
            else if (Actor.FactionId == playerId)
            {
                TileRenderer.AllyVisual();
            }
            else
            {
                SetAttackMaterial(playerId, activeUnit);
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