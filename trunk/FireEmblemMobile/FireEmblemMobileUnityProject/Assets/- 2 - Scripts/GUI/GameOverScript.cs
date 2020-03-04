using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GUI
{
    public class GameOverScript : MonoBehaviour
    {
        private const float DELAY = 2.0f;
        private bool active;
        private float time = 0;

        private void OnEnable()
        {
            active = false;
            time = 0;
        }

        private void Update()
        {
            if (active)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SceneManager.LoadSceneAsync("MainMenu");
                    active = false;
                }
            }
            else
            {
                time += Time.deltaTime;
                if (time >= DELAY)
                {
                    active = true;
                }
            }
        }
    }
}