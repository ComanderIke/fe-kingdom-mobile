using System;

namespace Game.GUI.Text
{
    public interface IPhaseRenderer
    {
        void Show(FactionId id, Action OnFinished);
        
    }
}