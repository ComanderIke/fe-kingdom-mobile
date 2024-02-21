using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/AreaData", fileName = "AreaData")]
public class AreaData : ScriptableObject
{
    public int Index = 0;
    public string Label = "";
    [SerializeField]public List<ColumnSpawn> ColumnSpawns;

    public TMP_ColorGradient ColorGradient;
    public List<string> textAnimatorTags;
}