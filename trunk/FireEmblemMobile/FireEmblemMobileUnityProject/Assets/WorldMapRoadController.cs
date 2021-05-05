using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;

[ExecuteInEditMode]
public class WorldMapRoadController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private WorldMapPosition StartPosition;
    [SerializeField] private GameObject RoadPrefab;
   
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Debug.Log("Create Roads");
        StartPosition.CreateRoads(RoadPrefab);
    }

    private void Update()
    {
        StartPosition.DrawRoads();
    }
}
