using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNDrop : MonoBehaviour
{
    private bool dragging = false;
    private float distance;
    private Vector3 originalPos;
    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
        originalPos = transform.position;
    }

    void OnMouseUp()
    {
        dragging = false;
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Collider2D[] collidersUnderMouse = new Collider2D[4];
        int numCollidersUnderMouse = Physics2D.OverlapPoint(mousePos, new ContactFilter2D().NoFilter(), collidersUnderMouse);
        Debug.Log(numCollidersUnderMouse);
        for (int i = 0; i < numCollidersUnderMouse; ++i)
        {
            if (collidersUnderMouse[i].tag == "EnergyBox")
            {
                EngBarCon engBar = collidersUnderMouse[i].GetComponent<EngBarCon>();
                engBar.AddEnergy(gameObject);
                gameObject.GetComponent<EnergyCon>().ChangeSection(engBar.section, engBar.getCount());
                GamCon.Instance.RemoveEng(gameObject);
            }
        }        
        if(numCollidersUnderMouse == 1)
            transform.position = originalPos;
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
