using UnityEngine;
using System;
using IWshRuntimeLibrary;
using File = System.IO.File;

public class UnrealTournament2004 : MonoBehaviour
{
    #region Requirements
    public static GameObject ExitButtonStatic;
    public static string GameName;
    public GameObject ExitButton;

    private static string ExeLocation = @"Unreal Tournament 2004\Unreal Tournament 2004\System\ut2004.exe";
    private static string ConfigLocation = @"Unreal Tournament 2004\Unreal Tournament 2004\System\User.ini";

    #endregion

    private void Start()
    {
        ExitButtonStatic = ExitButton;
        

    }

    #region Special trivia during install
    public static void Trivia() { }
    #endregion

    #region Special install rules
    public static void Special()
    {

        DownloadGame downloader = GameObject.Find("Downloader").GetComponent<DownloadGame>();

        downloader.GetComponent<DownloadGame>().InstallText.text = ("Bezig met toepassen van de config");
        string config = (Browser.Savefolder + ConfigLocation);
        string text = File.ReadAllText(config);
        Debug.Log(config + text);
        text = text.Replace("Spectator_122", InstallOptions.Nickname);
        File.WriteAllText(config, text);
        GameName = SpecialScripts.Gamename;
        CheckForShortcut();
    }

    private static void CheckForShortcut() // dont change this
    {
        if (InstallOptions.Shortcut == true)
        {
            CreateShortcut();
            InstallDone();
        }
        else
        {
            InstallDone();
        }
    } 

    static void CreateShortcut() // dont change this
    {
        WshShell wsh = new WshShell();
        IWshShortcut shortcut = wsh.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + GameName + ".lnk") as IWshShortcut;
        shortcut.TargetPath = (Browser.Savefolder + ExeLocation); // end location of .exe file
        shortcut.WindowStyle = 1; // default window setting
        shortcut.Description = GameName; // The name of the game
        shortcut.WorkingDirectory = Browser.Savefolder; // start folder
        shortcut.IconLocation = shortcut.TargetPath; // Shortcut icon location
        shortcut.Save();
    }
    #endregion

    #region End install  
    public static void InstallDone() // dont change this 
    {
        Unzipper Unzipper = GameObject.Find("Unzipper").GetComponent<Unzipper>();
        Unzipper.GetComponent<Unzipper>().InstallText.text = ("Afronden van installatie");
        DownloadGame downloader = GameObject.Find("Downloader").GetComponent<DownloadGame>();
        InstallOptions installoptions = GameObject.Find("InstallOptions").GetComponent<InstallOptions>();
        installoptions.ResetShortcut();
        downloader.GetComponent<DownloadGame>().CleanUp();
        Unzipper.GetComponent<Unzipper>().InstallText.text = ("Installatie Voltooid !");
        Gamebuttons.Enable();
        ExitButtonStatic.SetActive(true);
    }
    #endregion
}

