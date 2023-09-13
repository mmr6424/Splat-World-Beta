using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconFlip : MonoBehaviour
{
    public UnityEngine.GameObject menuOriginalPos, menuActivePos, menuPanel;
    public bool moveMenuPanel, moveMenuPanelBack;
    public float moveSpeed;
    public float tempPos;
    Vector3 tempVec;


    // Start is called before the first frame update
    void Start()
    {
        menuPanel.transform.localScale = menuOriginalPos.transform.localScale;
    }

    public void MovePanel()
    {
        tempVec = menuPanel.transform.localScale;
        tempVec.x *= -1;
        menuPanel.transform.localScale = tempVec;
    }
}
