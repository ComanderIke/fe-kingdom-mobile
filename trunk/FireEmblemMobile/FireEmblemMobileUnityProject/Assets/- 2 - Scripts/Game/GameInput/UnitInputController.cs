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
        public BoxCollider2D boxCollider;
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
                    Debug.Log("Dragged over: "+unit);
                    InputReceiver.DraggedOverActor(unit);
                }
        }
        public void OnMouseDrag()
        {
            if (!GridInputSystem.Active)
                return;
            if (!EventSystem.current.IsPointerOverGameObject() && (dragStarted || dragInitiated))
            {
               
                dragStarted = false;
                dragInitiated = true;
                if (unit.IsDragable()) DragManager.Dragging();
            }
        }


        public void OnMouseDown()
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
                    unitSelectedBeforeClicking = unit.TurnStateManager.IsSelected;
                    doubleClick = true;
                }
                else
                {
                    timerForDoubleClick = Time.time;
                    if (unit.IsDragable())
                        DragManager.StartDrag();
                }

                
            }
            else
            {
                Debug.Log("Clicked On Unit BUT POINTER OVER GAMEOBJECT");
            }
        }
        public void OnMouseUp()
        {
            if(unit.Faction.Id == GridGameManager.Instance.FactionManager.ActivePlayerNumber&&unit.Faction.IsPlayerControlled)
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

                boxCollider.enabled = true;
            }
            else if (!CameraSystem.IsDragging && (unitSelectedBeforeClicking||(unit.Faction.Id != GridGameManager.Instance.FactionManager.ActivePlayerNumber||doubleClick||unit.TurnStateManager.IsWaiting)))
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
            unitSelectedBeforeClicking = unit.TurnStateManager.IsSelected;
            if (!unit.TurnStateManager.IsSelected)
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    Unit.OnUnitActiveStateUpdated?.Invoke(unit, false, true);
                    InputReceiver.ActorClicked(unit);
                }
        }

        public void Dragging(float xPos, float yPos)
        {
            boxCollider.enabled = false;
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