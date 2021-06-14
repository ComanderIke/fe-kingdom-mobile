﻿using Game.GameActors.Units;
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
            
            Debug.Log("Dropped on START POS");
            touchInputReceiver?.OnDrop(this, eventData);
            if (touchInputReceiver == null)
            {
                Debug.LogError("touchInputReceiver is null");
            }
        }
        public Unit Actor { get; set; }
    }
}
