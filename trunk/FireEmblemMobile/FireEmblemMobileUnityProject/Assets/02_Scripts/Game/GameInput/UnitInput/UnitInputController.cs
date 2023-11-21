using System;
using __2___Scripts.Game.Utility;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Game.GameInput
{
    public class UnitInputController : MonoBehaviour, IMyDropHandler//, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
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

        [FormerlySerializedAs("unitBp")] public Unit unit;
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
            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                //Debug.Log("ON MOUSE Drag");
                touchInputReceiver?.OnMouseDrag(this);
            }
            else
            {
                
            }
        }

        private bool downClicked = false;
        private float holdDuration = 0;
        public const float HOLD_DURATION=1.5f;
        void Update()
        {
            if (downClicked)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Stationary||Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    holdDuration += Time.deltaTime;
                    if (holdDuration > HOLD_DURATION)
                    {
                        Debug.Log("ON MOUSE HOLD");
                        touchInputReceiver.OnMouseHold(this);
                    }
                }
                else
                {
                    holdDuration = 0;
                    downClicked = false;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                downClicked = false;
                holdDuration = 0;
                boxCollider.enabled = true;
            }
            
        }
        public void OnMouseDown()
        {
           // Debug.Log("UnitMouseDown");
            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                downClicked = true;
                holdDuration = 0;
                // Debug.Log("ON MOUSE DOWN");
                touchInputReceiver?.OnMouseDown(this);
            }
        }
        public void OnMouseUp()
        {
            
            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                // Debug.Log("ON MOUSE UP");
                touchInputReceiver?.OnMouseUp(this);
            }
        }
        
        public Transform GetTransform()
        {
            return transform;
        }

        
        
        public void OnDrop()
        {
            if (!UIClickChecker.CheckUIObjectsInPosition())
            {
                touchInputReceiver?.OnDrop(this);
            }
        }
    }
}