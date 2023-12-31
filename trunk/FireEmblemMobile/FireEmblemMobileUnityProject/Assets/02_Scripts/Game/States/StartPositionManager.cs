using System.Linq;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Manager;
using Game.Map;
using Game.Mechanics;
using UnityEngine;
using Utility;

namespace Game.States
{
    public class StartPositionManager
    {
        public StartPosition[] startPositions;
        private GridSystem gridSystem;
        private UnitPlacementInputSystem touchInputReceiver;
        public StartPositionManager(UnitPlacementInputSystem inputSystem)
        {
            touchInputReceiver = inputSystem;
            startPositions = GameObject.FindObjectsOfType<StartPosition>();
            gridSystem = GridGameManager.Instance.GetSystem<GridSystem>();
        }
        public void ShowSwapable(IGridActor unit)
        {
            var tile= gridSystem.GetTile(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
            tile.tileVfx.ShowSwapable(tile);
        }

        public void HideSwapable(IGridActor unit)
        {
            var tile= gridSystem.GetTile(unit.GridComponent.GridPosition.X, unit.GridComponent.GridPosition.Y);
            tile.tileVfx.Hide(tile);
        }
        public void SetUpInputForStartPos()
        {
            //Debug.Log("SETUP");
            foreach (var startPos in startPositions)
            {
                // Debug.Log("Set Up startPos inputReveiver: " + UnitPlacementInputSystem);
                startPos.touchInputReceiver = touchInputReceiver;
            }
        }
        public void ShowStartPos()
        {
            foreach (var startpos in startPositions)
            {
                var tile= gridSystem.GetTile(startpos.GetXOnGrid(), startpos.GetYOnGrid());
                tile.tileVfx.ShowSwapable(tile);
                //tile.TileRenderer.SwapVisual();
            }
        }
        public void HideStartPos()
        {
            for(int i= startPositions.Length-1; i>=0;i-- )
            {
                if (startPositions[i] == null || startPositions[i].gameObject == null)//Switched Scene
                    continue;
                var tile= gridSystem.GetTile(startPositions[i].GetXOnGrid(), startPositions[i].GetYOnGrid());
                tile.tileVfx.Hide(tile);
                GameObject.Destroy(startPositions[i].gameObject);
                //tile.TileRenderer.SwapVisual();
            }
        }
        public void SwapPosition(Unit unit, StartPosition startPos)
        {
            if (startPos.Actor == unit)
            {
                //TODO RESET POSITION
            }
            if (startPos.Actor == null)
            {

                startPositions.FirstOrDefault(s => s.Actor == unit).Actor = null;
                startPos.Actor = unit;
                gridSystem.SetUnitPosition(unit,startPos.GetXOnGrid(), startPos.GetYOnGrid());
            }
        }
        public void SwapUnits(Unit unit, Unit unit2)
        {
            if (unit == unit2)
            {
                return;
            }

            var startPos1= startPositions.FirstOrDefault(s => s.Actor == unit);
            var startPos2= startPositions.FirstOrDefault(s => s.Actor == unit2);
            startPos1.Actor = unit2;
            startPos2.Actor = unit;
            gridSystem.SwapUnits(unit,unit2);
            

            //unitInputController.transform.position = new Vector3(currentSelectedUnitController.unit.GridComponent.GridPosition.Xtransform.position);

        }

        public void Init()
        {
            
            touchInputReceiver.unitDroppedOnStartPos += SwapPosition;
            touchInputReceiver.unitDroppedOnOtherUnit += SwapUnits;
            touchInputReceiver.active = true;
            UnitSelectionSystem.OnDeselectCharacter += ShowSwapable;
            UnitSelectionSystem.OnSelectedCharacter += HideSwapable;
        }

        public void DeInitialize()
        {
            touchInputReceiver.unitDroppedOnOtherUnit -= SwapUnits;
            touchInputReceiver.active = false;
            UnitSelectionSystem.OnDeselectCharacter -= ShowSwapable;
            UnitSelectionSystem.OnSelectedCharacter -= HideSwapable;
        }
    }
}