using Game.GUI.Renderer;
using UnityEngine;

namespace Game.GUI.Other
{
    public class GameOverScript : MonoBehaviour, IGameOverRenderer
    {

        private Canvas canvas;

        private void Start()
        {
            canvas = GetComponent<Canvas>();
        }

        public void Show()
        {
            canvas.enabled = true;
        }

      
    }
}