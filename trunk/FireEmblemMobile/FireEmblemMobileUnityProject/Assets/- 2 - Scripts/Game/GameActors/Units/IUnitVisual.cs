using System;
using System.Collections.Generic;
using Game.GameResources;
using Game.Graphics;
using UnityEngine;

namespace Game.GameActors.Units
{
  [Serializable]
    public class UnitVisual
    {
        
        
        public CharacterSpriteSet CharacterSpriteSet;

        public IUnitEffectVisual UnitEffectVisual;

        
    }
}