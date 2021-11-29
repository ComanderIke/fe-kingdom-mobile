using System.Xml.Schema;
using Game.GameActors.Units;
using Game.Graphics;
using Game.Map;
using UnityEngine;

namespace Game.Grid
{
    public class Tile
    {
        private IGridActor actor;
        public IGridActor Actor
        {
            get
            {
                return actor;
            }
            set
            {
                //Debug.Log("Actor: "+actor);
                actor = value;
               //Debug.Log("Tile: "+this.X+" "+this.Y+ " "+actor);
               if (actor != null)
               {
                   actor.GridComponent.Tile = this;
               }

            }
        }

        public readonly int X;
        public readonly int Y;
      //  public bool IsActive;
     //   public bool IsAttackable;
     
        public readonly ITileRenderer TileRenderer;
        public ITileEffectVisualRenderer tileVfx;
        private Transform transform;

        public GridTerrainData TileData
        {
            get
            {
                return TileManager.Instance.GetData(X, Y);
            }
        }
        public Tile(int i, int j,  Transform transform, ITileRenderer tileRenderer, ITileEffectVisualRenderer tileVfx)
        {
            X = i;
            Y = j;
            TileRenderer = tileRenderer;
            this.tileVfx = tileVfx;
            this.transform = transform;

        }

        
        public Transform GetTransform()
        {
            return transform;
        }
        public void Reset()
        {
            TileRenderer.Reset();
            tileVfx.Hide(this);
        }

        public void SetAttackMaterial(FactionId playerId, bool activeUnit, bool activePlayer)
        {
            Reset();
            TileRenderer.SetVisualStyle(playerId);
           

            if (activeUnit)
            {
                if (Actor!=null && playerId != Actor.Faction.Id && activePlayer)
                {
                    tileVfx.ShowAttackable(this);
                    TileRenderer.ActiveAttackVisual();
                }
                else if (!TileData.walkable)
                {
                    TileRenderer.BlockedVisual();
                }
                else 
                {
                    TileRenderer.ActiveAttackVisual();
                }
                
            }
            else
            {
                if (Actor != null && playerId != Actor.Faction.Id && activePlayer)
                {
                    TileRenderer.AttackVisual();
                }
                else
                {
                    TileRenderer.AttackVisual();
                }
            }
        }

     
        public void SetMaterial( FactionId playerId, bool activeUnit, bool activePlayer)
        {
            Reset();
            TileRenderer.SetVisualStyle(playerId);
            if (Actor == null)
            {
                if(activeUnit)
                    TileRenderer.ActiveMoveVisual();
                else
                    TileRenderer.MoveVisual();
            }
            else if (Actor.Faction.Id == playerId)
            {
                TileRenderer.AllyVisual();
            }
            else
            {
                SetAttackMaterial(playerId, activeUnit, activePlayer);
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
            return Actor.Faction.Id != actor.Faction.Id;
        }


    }
}