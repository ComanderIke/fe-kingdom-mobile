using System;
using System.Collections.Generic;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid;
using Game.Mechanics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Graphics
{
    [Serializable]
    public struct InnSprites
    {
        public Sprite drink;
        public Sprite meat;
        public Sprite rest;
    }
    [Serializable]
    public class Visuals
    {
        [SerializeField]
        public List<CharacterSpriteSet> characterSpriteSets;
        [SerializeField]
        public List<IUnitEffectVisual> unitVisualEffects;
        [SerializeField]
        public InnSprites InnSprites;

        public Icons Icons;
        public DebugVisuals debug;
    }
    [Serializable]
    public class DebugVisuals
    {
        public GameObject auraRangePrefab;
    }
}