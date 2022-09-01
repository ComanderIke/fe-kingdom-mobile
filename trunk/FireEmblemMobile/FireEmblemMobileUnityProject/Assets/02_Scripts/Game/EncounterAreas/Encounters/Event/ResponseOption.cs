using System;
using Game.GameInput;

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
    public int nextSceneIndex=-1;
    public bool fight = false;
    public EnemyArmyData EnemyArmyData;
    public EventSceneType type;
    public ResponseOption(string text,int statRequirement, int statIndex, Reward reward)
    {
        this.Text = text;
        this.StatIndex = statIndex;
        this.StatRequirement = statRequirement;
        this.reward = reward;
    }
}