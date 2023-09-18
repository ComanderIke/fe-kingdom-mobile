using Game.GameActors.Units;

namespace Game.GUI
{
    public interface IClickedReceiver
    {
        void Clicked(Unit unit);
    }
}