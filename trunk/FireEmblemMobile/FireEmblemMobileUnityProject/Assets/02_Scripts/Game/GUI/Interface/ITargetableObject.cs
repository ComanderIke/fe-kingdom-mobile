using UnityEngine;

namespace Game.GUI.Interface
{
    public interface ITargetableObject
    {
        string GetName();
        string GetDescription();
        Sprite GetIcon();
    }
}