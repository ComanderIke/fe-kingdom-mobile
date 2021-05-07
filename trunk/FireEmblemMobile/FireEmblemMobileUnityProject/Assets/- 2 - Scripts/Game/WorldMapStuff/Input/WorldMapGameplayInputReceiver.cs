namespace Game.WorldMapStuff.Systems
{
    public class WorldMapGameplayInputReceiver: IWorldMapInputReceiver
    {
        private WM_GameplayInput gameplayInput;
        private IWM_SelectionDataProvider selectionDataProvider;
        private WM_LastInputPositionManager lastInputPositionManager;
    }
}