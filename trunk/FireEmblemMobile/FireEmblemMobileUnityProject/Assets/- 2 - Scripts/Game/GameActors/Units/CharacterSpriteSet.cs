﻿using Game.Graphics;
using UnityEngine;

namespace Game.GameActors.Units
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "GameData/Units/Visual", fileName = "UnitVisual")]
    public class CharacterSpriteSet:SerializeableSO
    {
        public Sprite FaceSprite;
        public Sprite MapSprite;
    }
}