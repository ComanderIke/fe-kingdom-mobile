﻿using Game.GameActors.Units;
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

        public void SetAttackMaterial(int playerId, bool activeUnit, bool activePlayer)
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
                
            }
            else
            {
                if (Actor != null && playerId != Actor.Faction.Id && activePlayer)
                {
                    TileRenderer.AttackVisual();
                }
            }
        }

     
        public void SetMaterial( int playerId, bool activeUnit, bool activePlayer)
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