using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelCredits;
    [SerializeField] private GameObject MainMenuPanel;

    private InputSystem_Actions _controls;

    public void Play()
   {
      SceneManager.LoadScene("SampleScene");
   }

   public void OpenSettings()
   {
      panelSettings.SetActive(true);
      panelCredits.SetActive(false);
      ActiveMainMenu(false);
   }

   public void OpenCredits()
   {
      panelCredits.SetActive(true);
      panelSettings.SetActive(false);
      ActiveMainMenu(false);
    }

   public void ClosePanels()
   {
      panelSettings.SetActive(false);
      panelCredits.SetActive(false);
        ActiveMainMenu(true);
   }

    public void ActiveMainMenu(bool IsActive)
    {
        MainMenuPanel.SetActive(IsActive);
    }

   

    public void Exit()
    {
        Application.Quit();
    }
}
