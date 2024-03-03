using UnityEngine;
using UnityEngine.Playables;

namespace Game.Graphics.BattleAnimations
{
    public class DebugTimelineController : MonoBehaviour
    {
        private bool paused = false;
        public PlayableDirector timeline;
    
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                timeline.Stop();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                timeline.Play();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                paused = !paused;
                if (paused)
                {
                    timeline.Pause();
                }
                else
                {
                    timeline.Resume();
                }
            }
        }
    }
}
