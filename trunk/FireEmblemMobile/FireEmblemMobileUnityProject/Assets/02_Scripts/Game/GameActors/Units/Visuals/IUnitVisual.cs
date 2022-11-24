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
        
        public IUnitEffectVisual UnitEffectVisual;

        public UnitPrefabs Prefabs;
        [HideInInspector]
        public ICharacterUI UnitCharacterCircleUI;
        [HideInInspector]
        public UnitRenderer unitRenderer;

        public UnitVisual(UnitVisual other)
        {
            CharacterSpriteSet = other.CharacterSpriteSet;
            UnitEffectVisual = other.UnitEffectVisual;
            Prefabs = other.Prefabs;
            UnitCharacterCircleUI = other.UnitCharacterCircleUI;
            unitRenderer = other.unitRenderer;
        }
    }

   
}