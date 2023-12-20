using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameObject helpPanel = null;
    public GameObject storyPanel = null;

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevel1()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Level1");
    }

    public void LoadStart()
    {
        storyPanel.SetActive(true);
    }
    public void LoadHelp()
    {
        helpPanel.SetActive(true);
    }

    public void UnloadHelp()
    {
        helpPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
