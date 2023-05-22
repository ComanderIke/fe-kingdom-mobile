using System;
using UnityEngine;

public abstract class DialogEvent : ScriptableObject
{
    public abstract void Action();
    public abstract Reward GetReward();

    public Action OnComplete;

}

public class NullDialogEvent : DialogEvent
{
    public override void Action()
    {
        
    }

    public override Reward GetReward()
    {
        return null;
    }
}