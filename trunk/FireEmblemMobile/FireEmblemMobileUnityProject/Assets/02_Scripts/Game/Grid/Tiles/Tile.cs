using Game.GameActors.Factions;
using Game.GameActors.Grid;
using Game.GameActors.Items.Consumables;
using Game.GameActors.Units;
using Game.Map;
using UnityEngine;

namespace Game.Grid.Tiles
{
    public class Tile
    {
 
        private IGridObject gridObject;
        public IGridObject GridObject
        {
            get
            {
                return gridObject;
            }
            set
            {
                MyDebug.LogTest("Actor: "+gridObject+" Value: "+value);
                gridObject = value;
               //Debug.Log("Tile: "+this.X+" "+this.Y+ " "+actor);
               if (gridObject != null)
               {
                   gridObject.GridComponent.Tile = this;
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
        private GlowSpot glowSpot;

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

        public override string ToString()
        {
            return "X :" + X + " Y: " + Y;
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
        public void SetCastCursorMaterial(EffectType effectType,FactionId playerId)
        {
            TileRenderer.SetVisualStyle(playerId);
            switch (effectType)
            {
                case EffectType.Heal: TileRenderer.CastHealVisual();
                    break;
                case EffectType.Good: TileRenderer.CastGoodVisual();
                    break;
                case EffectType.Bad: TileRenderer.CastBadVisual();
                    break;
                case EffectType.Neutral: TileRenderer.CastGoodVisual();
                    break;
            }
            
        }
        public void SetCastMaterial(FactionId playerId)
        {
            TileRenderer.SetVisualStyle(playerId);
            TileRenderer.CastVisual();
        }

        public void SetDangerMaterial(bool active)
        {
            TileRenderer.DangerVisual(active);
        }

        public void ShowAttackable(bool show)
        {
            if(show)
                tileVfx.ShowAttackable(this);
            else
            {
                tileVfx.Hide(this);
            }
        }
        public void SetAttackMaterial(FactionId playerId, bool activeUnit, bool activePlayer)
        {
            Reset();
            TileRenderer.SetVisualStyle(playerId);
           

            if (activeUnit)
            {
                if (GridObject != null&& playerId != GridObject.Faction.Id && activePlayer)
                {
                    ShowAttackable(true);
                    TileRenderer.ActiveAttackVisual();
                }
                else if (GridObject!=null && playerId != GridObject.Faction.Id && activePlayer)
                {
                    ShowAttackable(true);
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
                if (GridObject != null && playerId != GridObject.Faction.Id && activePlayer)
                {
                    TileRenderer.AttackVisual();
                }
                else if (GridObject != null && playerId != GridObject.Faction.Id && activePlayer)
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
            if (GridObject != null)
            {
                if (GridObject.Faction.Id == playerId)
                {
                    TileRenderer.AllyVisual();
                }
                else
                {
                    SetAttackMaterial(playerId, activeUnit, activePlayer);
                }

                return;
            }
            if (GridObject == null)
            {
                if(activeUnit)
                    TileRenderer.ActiveMoveVisual();
                else
                    TileRenderer.MoveVisual();
            }
            else if (GridObject.Faction.Id == playerId)
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
            return GridObject == null;
        }
        public bool CanMoveOnto(Unit unit)
        {
            return HasFreeSpace()&&TileData.CanMoveThrough(unit.MoveType);
        }


        public bool HasGlowSpot()
        {
            return glowSpot != null;
        }

        public void RemoveGlowSpot()
        {
            glowSpot.Remove();
            glowSpot = null;
        }

        public void SetGlowSpot(GlowSpot glowSpot1)
        {
            glowSpot = glowSpot1;
        }
    }
}