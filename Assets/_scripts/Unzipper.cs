using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System.ComponentModel;
using Unity.Jobs;
using Ionic.Zip;
using System;

using System.Text;

public class Unzipper : MonoBehaviour
{

    public string StartUrl; // Start of zipfile
    public Slider ProgressBar; // Unzip ProgressBar
    public Text ProgressPerCent; // Progress % 
    public Text InstallText; // Installation Text (e.g. file number )

    public void CallUnzip()
    {
        StartCoroutine(StartUnzip());
    }
    IEnumerator StartUnzip()
    {
        string ZipToUnpack = StartUrl;
        string TargetDir = Browser.Savefolder;
        WaitForSeconds wait = new WaitForSeconds(0.00f);
        int file = 1;
        Debug.Log(StartUrl + " this is the unzip start");

        using (ZipFile zip = ZipFile.Read(ZipToUnpack))
        {
            Debug.Log(zip.Count);
            foreach (ZipEntry e in zip)
            {    
                e.Extract(TargetDir, ExtractExistingFileAction.DoNotOverwrite);
                InstallText.text = ("Bezig met installeren van bestand " + file + " van de " + zip.Count);
                ProgressBar.value = ((file * 100 / zip.Count));
                ProgressPerCent.text = (ProgressBar.value.ToString() + " %");
                file++;
                yield return wait;
            }
        }
        SpecialScripts.SpecialUsernameRules();
    }
}
