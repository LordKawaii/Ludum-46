using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnergyCon : MonoBehaviour
{
    public float useTime = 5f;

    bool isInUse = false;
    Section currSection = Section.Eng;
    float timeLeft;
    EngBarCon currentBar;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = useTime;
    }

    // Update is called once per frame
    void Update()
    {
            
    }


    public void ChangeSection(Section newSec, int poolCount)
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("EnergyBox"))
        {
            EngBarCon objCon = obj.GetComponent<EngBarCon>();
            if (objCon.section == newSec)
            {
                currentBar = objCon;
            }
        }

        currSection = newSec;
        Debug.Log(currSection.ToString());
        isInUse = true;
        StartCoroutine(GamCon.Instance.deathCountdown(useTime * poolCount, gameObject));
    }

    public void RemoveFromPool()
    {
        currentBar.RemoveEnergy(gameObject);
     
    }

    

}
