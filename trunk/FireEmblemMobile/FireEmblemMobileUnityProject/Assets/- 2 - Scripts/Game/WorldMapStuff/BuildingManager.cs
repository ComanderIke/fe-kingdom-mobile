using System.Collections;
using System.Collections.Generic;
using Game.WorldMapStuff.Model;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public List<Transform> BuildingSpawnPoints;

    public GameObject smithyPrefab;

    public GameObject tavernPrefab;

    public GameObject barracksPrefab;

    public GameObject churchPrefab;

    public GameObject shopPrefab;

    public void SpawnBuildings(List<BuildingType> buildingTypes)
    {
        int cnt = 0;
        foreach (var buildingtype in buildingTypes)
        {
            switch (buildingtype)
            {
                case BuildingType.Barracks:
                    Instantiate(barracksPrefab, BuildingSpawnPoints[cnt], false); break;
                case BuildingType.Church:
                    Instantiate(churchPrefab, BuildingSpawnPoints[cnt], false); break;
                case BuildingType.Tavern:
                    Instantiate(tavernPrefab, BuildingSpawnPoints[cnt], false); break;
                case BuildingType.Shop:
                    Instantiate(shopPrefab, BuildingSpawnPoints[cnt], false); break;
                case BuildingType.Smithy:
                    Instantiate(smithyPrefab, BuildingSpawnPoints[cnt], false); break;
            }

            cnt++;
        }
        
    }
}
