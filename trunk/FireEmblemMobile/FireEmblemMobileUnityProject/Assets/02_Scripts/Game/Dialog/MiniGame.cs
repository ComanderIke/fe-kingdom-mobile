using System;
using Game.GameActors.Items;
using UnityEngine;

[System.Serializable]
public abstract class MiniGame:ScriptableObject
{
    public abstract void StartGame();

    public abstract Reward GetRewards();

    public abstract event Action OnComplete;
}