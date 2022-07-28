using Game.Graphics;
using GameEngine;
using UnityEngine;

namespace Game.GameResources
{
    [CreateAssetMenu(fileName = "GameAssets",menuName ="GameData/Config/GameAssets" )]
    public class GameAssets : SingletonScriptableObject<GameAssets>
    {

        public Visuals visuals;
        public Prefabs prefabs;
        public TileResources tiles;
        public GridResources grid;
      
    }
}
