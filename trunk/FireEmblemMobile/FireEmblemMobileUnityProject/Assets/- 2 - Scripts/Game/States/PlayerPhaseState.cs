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
        private CameraSystem cameraSystem;
        public IPlayerPhaseUI playerPhaseUI;//Inject

        public PlayerPhaseState()
        {
            gridGameManager = GridGameManager.Instance;
            factionManager = gridGameManager.FactionManager;
            conditionManager = new ConditionManager();
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
            Debug.Log("Enter GameplayState");
            cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
                new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(),new MouseInputProvider());
            int height = gridGameManager.GetSystem<GridSystem>().GridData.height;
            int width = gridGameManager.GetSystem<GridSystem>().GridData.width;
            cameraSystem.AddMixin<ClampCameraMixin>().Construct(width, height);
            cameraSystem.AddMixin<ViewOnGridMixin>().zoom = 0;
            gridInputSystem.SetActive(true);
            unitInputSystem.SetActive(true);
            playerPhaseUI.Show(gridGameManager.GetSystem<TurnSystem>().TurnCount);
            SetUpInputForUnits();
            // add as InputReceiver to all units
        }

        private void FindBetterName(Unit unit)
        {
            if(unit.Faction.Id == factionManager.ActivePlayerNumber&&unit.Faction.IsPlayerControlled)
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
                unit.GameTransformManager.UnitController.touchInputReceiver = unitInputSystem;
            }
        }

        public override GameState<NextStateTrigger> Update()
        {
            unitInputSystem.Update();
            gridInputSystem.Update();
            
            if (conditionManager.CheckLose(gridGameManager.FactionManager.Factions))
            {
                return  GridGameManager.Instance.GameStateManager.GameOverState;
            }
            else if (conditionManager.CheckWin(gridGameManager.FactionManager.Factions))
            {
                return  GridGameManager.Instance.GameStateManager.WinState;
            }
            return NextState;
        }

        public override void Exit()
        {
            Debug.Log("Exit GameplayState");
            
            cameraSystem.RemoveMixin<DragCameraMixin>();
            cameraSystem.RemoveMixin<ClampCameraMixin>();
            cameraSystem.RemoveMixin<ViewOnGridMixin>();
            gridInputSystem.ResetInput();
            gridInputSystem.SetActive(false);
            unitInputSystem.SetActive(false);
            playerPhaseUI.Hide();
            // remove as Input Receiver to all Units

        }

       
    }
}