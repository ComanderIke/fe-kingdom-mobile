using System;
using System.Collections.Generic;
using Game.GameResources;
using Game.Graphics;
using Object = UnityEngine.Object;

namespace Game.GameActors.Units
{
  [Serializable]
    public class UnitVisual
    {
        
        
        public CharacterSpriteSet CharacterSpriteSet;

        public IUnitEffectVisual UnitEffectVisual;

        public UnitPrefabs Prefabs;
    }

   
}