﻿using System;
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
        public ICharacterUI UnitCharacterCircleUI;
        [HideInInspector]
        public UnitRenderer unitRenderer;
    }

   
}