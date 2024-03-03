using Game.GameActors.Units;

namespace Game.GUI.Controller
{
    public interface IClickedReceiver
    {
        void Clicked(Unit unit);
    }
}