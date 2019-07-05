using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gamebuttons : MonoBehaviour
{


    public static GameObject[] Buttons;
    private void Start()
    {
        Buttons = GameObject.FindGameObjectsWithTag("Button");
    }


    public static void Enable()
    {
        foreach (GameObject B in Buttons)
        {
            B.SetActive(true);
        }
    }
    
    public static void Disable()
    {
        Buttons = GameObject.FindGameObjectsWithTag("Button");
        foreach (GameObject B in Buttons)
        {
            B.SetActive(false);
        }
    }
}
