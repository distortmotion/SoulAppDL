using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UnrealTournament2004 : MonoBehaviour
{
    #region Requirements
    public static GameObject ExitButtonStatic;
    public GameObject ExitButton;
    #endregion

    private void Start()
    {
        ExitButtonStatic = ExitButton;
    }

    #region Special install rules // change when needed
    public static void Special()
    {

        DownloadGame downloader = GameObject.Find("Downloader").GetComponent<DownloadGame>();
        downloader.GetComponent<DownloadGame>().InstallText.text = ("Bezig met toepassen van de config");
        string config = (Browser.Savefolder + @"Unreal Tournament 2004\Unreal Tournament 2004\System\User.ini");
        string text = File.ReadAllText(config);
        Debug.Log(config + text);
        text = text.Replace("Spectator_122", InstallOptions.Nickname);
        File.WriteAllText(config, text);

        InstallDone();
    }
    #endregion


    #region End install  // dont change this 
    public static void InstallDone()
    {
        Unzipper Unzipper = GameObject.Find("Unzipper").GetComponent<Unzipper>();
        Unzipper.GetComponent<Unzipper>().InstallText.text = ("Afronden van installatie");
        DownloadGame downloader = GameObject.Find("Downloader").GetComponent<DownloadGame>();
        downloader.GetComponent<DownloadGame>().CleanUp();
        Unzipper.GetComponent<Unzipper>().InstallText.text = ("Installatie Voltooid !");
        Gamebuttons.Enable();
        ExitButtonStatic.SetActive(true);
    }
    #endregion
}

