using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCon : MonoBehaviour
{
    public float shakeTime = .2f;
    public float minShake = 1f;
    public float maxShake = 1f;

    bool isShaking = false;
    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCam()
    {
        isShaking = true;
        StartCoroutine(shake());
    }

    IEnumerator shake()
    {
        float endTime = Time.time + shakeTime;
        while (isShaking)
        {
            if (endTime <= Time.time)
            {
                transform.position = startingPos;
                isShaking = false;
                break;
            }

            float newPosShakeX = Random.Range(minShake, maxShake);
            float newPosShakeY = Random.Range(minShake, maxShake);
            float shakePercentage = shakeTime / newPosShakeX;
            Vector3 shakeAmount = new Vector3(newPosShakeX, newPosShakeY, 0);
            shakeTime = Mathf.Lerp(shakeTime, 0, Time.deltaTime);
            
            transform.position = Vector3.Lerp(transform.position, shakeAmount, Time.deltaTime);
            yield return null;
        }
    }
}
