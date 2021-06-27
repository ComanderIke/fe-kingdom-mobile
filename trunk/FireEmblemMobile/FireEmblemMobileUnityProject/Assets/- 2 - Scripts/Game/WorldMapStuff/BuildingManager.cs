using System.Collections;
using System.Collections.Generic;
using Game.Utility;
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
    private GameObject smithyGo;
    private GameObject tavernGO;
    private GameObject barracksGo;
    private GameObject churchGo;
    private GameObject shopGo;
    public Vector3 zoomOffset;

    public TweenableUIController shopUI;
    public TweenableUIController tavernUI;
    public TweenableUIController smithyUI;
    public TweenableUIController churchUI;
    public TweenableUIController barracksUI;
    [SerializeField]
    private CameraZoomInScript cameraZoom;

    public void SpawnBuildings(List<BuildingType> buildingTypes)
    {
        int cnt = 0;
        foreach (var buildingtype in buildingTypes)
        {
            GameObject go = null;
            switch (buildingtype)
            {
                case BuildingType.Barracks:
                    go = Instantiate(barracksPrefab, transform, false);
                    barracksGo = go;
                    break;
                case BuildingType.Church:
                    go=Instantiate(churchPrefab, transform, false); churchGo = go;break;
                    
                case BuildingType.Tavern:
                    go=Instantiate(tavernPrefab, transform, false);tavernGO = go; break;
                case BuildingType.Shop:
                    go=Instantiate(shopPrefab, transform, false);shopGo = go; break;
                case BuildingType.Smithy:
                    go=Instantiate(smithyPrefab, transform, false); smithyGo = go;break;
            }
            go.transform.position = BuildingSpawnPoints[cnt].position;
            cnt++;
        }

        for (int i = BuildingSpawnPoints.Count - 1; i >= 0; i--)
        {
            Destroy(BuildingSpawnPoints[i].gameObject);
        }
        
    }

    public void BuildingClicked(BuildingType buildingType)
    {
        switch (buildingType)
        {
            case BuildingType.Barracks:
                cameraZoom.ZoomIn(barracksGo.transform.position+zoomOffset);

                barracksUI.Show();
                break;
            case BuildingType.Church:
                cameraZoom.ZoomIn(churchGo.transform.position+zoomOffset);
          
                churchUI.Show();
                break;
            case BuildingType.Tavern:
                cameraZoom.ZoomIn(tavernGO.transform.position+zoomOffset);
        
                tavernUI.Show();
                break;
            case BuildingType.Shop:
                cameraZoom.ZoomIn(shopGo.transform.position+zoomOffset);

                shopUI.Show();break;
            case BuildingType.Smithy:
                cameraZoom.ZoomIn(smithyGo.transform.position+zoomOffset);

                smithyUI.Show();break;
        }
    }
}
