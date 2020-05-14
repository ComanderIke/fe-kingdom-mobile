using UnityEngine;

namespace Assets.GameResources
{
    public class ResourceScript : MonoBehaviour {

        public static ResourceScript Instance;
        public Sprites Sprites;
        public Prefabs Prefabs;
        public ParticleSystems Particles;
        public Textures Textures;
        public Materials Materials;
        private void Awake()
        {
            Instance = this;
        }
    }
}
