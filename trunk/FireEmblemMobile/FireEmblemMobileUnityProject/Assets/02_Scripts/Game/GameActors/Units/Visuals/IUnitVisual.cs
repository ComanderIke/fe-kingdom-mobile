using System;
using System.Collections.Generic;
using Game.GameActors.Units.OnGameObject;
using Game.GameResources;
using Game.Graphics;
using Game.GUI;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.GameActors.Units
{
  [Serializable]
    public class UnitVisual
    {
        
        
        public CharacterSpriteSet CharacterSpriteSet;
        public CharacterSpriteSet CharacterClassUpgradeSpriteSet;
        
        public IUnitEffectVisual UnitEffectVisual;

        public UnitPrefabs Prefabs;
        [HideInInspector]
        public ICharacterUI UnitCharacterCircleUI;
        [HideInInspector]
        public UnitRenderer unitRenderer;

        public UnitVisual(UnitVisual other)
        {
            // CharacterSpriteSet = ScriptableObject.CreateInstance<CharacterSpriteSet>();
            // CharacterSpriteSet.animatedSprite = other.CharacterSpriteSet.animatedSprite;
            // CharacterSpriteSet.battleAnimatedSprite = other.CharacterSpriteSet.battleAnimatedSprite;
            // CharacterSpriteSet.FaceSprite = other.CharacterSpriteSet.FaceSprite;
            // CharacterSpriteSet.MapSprite = other.CharacterSpriteSet.MapSprite;
           
            CharacterSpriteSet = other.CharacterSpriteSet;
            CharacterClassUpgradeSpriteSet = other.CharacterClassUpgradeSpriteSet;
            UnitEffectVisual = other.UnitEffectVisual;
            Prefabs = other.Prefabs;
            UnitCharacterCircleUI = other.UnitCharacterCircleUI;
            unitRenderer = other.unitRenderer;
        }
    }

   
}