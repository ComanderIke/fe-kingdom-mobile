using System;
using Game.GameActors.Factions;

namespace Game.GUI.Text
{
    public interface IPhaseRenderer
    {
        void Show(FactionId id, Action OnFinished);
        
    }
}