using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/Map", fileName = "Map1")]
public class MapData : ScriptableObject
{
    public int width;
    public int height;
    public string name;
}

