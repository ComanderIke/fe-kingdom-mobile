using System;
using Game.GameActors.Players;
using Game.GameActors.Units;
using Game.Manager;
using Game.Map;
using GameEngine;
using UnityEngine;

namespace Game.GameInput
{
    public class GridInputSystem : IEngineSystem, IUnitInputReceiver, IGridInputReceiver
    {
        public event Action<bool> OnInputStateChanged;
        public static event Action OnResetInput;
        public bool Active { get; private set; }
        
        private GridSystem gridSystem;
        private GridInput gridInput;
        public IGameInputReceiver inputReceiver { get; set; }

        private int lastDragPosX = -1;
        private int lastDragPosY = -1;


        public void Init()
        {
            Active = true;
            gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            
            gridInput = new GridInput();
            gridInput.RegisterInputReceiver(this);
            
        }

        public void Deactivate()
        {
            
        }

        public void Activate()
        {
            
        }

        public void Update()
        {
            if (!Active)
                return;
            gridInput.Update();
        }

        public void SetActive(bool active)
        {
            Active = active;
            if (Active)
            {
                OnInputStateChanged?.Invoke(Active);
                 //Debug.Log(Active ? "Input Activated:" : "Input Deactivated");
            }
        }
        
        public void ActorDragEnded(IGridActor actor, int x, int y)
        {
            var tile = gridSystem.GetTile(x, y);
            if (tile.HasFreeSpace())
            {
                inputReceiver.DraggedOnGrid(x, y);
            }
            else
            {
                inputReceiver.DraggedOnObject(tile.GridObject);
            }

            ResetInput();
        }

        public void ActorDoubleClicked(IGridActor unit)
        {
            Debug.Log("Player Input: Unit double clicked: " + unit);
            if (!Active)
                return;
            inputReceiver.DoubleClickedActor(unit);
        }

        public void ActorClicked(IGridActor unit)
        {
            //Debug.Log("Player Input: Unit clicked: " + unit);
            if (!Active)
                return;
            inputReceiver.ClickedOnActor(unit);
        }

        public void StartDraggingActor(IGridActor actor)
        {
            inputReceiver.StartDraggingActor(actor);
        }

        public void DraggedOverObject(IGridObject actor)
        {
            inputReceiver.DraggedOverObject(actor);
        }

        public void ActorLongHold(IGridActor unit)
        {
            inputReceiver.LongClickOnCharacter(unit);
            //Debug.Log("LongHoldInGridInputReceiver??! Intenden?");
        }

        public void ActorDragged(IGridActor actor, int x, int y)
        {
            if (!Active || IsOldDrag(x, y) || gridSystem.IsOutOfBounds(x, y))
                return;
            lastDragPosX = x;
            lastDragPosY = y;
            //Debug.Log("Dragged on: "+x+" "+y);
            var tile = gridSystem.GetTile(x, y);
            if (tile.GridObject != null)
            {
                inputReceiver.DraggedOverObject(tile.GridObject);
            }
            else
            {
              
                inputReceiver.DraggedOverGrid(x, y);
            }
        }
        public void GridClicked(int x, int y)
        {
          //  Debug.Log("Grid Clicked: "+x+" "+y);
            inputReceiver.ClickedOnGrid(x, y);
        }

        public void GridClickedDown(int x, int y)
        {
            inputReceiver.ClickedDownOnGrid(x, y);
        }

        private bool IsOldDrag(int x, int y)
        {
            return x == lastDragPosX && y == lastDragPosY;
        }

        public void ResetInput()
        {
            lastDragPosX = -1;
            lastDragPosY = -1;
            inputReceiver.ResetInput();
            OnResetInput?.Invoke();
        }

       
    }
}