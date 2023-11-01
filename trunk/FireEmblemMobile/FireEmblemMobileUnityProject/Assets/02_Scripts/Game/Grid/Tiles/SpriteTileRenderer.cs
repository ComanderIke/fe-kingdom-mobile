using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class SpriteTileRenderer : ITileRenderer
    {
        private SpriteRenderer baseGrid;
        private SpriteRenderer middleLayer;
        private SpriteRenderer topLayer;
        private TileSprites activeSpriteSet;
        private Dictionary<int, TileSprites> spriteSets;
        private int idCounter;
        public SpriteTileRenderer(SpriteRenderer topLayer,SpriteRenderer middleLayer,SpriteRenderer baseLayer, TileSprites[] spriteSets)
        {
            this.topLayer = topLayer;
            this.middleLayer = middleLayer;
            this.baseGrid = baseLayer;
            this.spriteSets = new Dictionary<int, TileSprites>();
            foreach(var spriteSet in spriteSets)
                AddSpriteSet((spriteSet));
            activeSpriteSet = spriteSets[0];
           
        }

        public int AddSpriteSet(TileSprites spriteSet)
        {
            spriteSets.Add(idCounter, spriteSet);
            idCounter++;
            return idCounter - 1;

        }

        public void DangerVisual(bool active)
        {
            middleLayer.sprite = active?activeSpriteSet.GridDangerSprite:null;
        }
        public void SetVisualStyle(FactionId id)
        {
            activeSpriteSet = spriteSets[(int)id];
        }

        public void SwapVisual()
        {
            topLayer.sprite = activeSpriteSet.GridSwapSprite;
        }

        public void BlockedVisual()
        {
            topLayer.sprite = activeSpriteSet.BlockedSprite;
        }

        public void CastBadVisual()
        {
            topLayer.sprite = activeSpriteSet.CastBadSprite;
        }

        public void CastGoodVisual()
        {
            topLayer.sprite = activeSpriteSet.CastGoodSprite;
        }

        public void CastHealVisual()
        {
            topLayer.sprite = activeSpriteSet.CastHealSprite;
        }


        public void Reset()
        {
            topLayer.sprite = activeSpriteSet.GridSprite;
        }

        public void AttackVisual()
        {
            topLayer.sprite = activeSpriteSet.GridAttackSprite;
        }
        public void CastVisual()
        {
            topLayer.sprite = activeSpriteSet.CastSprite;
        }
    
        public void AllyVisual()
        {
            topLayer.sprite = activeSpriteSet.GridSpriteAlly;
        }

        public void MoveVisual()
        {
            topLayer.sprite = activeSpriteSet.GridMoveSprite;
        }

        public void ActiveAttackVisual()
        {
            topLayer.sprite = activeSpriteSet.GridActiveAttackSprite;
        }

        public void ActiveMoveVisual()
        {
            topLayer.sprite = activeSpriteSet.GridActiveMoveSprite;
        }

        public void StandOnVisual()
        {
            topLayer.sprite = activeSpriteSet.GridSpriteStandOn;
        }

    }
}