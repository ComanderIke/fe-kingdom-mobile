namespace Game.WorldMapStuff.Systems
{
    public class WM_SelectionManager : IWM_SelectionDataProvider

    {
        public IWM_Actor SelectedActor { get; private set; }
        private WorldMapPosition selectedLocation;
        private IWM_Actor selectedAttackTarget;

        public WM_SelectionManager()
        {
            SelectedActor = null;
            selectedAttackTarget = null;
            selectedLocation = null;
        }
        public WorldMapPosition GetSelectedLocation()
        {
            throw new System.NotImplementedException();
        }

        public void GetSelectedLocation(WorldMapPosition position)
        {
            throw new System.NotImplementedException();
        }

        public void SetSelectedAttackTarget(IWM_Actor target)
        {
            selectedAttackTarget?.SetAttackTarget(false);
            selectedAttackTarget = target;
            target.SetAttackTarget(true);
        }

        public IWM_Actor GetSelectedAttackTarget()
        {
            return selectedAttackTarget;
        }

        public void ClearData()
        {
            selectedLocation = null;
            SelectedActor = null;
            selectedAttackTarget = null;

        }

        public void ClearAttackTarget()
        {
            selectedAttackTarget = null;
        }
    }
}