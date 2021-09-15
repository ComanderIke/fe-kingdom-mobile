﻿using System.Xml.Schema;
using Game.GameActors.Units;
using Game.Graphics;
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
                Debug.Log("Actor: "+actor);
                actor = value;
               Debug.Log("Tile: "+this.X+" "+this.Y+ " "+actor);
               if (actor != null)
               {
                   actor.GridComponent.Tile = this;
                   Debug.Log("Tile: " + this.X + " " + this.Y + " ACtor: " + actor + " " +
                             actor.GridComponent.Tile.TileType.TerrainType);
               }

            }
        }

        public readonly int X;
        public readonly int Y;
      //  public bool IsActive;
     //   public bool IsAttackable;
        
        public TileType TileType;
        public readonly ITileRenderer TileRenderer;
        public ITileEffectVisualRenderer tileVfx;
        private Transform transform;

        public Tile(int i, int j, TileType tileType, Transform transform, ITileRenderer tileRenderer, ITileEffectVisualRenderer tileVfx)
        {
            X = i;
            Y = j;
            TileType = tileType;
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
                else if (TileType.TerrainType == TerrainType.Obstacle)
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