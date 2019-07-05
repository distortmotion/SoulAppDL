using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Ionic.Zip;
using System.Net;
using System.ComponentModel;
using Unity.Jobs;
using System;
using Component = UnityEngine.Component;

public class Game : MonoBehaviour
{

    public Button installButton;
    public Slider progressBar;
    public Text progressText,GameNameText;
    public string GameName;
    public string[] Files;
    public string Starturl;
    string CurrentFile;
    float progress;
    public static string Zipfile;
    public string RequiredSpace;
    string Thisgame;

    public Text ErrorMessageText;
    public UnityWebRequest www;

    private void Start()
    {
        installButton.GetComponentInChildren<Text>().text = GameName;
    }

    public void LoadData()
    {
        DownloadGame Downloader = GameObject.Find("Downloader").GetComponent<DownloadGame>();

        
        Downloader.ArraySize = Files.Length.ToString();
        Downloader.SyncFileArray();
        Downloader.Starturl = Starturl;
        Debug.Log(Starturl + " is a fileserver location");
        Array.Copy(Files, Downloader.Files, Files.Length);
        
         
  
    }

    public void StartDownloading()
    {
        InstallOptions InstallOptions = GameObject.Find("InstallOptions").GetComponent<InstallOptions>();
        InstallOptions.gameObject.GetComponent<Canvas>().enabled = true;
        InstallOptions.GetComponent<InstallOptions>().RequiredSpace = RequiredSpace;
        GameNameText.text = GameName;
        SpecialScripts.ThisGame = gameObject.name;
        InstallOptions.OpenOptions();

    }

}



