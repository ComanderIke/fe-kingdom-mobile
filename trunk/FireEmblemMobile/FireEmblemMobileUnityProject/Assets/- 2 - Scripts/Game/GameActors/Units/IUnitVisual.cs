using System;
using UnityEngine;

namespace Game.GameActors.Units
{
  [Serializable]
    public class UnitVisual
    {

        
        public CharacterSpriteSet CharacterSpriteSet;

        public int GetId()
        {
            return CharacterSpriteSet.id;
        }
    }
}