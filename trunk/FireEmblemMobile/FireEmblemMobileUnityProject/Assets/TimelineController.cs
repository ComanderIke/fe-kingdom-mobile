using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    private bool paused = false;
    public PlayableDirector timeline;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            timeline.Stop();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            timeline.Play();
            // timeline.gameObject.transform.position = new Vector3(-80, timeline.gameObject.transform.position.y,
            //     timeline.gameObject.transform.position.z);
            // foreach (var para in FindObjectsOfType<ParalaxController>())
            // {
            //     para.Reset();
            // }
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
