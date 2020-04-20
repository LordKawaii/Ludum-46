using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecSysBox : MonoBehaviour
{
    public Section section;

    [HideInInspector]
    public bool hasCrew = false;

    public int GetSysId()
    {
        switch(section)
        {
            case Section.Eng:
                return 0;
            case Section.Life:
                return 1;
            case Section.Bridge:
                return 2;
        }
        return 3;
    }

    public void AddCrew(GameObject crew)
    {
        crew.transform.position = transform.position;
        hasCrew = true;
    }
}
