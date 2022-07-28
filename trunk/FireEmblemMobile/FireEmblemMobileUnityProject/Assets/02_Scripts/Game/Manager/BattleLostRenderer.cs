using UnityEngine;

namespace Game.Manager
{
    public class BattleLostRenderer : MonoBehaviour, IBattleLostRenderer
    {
        private Canvas canvas;

        void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        public void Show()
        {
            canvas.enabled = true;
        }
    }
}