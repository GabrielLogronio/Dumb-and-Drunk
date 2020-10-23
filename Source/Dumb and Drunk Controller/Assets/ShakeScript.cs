using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeScript : MinigameScript
{
    int Counter = 0, MaxCounter = 10;

    bool Activated = false, RotX, RotY, RotZ, ToChange = false;

    float BaseY = 0f, timeSinceLastShake = 0f, MinShakeInterval = 0.2f;

    public override void Restart()
    {
        BaseY = (float)System.Math.Round(Input.acceleration.y, 2);
        Counter = 0;
        BeerImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -1270, 0);
        Activated = true;
    }

    public override void Finish()
    {
        Counter = MaxCounter;
        BeerImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -170, 0);
    }

    void Update()
    {
        if (Activated)
        {
            ToChange = false;
            if (Input.acceleration.x > 0.5 && RotX)
            {
                ToChange = true;
                RotX = false;

            }
            if (Input.acceleration.x < -0.5 && !RotX)
            {
                ToChange = true;
                RotX = true;

            }
            if (Input.acceleration.y > BaseY + 0.5f && RotY)
            {
                ToChange = true;
                RotY = false;

            }
            if (Input.acceleration.y < -BaseY - 0.5f && !RotY)
            {
                ToChange = true;
                RotY = true;

            }
            if (Input.acceleration.z > 0.5f && RotZ)
            {
                ToChange = true;
                RotZ = false;

            }
            if (Input.acceleration.z < - 0.5f && RotZ)
            {
                ToChange = true;
                RotZ = true;

            }

            if (ToChange && Time.unscaledTime >= timeSinceLastShake + MinShakeInterval)
            {
                Counter++;
                BeerImage.GetComponent<RectTransform>().localPosition = new Vector3(0, -1270 + Counter * 110, 0);
                timeSinceLastShake = Time.unscaledTime;
            } 

            if (Counter >= MaxCounter)
            {
                NetworkClientManager.instance.SendGetUp();
                Activated = false;
            }

        }

    }
}
