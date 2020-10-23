using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalSwipeScript : MinigameScript
{
    int Counter = 0, MaxCounter = 15;

    [SerializeField]
    bool RightSide = false;

    [SerializeField]
    Vector2 fingerPosition;

    float minDistanceForSwipe = 20f;

    private void Update()
    {
        Touch touch = Input.GetTouch(0);

        fingerPosition = touch.position;

        if (touch.phase == TouchPhase.Ended) Counter = 0;

        if ((fingerPosition.x > 960 && !RightSide) || (fingerPosition.x < 960 && RightSide))
        {
            RightSide = !RightSide;
            Counter++;
            BeerImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -1270 + Counter * 1100/MaxCounter, 0);

        }

        if (Counter >= MaxCounter) NetworkClientManager.instance.SendGetUp();

    }

    public override void Restart()
    {
        Counter = 0;
        BeerImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -1270, 0);
    }

    public override void Finish()
    {
        Counter = MaxCounter;
        BeerImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -170, 0);
    }

}
