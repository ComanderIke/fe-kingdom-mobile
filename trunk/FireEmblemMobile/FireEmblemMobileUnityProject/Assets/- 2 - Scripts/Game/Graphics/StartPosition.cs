using Game.GameActors.Units;
using Game.GameInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
    public class StartPosition : MonoBehaviour, IDropHandler {

        void Start () {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        public int GetXOnGrid()
        {
            return (int)transform.localPosition.x;
        }
        public int GetYOnGrid()
        {
            return (int)transform.localPosition.y;
        }
        public IUnitTouchInputReceiver touchInputReceiver { get; set; }
        public void OnDrop(PointerEventData eventData)
        {
            touchInputReceiver?.OnDrop(this, eventData);
        }
        public Unit Actor { get; set; }
    }
}
