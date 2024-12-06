using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TUPanelConctrol : MonoBehaviour
{
    private bool Timer = false;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForTime());
        Time.timeScale = 0;
    }

    private IEnumerator WaitForTime() 
    {
        yield return new WaitForSecondsRealtime(3f);
        Timer = true;
    }

    
    void Update()
    {
        if (Input.anyKeyDown && Timer)
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}
