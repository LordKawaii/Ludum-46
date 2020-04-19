using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngBarCon : MonoBehaviour
{
    public Section section = Section.Eng;
    public int maxEnergy = 10;

    EngPool engPool;
    SpriteRenderer sprite;
    private void Awake()
    {
        engPool = new EngPool(maxEnergy);
        sprite = GetComponent<SpriteRenderer>();
    }

    public void AddEnergy(GameObject engObj)
    {
        engPool.Add(engObj);
        if (engPool.Count() == 1)
        {
            engObj.transform.position = transform.position + new Vector3(sprite.bounds.size.x / 2 - 1.3f, 0);
        }
        else
        {
            engObj.transform.position = transform.position + new Vector3(sprite.bounds.size.x / 2 - 1.3f, 0) - new Vector3(.5f, 0f) * (engPool.Count() - 1f);
        }
    }

    public int getCount()
    {
        return engPool.Count();
    }

    public void RemoveEnergy(GameObject engObj)
    {
        engPool.RemoveObj(engObj);
    }

    public EngPool GetEngPool()
    {
        return engPool;
    }
}
