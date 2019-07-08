using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InstallOptions : MonoBehaviour
{

    #region requirements

    public GameObject Options,Warning,installscreen;
 
    public InputField NicknameField;
    public Text RequiredSpaceField;
    public static string Nickname;
    public string RequiredSpace;
    public static string FreeSpace;
    private int FreeSpaceINT, RequiredSpaceINT;
    private IEnumerator ShowError;
    public static bool Shortcut = false;
    #endregion

    public void OpenOptions()
    {
        Options.SetActive(true);
        Warning.SetActive(false);
        
        RequiredSpaceField.text = (RequiredSpace + " GB");
        Gamebuttons.Disable();
    }

    public void CancelInstall()
    {
        Gamebuttons.Enable();
        GetComponent<Canvas>().enabled = false;

    }
    public void MakeShortcut() {
        Shortcut = !Shortcut;
    }
    public void ResetShortcut()
    {
        GetComponentInChildren<Toggle>().GetComponent<Toggle>().isOn = false;
    }
    public void SaveOptionsAndStartInstall()
    {
        int.TryParse(FreeSpace, out FreeSpaceINT);
        int.TryParse(RequiredSpace, out RequiredSpaceINT);
        Nickname = NicknameField.text;

        Debug.Log(FreeSpaceINT + " = Current Free Space");
        Debug.Log(RequiredSpaceINT + " = Required Space");

        if (FreeSpaceINT < RequiredSpaceINT)
        {
            Warning.GetComponent<Text>().text = "Niet genoeg ruimte op deze schijf, je komt nog " + (RequiredSpaceINT - FreeSpaceINT) + " GB te kort";
            Debug.Log("Error, Low Disk Space");
            StartCoroutine ("ShowErrorTimer"); 
            
        } else if (Nickname == "")
        {
            Warning.GetComponent<Text>().text = "Je moet een Ingame naam invullen";
            StartCoroutine("ShowErrorTimer");
        }
        else
        {
            Warning.SetActive(false); //just to be sure 
            Debug.Log("Saved all settings, starting");
            StartInstallation();
        }
    }

    IEnumerator ShowErrorTimer()
    {
        Warning.SetActive(true);
        yield return new WaitForSeconds(3);
        Warning.SetActive(false);
    }
    
    void StartInstallation()
    {
        Debug.Log("Starting installation with name : " + Nickname);
        GetComponent<Canvas>().enabled = false;
        DownloadGame downloader = GameObject.Find("Downloader").GetComponent<DownloadGame>();
        installscreen.GetComponent<Canvas>().enabled = true;
        downloader.GetComponent<DownloadGame>().StartDownloader();
    }

}

