using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject engScreen;
    public GameObject bridgeScreen;
    public GameObject secScreen;
    public GameObject startScreen;

    public GameObject flicker;
    float flickerTime = .15f;

    public void ChangeScreen(Section section)
    {
        StartCoroutine(FlickerScreen());
        GamCon.Instance.sndCon.PlayScreenFlip();
        switch (section)
        {
            case Section.Eng:
                {
                    bridgeScreen.SetActive(false);
                    engScreen.SetActive(true);
                    foreach (GameObject engObj in GameObject.FindGameObjectsWithTag("Energy"))
                    {
                        engObj.transform.position = engObj.transform.position - new Vector3(15, 0);
                    }
                    GamCon.Instance.currentSec = Section.Eng;
                }
                break;
            
            case Section.Bridge:
                bridgeScreen.SetActive(true);
                if (GamCon.Instance.currentSec == Section.Eng)
                {
                    engScreen.SetActive(false);
                    foreach (GameObject engObj in GameObject.FindGameObjectsWithTag("Energy"))
                    {
                        engObj.transform.position = engObj.transform.position + new Vector3(15, 0);
                    }
                }
                if(GamCon.Instance.currentSec == Section.Start)
                {
                    startScreen.SetActive(false);
                }
                if (GamCon.Instance.currentSec == Section.Security)
                    secScreen.SetActive(false);
                GamCon.Instance.currentSec = Section.Bridge;
                break;
            
            case Section.Security:
                {
                    bridgeScreen.SetActive(false);
                    secScreen.SetActive(true);
                    GamCon.Instance.currentSec = Section.Security;
                }
                break;
        }
        
    }

    IEnumerator FlickerScreen()
    {
        bool flickering = true;
        while (flickering)
        {
            flicker.SetActive(true);
            yield return new WaitForSeconds(flickerTime);
            flicker.SetActive(false);
            flickering = false;
        }
    }
}
