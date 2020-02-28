using Assets.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[System.Serializable]
public abstract class CharacterState : ScriptableObject
{
    public int duration;
    private int currduration;

    public virtual bool TakeEffect(Unit c)
    {
        if (currduration > 0)
        {
            currduration -= 1;
        }
        else
            return true;
        return false;

    }
    public abstract void Remove(Unit c);
    private void OnEnable()
    {
        currduration = duration;
    }
}

