using UnityEngine;

namespace Game.Map
{
    [CreateAssetMenu(menuName = "GameData/Map", fileName = "Map1")]
    public class MapData : ScriptableObject
    {
        public int Width;
        public int Height;
        public string Name;
    }
}