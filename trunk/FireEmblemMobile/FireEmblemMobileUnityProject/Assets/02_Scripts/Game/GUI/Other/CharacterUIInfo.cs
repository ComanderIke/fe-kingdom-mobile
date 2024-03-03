﻿using Game.GameActors.Units.Numbers;
using UnityEngine;

namespace Game.GUI.Other
{
    [CreateAssetMenu(menuName = "GameData/UI/CharacterUIInfo", fileName="characterUIInfo")]
    public class CharacterUIInfo : ScriptableObject
    {
        public string Name;
        public Stats Stats;
        public int HP;
        public int SP;
        public Sprite FaceSprite;
    }
}