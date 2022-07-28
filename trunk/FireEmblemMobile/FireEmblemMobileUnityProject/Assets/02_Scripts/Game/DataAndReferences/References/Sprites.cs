using UnityEngine;

namespace Game.GameResources
{
    [System.Serializable]
    public class Sprites
    {
        [Header("Characters")] public Sprite[] MonsterOnMapSprites;
        public Sprite[] CharacterOnMapSprites;
        [Header("MovementArrow")] public Sprite MoveArrowHead;
        public Sprite MoveArrowCurve;
        public Sprite MoveArrowStraight;
        public Sprite MoveArrowStart;
        public Sprite StandOnArrowStart;
        public Sprite StandOnArrowStartNeutral;
        public Sprite WolfClaw;
        public Sprite CantAttackIcon;
        public Sprite GetMonsterOnMapSprites(int spriteId)
        {
            if (spriteId >= 0 && spriteId < MonsterOnMapSprites.Length)
                return MonsterOnMapSprites[spriteId];
            return null;
        }

        public Sprite GetCharacterOnMapSprites(int spriteId)
        {
            if (spriteId >= 0 && spriteId < CharacterOnMapSprites.Length)
                return CharacterOnMapSprites[spriteId];
            return null;
        }

    }
}