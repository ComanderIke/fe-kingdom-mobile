using UnityEngine;

namespace Game.GameResources
{
    [CreateAssetMenu(fileName = "GameAssets",menuName ="GameData/Config/GameAssets" )]
    public class GameAssets : ScriptableObject {

        public static GameAssets Instance;
        public Prefabs prefabs;
        public TileResources tiles;
        public GridResources grid;
        private void OnEnable()
        {
            Instance = this;
        }
    }
}
