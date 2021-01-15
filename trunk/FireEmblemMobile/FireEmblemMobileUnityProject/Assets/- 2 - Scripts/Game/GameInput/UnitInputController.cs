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
    public class UnitInputController : MonoBehaviour, IDragAble
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
        private DragManager DragManager { get; set; }
        private RaycastManager RaycastManager { get; set; }
        private IUnitInputReceiver InputReceiver { get; set; }


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
            InputReceiver = FindObjectOfType<GridInputSystem>();
        }

        private void Update()
        {
           
            if (!GridInputSystem.Active)
                return;
            DragManager.Update();
        }

        #region MouseInteraction

        private void OnMouseEnter()
        {
            if(DragManager.IsAnyUnitDragged)
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    InputReceiver.DraggedOverActor(unit);
                }
        }
        private void OnMouseDrag()
        {
            if (!GridInputSystem.Active)
                return;
            if (!EventSystem.current.IsPointerOverGameObject() && (dragStarted || dragInitiated))
            {
               
                dragStarted = false;
                dragInitiated = true;
                if (unit.UnitTurnState.IsDragable()) DragManager.Dragging();
            }
        }


        private void OnMouseDown()
        {

            dragStarted = false;
            dragInitiated = false;
            doubleClick = false;
            if (!GridInputSystem.Active)
                return;
            if (!unit.IsAlive())
                return;
           

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log(Time.time-timerForDoubleClick);
                if (timerForDoubleClick != 0 &&  Time.time - timerForDoubleClick < DOUBLE_CLICK_TIME)
                {
                   
                    timerForDoubleClick = 0;
                    unitSelectedBeforeClicking = unit.UnitTurnState.Selected;
                    doubleClick = true;
                }
                else
                {
                    timerForDoubleClick = Time.time;
                    if (unit.UnitTurnState.IsDragable())
                        DragManager.StartDrag();
                }

                
            }
        }
        private void OnMouseUp()
        {
            GridGameManager.Instance.GetSystem<CameraSystem>().ActivateMixin<DragCameraMixin>();
            if (DragManager.IsDragging)
            {
               
                DragManager.Update();// Update Dragmanager because he should notice first when MouseUp happens
                dragStarted = false;
                dragInitiated = false;
                Unit.OnUnitActiveStateUpdated?.Invoke(unit, true, false);
                var gridPos = RaycastManager.GetMousePositionOnGrid();
                if (RaycastManager.ConnectedLatestHit())
                {
                    InputReceiver.ActorDragEnded(unit, (int)gridPos.x, (int)gridPos.y);
                }

                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                
            }
            else if (!CameraSystem.IsDragging && (unitSelectedBeforeClicking||(unit.Faction.Id != GridGameManager.Instance.FactionManager.ActivePlayerNumber||doubleClick)))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if(doubleClick)
                        InputReceiver.ActorDoubleClicked(unit);
                    else
                        InputReceiver.ActorClicked(unit);
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
            GridGameManager.Instance.GetSystem<CameraSystem>().DeactivateMixin<DragCameraMixin>();
            dragStarted = true;
            InputReceiver.StartDraggingActor(unit);
            unitSelectedBeforeClicking = unit.UnitTurnState.Selected;
            if (!unit.UnitTurnState.Selected)
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Unit.OnUnitActiveStateUpdated?.Invoke(unit, false, true);
                    InputReceiver.ActorClicked(unit);
                }
        }

        public void Dragging(float xPos, float yPos)
        {
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            if (RaycastManager.ConnectedLatestHit())
            {
                InputReceiver.ActorDragged(unit, (int)gridPos.x, (int)gridPos.y);
            }
        }

        public void EndDrag()
        {
            GridGameManager.Instance.GetSystem<CameraSystem>().ActivateMixin<DragCameraMixin>();
        }

        public void NotDragging()
        {
        }

        #endregion

    }
}