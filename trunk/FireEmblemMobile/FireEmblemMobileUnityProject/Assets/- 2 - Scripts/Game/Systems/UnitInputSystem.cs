﻿using System;
using Game.GameActors.Units;
using Game.GameActors.Units.OnGameObject;
using Game.Manager;
using GameCamera;
using GameEngine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.GameInput
{
    public class UnitInputSystem : IEngineSystem, IDragAble, IUnitTouchInputReceiver
    {
        public event Action EndedDrag;
        public event Action StartedDrag;
        public event Action<Unit> MouseUp;
        public IUnitInputReceiver InputReceiver { get; set; }
        private RaycastManager RaycastManager { get; set; }
        public DragManager DragManager { get; set; }

        private bool dragInitiated;
        private bool dragStarted;
        private bool doubleClick;
        private float timerForDoubleClick;
        private const float DOUBLE_CLICK_TIME = 0.4f;
        private bool unitSelectedBeforeClicking = false;
        public bool Active { get; private set; }

        public void Init()
        {
            DragManager = new DragManager(this);
            RaycastManager = new RaycastManager();
        }
        public void SetActive(bool active)
        {
            Active = active;
        }
        public void Update()
        {
            DragManager.Update();
        }

        public void OnMouseEnter(UnitInputController unitController)
        {
            if (DragManager.IsAnyUnitDragged)
            {
                // if (EventSystem.current.currentSelectedGameObject==null||EventSystem.current.currentSelectedGameObject.CompareTag(TagManager.UnitTag))
                // {
                Debug.Log("Dragged over: " + unitController.unit);
                InputReceiver.DraggedOverActor(unitController.unit);
                // }
            }
        }

        public void OnMouseDrag(UnitInputController unitController)
        {
            // if (EventSystem.current.currentSelectedGameObject==null||EventSystem.current.currentSelectedGameObject.CompareTag(TagManager.UnitTag) && (dragStarted || dragInitiated))
            // {
            if ((dragStarted || dragInitiated))
            {
               
                dragStarted = false;
                dragInitiated = true;
                if (unitController.unit.IsDragable()) 
                    
                    DragManager.Dragging(unitController.transform);
            }
        }

        public void OnMouseDrag(UnitInputController unitInputController, PointerEventData eventData)
        {
            
        }

        public void OnMouseDown(UnitInputController unitController)
        {
            dragStarted = false;
            dragInitiated = false;
            doubleClick = false;
            if (InputReceiver==null)
            {
                return;
            }
            if (!unitController.unit.IsAlive())
                return;
           

            // if (EventSystem.current.currentSelectedGameObject==null||EventSystem.current.currentSelectedGameObject.CompareTag(TagManager.UnitTag))
            // {
                Debug.Log("Clicked Unit: "+unitController.unit.name);
                if (timerForDoubleClick != 0 &&  Time.time - timerForDoubleClick < DOUBLE_CLICK_TIME)
                {
                   
                    timerForDoubleClick = 0;
                    unitSelectedBeforeClicking = unitController.unit.TurnStateManager.IsSelected;
                    doubleClick = true;
                }
                else
                {
                    timerForDoubleClick = Time.time;
                    if (unitController.unit.IsDragable())
                        DragManager.StartDrag(unitController.transform);
                }

                
            // }
            // else
            // {
            //     Debug.Log("Clicked On Unit BUT POINTER OVER GAMEOBJECT" + EventSystem.current.currentSelectedGameObject.name+" "+EventSystem.current.currentSelectedGameObject.tag);
            // }
        }
        public void OnMouseUp(UnitInputController unitController)
        {
            var unit = unitController.unit;
            if (InputReceiver==null)
            {
                return;
            }

            MouseUp?.Invoke(unitController.unit);
            
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

                unitController.boxCollider.enabled = true;
            }
            else if (!CameraSystem.IsDragging && (unitSelectedBeforeClicking||(unit.Faction.Id != GridGameManager.Instance.FactionManager.ActivePlayerNumber||doubleClick||unit.TurnStateManager.IsWaiting)))
            {
                // if (EventSystem.current.currentSelectedGameObject==null||EventSystem.current.currentSelectedGameObject.CompareTag(TagManager.UnitTag))
                // {
                    if(doubleClick)
                        InputReceiver.ActorDoubleClicked(unit);
                    else
                        InputReceiver.ActorClicked(unit);
                // }
            }

            if (doubleClick)
            {
                doubleClick = false;
            }
        }

        public void OnBeginDrag(UnitInputController unitInputController, PointerEventData eventData)
        {
            
        }

        public void OnEndDrag(UnitInputController unitInputController, PointerEventData eventData)
        {
            
        }

        public void OnDrop(UnitInputController unitInputController, PointerEventData eventData)
        {
            
        }

        public void StartDrag(Transform dragObject)
        {
            StartedDrag?.Invoke();
            var unitController = dragObject.GetComponent<UnitInputController>();
            var unit = unitController.unit;
            
            dragStarted = true;
            InputReceiver.StartDraggingActor(unit);
            unitSelectedBeforeClicking = unit.TurnStateManager.IsSelected;
            if (!unit.TurnStateManager.IsSelected)
                // if (EventSystem.current.currentSelectedGameObject==null||EventSystem.current.currentSelectedGameObject.CompareTag(TagManager.UnitTag))
                // {
                    Unit.OnUnitActiveStateUpdated?.Invoke(unit, false, true);
                    InputReceiver.ActorClicked(unit);
                // }
        }

        public void Dragging(Transform dragObject, float xPos, float yPos)
        {
            var unitController = dragObject.GetComponent<UnitInputController>();
            var unit = unitController.unit;
            unitController.boxCollider.enabled = false;
            var gridPos = RaycastManager.GetMousePositionOnGrid();
            if (RaycastManager.ConnectedLatestHit())
            {
                InputReceiver.ActorDragged(unitController.unit, (int)gridPos.x, (int)gridPos.y);
            }
        }
        

        
        public void EndDrag()
        {
            EndedDrag?.Invoke();
            
        }

        public void NotDragging()
        {
        }

      
    }

    public interface IUnitTouchInputReceiver
    {
        void OnMouseEnter(UnitInputController unitInputController);
        void OnMouseDrag(UnitInputController unitInputController);
        
        void OnMouseDrag(UnitInputController unitInputController, PointerEventData eventData);
        void OnMouseDown(UnitInputController unitInputController);
        void OnMouseUp(UnitInputController unitInputController);
        void OnBeginDrag(UnitInputController unitInputController, PointerEventData eventData);
        void OnEndDrag(UnitInputController unitInputController, PointerEventData eventData);
        void OnDrop(UnitInputController unitInputController, PointerEventData eventData);
    }
}