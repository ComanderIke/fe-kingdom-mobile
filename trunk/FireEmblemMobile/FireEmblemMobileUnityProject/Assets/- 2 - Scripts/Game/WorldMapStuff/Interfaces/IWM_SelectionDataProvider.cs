using Game.WorldMapStuff.Controller;
using Game.WorldMapStuff.Model;

namespace Game.WorldMapStuff.Systems
{
    internal interface IWM_SelectionDataProvider
    {
        WM_Actor SelectedActor { get; }

        WorldMapPosition GetSelectedLocation();
        void SetSelectedLocation(WorldMapPosition position);
        void SetSelectedAttackTarget(WM_Actor target);
        WM_Actor GetSelectedAttackTarget();
        void ClearData();
        void ClearAttackTarget();
    }
}