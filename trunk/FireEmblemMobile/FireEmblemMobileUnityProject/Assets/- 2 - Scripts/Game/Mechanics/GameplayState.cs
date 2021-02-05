using Game.GameInput;
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
    public class GameplayState : GameState<NextStateTrigger>
    {
        private readonly GridGameManager gridGameManager;

        public GameplayState()
        {
            gridGameManager = GridGameManager.Instance;
        }

        public override void Enter()
        {
            Debug.Log("Enter GameplayState");
            var cameraSystem  = gridGameManager.GetSystem<CameraSystem>();
            cameraSystem.AddMixin<DragCameraMixin>().Construct(new WorldPosDragPerformer(1f, cameraSystem.camera),
                new ScreenPointToRayProvider(cameraSystem.camera), new HitChecker(),new MouseInputProvider());
            int height = gridGameManager.GetSystem<GridSystem>().GridData.height;
            int width = gridGameManager.GetSystem<GridSystem>().GridData.width;
            cameraSystem.AddMixin<ClampCameraMixin>().Construct(width, height);
            cameraSystem.AddMixin<ViewOnGridMixin>().zoom = 0;
        }

        public override GameState<NextStateTrigger> Update()
        {
            CheckGameOver();
            return NextState;
        }

        public override void Exit()
        {
            Debug.Log("Exit GameplayState");
           
            var cameraSystem  = gridGameManager.GetSystem<CameraSystem>();
            cameraSystem.RemoveMixin<DragCameraMixin>();
            cameraSystem.RemoveMixin<ClampCameraMixin>();
            cameraSystem.RemoveMixin<ViewOnGridMixin>();
            var gridInput = gridGameManager.GetSystem<GridInputSystem>();
            gridInput.ResetInput();
        }

        public void CheckGameOver()
        {

            foreach (var p in gridGameManager.FactionManager.Factions)
            {
                if (p.IsPlayerControlled && !p.IsAlive())
                {
                    Debug.Log("Game Over!");
                    //gridGameManager.GetSystem<UiSystem>()?.ShowGameOver();
                    GridInputSystem.SetActive(false);
                    gridGameManager.GameStateManager.Feed(NextStateTrigger.GameOver);

                    return;
                }
                else if (!p.IsPlayerControlled && !p.IsAlive())
                {
                    Debug.Log("Win!");
                    //gridGameManager.GetSystem<UiSystem>()?.ShowWinScreen();
                    GridInputSystem.SetActive(false);
                    gridGameManager.GameStateManager.Feed(NextStateTrigger.PlayerWon);
                    return;
                }
            }
        }
    }
}