using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapScript : MinigameScript
{
    int Counter = 0, MaxCounter = 33;

    bool Activated = false;

    public override void Restart()
    {
        Counter = 0;
        Activated = true;
        BeerImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -1270, 0);
    }

    public override void Finish()
    {
        Counter = MaxCounter;
        BeerImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -170, 0);
    }

    public void Tap()
    {
        if (Activated)
        {
            Counter++;
            BeerImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -1270 + Counter * 1100/MaxCounter, 0);
            if (Counter >= MaxCounter)
            {
                NetworkClientManager.instance.SendGetUp();
                Activated = false;
            }
        }
    }

}
