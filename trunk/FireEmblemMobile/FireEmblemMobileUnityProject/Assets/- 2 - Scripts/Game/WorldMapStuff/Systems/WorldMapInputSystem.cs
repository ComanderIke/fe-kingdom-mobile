using System.Collections;
using System.Collections.Generic;
using Game.GameInput;
using Game.WorldMapStuff.Model;
using GameEngine;
using UnityEngine;

public class WorldMapInputSystem: IEngineSystem , IWorldMapLocationInputReceiver, IWorldMapUnitInputReceiver
{


    private bool isActive = true;
    public IWorldMapInputReceiver inputReceiver { get; set; }
    public void LocationClicked(WorldMapPosition location)
    {
         if (!isActive)
                    return;
         inputReceiver.LocationClicked(location);
         Debug.Log("Input Received!");
    }

    public void Init()
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

    public void PartyClicked(Party party)
    {
        if (!isActive)
            return;
        Debug.Log("Party Clicked!" +party+" "+party.Faction.Id);
        inputReceiver.ActorClicked(party);
    }
}