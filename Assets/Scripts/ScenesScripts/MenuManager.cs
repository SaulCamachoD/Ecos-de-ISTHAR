using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject panelSettings;
    [SerializeField] private GameObject panelCredits;
    [SerializeField] private GameObject panelPause;
    private InputSystem_Actions _controls;


    private void Update()
    {
       Pause();
    }


    public void Play()
   {
      SceneManager.LoadScene("SampleScene");
   }

   public void OpenSettings()
   {
      panelSettings.SetActive(true);
      panelCredits.SetActive(false);
   }

   public void OpenCredits()
   {
      panelCredits.SetActive(true);
      panelSettings.SetActive(false);
   }

   public void ClosePanels()
   {
      panelSettings.SetActive(false);
      panelCredits.SetActive(false);
      panelPause.SetActive(false);
   }

   public void Pause()
   {
      if (Input.GetKeyDown(KeyCode.L))
      {
         panelPause.SetActive(true);
         Time.timeScale = 0;
         Debug.Log("PAUSAAAA");

      }
   }
}
