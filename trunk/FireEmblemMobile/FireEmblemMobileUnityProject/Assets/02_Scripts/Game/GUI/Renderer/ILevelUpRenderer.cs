

using System;
using Game.States;
using UnityEngine;

namespace Game.GUI
{
    public interface ILevelUpRenderer : IAnimation
    {
        void Play();
        void UpdateValues(string name, Sprite sprite, int levelBefore, int levelAfter, int[] stats, int[] statsIncreases, int rerollAmounts);
        event Action OnFinished;
        event Action OnReroll;
        void ResetForReroll();
    }
}