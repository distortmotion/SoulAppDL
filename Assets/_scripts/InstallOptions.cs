using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class InstallOptions : MonoBehaviour
{

    #region requirements
    //public static string Savefolder;
    //public InputField SavefolderInput;
    //public Text savetext;
    //public Dropdown drivelist;
    //List<string> Alldrives = new List<string>();
    //public static string GameName;


    public GameObject Options,screen,Warning,installscreen;
    public Button game1, game2, game3, game4;
 
    public InputField NicknameField;
    public Text  RequiredSpaceField;
    public static string Nickname;
    public string RequiredSpace;
    public static string FreeSpace;
    private int FreeSpaceINT, RequiredSpaceINT;
    private IEnumerator ShowError;

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
        GetComponent<Canvas>().enabled = false;
        Debug.Log("Starting installation with name : " + Nickname);


        DownloadGame downloader = GameObject.Find("Downloader").GetComponent<DownloadGame>();
        installscreen.GetComponent<Canvas>().enabled = true;
        downloader.GetComponent<DownloadGame>().StartDownloader();


    }

}

