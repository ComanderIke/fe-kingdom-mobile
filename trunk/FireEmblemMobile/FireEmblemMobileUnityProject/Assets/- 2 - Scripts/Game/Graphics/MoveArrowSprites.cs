using UnityEngine;

namespace Game.Graphics
{
    [CreateAssetMenu(menuName = "GameData/SpriteSet/MoveArrow", fileName =  "MoveArrowSpriteSet")]
    public class MoveArrowSprites: ScriptableObject
    {
        public Sprite moveArrowCurve;
        public Sprite moveArrowStraight;
        public Sprite moveArrowHead;
        public Sprite standOnArrowStart;
        public Sprite standOnArrowStartNeutral;
    }
}