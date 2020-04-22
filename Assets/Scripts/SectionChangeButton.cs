using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionChangeButton : MonoBehaviour
{
    public Section section;

    private void OnMouseDown()
    {
        Vector2 mousePos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Collider2D[] collidersUnderMouse = new Collider2D[4];
        int numCollidersUnderMouse = Physics2D.OverlapPoint(mousePos, new ContactFilter2D().NoFilter(), collidersUnderMouse);
        for (int i = 0; i < numCollidersUnderMouse; ++i)
        {
            if (collidersUnderMouse[i].gameObject == gameObject)
            {
                if (gameObject.tag == "ExitBtn")
                {
                    Application.Quit();
                    Debug.Log("Quitting");
                }
                else
                    GamCon.Instance.scrMan.ChangeScreen(section);
            }
        }

        if (gameObject.tag == "StartBtn")
        {
            GamCon.Instance.gameStart = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
