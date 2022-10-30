using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

namespace LostGrace
{
    [CreateAssetMenu(menuName = "GameConfig", fileName = "GameConfig")]
    public class GameConfigFile : ScriptableObject
    {
        public bool tutorial = false;
        public List<Unit> selectableCharacters;
    }
}