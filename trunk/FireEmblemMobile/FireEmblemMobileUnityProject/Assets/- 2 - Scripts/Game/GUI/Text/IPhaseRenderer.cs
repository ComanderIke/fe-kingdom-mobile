using System;

namespace Game.GUI.Text
{
    public interface IPhaseRenderer
    {
        void Show(int id, Action OnFinished);
        
    }
}