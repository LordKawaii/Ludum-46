using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamCon : MonoBehaviour
{
    public float energyInterval = 1f;
    public float startingEnergy = 10;
    public float energyGenSpeed = 1f;

    float enegPool;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator EnergyTimer()
    {
        while (true)
        {
            GenerateEnergy();
            yield return new WaitForSeconds(energyInterval);
        }


    float GenerateEnergy(float currEng, float genSpeed)
    {
        return currEng + genSpeed;
    }
}
