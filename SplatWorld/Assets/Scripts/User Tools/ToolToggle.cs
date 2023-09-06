using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolToggle : MonoBehaviour
{
    public GameObject brush, can, bucket;
    // Start is called before the first frame update

    public void ActivateBrush()
    {
        brush.SetActive(true);
        can.SetActive(false);
        bucket.SetActive(false);
    }

    public void ActivateCan()
    {
        brush.SetActive(false);
        can.SetActive(true);
        bucket.SetActive(false);
    }

    public void ActivateBucket()
    {
        brush.SetActive(false);
        can.SetActive(false);
        bucket.SetActive(true);
    }

    void Start()
    {
        brush.SetActive(false);
        can.SetActive(false);
        bucket.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
