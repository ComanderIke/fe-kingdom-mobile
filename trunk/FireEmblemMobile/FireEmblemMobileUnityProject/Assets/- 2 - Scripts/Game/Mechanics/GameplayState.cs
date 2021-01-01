
using Assets.Game.Manager;
using Assets.GameCamera;
using Assets.GameEngine;
using Assets.GameEngine.GameStates;
using Assets.GameInput;
using Assets.GUI;
using Assets.Map;
using Assets.Mechanics;
using UnityEngine;

namespace Assets.Game.GameStates
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
            gridGameManager.GetSystem<CameraSystem>().AddMixin<DragCameraMixin>().AddColliderTags(TagManager.UnitTag);
            
            //gridGameManager.GetSystem<CameraSystem>().AddMixin<SnapCameraMixin>();
            int height = gridGameManager.GetSystem<MapSystem>().GridData.Height;
            int width = gridGameManager.GetSystem<MapSystem>().GridData.Width;
            int uiHeight = gridGameManager.GetSystem<UiSystem>().GetUiHeight();
            int referenceHeight = gridGameManager.GetSystem<UiSystem>().GetReferenceHeight();
            //gridGameManager.GetSystem<CameraSystem>().AddMixin<ClampCameraMixin>().GridHeight(height).GridWidth(width)
            //    .UiHeight(uiHeight).ReferenceHeight(referenceHeight).Locked(false);
            gridGameManager.GetSystem<CameraSystem>().AddMixin<ViewOnGridMixin>().Zoom = 0;
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