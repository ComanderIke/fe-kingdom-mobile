using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Line
{
    public LineType LineType;
    public string sentence;
    public string CharacterName;
    public bool left = true;
    public List<string> options;
}