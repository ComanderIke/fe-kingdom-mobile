using System;
using System.Collections;
using System.Collections.Generic;
using Game.Utility;
using Game.WorldMapStuff.Model;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingInputController : MonoBehaviour
{
    public Material normal;

    public Material selected;
    public SpriteRenderer spriteRenderer;

    public BuildingType BuildingType;
    public BuildingManager BuildingManager;
    // Start is called before the first frame update
    void Awake()
    {
        BuildingManager = FindObjectOfType<BuildingManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void OnMouseDown()
    {
        if (!UIHelper.IsPointerOverGameObject())
            spriteRenderer.material = selected;
        
    }


    public void OnMouseOver()
    {
        
    }

    public void OnMouseUp()
    {
        spriteRenderer.material = normal;
    }
    public void OnMouseUpAsButton()
    {
        if (!UIHelper.IsPointerOverGameObject())
        {
            BuildingManager.BuildingClicked(BuildingType);
        }
    }
}
