using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.GUI
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