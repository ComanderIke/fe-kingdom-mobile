

using System;
using Game.States;
using UnityEngine;

namespace Game.GUI
{
    public interface ILevelUpRenderer : IAnimation
    {
        void UpdateValues(string name, Sprite sprite, int levelBefore, int levelAfter, int[] stats, int[] statsIncreases);
        event Action OnFinished;
    }
}