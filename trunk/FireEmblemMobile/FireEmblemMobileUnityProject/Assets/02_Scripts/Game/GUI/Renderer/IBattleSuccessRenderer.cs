using System;

namespace Game.GUI.Renderer
{
    public interface IBattleSuccessRenderer
    {
        public void Show();
        void Hide();
        event Action OnFinished;
    }
}