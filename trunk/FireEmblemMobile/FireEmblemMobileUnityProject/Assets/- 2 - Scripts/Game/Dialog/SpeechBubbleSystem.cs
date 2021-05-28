using System.Collections.Generic;
using UnityEngine;

namespace Unused.Dialogs
{
    public class SpeechBubbleSystem : MonoBehaviour
    {
        private const float SPEECH_BUBBLE_LIFE_TIME = 2.0f;
        private Queue<SpeechBubbleData> queue;
        private float timer = 0;

        private void Start()
        {
            queue = new Queue<SpeechBubbleData>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //ShowSpeechBubble(GridGameManager.Instance.FactionManager.ActiveFaction.Units[Random.Range(0, 4)],
                //    "This is a test!");
            }

            if (timer == 0)
            {
                if (queue.Count > 0)
                {
                    var data = queue.Dequeue();
                    data.SpeakAbleObject.ShowSpeechBubble(data.Text);
                    timer = SPEECH_BUBBLE_LIFE_TIME;
                }
            }

            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer < 0)
                    timer = 0;
            }
        }

        public void ShowSpeechBubble(ISpeakableObject speakableObject, string text)
        {
            var speechBubbleData = new SpeechBubbleData {Text = text, SpeakAbleObject = speakableObject};
            queue.Enqueue(speechBubbleData);
        }
    }
}