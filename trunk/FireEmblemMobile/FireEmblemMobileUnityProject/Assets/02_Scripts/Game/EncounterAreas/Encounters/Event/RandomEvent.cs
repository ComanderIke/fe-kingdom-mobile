using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "GameData/Event", fileName = "Event1")]
public class RandomEvent :ScriptableObject
{
    public List<EventScene> scenes;
   

    public RandomEvent(List<EventScene> scenes)
    {
        this.scenes = scenes;
    }
  
}