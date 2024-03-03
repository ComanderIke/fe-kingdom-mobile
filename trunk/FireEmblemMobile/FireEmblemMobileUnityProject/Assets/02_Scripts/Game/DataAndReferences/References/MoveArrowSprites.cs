using UnityEngine;

namespace Game.DataAndReferences.References
{
    [CreateAssetMenu(menuName = "GameData/SpriteSet/MoveArrowSpriteSet", fileName =  "MoveArrowSpriteSet")]
    public class MoveArrowSprites: ScriptableObject
    {
        public Sprite moveArrowCurve;
        public Sprite moveArrowStraight;
        public Sprite moveArrowHead;
        public Sprite standOnArrowStart;
        public Sprite standOnArrowStartNeutral;
    }
}