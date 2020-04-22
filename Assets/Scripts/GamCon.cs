using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Section
{
    Eng,
    Life,
    Start,
    Shields,
    Bridge,
    Security
}
/// <summary>
/// End of Section
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Start of EngPool
/// </summary>
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
/// <summary>
/// End of EngPool
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/// Start of GameCon
/// </summary>
public class GamCon : MonoBehaviour
{
    public float energyInterval = 1;
    public int startingEnergy = 1;
    public int energyGenSpeed = 1;
    public int maxEnergy = 10;
    public Transform energyBar;
    public List<GameObject> hullPoints;
    public GameObject gameOverScreen;
    public CamCon camCon;

    public GameObject energy;
    EngPool engObjPool;

    public EngBarCon lsPowerCon;
    public float lifeCheckMin;
    public float lifeCheckMax;
    public EngBarCon shieldPowerCon;
    public float shieldCheckMin;
    public float shieldCheckMax;

    [HideInInspector]
    public Section currentSec = Section.Start;
    [HideInInspector]
    public ScreenManager scrMan;
    [HideInInspector]
    public bool gameStart = false;
    [HideInInspector]
    public bool gameOver = false;
    [HideInInspector]
    public bool gameRunning = false;
    [HideInInspector]
    public SoundCon sndCon;

    int currentHull = 8;
    int enegPool;
    float engSpriteLng;
    SpriteRenderer engBarSprite;
    SecurityCon secCon;

    Coroutine dmgLsPool;
    Coroutine dmgShieldPool;

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

            //Get Other Controllers
            secCon = gameObject.GetComponent<SecurityCon>();
            scrMan = gameObject.GetComponent<ScreenManager>();
            sndCon = gameObject.GetComponent<SoundCon>();
            
            //Get Each Energy Container in Eng
            //foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("EnergyBox"))
            //{
            //    EngBarCon objCon = gameObj.GetComponent<EngBarCon>();
            //    if (objCon.section == Section.Life)
            //        lsPowerCon = objCon;
            //   if (objCon.section == Section.Shields)
            //        shieldPowerCon = objCon;
            //}
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        currentSec = Section.Start;
        enegPool = startingEnergy;
        engBarSprite = energyBar.gameObject.GetComponent<SpriteRenderer>();
        engSpriteLng = energy.GetComponent<SpriteRenderer>().size.x;

        lsPowerCon.SetupEngPool();
        shieldPowerCon.SetupEngPool();
        //Moved Coroutines to when prob starts
        //StartCoroutine(CheckForDamage(lifeCheckMax, lifeCheckMin, lsPowerCon.GetEngPool()));
        //StartCoroutine(CheckForDamage(shieldCheckMax, shieldCheckMin, shieldPowerCon.GetEngPool()));
        //StartCoroutine(CheckForDamage(secCon.maxTime, secCon.minTime, secCon));
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            gameRunning = true;
            StartCoroutine(EnergyTimer(energyInterval, energyGenSpeed, maxEnergy, startingEnergy));
            StartCoroutine(secCon.GenProb());
        }

        if (gameOver)
        {
            gameOverScreen.SetActive(true);
        }
        else if (gameRunning)
            CheckEngPool();
    }
    private void LateUpdate()
    {
        if (gameStart)
            gameStart = false;
    }

    IEnumerator EnergyTimer(float interval, int genSpeed, int maxEng, int startEng)
    {
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
                    if (currentSec != Section.Eng)
                        newEnergy.transform.position = newEnergy.transform.position + new Vector3(15, 0);
                    lastObjPos = newEnergy.transform.position;
                }
                else
                {
                    newEnergy.transform.position = energyBar.transform.position + new Vector3(engBarSprite.bounds.size.x / 2 - 1.3f, 0) - (new Vector3(.5f, 0f) * (engObjPool.Count() - 1));
                    if (currentSec != Section.Eng)
                        newEnergy.transform.position = newEnergy.transform.position + new Vector3(15, 0);
                }
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

    void CheckEngPool()
    {
        if (dmgLsPool == null)
        {
            if (lsPowerCon.getCount() == 0)
                dmgLsPool = StartCoroutine(CheckForDamage(lifeCheckMax, lifeCheckMin, lsPowerCon.GetEngPool()));
        }
        else
        {
            if (lsPowerCon.getCount() > 0)
                StopCoroutine(dmgLsPool);
        }

        if (dmgShieldPool == null)
        {
            if (shieldPowerCon.getCount() == 0)
                dmgShieldPool = StartCoroutine(CheckForDamage(shieldCheckMax, shieldCheckMin, shieldPowerCon.GetEngPool()));
        }
        else
        {
            if (shieldPowerCon.getCount() > 0)
                StopCoroutine(dmgShieldPool);
        }
    }

    public void RemoveEng(GameObject engObj)
    {
        engObjPool.RemoveObj(engObj);
        
    }

    public SecurityCon GetSecurityCon()
    {
        return secCon;
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


    public void TakeDamage()
    {
        currentHull--;
        camCon.ShakeCam();
        if (currentHull == 0)
        {
            sndCon.PlayPowerDown();
            gameOver = true;
        }
        else
            GameObject.Destroy(hullPoints[currentHull]);
        
    }

    public IEnumerator CheckForDamage(float maxTime, float minTime, EngPool pool)
    {
        while (!gameOver && pool.Count() == 0)
        {
            float newRadTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(newRadTime);

            

            if (pool.Count() == 0 && !gameOver)
            {
                TakeDamage();
                sndCon.PlayExplosion();
            }
        }

    }
    public IEnumerator CheckForDamage(float maxTime, float minTime, SecurityCon security, int probSys)
    {
        while (!gameOver && security.GetProbs()[probSys])
        {
            float newRadTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(newRadTime);

            if (!gameOver && security.GetProbs()[probSys])
            {
                TakeDamage();
                sndCon.PlayExplosion();
            }
        }

    }
}
