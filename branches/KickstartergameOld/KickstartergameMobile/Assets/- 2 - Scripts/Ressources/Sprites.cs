using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Ressources
{
    [System.Serializable]
    public class Sprites
    {
        [Header("Characters")]
        public Sprite[] monsterOnMapSprites;
        public Sprite[] characterOnMapSprites;
        [Header("SpecialAttack")]
        public Sprite[] specialAttackSprites;
        [Header("MovementArrow")]
        public Sprite moveArrowHead;
        public Sprite moveArrowCurve;
        public Sprite moveArrowStraight;
        public Sprite moveArrowStart;
        public Sprite standOnArrowStart;


        public Sprite GetMonsterOnMapSprites(int spriteId)
        {
            if (spriteId >= 0 && spriteId < monsterOnMapSprites.Length)
                return monsterOnMapSprites[spriteId];
            return null;
        }
        public Sprite GetCharacterOnMapSprites(int spriteId)
        {
            if (spriteId >= 0 && spriteId < characterOnMapSprites.Length)
                return characterOnMapSprites[spriteId];
            return null;
        }
        public Sprite GetSpecialAttackSprite(int spriteId)
        {
            if (spriteId >= 0 && spriteId < specialAttackSprites.Length)
                return specialAttackSprites[spriteId];
            return null;
        }
    }
}
