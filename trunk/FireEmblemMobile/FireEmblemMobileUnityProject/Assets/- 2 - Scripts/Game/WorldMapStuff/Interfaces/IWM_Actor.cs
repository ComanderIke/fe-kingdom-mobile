using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.WorldMapStuff.Model;
using UnityEngine;

namespace Game.WorldMapStuff.Systems
{
    public interface IWM_Actor
    {
        void SetAttackTarget(bool b);
        TurnStateManager TurnStateManager { get; set; }
        WM_Faction Faction { get; set; }
        GameTransformManager GameTransformManager { get; set; }
        WorldMapPosition location { get; set; }
        void ResetPosition();
    }
}