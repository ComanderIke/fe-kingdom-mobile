using System.Collections;
using System.Collections.Generic;
using Game.Grid;
using Game.Manager;
using Game.Utility;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace LostGrace
{
    public class SunSpotsController : MonoBehaviour
    {
        // Start is called before the first frame update
        private Rect spawnArea;
        [SerializeField] private List<GameObject> lightSpotPrefabs;
        private int lightSpotCount = 10;
        public Vector2 extraMapSize;
        public float minDistanceBetweenSpots = 2;
        private int mapWidth = 0;
        private int mapHeight = 0;
        private List<Vector3> positions;
        void Start()
        {
            MonoUtility.DelayFunction(Init, 0);
           
        }

        void Init()
        {
            transform.DeleteAllChildren();
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            mapWidth=(int)(gridSystem.width+extraMapSize.x*2);
            mapHeight=(int)(gridSystem.height+extraMapSize.y*2);
            transform.position = new Vector3(gridSystem.width / 2f , gridSystem.height / 2f,1);
            lightSpotCount = mapWidth * mapHeight / 7;
            MyDebug.LogTest("LightSpotCount: "+lightSpotCount);
            positions = new List<Vector3>();
            for (int i = 0; i < lightSpotCount; i++)
            {
                SpawnLightSpot();
            }
        }

        void SpawnLightSpot()
        {
            var go = Instantiate(lightSpotPrefabs[Random.Range(0, lightSpotPrefabs.Count)], transform);
            Vector3  spotPos= new Vector3(Random.Range(GetBoundLeft(),GetBoundRight()),
                Random.Range(GetBoundBottom(), GetBoundTop()));
            MyDebug.LogTest("LeftBound: "+GetBoundLeft());
            MyDebug.LogTest("RightBound: "+GetBoundRight());
            int cnt = 0;
            while (IsNearExistingSpot(spotPos, positions)&& cnt< 10)
            {
                spotPos = new Vector3(Random.Range(GetBoundLeft(),GetBoundRight()),
                    Random.Range(GetBoundBottom(), GetBoundTop()));
                cnt++;
            }

            go.transform.localPosition = spotPos;
            positions.Add(spotPos);
        }

        bool IsNearExistingSpot(Vector3 spotPos, List<Vector3> positions)
        {
            foreach (var pos in positions)
            {
                if (Vector3.Distance(pos, spotPos) <= minDistanceBetweenSpots)
                    return true;
            }

            return false;
        }
        float GetBoundTop()
        {
            return +extraMapSize.y + mapHeight / 2f;
        }
        float GetBoundLeft()
        {
            return -extraMapSize.x - mapWidth / 2f;
        }
        float GetBoundRight()
        {
            return extraMapSize.x + mapWidth / 2f;
        }
        float GetBoundBottom()
        {
            return -extraMapSize.y - mapHeight / 2f;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
