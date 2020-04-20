using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCon : MonoBehaviour
{
    public float minTime = 1f;
    public float maxTime = 5f;
    public float repairTime = 2f;

    bool isPorbEng = false;
    bool isPorbLs = false;
    bool isPorbBridge = false;

    bool isFixEng = false;
    bool isFixLs = false;
    bool isFixBridge = false;

    Coroutine damgEng;
    Coroutine damgLS;
    Coroutine damgBridge;
    private void Start()
    {
        StartCoroutine(GenProb(maxTime, minTime));
    }

    public void FixProblem(int probSys, GameObject crew, Vector3 origin)
    {
        switch(probSys)
        {
            case 0:
                isPorbEng = false;
                isFixEng = true;
                if (damgEng != null)
                    StopCoroutine(damgEng);
                break;
            case 1:
                isPorbLs = false;
                isFixLs = true;
                if (damgLS != null)
                    StopCoroutine(damgLS);
                break;
            case 2:
                isPorbBridge = false;
                isFixBridge = true;
                if (damgBridge != null)
                    StopCoroutine(damgBridge);
                break;
        }
        StartCoroutine(FixCountDown(repairTime, probSys, crew, origin));
    }

    public bool hasProbs()
    {
        if (isPorbEng || isPorbLs || isPorbBridge)
            return true;
        return false;
    }

    public bool[] GetProbs()
    {
        bool[] prob = new bool[3];
        prob[0] = isPorbEng;
        prob[1] = isPorbLs;
        prob[2] = isPorbBridge;
        return prob;
    }

    public bool[] GetIsFixing()
    {
        bool[] fixing = new bool[3];
        fixing[0] = isFixEng;
        fixing[1] = isFixLs;
        fixing[2] = isFixBridge;
        return fixing;
    }

    IEnumerator GenProb(float maxTime, float minTime)
    {
        while (!GamCon.Instance.gameOver)
        {
            float newRadTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(newRadTime);

            int randSys = Random.Range(0, 3);
            switch (randSys)
            {
                case 0:
                    if (!isPorbEng && !isFixEng)
                    {
                        isPorbEng = true;
                        damgEng = StartCoroutine(GamCon.Instance.CheckForDamage(maxTime, minTime, this, randSys));
                    }
                    break;
                case 1:
                    if (!isPorbLs && !isFixLs)
                    {
                        isPorbLs = true;
                        damgLS = StartCoroutine(GamCon.Instance.CheckForDamage(maxTime, minTime, this, randSys));
                    }
                    break;
                case 2:
                    if (!isPorbBridge && !isFixBridge)
                    {
                        isPorbBridge = true;
                        damgBridge = StartCoroutine(GamCon.Instance.CheckForDamage(maxTime, minTime, this, randSys));
                    }
                    break;
            }

            
        }

    }

    IEnumerator FixCountDown(float time, int sys, GameObject crew, Vector3 origin)
    {
        while (GetIsFixing()[sys])
        {
            yield return new WaitForSeconds(time);

            switch (sys)
            {
                case 0:
                    isFixEng = false;
                    break;
                case 1:
                    isFixLs = false;
                    break;
                case 2:
                    isFixBridge = false;
                    break;
            }
            crew.transform.position = origin;
            crew.GetComponent<DragNDrop>().canDrag = true;
        }
    }
}
