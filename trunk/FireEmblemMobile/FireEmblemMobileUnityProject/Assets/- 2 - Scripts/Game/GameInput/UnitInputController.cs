using System;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GameInput
{
    public class UnitInputController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {
        // public delegate void OnUnitDraggedEvent(int x, int y, Unit character);
        // public static event OnUnitDraggedEvent OnUnitDragged;
        // public delegate void OnStartDragEvent(IGridActor actor);
        // public static event OnStartDragEvent OnStartDrag;
        // public delegate void OnDraggedOverUnitEvent(Unit unit);
        // public static event OnDraggedOverUnitEvent OnDraggedOverUnit;
        // public static event Action OnEndDrag;
        // public delegate void OnUnitClickedEvent(Unit character);
        // public static event OnUnitClickedEvent OnUnitClicked;
        // public static event OnUnitClickedEvent OnUnitDoubleClicked;

        public Unit unit;
        public BoxCollider2D boxCollider;

        public IUnitTouchInputReceiver touchInputReceiver { get; set; }
        [SerializeField]
        private UnitRenderer unitRenderer;


        private bool dragInitiated;
        private bool dragStarted;
        private bool doubleClick;
        private float timerForDoubleClick;
        private const float DOUBLE_CLICK_TIME = 0.4f;
        //private bool unitSelectedBeforeClicking = false;
        
        
        private void OnMouseEnter()
        {
          // unitRenderer.ShowHover();
          //  touchInputReceiver?.OnMouseEnter(this);
        }

        private void OnMouseExit()
        {
           // unitRenderer.HideHover();
        }

        public void OnMouseDrag()
        {

            touchInputReceiver?.OnMouseDrag(this);
        }
        public void OnMouseDown()
        {
            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                touchInputReceiver?.OnMouseDown(this);
            }
        }
        public void OnMouseUp()
        {
            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                touchInputReceiver?.OnMouseUp(this);
            }
        }
        
        public Transform GetTransform()
        {
            return transform;
        }

   
        public void OnBeginDrag(PointerEventData eventData)
        {

            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                touchInputReceiver?.OnBeginDrag(this, eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                touchInputReceiver?.OnEndDrag(this, eventData);
            }

        }
  
        public void OnDrag(PointerEventData eventData)
        {
            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                touchInputReceiver?.OnMouseDrag(this, eventData);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            touchInputReceiver?.OnDrop(this, eventData);
        }
    }
}