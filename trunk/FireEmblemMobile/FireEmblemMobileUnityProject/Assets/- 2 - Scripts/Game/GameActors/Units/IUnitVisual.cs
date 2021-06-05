using System;
using Game.GameResources;
using UnityEngine;

namespace Game.GameActors.Units
{
  [Serializable]
    public class UnitVisual
    {
        
        
        public CharacterSpriteSet CharacterSpriteSet;

        public int ID
        {
            get
            {
                return CharacterSpriteSet.id;
            }
            set
            {
                CharacterSpriteSet.id = value;
            }
        }

        
    }
}