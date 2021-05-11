using Game.WorldMapStuff.Systems;

public interface IWorldMapInputReceiver
{
    void LocationClicked(WorldMapPosition location);
    void ActorClicked(IWM_Actor party);
}