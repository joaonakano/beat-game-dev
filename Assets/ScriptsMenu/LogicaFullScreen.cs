using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class LogicaFullScreen : MonoBehaviour
{
    public Toggle toggle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AtivarFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
