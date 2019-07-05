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



    public void StartDownloader()
    {

        Debug.Log(Starturl);
        CurrentFile = Files[0];
        Zipfile = Files[0];
        i = 0;
        DownloadNextFile();
    }


    #region Download
    public void SyncFileArray()
    {

        int.TryParse(ArraySize, out ArraySizeINT);
        Files = new string[ArraySizeINT];
    }
   
    void CheckFile()
    {
        
        progressBar.value = 0;
        WebClient client = new WebClient();
        Debug.Log("current : " + CurrentFile);
        string savePath = (Browser.Savefolder + @"temp\");
        Directory.CreateDirectory(savePath);
        TempSavePath = savePath;
        Uri url = new Uri(Starturl + CurrentFile);
        Debug.Log(url);
        string CompleteFileLocation = savePath + CurrentFile;
        if (File.Exists(CompleteFileLocation))
        {
            Debug.Log("bestaat al ");
            var LocalfileInfo = new System.IO.FileInfo(CompleteFileLocation);



            StartCoroutine(GetRemoteFileSize(Starturl + CurrentFile, (size) =>
            {
                if (size == LocalfileInfo.Length)
                {
                    DownloadNextFile();
                    Debug.Log(CurrentFile + "  remote Size: " + size + "  local size " + LocalfileInfo.Length);
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

    private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
    {
        
        progressBar.value = (e.ProgressPercentage);

    }

    void DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
    {

        if (e.Error == null)
        {
            DownloadNextFile();
        }else if (e.Error.Message.Contains("Could not find a part of the path"))
        {
            //error message can't create temp folder
            ErrorMessageText.text = "Kan tijdelijke folder niet aanmaken";
            Exitbutton.SetActive(true);
        }
        else if (e.Error.Message.Contains("The remote server returned an error: (404) Not Found."))
        {
            //error message can't find file
            ErrorMessageText.text = "Kan download niet vinden, neem contact op met Moxxi";
            Exitbutton.SetActive(true);
        }
        else { Debug.Log("All OK"); }

    }

    void DownloadNextFile()
    {
        if (i < (Files.Length))
        {
            
            InstallText.text = ("Downloaden van bestand " + (i+1) + " van de " + Files.Length);
            CurrentFile = Files[i++];
            Debug.Log(CurrentFile);
            CheckFile();
            
            
        }
        else
        {
            DownloadDone();
            InstallText.text = ("Voorbereiden van de installatie");
        }



    }
    public void ExitInstallButton()
    {

        Exitbutton.SetActive(false);
        ErrorMessageText.text = "";

        installscreen.GetComponent<Canvas>().enabled = false;
        Gamebuttons.Enable();

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
    
    #region Cleanup Temp Items

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
            SpecialScripts.SpecialScript();
        }
    }

    private void DeleteTempFolder()
    {
        Directory.Delete(TempSavePath);
       
    }

    private void DeleteTempFiles(string F)
    {
        Debug.Log(TempSavePath + F);
        File.Delete(TempSavePath + F);
    }

    #endregion

}
