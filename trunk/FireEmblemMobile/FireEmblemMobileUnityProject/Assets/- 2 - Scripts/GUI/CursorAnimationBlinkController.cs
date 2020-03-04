using Assets.Utility;
using UnityEngine;

namespace Assets.GUI
{
    public class CursorAnimationBlinkController : MonoBehaviour
    {
        private Animator animator;
        private readonly CursorAnimationBlinkController[] controllers;
        private AnimationTimer timer;

        private void Start()
        {
            animator = GetComponent<Animator>();
            animator.enabled = false;
            timer = FindObjectOfType<AnimationTimer>();
            animator.SetFloat("NormTime", timer.NormalizedTime);
            animator.enabled = true;
        }

        private void Update()
        {
            animator.SetFloat("NormTime", timer.NormalizedTime);
        }
    }
}