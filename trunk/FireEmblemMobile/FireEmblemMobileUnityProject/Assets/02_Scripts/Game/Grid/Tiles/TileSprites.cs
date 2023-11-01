using UnityEngine;

namespace Game.Grid
{
    [CreateAssetMenu(menuName="GameData/Sprites/TileSprites", fileName="TileSprites")]
    public class TileSprites:ScriptableObject
    {
        public Sprite GridSprite;
        public Sprite GridAttackSprite;
        public Sprite GridActiveAttackSprite;
        public Sprite GridMoveSprite;
        public Sprite GridActiveMoveSprite;
        public Sprite GridSpriteStandOn;
        public Sprite GridSpriteAlly;
        public Sprite GridSwapSprite;
        public Sprite GridDangerSprite;
        public Sprite BlockedSprite;
        public Sprite CastSprite;
        public Sprite CastHealSprite;
        public Sprite CastBadSprite;
        public Sprite CastGoodSprite;
    }
}