using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    private bool dragging = false;
    private float distance;

    [HideInInspector]
    public bool canDrag = true;
    [HideInInspector]
    public Vector3 originalPos;
    void OnMouseDown()
    {
        if (canDrag)
        { 
            distance = Vector3.Distance(transform.position, Camera.main.transform.position);
            dragging = true;
            originalPos = transform.position;
        }
    }

    void OnMouseUp()
    {
        if(dragging)
        { 
            if (gameObject.tag == "Energy") 
            { 
                Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                Collider2D[] collidersUnderMouse = new Collider2D[4];
                int numCollidersUnderMouse = Physics2D.OverlapPoint(mousePos, new ContactFilter2D().NoFilter(), collidersUnderMouse);
                Debug.Log(numCollidersUnderMouse);
                for (int i = 0; i < numCollidersUnderMouse; ++i)
                {
                    if (collidersUnderMouse[i].tag == "EnergyBox" && collidersUnderMouse[i].GetComponent<EngBarCon>().getCount() < collidersUnderMouse[i].GetComponent<EngBarCon>().maxEnergy)
                    {
                        EngBarCon engBar = collidersUnderMouse[i].GetComponent<EngBarCon>();
                        engBar.AddEnergy(gameObject);
                        gameObject.GetComponent<EnergyCon>().ChangeSection(engBar.section, engBar.getCount());
                        GamCon.Instance.RemoveEng(gameObject);
                        canDrag = false;
                        break;
                    }
                    else
                        transform.position = originalPos;
                }
            }

            if (gameObject.tag == "Crew")
            {
                Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
                Collider2D[] collidersUnderMouse = new Collider2D[4];
                int numCollidersUnderMouse = Physics2D.OverlapPoint(mousePos, new ContactFilter2D().NoFilter(), collidersUnderMouse);
                for (int i = 0; i < numCollidersUnderMouse; ++i)
                {
                    if (collidersUnderMouse[i].tag == "SecSys")
                    {
                        collidersUnderMouse[i].GetComponent<SecSysBox>().AddCrew(gameObject);
                        GamCon.Instance.GetSecurityCon().FixProblem(collidersUnderMouse[i].GetComponent<SecSysBox>().GetSysId(), gameObject, originalPos);

                        Debug.Log("isProblem " + GamCon.Instance.GetSecurityCon().GetProbs()[collidersUnderMouse[i].GetComponent<SecSysBox>().GetSysId()]);

                        canDrag = false;
                        break;
                    }
                    else
                        transform.position = originalPos;
                }
            }
        }
        dragging = false;

    }

    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = rayPoint;
        }
    }
}
