using System;
using Game.GameInput;
using Game.Manager;
using GameCamera;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Serialization;

namespace Game.GameActors.Units.OnGameObject
{
    public class UnitInputController : MonoBehaviour
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

        public UnitInputSystem InputSystem { get; set; }


        private bool dragInitiated;
        private bool dragStarted;
        private bool doubleClick;
        private float timerForDoubleClick;
        private const float DOUBLE_CLICK_TIME = 0.4f;
        private bool unitSelectedBeforeClicking = false;
        
        
        private void OnMouseEnter()
        {
            InputSystem.OnMouseEnter(this);
        }
        public void OnMouseDrag()
        {
            InputSystem.OnMouseDrag(this);
        }
        public void OnMouseDown()
        {
            InputSystem.OnMouseDown(this);
        }
        public void OnMouseUp()
        {
            InputSystem.OnMouseUp(this);
        }
        
        public Transform GetTransform()
        {
            return transform;
        }
    }
}