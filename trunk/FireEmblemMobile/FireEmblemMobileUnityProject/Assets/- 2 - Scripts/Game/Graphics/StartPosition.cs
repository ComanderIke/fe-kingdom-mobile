using Game.GameActors.Units;
using Game.GameInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Utility
{
    [ExecuteInEditMode]
    public class StartPosition : MonoBehaviour, IDropHandler {

        void Start () {
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        }

        public void Destroy()
        {
            Destroy(gameObject);
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
            
            Debug.Log("Dropped on START POS");
            touchInputReceiver?.OnDrop(this, eventData);
            if (touchInputReceiver == null)
            {
                Debug.LogError("touchInputReceiver is null");
            }
        }
        void Update()
        {
            transform.localPosition = new Vector3((int) transform.localPosition.x, (int) transform.localPosition.y,
                (int) transform.localPosition.z);
        }
        public Unit Actor { get; set; }
    }
}
