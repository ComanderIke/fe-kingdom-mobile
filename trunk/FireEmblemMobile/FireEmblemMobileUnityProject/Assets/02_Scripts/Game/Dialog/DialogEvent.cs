using UnityEngine;

public abstract class DialogEvent : ScriptableObject
{
    public abstract void Action();
   
}

public class NullDialogEvent : DialogEvent
{
    public override void Action()
    {
        
    }
}