using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Section
{
    Eng,
    Life,
    Shields,
    Bridge
}

public class EngPool
{
    GameObject[] engObjPool;
    int currentPlace = 0;
    int maxEng = 0;

    public EngPool(int maxNumEng)
    {
        engObjPool = new GameObject[maxNumEng];
        maxEng = maxNumEng;
    }

    public void Add(GameObject engObj)
    {
        engObjPool[currentPlace] = engObj;
        currentPlace++;
    }

    public void RemoveObj(GameObject engObj)
    {
        for (int i = 0; i < maxEng; i++)
        {
            if (engObj == engObjPool[i])
            {
                engObjPool[i] = null;
                currentPlace--;
                for (int j = i; j < maxEng; j++)
                {
                    if (j+1 >= maxEng)
                    {
                        engObjPool[j] = null;
                        break;
                    }
                    else if(engObjPool[j + 1] == null)
                    {
                        engObjPool[j] = null;
                        break;
                    }
                    //(engObjPool[j + 1] != null)
                    else
                    { 
                        engObjPool[j] = engObjPool[j + 1];
                        engObjPool[j].transform.position = engObjPool[j+1].transform.position + new Vector3(.5f, 0f);
                    }

                }
            }
        }
    }

    public GameObject findObj(GameObject engObj)
    {
        for (int i = 0; i < maxEng; i++)
        {
            if (engObj == engObjPool[i])
            {
                return engObjPool[i];
            }
        }

        return null;
    }

    public GameObject findObj(int ojbLoc)
    {
        return engObjPool[ojbLoc];
    }


    public int Count()
    {
        /*int numObj = 0;
        for (int i = 0; i < maxEng; i++)
        {
            if (engObjPool[i] != null)
            {
                numObj++;
            }
            else
                break;
        }
        return numObj;
        */
        return currentPlace;
    }
}

public class GamCon : MonoBehaviour
{
    public float energyInterval = 1;
    public int startingEnergy = 1;
    public int energyGenSpeed = 1;
    public int maxEnergy = 10;
    public Transform energyBar;
    public List<GameObject> hullPoints;

    public GameObject energy;
    EngPool engObjPool;

    int currentHull = 8;
    int enegPool;
    float engSpriteLng;
    SpriteRenderer engBarSprite;
    bool gameOver = false;

    EngBarCon lsPowerCon;
    public float lifeCheckMin;
    public float lifeCheckMax;
    EngBarCon shieldPowerCon;
    public float shieldCheckMin;
    public float shieldCheckMax;

    [HideInInspector]
    public Section currentSec = Section.Eng;
    [HideInInspector]
    public ScreenManager scrMan;

    public static GamCon Instance;

    private void Awake()
    {
        ///Set this to be main Game Controller
        if (Instance)
        {
            Debug.Log("I already exist: destroying self");
            Destroy(gameObject);
        }
        else
        {

            Instance = this;
            DontDestroyOnLoad(gameObject);

            engObjPool = new EngPool(maxEnergy);
            scrMan = gameObject.GetComponent<ScreenManager>();

            foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("EnergyBox"))
            {
                EngBarCon objCon = gameObj.GetComponent<EngBarCon>();
                if (objCon.section == Section.Life)
                    lsPowerCon = objCon;
                if (objCon.section == Section.Shields)
                    shieldPowerCon = objCon;
            }
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        enegPool = startingEnergy;
        engBarSprite = energyBar.gameObject.GetComponent<SpriteRenderer>();
        engSpriteLng = energy.GetComponent<SpriteRenderer>().size.x;
        StartCoroutine(EnergyTimer(energyInterval, energyGenSpeed, maxEnergy, startingEnergy));
        StartCoroutine(CheckForDamage(lifeCheckMax, lifeCheckMin, lsPowerCon.GetEngPool()));
        StartCoroutine(CheckForDamage(shieldCheckMax, shieldCheckMin, shieldPowerCon.GetEngPool()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnergyTimer(float interval, int genSpeed, int maxEng, int startEng)
    {
        int currentEng = startEng;
        Vector3 lastObjPos = Vector3.zero;

        while (true)
        {
            if (engObjPool.Count() < maxEng)
            {
                enegPool = GenerateEnergy(enegPool, genSpeed);
                GameObject newEnergy = GenEngeryObj(energy);
                engObjPool.Add(newEnergy);
                if (engObjPool.Count() == 1)
                {
                    newEnergy.transform.position = energyBar.transform.position + new Vector3(engBarSprite.bounds.size.x/2 - 1.3f, 0);
                    //lastObjPos = newEnergy.transform.position;
                }
                else
                {
                    newEnergy.transform.position = energyBar.transform.position + new Vector3(engBarSprite.bounds.size.x / 2 - 1.3f, 0) - (new Vector3(.5f, 0f) * (engObjPool.Count() - 1));
                    //lastObjPos = lastObjPos - new Vector3(.5f, 0f);
                }
                //newEnergy.transform.SetParent(scrMan.engScreen.transform);
                currentEng++;
            }

            yield return new WaitForSeconds(interval);
        }
    }

    int GenerateEnergy(int currEng, int genSpeed)
    {
        int newEng = currEng + genSpeed;
        return newEng;
    }

    GameObject GenEngeryObj(GameObject energyObj)
    {
        GameObject newEnergy = Instantiate(energyObj);
        return newEnergy;
    }

    public void RemoveEng(GameObject engObj)
    {
        engObjPool.RemoveObj(engObj);
        
    }

    public IEnumerator deathCountdown(float time, GameObject obj)
    {
        float timeLeft = time;
        while (true)
        {
            timeLeft = timeLeft - Time.deltaTime;

            if (timeLeft <= 0)
            {
                obj.GetComponent<EnergyCon>().RemoveFromPool();
                Destroy(obj);
            }
            yield return new WaitForSeconds(.03f);
        }
    }

    void TakeDamage()
    {
        GameObject.Destroy(hullPoints[currentHull - 1]);
        currentHull--;

        if (hullPoints.Count == 0)
            gameOver = true;
    }

    IEnumerator CheckForDamage(float maxTime, float minTime, EngPool pool)
    {
        while (true)
        {
            float newRadTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(newRadTime);

            if (pool.Count() == 0)
                TakeDamage();
        }

    }
}
