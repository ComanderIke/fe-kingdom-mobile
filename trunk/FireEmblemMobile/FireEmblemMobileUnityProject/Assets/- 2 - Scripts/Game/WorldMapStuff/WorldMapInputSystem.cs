using System.Collections;
using System.Collections.Generic;
using GameEngine;
using UnityEngine;

public class WorldMapInputSystem: IEngineSystem , IWorldMapInputReceiver
{
   

    public void LocationClicked(WorldMapPosition location)
    {
        Debug.Log("Input Received!");
    }

    public void Init()
    {
        
    }
}

public interface IWorldMapInputReceiver
{
    void LocationClicked(WorldMapPosition location);
}
