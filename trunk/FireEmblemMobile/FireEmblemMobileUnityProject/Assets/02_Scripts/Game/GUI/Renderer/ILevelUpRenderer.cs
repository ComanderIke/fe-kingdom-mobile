using System;
using Game.GameActors.Units;
using Game.Interfaces;
using UnityEngine;

namespace Game.GUI.Renderer
{
    public interface ILevelUpRenderer : IAnimation
    {
        void Play();
        void UpdateValues(Unit unit, string name, Sprite sprite, int levelBefore, int levelAfter, int[] stats, int[] statsIncreases, int rerollAmounts);
        event Action OnFinished;
        event Action OnReroll;
        void ResetForReroll();
    }
}