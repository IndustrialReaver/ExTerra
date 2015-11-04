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

    public void SaveGame()
    {
        GameManager gm = Camera.main.GetComponent<GameManager>();
        string SaveData = gm.save();
        Debug.Log(SaveData);
        globals.currentSave = SaveData;
        Application.LoadLevel("ExTerraMainMenu");
    }


    public void LoadGame()
    {
        globals.shouldload = true;
        Debug.Log(globals.currentSave);
        Application.LoadLevel("ExTerraStaging");
    }

    public void About()
    {
        Application.LoadLevel("ExTerraAbout");
    }
}