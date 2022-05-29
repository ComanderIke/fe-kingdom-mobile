using Game.AI;
using Game.GameActors.Units;
using Game.GameInput;
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
    public enum PPStateTrigger
    {
        ChooseTarget,
        Cancel
    }

    public class PlayerPhaseState : GameState<NextStateTrigger>, IDependecyInjection
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
            chooseTargetState = new ChooseTargetState(gridGameManager,gridInputSystem, unitInputSystem, this);
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

        public override void Enter()
        {

            stateMachine = new StateMachine<PPStateTrigger>(mainState);
           stateMachine.Init();

           cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
                new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(),new MouseCameraInputProvider());
            int height = gridGameManager.BattleMap.height;
            int width = gridGameManager.BattleMap.width;
           // cameraSystem.AddMixin<ClampCameraMixin>().Construct(width, height);
            cameraSystem.AddMixin<ViewOnGridMixin>().Construct(width, height);
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
                return  GridGameManager.Instance.GameStateManager.WinState;
            }
            return NextState;
        }

       
        public override void Exit()
        {
            if (stateMachine.GetCurrentState() != mainState)
                stateMachine.SwitchState(mainState);
            stateMachine.Exit();
            
            cameraSystem.RemoveMixin<DragCameraMixin>();
            cameraSystem.RemoveMixin<ClampCameraMixin>();
            cameraSystem.RemoveMixin<ViewOnGridMixin>();
        }
      
    
      
      
    }
}