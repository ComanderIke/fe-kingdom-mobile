using System.Collections.Generic;
using Game.GameActors.Units;
using UnityEngine;

[System.Serializable]
public class Line
{
    public string sentence;
    [SerializeField] DialogActor actor;
    public bool left = true;
    public IDialogActor Actor => actor;
}