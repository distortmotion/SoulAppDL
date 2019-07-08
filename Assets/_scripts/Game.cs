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
    [Header("!! Do not change this !!")]
    public Button installButton;
    public Text GameNameText;
    string Thisgame;

    [Header("Change this !!")]
    [Tooltip("De hoofpad van de installatie ( b.v. 1.1.1.1/files/ )")]
    public string Starturl = "http://distortworks.com/";
    [Tooltip("De naam van de game")]
    public string GameName;
    [Tooltip("Het aantal bestanden en de bestandnamen (dit moet in zip format zijn( 1.zip , 1.z01 etc))")]
    public string[] Files;
    [Tooltip("De schijfruimte die nodig is voor de installatie")]
    public string RequiredSpace;

    private void Start()
    {
        installButton.GetComponentInChildren<Text>().text = GameName;


    }



    public void LoadData()
    {
        if (Starturl == "")
        {
            Debug.Log("Geen start URL");
        }
        else if (GameName == "")
        {
            Debug.Log("Geen Gamename ingevoerd");
        }
        else if (Files.Length == 0)
        {
            Debug.Log("Geen files om te downloaden");
        }
        else if (RequiredSpace == "")
        {
            Debug.Log("Geen bnodigde ruimte ingevoerd");
        }
        else
        {
            SyncSettings();
            StartDownloading();
        }





    }

    private void SyncSettings()
    {
        DownloadGame Downloader = GameObject.Find("Downloader").GetComponent<DownloadGame>();
        Downloader.ArraySize = Files.Length.ToString();
        Downloader.SyncFileArray();
        Downloader.Starturl = Starturl;
        SpecialScripts.Gamename = GameName;
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



