using System;
using Game.GameActors.Units;
using Game.Manager;
using Game.Map;
using GameEngine;
using UnityEngine;

namespace Game.GameInput
{
    public class GridInputSystem : MonoBehaviour, IEngineSystem, IUnitInputReceiver, IGridInputReceiver
    {
        public static event Action<bool> OnInputStateChanged;
        public static bool Active { get; private set; }
        
        private GridSystem gridSystem;
        private GridInput gridInput;
        private IGameInputReceiver inputReceiver;

        private int lastDragPosX = -1;
        private int lastDragPosY = -1;

        private void Start()
        {
            Active = true;
            gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
            inputReceiver = new GameInputReceiver();
            gridInput = new GridInput();
            gridInput.RegisterInputReceiver(this);
        }

        private void Update()
        {
            if (!Active)
                return;
            gridInput.Update();
        }

        public static void SetActive(bool active)
        {
            Active = active;
            if (Active)
            {
                OnInputStateChanged?.Invoke(Active);
                Debug.Log(Active ? "Input Activated:" : "Input Deactivated");
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
                inputReceiver.DraggedOnActor(tile.Actor);
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
            Debug.Log("Player Input: Unit clicked: " + unit);
            if (!Active)
                return;
            inputReceiver.ClickedOnActor(unit);
        }

        public void StartDraggingActor(IGridActor actor)
        {
            inputReceiver.StartDraggingActor(actor);
        }

        public void DraggedOverActor(IGridActor actor)
        {
            inputReceiver.DraggedOverActor(actor);
        }

        public void ActorDragged(IGridActor actor, int x, int y)
        {
            if (!Active || IsOldDrag(x, y) || gridSystem.IsOutOfBounds(x, y))
                return;

            lastDragPosX = x;
            lastDragPosY = y;

            var tile = gridSystem.GetTile(x, y);
            if (tile.Actor != null)
            {
                inputReceiver.DraggedOverActor(tile.Actor);
            }
            else
            {
                inputReceiver.DraggedOverGrid(x, y);
            }
        }
        public void GridClicked(int x, int y)
        {
            inputReceiver.ClickedOnGrid(x, y);
        }

        private bool IsOldDrag(int x, int y)
        {
            return x == lastDragPosX && y == lastDragPosY;
        }

        private void ResetInput()
        {
            lastDragPosX = -1;
            lastDragPosY = -1;
            inputReceiver.ResetInput();
        }
    }
}