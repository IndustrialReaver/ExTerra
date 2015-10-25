using UnityEngine;
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

    public void LoadGame()
    {
        Application.LoadLevel("ExTerraLoad");
    }

    public void SaveGame()
    {

    }

    public void About()
    {
        Application.LoadLevel("ExTerraAbout");
    }
}