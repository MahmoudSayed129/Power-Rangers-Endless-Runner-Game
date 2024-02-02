using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class titleScreenControl : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject howtoPlayPanel;
    public GameObject creditsPanel;
    private void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        optionsPanel.SetActive(false);
        howtoPlayPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    
    
    public void QuitGame()
    {
        Application.Quit();
    }
    public void OpenOptions()
    {
        optionsPanel.SetActive(true);
    }
    public void CloseOptions()
    {
        optionsPanel.SetActive(false);
        howtoPlayPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }
    public void CloseHowtoPaly()
    {
        howtoPlayPanel.SetActive(false);
    }
    public void OpenHowtoPaly()
    {
        howtoPlayPanel.SetActive(true);
    }
    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }
    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("GamePlay");
    }
}
