using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    public abstract class WM_Actor:ScriptableObject, IActor
    {
        public TurnStateManager TurnStateManager { get; set; }

        protected WM_Actor()
        {
            TurnStateManager = new TurnStateManager(this);
        }
        public abstract void SetAttackTarget(bool b);
        [SerializeField]
        private WM_Faction faction;

        public WM_Faction Faction
        {
            get
            {
                return faction;
            }
            set
            {
                faction = value;
            }
        }

        public GameTransformManager GameTransformManager { get; set; }
        public WorldMapPosition location { get; set; }
        public abstract void ResetPosition();
    }
}