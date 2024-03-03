using UnityEngine;

namespace Game.GUI.Controller
{
    public class AllySpriteController : MonoBehaviour
    {
        public Animator MaskBlinkAnimator;
        public Animator SpriteAnimator;

        public void StartBlinkAnimation()
        {
            MaskBlinkAnimator.SetTrigger("Play");
        }

        public void StartAttackAnimation()
        {
            SpriteAnimator.SetTrigger("Attack");
        }
    }
}