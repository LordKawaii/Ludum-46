using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public GameObject engScreen;
    public GameObject bridgeScreen;

    bool isEng = true;

    public void ChangeScreen(Section section)
    {
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
                GamCon.Instance.currentSec = Section.Bridge;
                break;
        }
        
    }
}
