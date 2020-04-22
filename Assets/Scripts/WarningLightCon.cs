using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WarningLightCon : MonoBehaviour
{
    public EngBarCon shildBar;
    public EngBarCon lsBar;
    public WarningLight engLight;
    public WarningLight secLight;

    public WarningLight scuEngLight;
    public WarningLight scuLsLight;
    public WarningLight scuBridgeLight;

    public float blinkSpeed = .2f;

    // Update is called once per frame
    void Update()
    {
        if (GamCon.Instance.gameRunning) 
        { 
            if (CheckEngProb())
            {
                if (engLight.gameObject.activeSelf)
                {
                    if (!engLight.isFlashing)
                    {
                        GamCon.Instance.sndCon.PlayEngAlarm();
                        engLight.SetFlashing(true);
                        StartCoroutine(FlashLight(engLight, blinkSpeed));
                    }
                }
            
            }
            else
            {
                engLight.SetFlashing(false);
                engLight.TurnOff();
            }

            CheckSecurityProb();
        }
    }

    bool CheckEngProb()
    {
        if (shildBar.getCount() == 0 || lsBar.getCount() == 0)
            return true;
        else
            return false;
    }

    void CheckSecurityProb()
    {
        if(GamCon.Instance.GetSecurityCon().hasProbs())
        {
            if (!secLight.isFlashing)
            {
                GamCon.Instance.sndCon.PlaySecAlarm();
                secLight.SetFlashing(true);
                StartCoroutine(FlashLight(secLight, blinkSpeed));
            }
           
        }
        else
        {
            secLight.SetFlashing(false);
            secLight.TurnOff();
        }
        
        
        if (GamCon.Instance.GetSecurityCon().GetProbs()[0])
        {
            if (!scuEngLight.isFlashing)
            {
                scuEngLight.SetFlashing(true);
                StartCoroutine(FlashLight(scuEngLight, blinkSpeed));
            }
        }
        else
        {
            scuEngLight.SetFlashing(false);
            scuEngLight.TurnOff();
        }

        if (GamCon.Instance.GetSecurityCon().GetProbs()[1])
        {
            if (!scuLsLight.isFlashing)
            {
                scuLsLight.SetFlashing(true);
                StartCoroutine(FlashLight(scuLsLight, blinkSpeed));
            }
        }
        else
        {
            scuLsLight.SetFlashing(false);
            scuLsLight.TurnOff();
        }

        if (GamCon.Instance.GetSecurityCon().GetProbs()[2])
        {
            if (!scuBridgeLight.isFlashing)
            {
                scuBridgeLight.SetFlashing(true);
                StartCoroutine(FlashLight(scuBridgeLight, blinkSpeed));
            }
        }
        else
        {
            scuBridgeLight.SetFlashing(false);
            scuBridgeLight.TurnOff();
        }

    }

    IEnumerator FlashLight(WarningLight light, float speed)
    {
        bool isLit = false;

        light.TurnOn();

        while (light.isFlashing)
        {
            if (isLit)
                light.TurnOn();
            else
                light.TurnOff();
            isLit = !isLit;

            yield return new WaitForSeconds(speed);
        }

        light.TurnOff();
    }
}
