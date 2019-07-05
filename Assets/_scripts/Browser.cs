﻿using UnityEditor;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Browser : MonoBehaviour
{
    #region requirements
    public static string Savefolder = (@"C:\Games\");
    public InputField SavefolderInput;
    public Text savetext;
    public Dropdown drivelist;
    public Text FreeSpace;
    List<string> Alldrives = new List<string>();
    #endregion


    void CreateDriveList()
    {
        
        string[] Drives = Directory.GetLogicalDrives();
        foreach (string d in Drives)
        {
            Alldrives.Add(d);
            Debug.Log("Disk available " + d);
            Debug.Log("Free Space on " + d + " " + GetTotalFreeSpace(d) / 1024 / 1024 / 1024 + " GB");
        }

    }
    public long GetTotalFreeSpace(string driveName)
    {
        foreach (DriveInfo drive in DriveInfo.GetDrives())
        {
            if (drive.IsReady && drive.Name == driveName)
            {
                return (drive.TotalFreeSpace);
            }
        }
        return -1;
    }


    private void Start()
    {
        drivelist.ClearOptions();
        Debug.Log("Setting save folder to : " + Savefolder);
        CreateDriveList();
        savetext.text = Savefolder;
        drivelist.AddOptions(Alldrives);
        UpdateSaveFolderText();
    }

    public void SaveFolder()
    {
        //add / to the endfolder if not provided
        if (SavefolderInput.text == "")
        {
            Savefolder = (@"C:\Games\");
            Debug.Log("Setting save folder to : " + Savefolder);
            UpdateSaveFolderText();
        }
        else if (!SavefolderInput.text.EndsWith(@"\"))
        {
            Savefolder = ((drivelist.options[drivelist.value].text) + SavefolderInput.text + @"\");
            Debug.Log("Setting save folder to : " + Savefolder);
            UpdateSaveFolderText();
        }
        else
        {
            Savefolder = ((drivelist.options[drivelist.value].text) +  SavefolderInput.text);
            Debug.Log("Setting save folder to : " + Savefolder);
            UpdateSaveFolderText();
        }

    }

    private void UpdateSaveFolderText()
    {
        savetext.text = Savefolder;
        FreeSpace.text = (GetTotalFreeSpace(drivelist.options[drivelist.value].text) / 1024 / 1024 / 1024 + " GB");
        InstallOptions.FreeSpace = (GetTotalFreeSpace(drivelist.options[drivelist.value].text) / 1024 / 1024 / 1024 +"");
    }

    public static bool WriteFile(string path, string fileName, string data)
    {
        bool retValue = false;
        try
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            System.IO.File.WriteAllText(path + fileName, data);
            retValue = true;
        }
        catch (System.Exception ex)
        {
            string ErrorMessages = "File Write Error\n" + ex.Message;
            retValue = false;
            Debug.LogError(ErrorMessages);
        }
        return retValue;
    }

}

