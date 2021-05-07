using System.Collections;
using System.Collections.Generic;
using Game.GameInput;
using GameEngine;
using UnityEngine;

public class WorldMapInputSystem: IEngineSystem , IWorldMapLocationInputReceiver, IWorldMapUnitInputReceiver
{


    private bool isActive = true;
    public IWorldMapInputReceiver inputReceiver { get; set; }
    public void LocationClicked(WorldMapPosition location)
    {
        Debug.Log("Input Received!");
    }

    public void Init()
    {
        
    }

    public void Update()
    {
        if (isActive)
        {
            
        }
    }

    public void SetActive(bool b)
    {
        isActive = b;
    }

    public void UnitClicked(Party party)
    {
        Debug.Log("Party Clicked!");
    }
}