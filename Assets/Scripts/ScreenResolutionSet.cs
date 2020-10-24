using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionSet : MonoBehaviour
{
   void Start()
    {
        Screen.SetResolution(720, 1280, false);
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
