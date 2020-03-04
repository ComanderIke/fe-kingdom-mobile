using UnityEngine;

namespace Assets.GUI
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