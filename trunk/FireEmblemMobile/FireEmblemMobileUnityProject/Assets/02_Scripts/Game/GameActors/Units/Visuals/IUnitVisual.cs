using System;
using Game.GameActors.Units.OnGameObject;
using Game.Graphics.Interfaces;
using Game.GUI.Other;
using UnityEngine;

namespace Game.GameActors.Units.Visuals
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

        public float AttackDelay;

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
            Prefabs = new UnitPrefabs(other.Prefabs);
            UnitCharacterCircleUI = other.UnitCharacterCircleUI;
            unitRenderer = other.unitRenderer;
            AttackDelay = other.AttackDelay;
        }
    }

   
}