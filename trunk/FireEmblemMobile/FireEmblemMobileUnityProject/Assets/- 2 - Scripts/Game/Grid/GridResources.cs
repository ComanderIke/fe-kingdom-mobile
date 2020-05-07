using Assets.GameInput;
using UnityEngine;

namespace Assets.Grid
{
    [System.Serializable]
    public class GridResources
    {
        public Texture HealTexture;
        public Texture MouseHoverTexture;
        public Texture SkillRangeTexture;
        public Sprite GridSprite;
        public Sprite GridAttackSprite;
        public Sprite GridAttackSpriteEnemy;
        public Sprite GridMoveSprite;
        public Sprite GridMoveSpriteEnemy;
        public Sprite GridSpriteInvalid;
        public Sprite GridSpriteStandOn;
        public Sprite GridSpriteAlly;
        public Sprite GridSpriteAttackAbleEnemy;
        public Material CellMaterialValid;
        public Material CellMaterialInvalid;
    }
}