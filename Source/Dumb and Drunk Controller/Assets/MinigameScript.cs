using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MinigameScript : MonoBehaviour
{
    [SerializeField]
    protected GameObject BeerImage;

    public abstract void Restart();

    public abstract void Finish();

}
