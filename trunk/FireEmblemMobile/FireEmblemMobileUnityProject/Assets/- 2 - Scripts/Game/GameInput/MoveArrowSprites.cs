using UnityEngine;

namespace Game.GameInput
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