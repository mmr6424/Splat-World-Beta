using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public UnityEngine.GameObject menuOriginalPosR, menuActivePosR, menuOriginalPosL, menuActivePosL, menuPanel;
    public bool moveMenuPanel, moveMenuPanelBack, leftOrRight;
    public float moveSpeed;
    public float tempPos;


    // Start is called before the first frame update
    void Start()
    {
        leftOrRight = false;
        //menuPanel.transform.position = menuOriginalPosL.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (leftOrRight)
        {
            if (menuPanel.transform.position == menuOriginalPosL.transform.position)
            {
                //menuPanel.transform.position = menuOriginalPosR.transform.position;
            }

            if (menuActivePosL.transform.position == menuActivePosL.transform.position)
            {
                //menuPanel.transform.position = menuActivePosR.transform.position;
            }

            if (moveMenuPanel)
            {
                menuPanel.transform.position = UnityEngine.Vector3.Lerp(menuPanel.transform.position, menuActivePosR.transform.position, moveSpeed * UnityEngine.Time.deltaTime);

                if (menuPanel.transform.localPosition.x == tempPos)
                {
                    moveMenuPanel = false;
                    menuPanel.transform.position = menuActivePosR.transform.position;
                    tempPos = -9999999999999.99f;
                }
                if (moveMenuPanel)
                {
                    tempPos = menuPanel.transform.position.x;
                }
            }

            if (moveMenuPanelBack)
            {
                menuPanel.transform.position = UnityEngine.Vector3.Lerp(menuPanel.transform.position, menuOriginalPosR.transform.position, moveSpeed * UnityEngine.Time.deltaTime);

                if (menuPanel.transform.localPosition.x == tempPos)
                {
                    moveMenuPanelBack = false;
                    menuPanel.transform.position = menuOriginalPosR.transform.position;
                    tempPos = -9999999999999.99f;
                }
                if (moveMenuPanelBack)
                {
                    tempPos = menuPanel.transform.position.x;
                }
            }
        } else if (leftOrRight == false)
        {

            if (moveMenuPanel)
            {
                menuPanel.transform.position = UnityEngine.Vector3.Lerp(menuPanel.transform.position, menuActivePosL.transform.position, moveSpeed * UnityEngine.Time.deltaTime);

                if (menuPanel.transform.localPosition.x == tempPos)
                {
                    moveMenuPanel = false;
                    menuPanel.transform.position = menuActivePosL.transform.position;
                    tempPos = -9999999999999.99f;
                }
                if (moveMenuPanel)
                {
                    tempPos = menuPanel.transform.position.x;
                }
            }

            if (moveMenuPanelBack)
            {
                menuPanel.transform.position = UnityEngine.Vector3.Lerp(menuPanel.transform.position, menuOriginalPosL.transform.position, moveSpeed * UnityEngine.Time.deltaTime);

                if (menuPanel.transform.localPosition.x == tempPos)
                {
                    moveMenuPanelBack = false;
                    menuPanel.transform.position = menuOriginalPosL.transform.position;
                    tempPos = -9999999999999.99f;
                }
                if (moveMenuPanelBack)
                {
                    tempPos = menuPanel.transform.position.x;
                }
            }
        }
    }
    public void MovePanel()
    {
        moveMenuPanelBack = false;
        moveMenuPanel = true;
        Debug.Log("Panel moved");

    }

    public void MovePanelBack()
    {
        moveMenuPanel = false;
        moveMenuPanelBack = true;
        Debug.Log("Panel moved back");

    }

    public void ToggleLeftRight()
    {
        //true = right, false = left
        leftOrRight = !leftOrRight;
    }
}
