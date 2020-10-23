using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccelerometerController : MonoBehaviour
{
    float RefreshRate = 0.25f;

    double baseX = 0f, baseY = 0f;

    public void Setup()
    {
        baseX = System.Math.Round(Input.acceleration.x, 2);
        baseY = System.Math.Round(Input.acceleration.y, 2);

        CancelInvoke();
        SendInfo();
    }

    void SendInfo()
    {
        string toSend = ""; // RLDU

        if (Input.acceleration.x - baseX > 0.15f)
            toSend += "T|";
        else toSend += "F|";
        if (Input.acceleration.x - baseX < -0.15f)
            toSend += "T|";
        else toSend += "F|";
        if (Input.acceleration.y - baseY > 0.15f)
            toSend += "T|";
        else toSend += "F|";
        if (Input.acceleration.y - baseY < -0.15f)
            toSend += "T|";
        else toSend += "F|";

        // DebugText.instance.Add(" -" + toSend + "- ");
        NetworkClientManager.instance.SendGyroscopeInfo(toSend);

        Invoke("SendInfo", RefreshRate);

    }

}
