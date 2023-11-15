using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public enum DifficultyVariableStyle
{
    Normal,
    Increase,
    Decrease
}
[System.Serializable]
public struct DifficultyVariable
{
    public string label;
    public string value;
    public DifficultyVariableStyle textStyle;

    public DifficultyVariable(string label, string value, DifficultyVariableStyle style)
    {
        this.label = label;
        this.value = value;
        this.textStyle = style;
    }
}





    
