using UnityEngine;

namespace Game.GameResources
{
    public class ResourceScript : MonoBehaviour {

        public static ResourceScript Instance;
        public Sprites sprites;
        public Prefabs prefabs;
        public ParticleSystems particles;
        public Textures textures;
        public Materials materials;
        public TileResources tiles;
        public GridResources grid;
        private void Awake()
        {
            Instance = this;
        }
    }
}
