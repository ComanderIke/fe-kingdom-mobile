using Assets.Core;
using Assets.GameInput;
using Assets.GUI;
using Assets.Mechanics.Dialogs;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.GameActors.Units.OnGameObject
{
    public class UnitInputController : MonoBehaviour, IDragAble
    {
        public delegate void OnUnitDraggedEvent(int x, int y, Unit character);
        public static event OnUnitDraggedEvent OnUnitDragged;
        public delegate void OnStartDragEvent(int gridX, int gridY);
        public static event OnStartDragEvent OnStartDrag;
        public delegate void OnDraggedOverUnitEvent(Unit unit);
        public static event OnDraggedOverUnitEvent OnDraggedOverUnit;
        public static event Action OnEndDrag;
        public delegate void OnUnitClickedEvent(Unit character);
        public static event OnUnitClickedEvent OnUnitClicked;
        public static event OnUnitClickedEvent OnUnitDoubleClicked;

        public Unit Unit;
        public DragManager DragManager { get; set; }
        public RaycastManager RaycastManager { get; set; }

        public static bool LockInput = true;
        private bool dragInitiated;
        private bool dragStarted;
        private bool doubleClick;
        private float timerForDoubleClick;
        private const float DOUBLE_CLICK_TIME = 0.4f;
        private bool unitSelectedBeforeClicking = false;

        private void Start()
        {
            DragManager = new DragManager(this);
            RaycastManager = new RaycastManager();
        }

        private void Update()
        {
            if (LockInput)
                return;
            DragManager.Update();
        }

        #region MouseInteraction

        private void OnMouseEnter()
        {
            if(DragManager.IsAnyUnitDragged)
                if (!EventSystem.current.IsPointerOverGameObject()) OnDraggedOverUnit(Unit);
        }

        private void OnMouseDrag()
        {
            if (LockInput)
                return;
            if (!EventSystem.current.IsPointerOverGameObject() && (dragStarted || dragInitiated))
            {
                dragStarted = false;
                dragInitiated = true;
                if (Unit.UnitTurnState.IsDragable()) DragManager.Dragging();
            }
        }


        private void OnMouseDown()
        {

            dragStarted = false;
            dragInitiated = false;
            doubleClick = false;
            
            if (!Unit.IsAlive())
                return;
            if (LockInput)
                return;

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log(Time.time-timerForDoubleClick);
                if (timerForDoubleClick != 0 &&  Time.time - timerForDoubleClick < DOUBLE_CLICK_TIME)
                {
                   
                    timerForDoubleClick = 0;
                    unitSelectedBeforeClicking = Unit.UnitTurnState.Selected;
                    doubleClick = true;
                }
                else
                {
                    timerForDoubleClick = Time.time;
                    DragManager.StartDrag();
                }

                
            }
        }
        private void OnMouseUp()
        {
            
            if (DragManager.IsDragging)
            {
                DragManager.Update();// Update Dragmanager because he should notice first when MouseUp happens
                dragStarted = false;
                dragInitiated = false;
                Unit.OnUnitActiveStateUpdated(Unit, true, false);
               
                OnEndDrag();
                gameObject.GetComponent<BoxCollider>().enabled = true;
            }
            else if (unitSelectedBeforeClicking)
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if(doubleClick)
                        OnUnitDoubleClicked(Unit);
                    else
                        OnUnitClicked(Unit);
                }
            }

            if (doubleClick)
            {
                doubleClick = false;
                Debug.Log("DOUBLE CLICK");
            }
        }

        #endregion

        #region INTERFACE DragAble

        public Transform GetTransform()
        {
            return transform;
        }

        public void StartDrag()
        {
            dragStarted = true;
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            OnStartDrag((int) gridPos.x, (int) gridPos.y);
            unitSelectedBeforeClicking = Unit.UnitTurnState.Selected;
            if (!Unit.UnitTurnState.Selected)//If unit is already selected wait for MouseUp/DragEnd to invoke OnUnitClicked
                if (!EventSystem.current.IsPointerOverGameObject()) OnUnitClicked(Unit);
        }

        public void Dragging()
        {
            Unit.OnUnitActiveStateUpdated(Unit, false, true);
            GetComponent<BoxCollider>().enabled = false;
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            if (RaycastManager.ConnectedLatestHit())
                OnUnitDragged((int) gridPos.x, (int) gridPos.y, Unit);
        }

        public void EndDrag()
        {
        }

        public void NotDragging()
        {
        }

        #endregion

    }
}