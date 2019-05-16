using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwipeDetect : MonoBehaviour
{

    // DICLAIMER NOT MY WORK


    private Vector2 fingerDownPosition, fingerUpPosition;

    [SerializeField]
    private bool DetectSwipeOnlyAfterRelease = false;

    public float distanceForSwipeMIN = 20f;

    public static event Action<SwipeData> OnSwipe = delegate { };

    private void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerDownPosition = fingerUpPosition = touch.position;
            }

            if (!DetectSwipeOnlyAfterRelease && touch.phase == TouchPhase.Moved)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }

            if (touch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = touch.position;
                DetectSwipe();
            }
        }
    }

    private void DetectSwipe()
    {
        if (Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y) >= distanceForSwipeMIN || Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x) >= distanceForSwipeMIN)
        {
            if (Mathf.Abs(fingerDownPosition.y - fingerUpPosition.y) > Mathf.Abs(fingerDownPosition.x - fingerUpPosition.x))
            {
                var direction = fingerDownPosition.y - fingerUpPosition.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
                SendSwipe(direction);
            }
            else
            {
                var direction = fingerDownPosition.x - fingerUpPosition.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
                SendSwipe(direction);
            }
        }
        fingerUpPosition = fingerDownPosition;
    }

    private void SendSwipe(SwipeDirection direction)
    {
        SwipeData swipeData = new SwipeData()
        {
            Direction = direction,
            StartPosition = fingerDownPosition,
            EndPosition = fingerUpPosition
        };
    }
}
public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}
public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public SwipeDirection Direction;
}

