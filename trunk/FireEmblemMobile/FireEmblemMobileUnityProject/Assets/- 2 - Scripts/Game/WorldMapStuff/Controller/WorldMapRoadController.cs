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
    [SerializeField] private Transform RoadParent;
   
    void Start()
    {
        
    }

    private void OnEnable()
    {
        Debug.Log("Create Roads");
        foreach (Transform child in RoadParent.GetComponentsInChildren<Transform>()) {
            if(child!=RoadParent)
                GameObject.DestroyImmediate(child.gameObject);
        }
       StartPosition.CreateRoads(RoadPrefab, RoadParent);
    }

    private void Update()
    {
        StartPosition.DrawRoads();
    }
}
