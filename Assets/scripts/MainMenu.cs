using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{



    public GameObject LoadImage = null;
    public GameObject CreditImage = null;
    public GameObject TutorialImage = null;

    public void Exit()
    {
        Application.Quit();
    }

    public void NewGame()
    {
        if (LoadImage != null)
        {
            LoadImage.SetActive(true);
        }
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
        //Application.LoadLevel("ExTerraMainMenu");
    }

    public void LoadGame()
    {
        if(LoadImage != null)
        {
            LoadImage.SetActive(true);
        }
        globals.shouldload = true;
        Debug.Log(globals.currentSave);
        Application.LoadLevel("ExTerraStaging");
    }

    public void About()
    {
        Application.LoadLevel("ExTerraAbout");
    }

    public void ToggleCredits()
    {
        if (TutorialImage != null)
        {
            TutorialImage.SetActive(false);
        }
        if (CreditImage != null)
        {
            CreditImage.SetActive(!CreditImage.activeSelf);
        }
    }

    public void ToggleTutorial()
    {
        if (CreditImage != null)
        {
            CreditImage.SetActive(false);
        }
        if (TutorialImage != null)
        {
            TutorialImage.SetActive(!TutorialImage.activeSelf);
        }
    }
}