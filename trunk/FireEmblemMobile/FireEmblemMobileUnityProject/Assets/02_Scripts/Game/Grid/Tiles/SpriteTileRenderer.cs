using System.Collections.Generic;
using UnityEngine;

namespace Game.Grid
{
    public class SpriteTileRenderer : ITileRenderer
    {
        private SpriteRenderer spriteRenderer;
        private TileSprites activeSpriteSet;
        private Dictionary<int, TileSprites> spriteSets;
        private int idCounter;
        public SpriteTileRenderer(SpriteRenderer spriteRenderer, TileSprites[] spriteSets)
        {
            this.spriteRenderer = spriteRenderer;
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

        public void SetVisualStyle(FactionId id)
        {
            activeSpriteSet = spriteSets[(int)id];
        }

        public void SwapVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.GridSwapSprite;
        }

        public void BlockedVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.BlockedSprite;
        }

        public void CastBadVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.CastBadSprite;
        }

        public void CastGoodVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.CastGoodSprite;
        }

        public void CastHealVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.CastHealSprite;
        }


        public void Reset()
        {
            spriteRenderer.sprite = activeSpriteSet.GridSprite;
        }

        public void AttackVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.GridAttackSprite;
        }
        public void CastVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.CastSprite;
        }
    
        public void AllyVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.GridSpriteAlly;
        }

        public void MoveVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.GridMoveSprite;
        }

        public void ActiveAttackVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.GridActiveAttackSprite;
        }

        public void ActiveMoveVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.GridActiveMoveSprite;
        }

        public void StandOnVisual()
        {
            spriteRenderer.sprite = activeSpriteSet.GridSpriteStandOn;
        }

    }
}