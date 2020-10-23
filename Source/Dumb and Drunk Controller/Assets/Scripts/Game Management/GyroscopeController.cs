using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GyroscopeController : MonoBehaviour
{
    [SerializeField]
    bool TestingGyro = false;

    bool gyroEnabled = false;

    double baseX = 0f, baseY = 0f, prevX = 0f, prevY = 0f;

    Gyroscope gyro;

    bool EnableGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Debug.Log("Gyro activated");
            gyro = Input.gyro;
            gyro.enabled = true;
            gyro.updateInterval = 0.0167f;
            return true;
        }
        Debug.Log("Impossible to activate");
        return false;
    }

    public void Setup()
    {
        gyroEnabled = EnableGyroscope();

        if (gyroEnabled)
        {
            baseX = System.Math.Round(gyro.attitude.x, 2);
            baseY = System.Math.Round(gyro.attitude.y, 2);
 
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (gyroEnabled)
        {
            // Delta.x <<0 tilted to the right, 0>> to the left, *(-1f) to mirror, <<0.25 tilted forwards, 0.25>> tilted backwards, -0.25 to center in 0

            Quaternion GyroInput = gyro.attitude * new Quaternion(0, 0, 1, 0);
            double x = -System.Math.Round(GyroInput.x, 2), y = System.Math.Round(GyroInput.y, 2) - baseY;

            if (x > 0.04f && x - prevX > 0.06f) NetworkClientManager.instance.SendGyroscopeInfo("R");
            if (x < -0.04f && x - prevX < -0.06f) NetworkClientManager.instance.SendGyroscopeInfo("L");
            if (y > 0.04f && y - prevY > 0.06f) NetworkClientManager.instance.SendGyroscopeInfo("U");
            if (y < -0.04f && y - prevY < -0.06f) NetworkClientManager.instance.SendGyroscopeInfo("D");

            //DebugText.instance.Log("Gyro input: " + x + ", " + y);

            prevX = x;
            prevY = y;

        }
    }
}
