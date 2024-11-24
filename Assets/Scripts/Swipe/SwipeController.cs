using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour
{
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 swipeDelta;

    public Image image;
    public float swipeThreshold = 50f; // Минимальная длина свайпа, чтобы распознать его как действительный

    private bool isSwiping;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
            isSwiping = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (isSwiping)
            {
                endTouchPosition = Input.mousePosition;
                swipeDelta = endTouchPosition - startTouchPosition;

                if (swipeDelta.magnitude >= swipeThreshold)
                {
                    DetectSwipeDirection();
                }

                isSwiping = false;
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (isSwiping)
                {
                    endTouchPosition = touch.position;
                    swipeDelta = endTouchPosition - startTouchPosition;

                    if (swipeDelta.magnitude >= swipeThreshold)
                    {
                        DetectSwipeDirection();
                    }

                    isSwiping = false;
                }
            }
        }
    }

    private void DetectSwipeDirection()
    {
        float x = swipeDelta.x;
        float y = swipeDelta.y;

        // Проверяем, по какому направлению произошло смещение
        if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            // Горизонтальный свайп
            if (x > 0)
            {
                OnSwipeRight();
            }
            else
            {
                OnSwipeLeft();
            }
        }
        else
        {
            // Вертикальный свайп
            if (y > 0)
            {
                OnSwipeUp();
            }
            else
            {
                OnSwipeDown();
            }
        }

        // Диагональный свайп (если вам нужно его отдельно обрабатывать)
        if (Mathf.Abs(x) > swipeThreshold && Mathf.Abs(y) > swipeThreshold)
        {
            if (x > 0 && y > 0)
            {
                OnSwipeUpRight();
            }
            else if (x < 0 && y > 0)
            {
                OnSwipeUpLeft();
            }
            else if (x > 0 && y < 0)
            {
                OnSwipeDownRight();
            }
            else if (x < 0 && y < 0)
            {
                OnSwipeDownLeft();
            }
        }
    }

    // Эти методы можно заменить вашими действиями
    private void OnSwipeUp() { image.color = new Color32(255,255,255,255); Debug.Log("Swipe Up"); }
    private void OnSwipeDown() { image.color = new Color32(255, 255, 150, 255); Debug.Log("Swipe Down"); }
    private void OnSwipeLeft() { image.color = new Color32(255, 150, 255, 255); Debug.Log("Swipe Left"); }
    private void OnSwipeRight() { image.color = new Color32(150, 255, 255, 255); Debug.Log("Swipe Right"); }

    private void OnSwipeUpRight() { image.color = new Color32(100, 255, 255, 255); Debug.Log("Swipe Up Right"); }
    private void OnSwipeUpLeft() { image.color = new Color32(255, 100, 255, 255); Debug.Log("Swipe Up Left"); }
    private void OnSwipeDownRight() { image.color = new Color32(255, 255, 100, 255); Debug.Log("Swipe Down Right"); }
    private void OnSwipeDownLeft() { image.color = new Color32(110, 255, 150, 255); Debug.Log("Swipe Down Left"); }
}
