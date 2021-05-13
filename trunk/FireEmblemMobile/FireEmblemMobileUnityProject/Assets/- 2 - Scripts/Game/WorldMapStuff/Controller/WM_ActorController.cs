using Game.WorldMapStuff.Interfaces;
using Game.WorldMapStuff.Model;
using Game.WorldMapStuff.Visuals;
using UnityEngine;

namespace Game.WorldMapStuff.Controller
{
    [RequireComponent(typeof(WM_ActorRenderer))]
    public class WM_ActorController : MonoBehaviour
    {
        // Start is called before the first frame update
        private bool selected;
        public WM_Actor actor;
        [SerializeField] private WorldMapPosition currentLocation;
        public IWorldMapUnitInputReceiver inputReceiver { get; set; }
        void Start()
        {
            actor.location = currentLocation;
            currentLocation.Actor = actor;
            actor.GameTransformManager.GameObject = gameObject;
            Debug.Log("Start"+gameObject);
        }
        void OnMouseDown()
        {
            inputReceiver.ActorClicked(actor);

        }
    }
}