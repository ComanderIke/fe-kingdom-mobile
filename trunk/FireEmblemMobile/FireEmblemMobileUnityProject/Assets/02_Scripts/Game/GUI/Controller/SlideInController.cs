using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideInController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        SwipeDetector.OnSwipe += OnSwipe;
    }

    void OnSwipe(SwipeData swipeData)
    {
       // Debug.Log("TODO SWIPE DETECTED " +swipeData.Direction);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
