namespace Game.WorldMapStuff.Systems
{
    internal interface IWM_SelectionDataProvider
    {
        IWM_Actor SelectedActor { get; }

        WorldMapPosition GetSelectedLocation();
        void GetSelectedLocation(WorldMapPosition position);
        void SetSelectedAttackTarget(IWM_Actor target);
        IWM_Actor GetSelectedAttackTarget();
        void ClearData();
        void ClearAttackTarget();
    }
}