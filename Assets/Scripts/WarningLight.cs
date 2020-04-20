using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningLight : MonoBehaviour
{
    public float blinkSpeed = .2f;
    public Sprite litUp;
    public Sprite unlit;

    [HideInInspector]
    public bool isFlashing = false;


    public void SetFlashing(bool flashing)
    {
        isFlashing = flashing;
    }

    public void TurnOn()
    {

        gameObject.GetComponent<SpriteRenderer>().sprite = litUp;
    }

    public void TurnOff()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = unlit;
    }

}
