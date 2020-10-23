using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugText : MonoBehaviour
{
    public static DebugText instance = null;

    Text text;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        text = GetComponent<Text>();
    }

    public void Log(string ToAdd)
    {
        text.text = ToAdd + " - ";
        if (text.text.Length > 200) text.text = "";
    }

    public void Add(string ToAdd)
    {
        text.text += ToAdd + " - ";
        if (text.text.Length > 200) text.text = "";
    }
}
