using System;
using System.Collections.Generic;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameInput;
using UnityEngine;

public enum EventSceneType
{
    Normal,
    Memory,
    Fight,
    Merchant
}
[Serializable]
public class ResponseOption
{
    public string Text;
    public int StatRequirement;
    public int StatIndex;
    public Reward reward;

    public bool fight = false;
    public EnemyArmyData EnemyArmyData;
    public EventSceneType type;
    public Unit EnemyToFight;
    public bool statcheck;
    public List<EventOutcome> outcomes;

    public ResponseOption(string text,int statRequirement, int statIndex, Reward reward)
    {
        this.Text = text;
        this.StatIndex = statIndex;
        this.StatRequirement = statRequirement;
        this.reward = reward;
    }

    
}

[Serializable]
public class EventOutcome
{
    public int nextSceneIndex=-1;
}