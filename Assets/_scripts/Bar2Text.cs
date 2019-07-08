using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar2Text : MonoBehaviour
{
    public Slider progressbar;
    public Text progresstext;

    public void StartBar()
    {
        progresstext.text = (progressbar.value.ToString() + " %");
        StartUpdating();
    }
    public void StartUpdating()
    {
        InvokeRepeating("GetProgress", 0f, 0.01f); 
    }

   public void GetProgress()
    {
        Debug.Log(progressbar.value.ToString());
        if(progressbar.value.ToString() == "100") {
            progresstext.text = (progressbar.value.ToString() + " %");
            Progressdone();
        }
        else
        {
            progresstext.text = (progressbar.value.ToString() + " %");
            
        }
    }

    public void Progressdone()
    {
        CancelInvoke("GetProgress");
    }

}
