using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGlow : MonoBehaviour
{
    public SpriteRenderer spriteRend;
    public float Speed = 2f;
    public float maxRed = 10f;

    bool isTintingRed = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRend.color.b > 1/maxRed)
        { 
            spriteRend.color -= new Color(0, Time.deltaTime * (Speed / 10), Time.deltaTime * (Speed / 10), 0);

        }
        else
            isTintingRed = false;

        if (spriteRend.color.b < 1f && !isTintingRed)
        {
            spriteRend.color += new Color(0, Time.deltaTime * (Speed / 10), Time.deltaTime * (Speed / 10), 0);
            if (100 / (Time.deltaTime * (Speed / 10)) + spriteRend.color.b > 1f)
                spriteRend.color = Color.white;
            //Debug.Log(100 / ((255) - (Time.deltaTime * Speed / 10)) + " " + spriteRend.color.b);
        }

        if (spriteRend.color == Color.white)
            isTintingRed = true;

    }
}
