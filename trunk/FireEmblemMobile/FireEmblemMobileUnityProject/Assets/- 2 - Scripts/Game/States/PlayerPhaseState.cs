using System.Linq;
using Game.AI;
using Game.GameActors.Units;
using Game.GameInput;
using Game.Grid;
using Game.GUI;
using Game.Manager;
using Game.Map;
using GameCamera;
using GameEngine;
using GameEngine.GameStates;
using GameEngine.Input;
using GameEngine.Tools;
using UnityEngine;

namespace Game.Mechanics
{
    public class PlayerPhaseState : GameState<NextStateTrigger>, IDependecyInjection
    {
        private readonly GridGameManager gridGameManager;

        private FactionManager factionManager;
        private ConditionManager conditionManager;
        private GridInputSystem gridInputSystem;
        private UnitInputSystem unitInputSystem;
       // private ISelectionDataProvider selectionDataProvider;
        private CameraSystem cameraSystem;
        public IPlayerPhaseUI playerPhaseUI;//Inject

        public PlayerPhaseState(ConditionManager conditionManager)
        {
            gridGameManager = GridGameManager.Instance;
            factionManager = gridGameManager.FactionManager;
            this.conditionManager = conditionManager;
            cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
            gridInputSystem = new GridInputSystem();
            unitInputSystem = new UnitInputSystem();
            
           
          
            unitInputSystem.InputReceiver = gridInputSystem;
            unitInputSystem.EndedDrag += ActivateCameraDrag;
            unitInputSystem.StartedDrag += DeactivateCameraDrag;
            unitInputSystem.MouseUp += FindBetterName;
           
        }

        public void Init()
        {
            gridInputSystem.inputReceiver = new GameInputReceiver(gridGameManager.GetSystem<GridSystem>());
         
            gridInputSystem.Init();
            unitInputSystem.Init();
            cameraSystem.Init();
        }

        public override void Enter()
        {
           // Debug.Log("Enter GameplayState");
           gridGameManager.GetSystem<GridSystem>().cursor.OnCursorPositionChanged += CursorPosChanged;
            cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
                new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(),new MouseCameraInputProvider());
            int height = gridGameManager.GetSystem<GridSystem>().GridData.height;
            int width = gridGameManager.GetSystem<GridSystem>().GridData.width;
           // cameraSystem.AddMixin<ClampCameraMixin>().Construct(width, height);
            cameraSystem.AddMixin<ViewOnGridMixin>().zoom = 0;
            gridInputSystem.SetActive(true);
            unitInputSystem.SetActive(true);
            playerPhaseUI.Show(gridGameManager.GetSystem<TurnSystem>().TurnCount);
            playerPhaseUI.SubscribeOnBackClicked(Undo);
            
            SetUpInputForUnits();
            UnitSelectionSystem.OnSelectedInActiveCharacter += OnSelectedCharacter;
            UnitSelectionSystem.OnSelectedCharacter += OnSelectedCharacter;
            // add as InputReceiver to all units
        }

        public void Undo()
        {
            Debug.Log("UNDO123");
        }

        private void CursorPosChanged(Vector2Int tilePos)
        {
            playerPhaseUI.ShowTileInfo(gridGameManager.GetSystem<GridSystem>().cursor.GetCurrentTile());
        }

        private void FindBetterName(Unit unit)
        {
            if(factionManager.IsActiveFaction(unit.Faction)&&unit.Faction.IsPlayerControlled)
                ActivateCameraDrag();
        }
        private void ActivateCameraDrag()
        {
            cameraSystem.ActivateMixin<DragCameraMixin>();
        }
        private void DeactivateCameraDrag()
        {
            cameraSystem.DeactivateMixin<DragCameraMixin>();
        }
        private void SetUpInputForUnits()
        {
            foreach (var unit in factionManager.Factions.SelectMany(faction => faction.Units))
            {
                if (unit.GameTransformManager.GameObject != null)
                {
                    unit.GameTransformManager.UnitController.touchInputReceiver = unitInputSystem;
                    
                }
            }
        }

        public override GameState<NextStateTrigger> Update()
        {
            unitInputSystem.Update();
            gridInputSystem.Update();
            
            
            if (conditionManager.CheckLose())
            {
                return  GridGameManager.Instance.GameStateManager.GameOverState;
            }
            else if (conditionManager.CheckWin())
            {
                return  GridGameManager.Instance.GameStateManager.WinState;
            }
            return NextState;
        }

        private void OnSelectedCharacter(IGridActor character)
        {
            gridInputSystem.inputReceiver.ResetInput();
        }
        public override void Exit()
        {
            playerPhaseUI.UnsubscribeOnBackClicked(Undo);
            cameraSystem.RemoveMixin<DragCameraMixin>();
            cameraSystem.RemoveMixin<ClampCameraMixin>();
            cameraSystem.RemoveMixin<ViewOnGridMixin>();
            UnitSelectionSystem.OnSelectedInActiveCharacter -=OnSelectedCharacter;
            UnitSelectionSystem.OnSelectedCharacter -= OnSelectedCharacter;
            gridGameManager.GetSystem<GridSystem>().cursor.OnCursorPositionChanged -= CursorPosChanged;
            gridInputSystem.ResetInput();
            gridInputSystem.SetActive(false);
            unitInputSystem.SetActive(false);
            playerPhaseUI.Hide();
            playerPhaseUI.HideTileInfo();
            
            // remove as Input Receiver to all Units

        }

       
    }
}