using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField, Range(0f, 90f)] float sensitivity = 40f;
    [SerializeField] Vector3 axis = Vector3.up;
    [SerializeField] bool swipeX = true, swipeY = false;

    private Vector2 lastPos, direction;

    private void Awake()
    {
        if (swipeX && swipeY)
        {
            Debug.LogWarning("Both X and Y swiping enabled. This can cause unpredicatable behavious!");
        }
    }

    void Update()
    {
        // Handle native touch events
        foreach (Touch touch in Input.touches)
        {
            HandleTouch(touch.fingerId, touch.position, touch.phase);
        }

        // Simulate touch events from mouse events
        if (Input.touchCount == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleTouch(10, Input.mousePosition, TouchPhase.Began);
            }
            if (Input.GetMouseButton(0))
            {
                HandleTouch(10, Input.mousePosition, TouchPhase.Moved);
            }
            if (Input.GetMouseButtonUp(0))
            {
                HandleTouch(10, Input.mousePosition, TouchPhase.Ended);
            }
        }
    }

    private void HandleTouch(int touchFingerId, Vector2 touchPosition, TouchPhase touchPhase)
    {
        /*Debug.Log("Handling Touch");
*/
        switch (touchPhase)
        {
            case TouchPhase.Began:
                lastPos = touchPosition;
                direction = Vector2.zero;
                break;

            case TouchPhase.Moved:
                direction = touchPosition - lastPos;
                if (direction.magnitude > 0.05f)
                {
                    float angle = Time.deltaTime * sensitivity * direction.magnitude;

                    if (swipeX)
                    {
                        angle *= direction.normalized.x;
                    }

                    if (swipeY)
                    {
                        angle *= direction.normalized.y;
                    }

                    transform.RotateAround(transform.position, axis, angle);
                }

                lastPos = touchPosition;
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                break;
        }
    }
}
