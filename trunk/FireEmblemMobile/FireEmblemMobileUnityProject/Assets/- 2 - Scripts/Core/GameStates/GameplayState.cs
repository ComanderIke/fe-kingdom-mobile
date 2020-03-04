using Assets.GameCamera;
using Assets.GameInput;
using Assets.GUI;
using Assets.Map;
using UnityEngine;

namespace Assets.Core.GameStates
{
    public class GameplayState : GameState<NextStateTrigger>
    {
        private readonly MainScript mainScript;

        public GameplayState()
        {
            mainScript = MainScript.Instance;
        }

        public override void Enter()
        {
            mainScript.GetSystem<CameraSystem>().AddMixin<DragCameraMixin>();
            mainScript.GetSystem<CameraSystem>().AddMixin<SnapCameraMixin>();
            int height = mainScript.GetSystem<MapSystem>().GridData.Height;
            int width = mainScript.GetSystem<MapSystem>().GridData.Width;
            int uiHeight = mainScript.GetSystem<UiSystem>().GetUiHeight();
            int referenceHeight = mainScript.GetSystem<UiSystem>().GetReferenceHeight();
            mainScript.GetSystem<CameraSystem>().AddMixin<ClampCameraMixin>().GridHeight(height).GridWidth(width)
                .UiHeight(uiHeight).ReferenceHeight(referenceHeight).Locked(true);
            mainScript.GetSystem<CameraSystem>().AddMixin<ViewOnGridMixin>().Zoom = 0;
        }

        public override GameState<NextStateTrigger> Update()
        {
            CheckGameOver();
            return NextState;
        }

        public override void Exit()
        {
            mainScript.GetSystem<CameraSystem>().RemoveMixin<DragCameraMixin>();
            mainScript.GetSystem<CameraSystem>().RemoveMixin<SnapCameraMixin>();
            mainScript.GetSystem<CameraSystem>().RemoveMixin<ClampCameraMixin>();
            mainScript.GetSystem<CameraSystem>().RemoveMixin<ViewOnGridMixin>();
        }

        public void CheckGameOver()
        {

            foreach (var p in mainScript.PlayerManager.Players)
            {
                if (p.IsPlayerControlled && !p.IsAlive())
                {
                    mainScript.GetSystem<UiSystem>().ShowGameOver();
                    mainScript.GetSystem<InputSystem>().Active = false;
                    mainScript.GameStateManager.Feed(NextStateTrigger.GameOver);

                    return;
                }
                else if (!p.IsPlayerControlled && !p.IsAlive())
                {
                    mainScript.GetSystem<UiSystem>().ShowWinScreen();
                    mainScript.GetSystem<InputSystem>().Active = false;
                    mainScript.GameStateManager.Feed(NextStateTrigger.PlayerWon);
                    return;
                }
            }
        }
    }
}