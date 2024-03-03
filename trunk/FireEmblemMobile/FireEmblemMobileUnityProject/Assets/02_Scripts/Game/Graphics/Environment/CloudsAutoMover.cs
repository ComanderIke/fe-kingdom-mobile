using System;
using System.Collections.Generic;
using Game.Grid;
using Game.Manager;
using Game.Utility;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace Game.Graphics.Environment
{
    public class CloudsAutoMover : MonoBehaviour
    {
        public List<GameObject> cloudPrefabs;
        public Vector2 cloudOffsetMin;
        public int cloudCount = 8;
        public float minDistanceBetweenClouds = 2;
        public List<GameObject> instantiatedClouds;

       
        public Vector2 directionMin;
        public Vector2 directionMax;
        private Vector2 direction;
        private float checkFrame = 100;

       
        private int mapWidth;

        private int mapHeight;

        private Rect SpawnZoneTop;

        private Rect SpawnZoneTopRight;

        private Rect SpawnZoneRight;
        private Rect SpawnZoneBottom;

        private Rect SpawnZoneTopLeft;
        private Rect SpawnZoneBottomLeft;
        private Rect SpawnZoneBottomRight;

        private Rect SpawnZoneLeft;

        private List<Rect> activeSpawnZones;
        private void OnDrawGizmos()
        {
           
            Gizmos.color = Color.magenta;
            if (SpawnZoneTop != null)
            {
                Gizmos.color = activeSpawnZones.Contains(SpawnZoneTop) ? Color.green : Color.magenta;
                Gizmos.DrawCube(SpawnZoneTop.center, SpawnZoneTop.size);
            }
            if (SpawnZoneTopRight != null)
            {
                Gizmos.color = activeSpawnZones.Contains(SpawnZoneTopRight) ? Color.green : Color.magenta;
                Gizmos.DrawCube(SpawnZoneTopRight.center, SpawnZoneTopRight.size);
            }
            if (SpawnZoneRight != null)
            {
                Gizmos.color = activeSpawnZones.Contains(SpawnZoneRight) ? Color.green : Color.magenta;
                Gizmos.DrawCube(SpawnZoneRight.center, SpawnZoneRight.size);
            }
            if (SpawnZoneBottom != null)
            {
                Gizmos.color = activeSpawnZones.Contains(SpawnZoneBottom) ? Color.green : Color.magenta;
                Gizmos.DrawCube(SpawnZoneBottom.center, SpawnZoneBottom.size);
            }
            if (SpawnZoneLeft != null)
            {
                Gizmos.color = activeSpawnZones.Contains(SpawnZoneLeft) ? Color.green : Color.magenta;
                Gizmos.DrawCube(SpawnZoneLeft.center, SpawnZoneLeft.size);
            }
            if (SpawnZoneTopLeft != null)
            {
                Gizmos.color = activeSpawnZones.Contains(SpawnZoneTopLeft) ? Color.green : Color.magenta;
                Gizmos.DrawCube(SpawnZoneTopLeft.center, SpawnZoneTopLeft.size);
            }
            if (SpawnZoneBottomLeft != null)
            {
                Gizmos.color = activeSpawnZones.Contains(SpawnZoneBottomLeft) ? Color.green : Color.magenta;
                Gizmos.DrawCube(SpawnZoneBottomLeft.center, SpawnZoneBottomLeft.size);
            }
            if (SpawnZoneBottomRight != null)
            {
                Gizmos.color = activeSpawnZones.Contains(SpawnZoneBottomRight) ? Color.green : Color.magenta;
                Gizmos.DrawCube(SpawnZoneBottomRight.center, SpawnZoneBottomRight.size);
            }
            
        }

        // Start is called before the first frame update
        void Start()
        {
            MonoUtility.DelayFunction(Init, 0f);
        }

        void Init()
        {
            
            activeSpawnZones = new List<Rect>();
            var gridSystem = ServiceProvider.Instance.GetSystem<GridSystem>();
            mapWidth=(int)(gridSystem.width+cloudOffsetMin.x*2);
            mapHeight=(int)(gridSystem.height+cloudOffsetMin.y*2);
            transform.position = new Vector3(gridSystem.width / 2f , gridSystem.height / 2f,1);
            SpawnZoneTop = new Rect(-cloudOffsetMin.x, gridSystem.height+ cloudOffsetMin.y, gridSystem.width+cloudOffsetMin.x*2, cloudOffsetMin.y);
            SpawnZoneTopRight = new Rect(gridSystem.width+ cloudOffsetMin.x, gridSystem.height+ cloudOffsetMin.y, cloudOffsetMin.x, cloudOffsetMin.y);
            SpawnZoneRight = new Rect(gridSystem.width+ cloudOffsetMin.x, -cloudOffsetMin.y, cloudOffsetMin.x, gridSystem.height+cloudOffsetMin.y*2);
            SpawnZoneLeft = new Rect(-cloudOffsetMin.x*2, -cloudOffsetMin.y, cloudOffsetMin.x, gridSystem.height+cloudOffsetMin.y*2);
            SpawnZoneBottom= new Rect(-cloudOffsetMin.x,  -cloudOffsetMin.y*2, gridSystem.width+cloudOffsetMin.x*2, cloudOffsetMin.y);
            SpawnZoneTopLeft= new Rect(-cloudOffsetMin.x*2, gridSystem.height+ cloudOffsetMin.y, cloudOffsetMin.x, cloudOffsetMin.y);
            SpawnZoneBottomLeft= new Rect(-cloudOffsetMin.x*2, -cloudOffsetMin.y*2, cloudOffsetMin.x, cloudOffsetMin.y);
            SpawnZoneBottomRight= new Rect(gridSystem.width+ cloudOffsetMin.x, -cloudOffsetMin.y*2, cloudOffsetMin.x, cloudOffsetMin.y);
            cloudCount = mapWidth * mapHeight / 15;
            transform.DeleteAllChildren();
            instantiatedClouds = new List<GameObject>();
            List<Vector3> positions = new List<Vector3>();
            for (int i = 0; i < cloudCount; i++)
            {
                var cloudGo = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Count)], transform);
                Vector3  cloudPos= new Vector3(Random.Range(GetBoundLeft(),GetBoundRight()),
                    Random.Range(GetBoundBottom(), GetBoundTop()));
                int cnt = 0;
                while (IsNearExistingCloud(cloudPos, positions)&& cnt< 10)
                {
                    cloudPos= new Vector3(Random.Range(GetBoundLeft(),GetBoundRight()),
                        Random.Range(GetBoundBottom(), GetBoundTop()));
                    cnt++;
                }

                cloudGo.transform.localPosition = cloudPos;
                positions.Add(cloudPos);
                instantiatedClouds.Add(cloudGo);
            }
            direction = new Vector2(Random.Range(directionMin.x, directionMax.x)*(Random.value>.5f?1:-1), Random.Range(directionMin.y, directionMax.y)*(Random.value>.5f?1:-1));
            
            MyDebug.LogTest("Direction: "+direction);
            if(direction.x<0)
                activeSpawnZones.Add(SpawnZoneRight);
            else if(direction.x>0)
                activeSpawnZones.Add(SpawnZoneLeft);
            if (direction.y > 0)
                activeSpawnZones.Add(SpawnZoneBottom);
            else if(direction.y<0)
                activeSpawnZones.Add(SpawnZoneTop);

            if (direction.x > 0 && direction.y > 0)
            {
                activeSpawnZones.Add(SpawnZoneBottomLeft);
            }
            else if (direction.x > 0 && direction.y < 0)
            {
                activeSpawnZones.Add(SpawnZoneTopLeft);
            }
            else if (direction.x < 0 && direction.y < 0)
            {
                activeSpawnZones.Add(SpawnZoneTopRight);
            }
            else if (direction.x < 0 && direction.y > 0)
            {
                activeSpawnZones.Add(SpawnZoneBottomRight);
            }
                

        }

        float GetBoundTop()
        {
            return +cloudOffsetMin.y + mapHeight / 2;
        }
        float GetBoundLeft()
        {
            return -cloudOffsetMin.x - mapWidth / 2;
        }
        float GetBoundRight()
        {
            return cloudOffsetMin.x + mapWidth / 2;
        }
        float GetBoundBottom()
        {
            return -cloudOffsetMin.y - mapHeight / 2;
        }
        bool IsNearExistingCloud(Vector3 cloudPos, List<Vector3> positions)
        {
            foreach (var pos in positions)
            {
                if (Vector3.Distance(pos, cloudPos) <= minDistanceBetweenClouds)
                    return true;
            }

            return false;
        }

        // Update is called once per frame
        void Update()
        {
            transform.Translate(direction*Time.deltaTime);
            if (Time.frameCount % checkFrame == 0)
            {
                for (int i = instantiatedClouds.Count-1; i >= 0; i--)
                {
                    var cloud = instantiatedClouds[i];
                    if (cloud.transform.position.x >= GetBoundRight() || cloud.transform.position.x <=
                                                                            GetBoundLeft()
                                                                            || cloud.transform.position.y >=
                                                                           GetBoundTop()||
                                                                            cloud.transform.position.y <=
                                                                            GetBoundBottom())
                    {
                        instantiatedClouds.Remove(cloud);
                        Destroy(cloud);
                       
                    }
                }

                while (instantiatedClouds.Count < cloudCount)
                {
                    SpawnNewCloud();
                }
                
            }
        }

        void SpawnNewCloud()
        {
            var zone = activeSpawnZones[Random.Range(0, activeSpawnZones.Count)];
            var cloudGo=Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Count)], transform);
            cloudGo.transform.position = new Vector3(Random.Range(zone.x, zone.x+ zone.width), Random.Range(zone.y, zone.y+zone.height));
            
            instantiatedClouds.Add(cloudGo);
        }
    }
}
