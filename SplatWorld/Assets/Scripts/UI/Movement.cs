using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public UnityEngine.GameObject menuOriginalPos, menuActivePos, menuPanel;
    public bool moveMenuPanel, moveMenuPanelBack;
    public float moveSpeed;
    public float tempPos;


    // Start is called before the first frame update
    void Start()
    {
        menuPanel.transform.position = menuOriginalPos.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveMenuPanel)
        {
            menuPanel.transform.position = UnityEngine.Vector3.Lerp(menuPanel.transform.position, menuActivePos.transform.position, moveSpeed * UnityEngine.Time.deltaTime);

            if(menuPanel.transform.localPosition.x == tempPos)
            {
                moveMenuPanel = false;
                menuPanel.transform.position = menuActivePos.transform.position;
                tempPos = -9999999999999.99f;
            }
            if (moveMenuPanel)
            {
                tempPos = menuPanel.transform.position.x;
            }
        }

        if (moveMenuPanelBack)
        {
            menuPanel.transform.position = UnityEngine.Vector3.Lerp(menuPanel.transform.position, menuOriginalPos.transform.position, moveSpeed * UnityEngine.Time.deltaTime);

            if (menuPanel.transform.localPosition.x == tempPos)
            {
                moveMenuPanelBack = false;
                menuPanel.transform.position = menuOriginalPos.transform.position;
                tempPos = -9999999999999.99f;
            }
            if (moveMenuPanelBack)
            {
                tempPos = menuPanel.transform.position.x;
            }
        }
    }
    public void MovePanel()
    {
        moveMenuPanelBack = false;
        moveMenuPanel = true;

    }

    public void MovePanelBack()
    {
        moveMenuPanel = false;
        moveMenuPanelBack = true;
    }
}
