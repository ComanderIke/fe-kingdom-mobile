using Game.States;

namespace Game.GUI
{
    public interface IExpRenderer : IAnimation
    {
        void UpdateValues(int currentExp, int addedExp);
    }
}