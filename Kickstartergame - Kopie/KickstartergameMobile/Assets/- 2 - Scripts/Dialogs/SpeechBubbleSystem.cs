using Assets.Scripts.Dialogs;
using Assets.Scripts.Engine;
using Assets.Scripts.GameStates;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct SpeechBubbleData
{
    public SpeakableObject speakAbleObject;
    public string text;
}
public class SpeechBubbleSystem : MonoBehaviour, EngineSystem {
    const float SPEECH_BUBBLE_LIFE_TIME = 2.0f;
    Queue<SpeechBubbleData> queue;
    float timer = 0;

	// Use this for initialization
	void Start () {
        queue = new Queue<SpeechBubbleData>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShowSpeechBubble(MainScript.GetInstance().GetSystem<TurnSystem>().ActivePlayer.Units[(int)Random.Range(0,4)], "This is a test!");
        }
        if (timer == 0)
        {
            if (queue.Count > 0)
            {
                SpeechBubbleData data = queue.Dequeue();
                data.speakAbleObject.ShowSpeechBubble(data.text);
                timer = SPEECH_BUBBLE_LIFE_TIME;
            }
        }
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
                timer = 0;
        }

    }
    public void ShowSpeechBubble(SpeakableObject speakableObject, string text)
    {
        SpeechBubbleData speechBubbleData = new SpeechBubbleData();
        speechBubbleData.text = text;
        speechBubbleData.speakAbleObject = speakableObject;
        queue.Enqueue(speechBubbleData);
    }
}
