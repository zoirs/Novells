using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MenuSystemWithZenject.ItemsUI {
public class SwipeDetector : MonoBehaviour
{
    private Vector2 fingerDown;
    private Vector2 fingerUp;

    private Action callback;
    public float SWIPE_THRESHOLD = 20f;

    private bool startSwipe;
    public Action Callback {
        get => callback;
        set => callback = value;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && IsPointerOverCurrentUIElement()) {
            startSwipe = true;
            fingerDown = Input.mousePosition;
        }

        if (startSwipe && Input.GetMouseButton(0)) {
            fingerUp = Input.mousePosition;
            if (startSwipe) {
                // startSwipe = false;
                bool swipe = checkSwipe();
                startSwipe = !swipe;
            }
        }
    }

    bool checkSwipe() {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
            return true;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
            return true;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
            return false;
        }

    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        Debug.Log("Swipe UP");
        if (callback != null) {
            callback.Invoke();
        }
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
        if (callback != null) {
            callback.Invoke();
        }
    }

    void OnSwipeLeft()
    {
        Debug.Log("Swipe Left");
    }

    void OnSwipeRight()
    {
        Debug.Log("Swipe Right");
    }
    
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public bool IsPointerOverCurrentUIElement()
    {
        return IsPointerOverCurrentUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    private bool IsPointerOverCurrentUIElement(List<RaycastResult> eventSystemRaysastResults )
    {
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];
            if (curRaysastResult.gameObject.GetComponent<SwipeDetector>() == this) {
                return true;
            }
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position =  Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, raysastResults );
        return raysastResults;
    }
}
}