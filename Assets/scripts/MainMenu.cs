﻿using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{

    
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        Application.LoadLevel("ExTerraStaging");
    }

    public void MM()
    {
        Application.LoadLevel("ExTerraMainMenu");
    }

    public void SaveGame()
    {

        
    }


    public void LoadGame()
    {
        
    }

    public void About()
    {
        Application.LoadLevel("ExTerraAbout");
    }
}