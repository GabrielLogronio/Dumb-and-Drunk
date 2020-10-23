using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    static SoundManager instance = null;

    bool RandomON = false;

    float Min = 5f, Max = 10f;

    private void Start()
    {
        if (!instance) instance = this;
        else if (instance != this) Destroy(this);

    }

    public static SoundManager GetInstance()
    {
        return instance;
    }

    public void PlaySound(int i)
    {
        StopSound();
        if(i == 4 || i == 5)
            transform.GetChild(i).GetChild(NetworkClientManager.instance.ID()).GetComponent<AudioSource>().Play();
        else
            transform.GetChild(i).GetComponent<AudioSource>().Play();
        /*
        0 - Connected
        1 - Start
        2 - Fallen
        3 - GotUp
        4 - Hicchup
        5 - Laugh
        6 - Won
        7 - Lost
        */
    }

    void StopSound()
    {
        for (int i = 0; i < 8; i++)
        {
            if (i == 4 || i == 5)
                transform.GetChild(i).GetChild(NetworkClientManager.instance.ID()).GetComponent<AudioSource>().Stop();
            else
                transform.GetChild(i).GetComponent<AudioSource>().Stop();
        }
    }

    public void SetRandom(bool ToSet)
    {
        RandomON = ToSet;
        if (ToSet) Invoke("PlayRandom", Random.Range(Min, Max));
        else CancelInvoke();
    }

    void PlayRandom()
    {
        transform.GetChild(Random.Range( 4, 6)).GetComponent<AudioSource>().Play();
        Invoke("PlayRandom", Random.Range(Min, Max));

    }

}
