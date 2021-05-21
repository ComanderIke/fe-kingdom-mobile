using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;

namespace Game.WorldMapStuff.Systems
{
    public class WM_SelectionManager : IWM_SelectionDataProvider

    {
        private WM_PartySelectionSystem partySelectionSystem;
        public WM_Actor SelectedActor => partySelectionSystem.SelectedActor;
        private WorldMapPosition selectedLocation;
        private WM_Actor selectedAttackTarget;

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

        public void SetSelectedAttackTarget(WM_Actor target)
        {
            selectedAttackTarget?.SetAttackTarget(false);
            selectedAttackTarget = target;
            target.SetAttackTarget(true);
        }

        public WM_Actor GetSelectedAttackTarget()
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