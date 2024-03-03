using Game.GameActors.Units;
using Game.GameInput.CameraInput;
using Game.GameInput.GridInput;
using Game.GameInput.InputReceivers;
using Game.Grid;
using Game.Manager;
using Game.States.Mechanics;
using Game.Systems;
using Game.Utility;
using GameCamera;
using GameEngine;
using GameEngine.GameStates;
using UnityEngine;

namespace Game.States
{
    public enum PPStateTrigger
    {
        ChooseTarget,
        Cancel
    }

    public class PlayerPhaseState : GameState<NextStateTrigger>
    {
        private readonly GridGameManager gridGameManager;

        private FactionManager factionManager;
        private ConditionManager conditionManager;
        private GridInputSystem gridInputSystem;
        private UnitInputSystem unitInputSystem;
       // private ISelectionDataProvider selectionDataProvider;
        private CameraSystem cameraSystem;
       
        public MainPlayerPhaseState mainState;
        public ChooseTargetState chooseTargetState;
        protected StateMachine<PPStateTrigger> stateMachine;

        public PlayerPhaseState(ConditionManager conditionManager)
        {
            gridGameManager = GridGameManager.Instance;
            factionManager = gridGameManager.FactionManager;
            this.conditionManager = conditionManager;
            cameraSystem = GameObject.FindObjectOfType<CameraSystem>();
            gridInputSystem = new GridInputSystem();
            unitInputSystem = new UnitInputSystem();
            
            mainState = new MainPlayerPhaseState(gridGameManager,factionManager,gridInputSystem, unitInputSystem, this);
            chooseTargetState = new ChooseTargetState(gridInputSystem, unitInputSystem, this);

            mainState.AddTransition(chooseTargetState,PPStateTrigger.ChooseTarget);
            chooseTargetState.AddTransition(mainState,PPStateTrigger.Cancel);
            
            unitInputSystem.InputReceiver = gridInputSystem;
            unitInputSystem.EndedDrag += ActivateCameraDrag;
            unitInputSystem.StartedDrag += DeactivateCameraDrag;
            unitInputSystem.MouseUp += FindBetterName;
           
        }

        public void Feed(PPStateTrigger trigger)
        {
            stateMachine.Feed(trigger);
        }
        public void Init()
        {
            gridInputSystem.inputReceiver = new GameInputReceiver(gridGameManager.GetSystem<GridSystem>());
         
            gridInputSystem.Init();
            unitInputSystem.Init();
            cameraSystem.Init();
        }

        private int zoomLevel = 0;
        public void ToggleZoom()
        {
            cameraSystem.GetMixin<ViewOnGridMixin>().ToogleZoom();
            zoomLevel = cameraSystem.GetMixin<ViewOnGridMixin>().zoomLevel;
        }
        public override void Enter()
        {

            // Debug.Log("PlayerPhaseEnter");
            stateMachine = new StateMachine<PPStateTrigger>(mainState);
           stateMachine.Init();
           int height = gridGameManager.BattleMap.GetHeight();
           int width = gridGameManager.BattleMap.GetWidth();
           if(!cameraSystem.HasMixin<ViewOnGridMixin>())
               cameraSystem.AddMixin<ViewOnGridMixin>().Construct(width, height,zoomLevel);

           if(!cameraSystem.HasMixin<DragCameraMixin>())
           cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
                new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(),new MouseCameraInputProvider());
           if(!cameraSystem.HasMixin<FocusCameraMixin>())
           cameraSystem.AddMixin<FocusCameraMixin>().Construct(.5f,false, true);
          
           // cameraSystem.AddMixin<ClampCameraMixin>().Construct(width, height);
         
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

        public override GameState<NextStateTrigger> Update()
        {
            stateMachine.Update();
            if (conditionManager.CheckLose())
            {
                return  GridGameManager.Instance.GameStateManager.GameOverState;
            }
            else if (conditionManager.CheckWin())
            {
                MyDebug.LogLogic("Won Battle");
                return WinState.Create();
            }
            return NextState;
        }

       
        public override void Exit()
        {
            // Debug.Log("PlayerphaseExit");
            if (stateMachine.GetCurrentState() != mainState)
                stateMachine.SwitchState(mainState);
            stateMachine.Exit();

            if (cameraSystem != null && cameraSystem.gameObject != null)
            {
                // cameraSystem.RemoveMixin<DragCameraMixin>();
                // cameraSystem.RemoveMixin<ClampCameraMixin>();
                // cameraSystem.RemoveMixin<ViewOnGridMixin>();
                // cameraSystem.RemoveMixin<FocusCameraMixin>();
            }
        }
      
    
      
      
    }
}