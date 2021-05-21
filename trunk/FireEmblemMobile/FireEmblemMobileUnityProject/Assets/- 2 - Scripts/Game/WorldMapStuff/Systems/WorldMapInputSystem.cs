using System.Collections;
using System.Collections.Generic;
using Game.GameInput;
using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Interfaces;
using Game.WorldMapStuff.Model;
using GameEngine;
using UnityEngine;

public class WorldMapInputSystem: IEngineSystem , IWorldMapLocationInputReceiver, IWorldMapUnitInputReceiver
{


    private bool isActive = true;
    public IWorldMapInputReceiver inputReceiver { get; set; }
    public void LocationClicked(LocationController location)
    {
         if (!isActive)
                    return;
         inputReceiver.LocationClicked(location);
    }

    public void Init()
    {
        
    }

    public void Deactivate()
    {
        
    }

    public void Activate()
    {
        
    }

    public void Update()
    {
        // if (isActive)
        // {
        //     
        // }
    }

    public void SetActive(bool b)
    {
        isActive = b;
    }

    public void ActorClicked(WM_Actor party)
    {
        if (!isActive)
            return;
        inputReceiver.ActorClicked(party);
    }
}