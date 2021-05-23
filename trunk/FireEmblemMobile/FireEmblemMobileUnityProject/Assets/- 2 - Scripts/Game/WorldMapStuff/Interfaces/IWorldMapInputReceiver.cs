using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Systems;

public interface IWorldMapInputReceiver
{
    void LocationClicked(LocationController location);
    void ActorClicked(WM_Actor party);
    void WorldClicked();
}