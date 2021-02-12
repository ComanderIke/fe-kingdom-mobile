using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDownPos;

    private Vector2 fingerUpPos;

    public bool detectAfterRelease;

    [SerializeField]
    private float minDistance = 20;
    // Start is called before the first frame update
    public static event Action<SwipeData> OnSwipe = delegate{ };


    void Update()
    {
        foreach(var touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPos = touch.position;
                fingerDownPos = touch.position;
            }

            if (!detectAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPos = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPos = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if (SwipeDistanceCheck())
        {
            if (IsVerticalSwipe())
            {
                var direction = fingerDownPos.y - fingerUpPos.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = fingerDownPos.x - fingerUpPos.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }

            fingerUpPos = fingerDownPos;
        }
    }

    private void SendSwipe(SwipeDirection direction)
    {
        var swipeData = new SwipeData()
        {
            Direction =  direction,
            StartPosition = fingerDownPos,
            EndPosition = fingerUpPos
        };
        OnSwipe(swipeData);
    }

    private bool IsVerticalSwipe()
    {
        return VerticalMovementDistance() > HorizontalMovementDistance();
    }

    private float VerticalMovementDistance()
    {
        return Math.Abs(fingerDownPos.y - fingerUpPos.y);
    }

    private float HorizontalMovementDistance()
    {
        return Math.Abs(fingerDownPos.x - fingerUpPos.x);
    }

    private bool SwipeDistanceCheck()
    {
        return VerticalMovementDistance() > minDistance || HorizontalMovementDistance() > minDistance;
    }
}

public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
    
}
public enum SwipeDirection
{
    Up,Down, Left, Right
}
