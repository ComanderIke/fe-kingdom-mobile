using Game.WorldMapStuff.Model;

namespace Game.WorldMapStuff.Interfaces
{
    public interface IWorldMapUnitInputReceiver
    {
        void ActorClicked(WM_Actor party);
    }
}