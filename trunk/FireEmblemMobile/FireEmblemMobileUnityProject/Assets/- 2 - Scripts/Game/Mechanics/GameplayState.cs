using Game.GameInput;
using Game.GUI;
using Game.Manager;
using Game.Map;
using GameCamera;
using GameEngine;
using GameEngine.GameStates;
using GameEngine.Input;
using GameEngine.Tools;

namespace Game.Mechanics
{
    public class GameplayState : GameState<NextStateTrigger>
    {
        private readonly GridGameManager gridGameManager;

        public GameplayState()
        {
            gridGameManager = GridGameManager.Instance;
        }

        public override void Enter()
        {
            var cameraSystem  = gridGameManager.GetSystem<CameraSystem>();
            cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
                new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(TagManager.UnitTag),new MouseInputProvider());
            int height = gridGameManager.GetSystem<MapSystem>().GridData.Height;
            int width = gridGameManager.GetSystem<MapSystem>().GridData.Width;
           // int uiHeight = gridGameManager.GetSystem<UiSystem>().GetUiHeight();
          //  int referenceHeight = gridGameManager.GetSystem<UiSystem>().GetReferenceHeight();
            cameraSystem.AddMixin<ClampCameraMixin>().Construct(width, height);
                //.UiHeight(uiHeight).ReferenceHeight(referenceHeight);
            cameraSystem.AddMixin<ViewOnGridMixin>().zoom = 0;
        }

        public override GameState<NextStateTrigger> Update()
        {
            CheckGameOver();
            return NextState;
        }

        public override void Exit()
        {
            gridGameManager.GetSystem<CameraSystem>().RemoveMixin<DragCameraMixin>();
            //gridGameManager.GetSystem<CameraSystem>().RemoveMixin<SnapCameraMixin>();
            //gridGameManager.GetSystem<CameraSystem>().RemoveMixin<ClampCameraMixin>();
            gridGameManager.GetSystem<CameraSystem>().RemoveMixin<ViewOnGridMixin>();
        }

        public void CheckGameOver()
        {

            foreach (var p in gridGameManager.FactionManager.Factions)
            {
                if (p.IsPlayerControlled && !p.IsAlive())
                {
                    gridGameManager.GetSystem<UiSystem>().ShowGameOver();
                    InputSystem.OnSetActive?.Invoke(false, this);
                    gridGameManager.GameStateManager.Feed(NextStateTrigger.GameOver);

                    return;
                }
                else if (!p.IsPlayerControlled && !p.IsAlive())
                {
                    gridGameManager.GetSystem<UiSystem>().ShowWinScreen();
                    InputSystem.OnSetActive?.Invoke(false, this);
                    gridGameManager.GameStateManager.Feed(NextStateTrigger.PlayerWon);
                    return;
                }
            }
        }
    }
}