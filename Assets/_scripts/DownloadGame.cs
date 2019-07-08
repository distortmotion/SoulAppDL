using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using Ionic.Zip;
using System.ComponentModel;
using System.IO;
using System;

public class DownloadGame : MonoBehaviour
{
#region Requirements

    public string Starturl;
    string CurrentFile;
    public static string Zipfile;
    public Slider progressBar;
    public Text InstallText, ErrorMessageText;
    int i, ArraySizeINT;
    float progress;
    public string ArraySize;
    public GameObject Exitbutton, installscreen;
    public string[] Files;
    string TempSavePath = (Browser.Savefolder + @"temp\");
#endregion


    public void StartDownloader()
    {

        Debug.Log(Starturl);
        CurrentFile = Files[0];
        Zipfile = Files[0];
        i = 0;
        DownloadNextFile();
    }


#region Downloader
    public void SyncFileArray()
    {

        int.TryParse(ArraySize, out ArraySizeINT);
        Files = new string[ArraySizeINT];
    }
   
    void GetFileLocations()
    {

        progressBar.value = 0;
        WebClient client = new WebClient();
        Debug.Log("current : " + CurrentFile);
        string savePath = (Browser.Savefolder + @"temp\");
        Directory.CreateDirectory(savePath);
        TempSavePath = savePath;
        Uri url = new Uri(Starturl + CurrentFile);
        Debug.Log(url);
        DoesFileExist(client, savePath, url);
    }

    IEnumerator GetRemoteFileSize(string url, Action<long> resut)
    {
        UnityWebRequest RemoteSize = UnityWebRequest.Head(url);
        yield return RemoteSize.SendWebRequest();
        string size = RemoteSize.GetResponseHeader("Content-Length");

        if (RemoteSize.isNetworkError || RemoteSize.isHttpError)
        {
            Debug.Log("Error While Getting Length: " + RemoteSize.error);
            if (resut != null)
                resut(-1);
        }
        else
        {
            if (resut != null)
                resut(Convert.ToInt64(size));
        }
    }

    private void DoesFileExist(WebClient client, string savePath, Uri url)
    {
        string CompleteFileLocation = savePath + CurrentFile;
        if (File.Exists(CompleteFileLocation))
        {
            var LocalfileInfo = new System.IO.FileInfo(CompleteFileLocation);
            StartCoroutine(GetRemoteFileSize(Starturl + CurrentFile, (size) =>
            {
                if (size == LocalfileInfo.Length)
                {
                    DownloadNextFile();
                }
                else
                {
                    StartFileDownload(client, savePath, url);
                }
            }));
        }
        else
        {
            StartFileDownload(client, savePath, url);
        }
    }

    private void StartFileDownload(WebClient client, string savePath, Uri url)
    {
        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFileCompleted);
        client.DownloadFileAsync(url, savePath + CurrentFile);
    }

    void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {
        
        if (e.Error == null)
        {
            DownloadNextFile();
        }
        else if (e.Error.Message.Contains("Could not find a part of the path"))
        {
            //error message can't create temp folder
            ErrorMessageText.text = "Kan tijdelijke folder niet aanmaken";
            AbortDownload();
        }
        else if (e.Error.Message.Contains("The remote server returned an error: (404) Not Found."))
        {
            //error message can't find file
            ErrorMessageText.text = "Kan download niet vinden, neem contact op met Moxxi";
            AbortDownload();
        }
        else { Debug.Log("All OK"); }

    }
      
    void DownloadNextFile()
    {
        if (i < (Files.Length))
        {

            InstallText.text = ("Downloaden van bestand " + (i + 1) + " van de " + Files.Length);
            CurrentFile = Files[i++];
            Debug.Log(CurrentFile);
            GetFileLocations();


        }
        else
        {
            DownloadDone();
            InstallText.text = ("Voorbereiden van de installatie");
        }



    }

    private void AbortDownload()
    {
        Bar2Text ProgressBarText = GameObject.Find("InstallPanel").GetComponent<Bar2Text>();
        ProgressBarText.Progressdone();
        CleanUp();
        DeleteGameFolder();
        Exitbutton.SetActive(true);
    }
    
    void DownloadDone()

    {
        Unzipper unzipper = GameObject.Find("Unzipper").GetComponent<Unzipper>();
        unzipper.StartUrl = TempSavePath + Files[0];
        Debug.Log("Download Complete");
        i = 0;
        unzipper.CallUnzip();
    }

#endregion
    
#region callback and exit fuction

    private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
    {

        progressBar.value = (e.ProgressPercentage);

    }

    public void ExitInstallButton()
    {

        Exitbutton.SetActive(false);
        ErrorMessageText.text = "";

        installscreen.GetComponent<Canvas>().enabled = false;
        Gamebuttons.Enable();

    }
#endregion

#region Cleanup Temp Items after install

    public void CleanUp()
    {
        InstallText.text = ("Bezig met opruimen van tijdelijke bestanden");
        foreach (string F in Files)
        {
            DeleteTempFiles(F);
        }
        if (Directory.GetFiles(TempSavePath).Length == 0) // Deletes only when folder is empty
        {
            DeleteTempFolder();
        }
        else {
            SpecialScripts.SpecialUsernameRules();
        }
    }

    private void DeleteTempFiles(string F)
    {
        Debug.Log(TempSavePath + F);
        File.Delete(TempSavePath + F);
    }

    private void DeleteTempFolder()
    {
        Directory.Delete(TempSavePath);
       
    }

    private static void DeleteGameFolder()
    {
        if (Directory.GetFiles(Browser.Savefolder).Length == 0)
        {
            Directory.Delete(Browser.Savefolder);
        }
    }

#endregion

}
