using UnityEngine;

namespace Game.Grid
{
    [CreateAssetMenu(menuName="GameData/Sprites/TileSprites", fileName="TileSprites")]
    public class TileSprites:ScriptableObject
    {
        public Sprite GridSprite;
        public Sprite GridAttackSprite;
        public Sprite GridMoveSprite;
        public Sprite GridSpriteStandOn;
        public Sprite GridSpriteAlly;
    }
}