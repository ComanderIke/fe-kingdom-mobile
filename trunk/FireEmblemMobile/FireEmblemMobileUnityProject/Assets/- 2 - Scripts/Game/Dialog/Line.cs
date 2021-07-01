using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

[System.Serializable]
public class Line
{
    public LineType LineType;
    public string sentence;
    public Unit unit;
    public bool left = true;
    public List<string> options;
}