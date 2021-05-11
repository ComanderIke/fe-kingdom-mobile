namespace Game.WorldMapStuff.Systems
{
    public class WM_SelectionManager : IWM_SelectionDataProvider

    {
        private WM_PartySelectionSystem partySelectionSystem;
        public IWM_Actor SelectedActor => partySelectionSystem.SelectedActor;
        private WorldMapPosition selectedLocation;
        private IWM_Actor selectedAttackTarget;

        public WM_SelectionManager(WM_PartySelectionSystem partySelectionSystem)
        {
            selectedAttackTarget = null;
            selectedLocation = null;
            this.partySelectionSystem = partySelectionSystem;
        }
        public WorldMapPosition GetSelectedLocation()
        {
            return selectedLocation;
        }

        public void SetSelectedLocation(WorldMapPosition position)
        {
            selectedLocation = position;
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
            ClearAttackTarget();

        }

        public void ClearAttackTarget()
        {
            selectedAttackTarget = null;
        }
    }
}