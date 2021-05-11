using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.WorldMapStuff.Model;

namespace Game.WorldMapStuff.Systems
{
    public interface IWM_Actor
    {
        void SetAttackTarget(bool b);
        TurnStateManager TurnStateManager { get; set; }
        WM_Faction Faction { get; set; }
        void ResetPosition();
    }
}