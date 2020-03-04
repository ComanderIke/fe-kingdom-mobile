using UnityEngine;

namespace Assets.Utility
{
    public class InitializeLevel : MonoBehaviour {

        public GameObject ResourcePrefab;
        public GameObject GameDataPrefab;
        public GameObject AudioPrefab;
        // Use this for initialization
        void Awake () {
            if (GameObject.Find("ResourceScript") == null)
            {
                var go = Instantiate(ResourcePrefab);
                go.name = "ResourceScript";
            }
            if (GameObject.Find("GameData") == null)
            {
                var go = Instantiate(GameDataPrefab);
                go.name = "GameData";
            }
        }
    }
}
