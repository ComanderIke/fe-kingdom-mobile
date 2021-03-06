using Game.GUI.Text;
using Game.Manager;
using GameEngine;
using GameEngine.GameStates;
using Menu;
using UnityEngine;
using UnityEngine.Playables;

namespace Game.States
{
    public class AnimationState : GameState<NextStateTrigger>
    {
        private IAnimation animation;
        private bool finished;

        public AnimationState(IAnimation animation, GameState<NextStateTrigger> nextState)
        {
            this.animation = animation;
            NextState = nextState;
        }
        public AnimationState(IAnimation animation)
        {
            this.animation = animation;
            NextState = PreviousState;
        }

        public override void Enter()
        {
            Debug.Log("AnimationStart: ");
            finished = false;
            animation.Play(Finished);

        }

        private void Finished()
        {
            finished = true;
        }

        public override void Exit()
        {
            Debug.Log("Animationend");
        }

        public override GameState<NextStateTrigger> Update()
        {
            return finished ? NextState : null;
        }
    }
}