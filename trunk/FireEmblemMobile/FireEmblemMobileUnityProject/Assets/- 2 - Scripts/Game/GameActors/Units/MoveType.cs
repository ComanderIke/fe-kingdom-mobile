using System;
using System.Collections.Generic;
using Game.Grid;
using UnityEngine;

namespace Game.GameActors.Units
{
    [CreateAssetMenu(menuName = "GameData/Unit/MoveType", fileName = "MoveType")]
    public class MoveType: ScriptableObject
    {
        public int moveTypeId;
        public Sprite icon;
        public int baseMovement = 3;
        
    }

 
}