using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.WorldMapStuff.Controller;
using UnityEngine;

namespace Game.WorldMapStuff.Model
{
    public abstract class WM_Actor:ScriptableObject, IActor
    {
        public TurnStateManager TurnStateManager { get; set; }

        protected WM_Actor()
        {
            TurnStateManager = new TurnStateManager(this);
            GameTransformManager = new WM_GameTransformManager();
        }
        public abstract void SetAttackTarget(bool b);
        [SerializeField][HideInInspector]
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

        public WM_GameTransformManager GameTransformManager { get; set; }
        public LocationController location { get; set; }
        public abstract void ResetPosition();

        public void Defeated()
        {
            location.Actor = null;
            faction.Parties.Remove((Party)this);
            Destroy(GameTransformManager.GameObject);
            Destroy(this);
        }
    }
}