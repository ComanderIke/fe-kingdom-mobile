using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;

public interface IWorldMapInputReceiver
{
    void LocationClicked(WorldMapPosition location);
    void ActorClicked(WM_Actor party);
}